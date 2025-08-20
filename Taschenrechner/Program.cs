namespace MeinTaschenrechner
{
    public class Program
    {
        public List<string> berechnungsHistorie = new List<string>();
        public string historieDatei = "berechnungen.txt";

        public string konfigJson = "config.json";
        public string konfigToml = "config.toml";

        public TaschenrechnerKonfiguration config = new TaschenrechnerKonfiguration();

        public List<Berechnung> detaillierteBerechnungen = new List<Berechnung>();

        public string benutzer = "";

        static Benutzer aktuellerBenutzer = null;

        private static TaschenrechnerContext DbContext;

        static Historiemenu historiemenu = new Historiemenu();
        static Backupmenu backupmenu = new Backupmenu();
        static Grundrechnung grundrechnung = new Grundrechnung();
        static Datenbankmenu datenbankmenu = new Datenbankmenu();
        static Statistikmenu statistikmenu = new Statistikmenu();
        static Konfigmenu konfigmenu = new Konfigmenu();
        static Program program = new Program();
        static Optimierter_Rechner optRechner = new Optimierter_Rechner();
        static Hilfsfunktionen help = new Hilfsfunktionen();

        static void Main(string[] args) 
        {
            DbContext = new TaschenrechnerContext();
            DbContext.Database.EnsureCreated();

            bool programmLaeuft = true;

            program.BenutzerAnmelden();

            help.Write("Lade gespeicherte Historie...");
            historiemenu.HistorieLaden();

            if (program.config.AutoSpeichern)
            {
                backupmenu.BackupErstellen();
            }

            while (programmLaeuft)
            {
                help.Mischen();

                help.Write("Willkommen zum Taschenrechner!");
                help.Write("Wähle eine Operation:");
                help.Write("1. Rechenmenü");
                help.Write("2. Historie-Optionen");
                help.Write("3. Konfiguration bearbeiten");
                help.Write("4. Backups verwalten");
                help.Write("5. Benutzer Wechseln");
                help.Write("6. Benutzer Löschen");
                help.Write("7. Datenbank optionen");
                help.Write("8. Benutzer-Statistik anzeigen");
                help.Write("9. Optimierter Rechner");
                help.Write("10. Beenden");
                help.Write("Deine Wahl (1-10): ");
                int wahl = help.MenuWahlEinlesen();

                switch (wahl)
                {
                    case 1:
                        grundrechnung.RechenMenu();
                        break;
                    case 2:
                        historiemenu.HistorieMenu();
                        break;
                    case 3:
                        konfigmenu.KonfigMenu();
                        break;
                    case 4:
                        backupmenu.BackupMenu();
                        break;
                    case 5:
                        program.BenutzerWechseln();
                        break;
                    case 6:
                        program.BenutzerLöschen();
                        break;
                    case 7:
                        datenbankmenu.DatenbankMenu();
                        break;
                    case 8:
                        statistikmenu.StatistikMenu();
                        break;
                    case 9:
                        optRechner.Start();
                        break;
                    case 10:
                        help.Write("Speichere Historie...");
                        historiemenu.HistorieSpeichern();

                        programmLaeuft = false;
                        help.Write("Auf Wiedersehen!");
                        break;
                    default:
                        help.Write("Ungültige Wahl!");
                        break;
                }

                if (programmLaeuft)
                {
                    help.Write("\nDrücke Enter für Hauptmenü...");
                    Console.ReadLine();
                }
            }
        }

        public Benutzer getAktBenutzer()
        {
            return aktuellerBenutzer;
        }

        public void BenutzerAnmelden()
        {
            help.Write("=== BENUTZER-ANMELDUNG ===");
            help.Write("Benutzername: ");
            string name = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(name))
            {
                help.Write("Ungültiger Benutzername!");
                return;
            }

            // Benutzer suchen
            var benutzer = DbContext.Benutzer.FirstOrDefault(u => u.Name == name);

            if (benutzer == null)
            {
                // Neuen Benutzer erstellen
                help.Write($"Benutzer '{name}' nicht gefunden.");
                help.Write("Neuen Benutzer erstellen? (j/n): ");

                if (Console.ReadLine()?.ToLower() == "j")
                {
                    benutzer = BenutzerErstellen(name, DbContext);
                    aktuellerBenutzer = benutzer;
                }
            }
            else
            {
                aktuellerBenutzer = benutzer;
                program.StandardEinstellungenErstellen(aktuellerBenutzer.Id, DbContext);
            }
            try
            {
                help.Write($"Angemeldet als: {aktuellerBenutzer.Name}");

                string benutzerOrdner = Path.Join("Benutzer", aktuellerBenutzer.Name);
                konfigmenu.KonfigurationLaden();

                if (!Directory.Exists(benutzerOrdner))
                {
                    Directory.CreateDirectory(benutzerOrdner);
                }
            }
            catch (Exception ex)
            {
                help.Write("ACHTUNG! " + ex);
            }
            // Benutzereinstellungen laden
            BenutzereinstellungenLaden();
        }

        static Benutzer BenutzerErstellen(string name, TaschenrechnerContext context)
        {
            help.Write("Email (optional): ");
            string email = Console.ReadLine()?.Trim();

            var neuerBenutzer = new Benutzer
            {
                Name = name,
                Email = string.IsNullOrEmpty(email) ? "" : email,
                ErstelltAm = DateTime.Now
            };

            try
            {
                context.Benutzer.Add(neuerBenutzer);
                context.SaveChanges();

                // Standard-Einstellungen erstellen
                program.StandardEinstellungenErstellen(neuerBenutzer.Id, context);

                help.Write($"Benutzer '{name}' erfolgreich erstellt!");
                return neuerBenutzer;
            }
            catch (Exception ex)
            {
                help.Write($"Fehler beim Erstellen des Benutzers: {ex.Message}");
                return null;
            }
        }

        public void StandardEinstellungenErstellen(int benutzerId, TaschenrechnerContext context)
        {
            // Anzahl der benötigten IDs (entspricht der Anzahl der Standard-Einstellungen)
            int benötigteIds = 6;

            // Hole alle existierenden IDs aus der Tabelle
            var existingIds = context.Einstellungen.Select(e => e.Id).ToHashSet();

            // Berechne die kleinstmöglichen freien IDs
            var freieIds = new List<int>();
            int aktuelleId = 1;

            while (freieIds.Count < benötigteIds)
            {
                if (!existingIds.Contains(aktuelleId))
                {
                    freieIds.Add(aktuelleId);
                }
                aktuelleId++;
            }

            var vorhandeneEinstellungen = context.Einstellungen
                .Where(e => e.BenutzerId == benutzerId)
                .Select(e => e.Schluessel)
                .ToHashSet();

            // Erstelle die Standard-Einstellungen mit den berechneten IDs
            var standardEinstellungen = new List<Einstellung>
            {
                new Einstellung { Id = freieIds[0], BenutzerId = benutzerId, Schluessel = "Nachkommastellen", Wert = "2" },
                new Einstellung { Id = freieIds[1], BenutzerId = benutzerId, Schluessel = "Thema", Wert = "Hell" },
                new Einstellung { Id = freieIds[2], BenutzerId = benutzerId, Schluessel = "Standardrechner", Wert = "Basis" },
                new Einstellung { Id = freieIds[3], BenutzerId = benutzerId, Schluessel = "AutoSpeichern", Wert = "true" },
                new Einstellung { Id = freieIds[4], BenutzerId = benutzerId, Schluessel = "Sprache", Wert = "Deutsch" },
                new Einstellung { Id = freieIds[5], BenutzerId = benutzerId, Schluessel = "ZeigeZeitstempel", Wert = "true" }
            };

            // Füge die Einstellungen zur Datenbank hinzu
            var fehlendeEinstellungen = standardEinstellungen
                .Where(e => !vorhandeneEinstellungen.Contains(e.Schluessel))
                .ToList();

            // Nur fehlende Einstellungen hinzufügen
            if (fehlendeEinstellungen.Any())
            {
                context.Einstellungen.AddRange(fehlendeEinstellungen);
                context.SaveChanges();
                help.Write($"Es wurden {fehlendeEinstellungen.Count} fehlende Standardeinstellungen hinzugefügt.");
            }
            else
            {
                help.Write("Alle Standardeinstellungen sind bereits vorhanden.");
            }
        }

        static void BenutzereinstellungenLaden()
        {
            if (aktuellerBenutzer == null)
            {
                help.Write("Kein Benutzer angemeldet! Konnte keine Einstellungen laden!");
                return;
            }
            using var context = new TaschenrechnerContext();

            var einstellungen = context.Einstellungen
                .Where(e => e.BenutzerId == aktuellerBenutzer.Id)
                .ToList();

            // Einstellungen in das config-Objekt laden
            foreach (var einstellung in einstellungen)
            {
                switch (einstellung.Schluessel)
                {
                    case "Nachkommastellen":
                        if (int.TryParse(einstellung.Wert, out int stellen))
                            program.config.Nachkommastellen = stellen;
                        break;
                    case "Thema":
                        program.config.Thema = einstellung.Wert;
                        break;
                    case "AutoSpeichern":
                        if (bool.TryParse(einstellung.Wert, out bool autoSave))
                            program.config.AutoSpeichern = autoSave;
                        break;
                    case "Sprache":
                        program.config.Sprache = einstellung.Wert;
                        break;
                    case "Standardrechner":
                        program.config.Standardrechner = einstellung.Wert;
                        break;
                    case "ZeigeZeitstempel":
                        if (bool.TryParse(einstellung.Wert, out bool zeigeZeitstempel))
                            program.config.ZeigeZeitstempel = zeigeZeitstempel;
                        break;

                }
            }

            help.Write("Benutzereinstellungen geladen.");
        }

        public void BenutzerWechseln()
        {
            program.BenutzerAnmelden();
        }

        public void BenutzerLöschen()
        {
            help.Write("Welchen Benutzer möchtest du löschen?");
            help.Write("Benutzername: ");
            string name = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(name))
            {
                help.Write("Ungültiger Benutzername!");
                return;
            }

            var benutzer = DbContext.Benutzer.FirstOrDefault(u => u.Name == name);

            if (benutzer == null)
            {
                help.Write($"Benutzer '{name}' nicht gefunden.");
                return;
            }

            help.Write($"Bist du sicher, dass du den Benutzer '{benutzer.Name}' und alle zugehörigen Daten löschen möchtest? (j/n): ");
            string bestaetigung = Console.ReadLine()?.ToLower();

            if (bestaetigung != "j")
            {
                help.Write("Löschung abgebrochen.");
                return;
            }

            try
            {
                // Alle Berechnungen des Benutzers löschen
                var berechnungen = DbContext.Berechnungen.Where(b => b.BenutzerId == benutzer.Id).ToList();
                DbContext.Berechnungen.RemoveRange(berechnungen);
                // Benutzer löschen
                DbContext.Benutzer.Remove(benutzer);
                DbContext.SaveChanges();
                help.Write($"Benutzer '{benutzer.Name}' und alle zugehörigen Daten wurden gelöscht.");
            }
            catch (Exception ex)
            {
                help.Write($"Fehler beim Löschen des Benutzers: {ex.Message}");
            }
        }
    }
}
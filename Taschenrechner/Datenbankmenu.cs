using System.Text.Json;

namespace MeinTaschenrechner
{
    internal class Datenbankmenu
    {
        static Program programm = new Program();
        static Hilfsfunktionen help = new Hilfsfunktionen();

        public void DatenbankMenu()
        {
            bool datenbankMenuAktiv = true;
            while (datenbankMenuAktiv)
            {
                help.Mischen();
                help.Write("\n=== DATENBANK-MENÜ ===");
                help.Write("1. Datenbank-Historie anzeigen");
                help.Write("2. Berechnungen suchen");
                help.Write("3. Datenbank exportieren");
                help.Write("4. Datenbank bereinigen");
                help.Write("5. Datenbank Backup");
                help.Write("6. Zurück zum Hauptmenü");
                help.Write("Deine Wahl (1-6): ");
                int wahl = help.MenuWahlEinlesen();
                switch (wahl)
                {
                    case 1:
                        DatenbankHistorieAnzeigen();
                        break;
                    case 2:
                        BerechnungenSuchen();
                        break;
                    case 3:
                        DatenbankExportieren();
                        break;
                    case 4:
                        DatenbankBereinigen();
                        break;
                    case 5:
                        DatenbankBackup();
                        break;
                    case 6:
                        datenbankMenuAktiv = false;
                        help.Write("Zurück zum Hauptmenü.");
                        break;
                    default:
                        help.Write("Ungültige Wahl!");
                        break;
                }
                if (datenbankMenuAktiv)
                {
                    help.Write("\nDrücke Enter für Menü...");
                    Console.ReadLine();
                }
            }
        }

        public void BerechnungInDatenbankSpeichern(string operation, double[] eingaben, double ergebnis, string kommentar = "", string rechnertyp = "Basis")
        {
            Benutzer akt = programm.getAktBenutzer();
            if (akt == null)
            {
                help.Write("Kein Benutzer angemeldet!");
                return;
            }

            try
            {
                using var context = new TaschenrechnerContext();

                var berechnungDB = new BerechnungDB
                {
                    BenutzerId = akt.Id,
                    Operation = operation,
                    Eingaben = JsonSerializer.Serialize(eingaben), // Array als JSON speichern
                    Ergebnis = ergebnis,
                    Kommentar = kommentar,
                    Rechnertyp = rechnertyp,
                    Zeitstempel = DateTime.Now
                };

                context.Berechnungen.Add(berechnungDB);
                context.SaveChanges();

                help.Write("Berechnung in Datenbank gespeichert.");
            }
            catch (Exception ex)
            {
                help.Write($"Fehler beim Speichern in Datenbank: {ex.Message}");
            }
        }

        public void DatenbankHistorieAnzeigen()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Benutzer akt = programm.getAktBenutzer();
            if (akt == null)
            {
                help.Write("Kein Benutzer angemeldet!");
                return;
            }

            using var context = new TaschenrechnerContext();

            var berechnungen = context.Berechnungen
                .Where(b => b.BenutzerId == akt.Id)
                .OrderByDescending(b => b.Zeitstempel)
                .Take(20) // Nur die letzten 20 anzeigen
                .ToList();

            if (!berechnungen.Any())
            {
                help.Write("Keine Berechnungen in der Datenbank gefunden.");
                return;
            }

            help.Write("=== DATENBANK-HISTORIE (letzte 20) ===");
            foreach (var berechnung in berechnungen)
            {
                try
                {
                    double[] eingaben = JsonSerializer.Deserialize<double[]>(berechnung.Eingaben);
                    if (berechnung.Operation == "$")
                    {
                        help.Write($"[{berechnung.Zeitstempel:HH:mm:ss}] " + $"{eingaben[0]}€ = ${berechnung.Ergebnis.ToString($"F{programm.config.Nachkommastellen}")} " + $"({berechnung.Rechnertyp})");
                    }
                    else if (berechnung.Operation == "/, *")
                    {
                        help.Write($"[{berechnung.Zeitstempel:HH:mm:ss}] " + $"({eingaben[0]} / {eingaben[1]}) * {eingaben[2]} = {berechnung.Ergebnis.ToString($"F{programm.config.Nachkommastellen}")} " + $"({berechnung.Rechnertyp})");
                    }
                    else if (berechnung.Operation == "*, /")
                    {
                        help.Write($"[{berechnung.Zeitstempel:HH:mm:ss}] " + $"({eingaben[0]} * {eingaben[1]}) / {eingaben[2]} = {berechnung.Ergebnis.ToString($"F{programm.config.Nachkommastellen}")} " + $"({berechnung.Rechnertyp})");
                    }
                    else
                    {
                        string eingabenStr = string.Join($" {berechnung.Operation} ", eingaben);

                        help.Write($"[{berechnung.Zeitstempel:HH:mm:ss}] " + $"{eingabenStr} = {berechnung.Ergebnis.ToString($"F{programm.config.Nachkommastellen}")} " + $"({berechnung.Rechnertyp})");
                    }

                    if (!string.IsNullOrEmpty(berechnung.Kommentar))
                    {
                        help.Write($"    Kommentar: {berechnung.Kommentar}");
                    }
                }
                catch (Exception ex)
                {
                    help.Write($"Fehler beim Anzeigen einer Berechnung: {ex.Message}");
                }
            }
        }

        static void BerechnungenSuchen()
        {
            Benutzer akt = programm.getAktBenutzer();
            if (akt == null)
            {
                help.Write("Kein Benutzer angemeldet!");
                return;
            }

            help.Write("=== BERECHNUNGEN SUCHEN ===");
            help.Write("1. Nach Operation suchen (+, -, *, /)");
            help.Write("2. Nach Datum suchen");
            help.Write("3. Nach Rechnertyp suchen");
            help.Write("4. Nach Ergebnis-Bereich suchen");

            int wahl = (int)help.ZahlEinlesen("Deine Wahl (1-4): ");

            using var context = new TaschenrechnerContext();
            IQueryable<BerechnungDB> query = context.Berechnungen
                .Where(b => b.BenutzerId == akt.Id);

            switch (wahl)
            {
                case 1:
                    help.Write("Operation eingeben (+, -, *, /, etc.): ");
                    string operation = Console.ReadLine();
                    query = query.Where(b => b.Operation == operation);
                    break;

                case 2:
                    help.Write("Datum (yyyy-mm-dd): ");
                    if (DateTime.TryParse(Console.ReadLine(), out DateTime datum))
                    {
                        var startDatum = datum.Date;
                        var endDatum = startDatum.AddDays(1);
                        query = query.Where(b => b.Zeitstempel >= startDatum && b.Zeitstempel < endDatum);
                    }
                    break;

                case 3:
                    help.Write("Rechnertyp (Basis, Wissenschaftlich, Finanz): ");
                    string rechnertyp = Console.ReadLine();
                    query = query.Where(b => b.Rechnertyp.Contains(rechnertyp));
                    break;

                case 4:
                    double min = help.ZahlEinlesen("Minimum: ");
                    double max = help.ZahlEinlesen("Maximum: ");
                    query = query.Where(b => b.Ergebnis >= min && b.Ergebnis <= max);
                    break;

                default:
                    help.Write("Ungültige Wahl!");
                    return;
            }

            var ergebnisse = query.OrderByDescending(b => b.Zeitstempel).ToList();

            help.Write($"\n{ergebnisse.Count} Ergebnisse gefunden:");
            foreach (var berechnung in ergebnisse)
            {
                double[] eingaben = JsonSerializer.Deserialize<double[]>(berechnung.Eingaben);
                string eingabenStr = string.Join($" {berechnung.Operation} ", eingaben);

                help.Write($"[{berechnung.Zeitstempel:dd.MM.yyyy HH:mm}] " +
                      $"{eingabenStr} = {berechnung.Ergebnis} ({berechnung.Rechnertyp})");
            }
        }

        static void DatenbankExportieren()
        {
            Benutzer akt = programm.getAktBenutzer();
            if (akt == null)
            {
                help.Write("Kein Benutzer angemeldet!");
                return;
            }

            try
            {
                using var context = new TaschenrechnerContext();

                var berechnungen = context.Berechnungen
                    .Where(b => b.BenutzerId == akt.Id)
                    .OrderBy(b => b.Zeitstempel)
                    .ToList();

                string exportDatei = $"export_{akt.Name}_{DateTime.Now:yyyyMMdd_HHmmss}.json";

                var exportData = new
                {
                    Benutzer = akt.Name,
                    ExportiertAm = DateTime.Now,
                    AnzahlBerechnungen = berechnungen.Count,
                    Berechnungen = berechnungen.Select(b => new
                    {
                        b.Zeitstempel,
                        b.Operation,
                        Eingaben = JsonSerializer.Deserialize<double[]>(b.Eingaben),
                        b.Ergebnis,
                        b.Kommentar,
                        b.Rechnertyp
                    })
                };

                string json = JsonSerializer.Serialize(exportData, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(exportDatei, json);
                help.Write($"Datenbank exportiert nach: {exportDatei}");
            }
            catch (Exception ex)
            {
                help.Write($"Fehler beim Export: {ex.Message}");
            }
        }

        static void DatenbankBereinigen()
        {
            Benutzer akt = programm.getAktBenutzer();
            if (akt == null)
            {
                help.Write("Kein Benutzer angemeldet!");
                return;
            }

            help.Write("=== DATENBANK BEREINIGEN ===");
            help.Write("Warnung: Diese Aktion löscht alte Berechnungen unwiderruflich!");

            int tage = (int)help.ZahlEinlesen("Berechnungen älter als wie viele Tage löschen? ");

            var grenzDatum = DateTime.Now.AddDays(-tage);

            using var context = new TaschenrechnerContext();

            var zuLoeschendeEintraege = context.Berechnungen
                .Where(b => b.BenutzerId == akt.Id &&
                           b.Zeitstempel < grenzDatum)
                .ToList();

            help.Write($"{zuLoeschendeEintraege.Count} Berechnungen würden gelöscht werden.");
            help.Write("Fortfahren? (ja/nein): ");

            if (Console.ReadLine()?.ToLower() == "ja")
            {
                try
                {
                    context.Berechnungen.RemoveRange(zuLoeschendeEintraege);
                    context.SaveChanges();
                    help.Write($"{zuLoeschendeEintraege.Count} Berechnungen gelöscht.");
                }
                catch (Exception ex)
                {
                    help.Write($"Fehler beim Löschen: {ex.Message}");
                }
            }
            else
            {
                help.Write("Bereinigung abgebrochen.");
            }
        }

        public void DatenbankBackup()
        {
            try
            {
                string quellDatei = "taschenrechner.db";
                string backupDatei = $"backup_taschenrechner_{DateTime.Now:yyyyMMdd_HHmmss}.db";

                if (File.Exists(quellDatei))
                {
                    File.Copy(quellDatei, backupDatei);
                    help.Write($"Datenbank-Backup erstellt: {backupDatei}");

                    // Dateigröße anzeigen
                    var info = new FileInfo(backupDatei);
                    help.Write($"Backup-Größe: {info.Length / 1024.0:F1} KB");
                }
                else
                {
                    help.Write("Keine Datenbank-Datei gefunden!");
                }
            }
            catch (Exception ex)
            {
                help.Write($"Fehler beim Backup: {ex.Message}");
            }
        }
    }
}
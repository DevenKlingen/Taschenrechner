using System.Text.Json;
using Tomlyn;

namespace MeinTaschenrechner
{
    public class Konfigmenu
    {
        static Program programm = new Program();
        static Hilfsfunktionen help = new Hilfsfunktionen();

        /// <summary>
        /// Zeigt das Konfiurationsmenü an, wrtet die Eingabe aus und führt die dazugehörige Funktion aus
        /// </summary>
        public void KonfigMenu()
        {
            bool konfigurierung = true;
            while (konfigurierung)
            {
                help.Mischen();

                help.Write("\n=== KONFIGURATIONSMENU ===");
                help.Write("1. Konfiguration laden");
                help.Write("2. Konfiguration anzeigen");
                help.Write("3. Konfiguration bearbeiten");
                help.Write("4. Konfiguration speichern");
                help.Write("5. Zurück zum Hauptmenü");
                help.Write("Deine Wahl (1-5): ");
                int wahl = help.MenuWahlEinlesen();
                switch (wahl)
                {
                    case 1:
                        KonfigurationLaden();
                        break;
                    case 2:
                        KonfigurationAnzeigen();
                        break;
                    case 3:
                        KonfigurationAendern();
                        break;
                    case 4:
                        KonfigurationSpeichern();
                        break;
                    case 5:
                        konfigurierung = false;
                        help.Write("Zurück zum Hauptmenü.");
                        break;
                    default:
                        help.Write("Ungültige Wahl!");
                        break;
                }

                if (konfigurierung)
                {
                    help.Write("\nDrücke Enter für Menü...");
                    Console.ReadLine();
                }
            }
        }

        /// <summary>
        /// Lädt die Konfiguration aus einer Datei (TOML oder JSON)
        /// </summary>
        public void KonfigurationLaden()
        {
            try
            {
                SetzeKonfigDateiPfad();
                if (File.Exists(programm.konfigToml))
                {
                    string tomlString = File.ReadAllText(programm.konfigToml);
                    programm.config = Toml.ToModel<TaschenrechnerKonfiguration>(tomlString);
                    help.Write("Konfiguration aus TOML geladen.");
                }
                else if (File.Exists(programm.konfigJson))
                {
                    string jsonString = File.ReadAllText(programm.konfigJson);
                    programm.config = JsonSerializer.Deserialize<TaschenrechnerKonfiguration>(jsonString);
                    help.Write("Konfiguration aus JSON geladen.");
                }
                else
                {
                    help.Write("Verwende Standard-Konfiguration.");
                    KonfigurationSpeichern(); // Erstelle Standard-Datei
                }
            }
            catch (Exception ex)
            {
                help.Write($"Fehler beim Laden der Konfiguration: {ex.Message}");
                programm.config = new TaschenrechnerKonfiguration(); // Fallback
            }

            var akt = programm.getAktBenutzer();
            using var context = new TaschenrechnerContext();
            var userSetting = context.Einstellungen
                    .FirstOrDefault(us => us.BenutzerId == akt.Id && us.Schluessel == "Thema");
            switch (userSetting?.Wert.ToLower())
            {
                case "hell":
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    programm.config.Thema = "Hell";
                    break;
                case "dunkel":
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    programm.config.Thema = "Dunkel";
                    break;
                case "grün":
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Green;
                    programm.config.Thema = "Grün";
                    break;
                case "gelb":
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    programm.config.Thema = "Gelb";
                    break;
                case "blau":
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Blue;
                    programm.config.Thema = "Blau";
                    break;
                case "rot":
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Red;
                    programm.config.Thema = "Rot";
                    break;
                case "lila":
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    programm.config.Thema = "Lila";
                    break;
                case "matrix":
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.BackgroundColor = ConsoleColor.Black;
                    programm.config.Thema = "Matrix";
                    break;
                default:
                    help.Write("Ungültige Wahl!");
                    break;
            }
        }

        /// <summary>
        /// Zeigt die aktuelle Konfiguration an
        /// </summary>
        static void KonfigurationAnzeigen()
        {
            help.Write("=== AKTUELLE KONFIGURATION ===");
            help.Write($"Thema: {programm.config.Thema}");
            help.Write($"Nachkommastellen: {programm.config.Nachkommastellen}");
            help.Write($"Standardrechner: {programm.config.Standardrechner}");
            help.Write($"Auto-Speichern: {programm.config.AutoSpeichern}");
            help.Write($"Sprache: {programm.config.Sprache}");
            help.Write($"Zeitstempel anzeigen: {programm.config.ZeigeZeitstempel}");
        }

        /// <summary>
        /// Erlaubt dem Nutzer, die Konfiguration zu ändern
        /// </summary>
        public void KonfigurationAendern()
        {
            help.Mischen();
            Benutzer akt = programm.getAktBenutzer();

            using var context = new TaschenrechnerContext();
            var userSetting = context.Einstellungen
                .FirstOrDefault(us => us.BenutzerId == akt.Id && us.Schluessel == "Thema");

            help.Write("=== KONFIGURATION ÄNDERN ===");

            Console.Write($"Thema ({programm.config.Thema}): ");
            string eingabe = Console.ReadLine().ToLower();

            switch (eingabe)
            {
                case "hell":
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    programm.config.Thema = "Hell";
                    userSetting.Wert = "Hell";
                    context.SaveChanges();
                    Console.Clear();
                    Console.WriteLine("Farbe wurde geändert!");
                    break;
                case "dunkel":
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    programm.config.Thema = "Dunkel";
                    userSetting.Wert = "Dunkel";
                    context.SaveChanges();
                    Console.Clear();
                    Console.WriteLine("Farbe wurde geändert!");
                    break;
                case "grün":
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Green;
                    programm.config.Thema = "Grün";
                    userSetting.Wert = "Grün";
                    context.SaveChanges();
                    Console.Clear();
                    Console.WriteLine("Farbe wurde geändert!");
                    break;
                case "gelb":
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    programm.config.Thema = "Gelb";
                    userSetting.Wert = "Gelb";
                    context.SaveChanges();
                    Console.Clear();
                    Console.WriteLine("Farbe wurde geändert!");
                    break;
                case "blau":
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Blue;
                    programm.config.Thema = "Blau";
                    userSetting.Wert = "Blau";
                    context.SaveChanges();
                    Console.Clear();
                    Console.WriteLine("Farbe wurde geändert!");
                    break;
                case "rot":
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Red;
                    programm.config.Thema = "Rot";
                    userSetting.Wert = "Rot";
                    context.SaveChanges();
                    Console.Clear();
                    Console.WriteLine("Farbe wurde geändert!");
                    break;
                case "lila":
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    programm.config.Thema = "Lila";
                    userSetting.Wert = "Lila";
                    context.SaveChanges();
                    Console.Clear();
                    Console.WriteLine("Farbe wurde geändert!");
                    break;
                case "bunt":
                    programm.config.Thema = "Bunt";
                    userSetting.Wert = "Bunt";
                    context.SaveChanges();
                    Console.Clear();
                    Console.WriteLine("Farbe wurde geändert!");
                    break;
                case "matrix":
                    programm.config.Thema = "Matrix";
                    userSetting.Wert = "Matrix";
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.BackgroundColor = ConsoleColor.Black;
                    context.SaveChanges();
                    Console.Clear();
                    Console.WriteLine("Farbe wurde geändert!");
                    break;
                default:
                    help.Write("Ungültige Wahl!");
                    break;
            }

            help.Write($"Nachkommastellen ({programm.config.Nachkommastellen}): ");
            eingabe = Console.ReadLine();

            userSetting = context.Einstellungen
                .FirstOrDefault(us => us.BenutzerId == akt.Id && us.Schluessel == "Nachkommastellen");

            if (int.TryParse(eingabe, out int stellen) && stellen >= 0 && stellen <= 10)
            {
                programm.config.Nachkommastellen = stellen;
                userSetting.Wert = stellen.ToString();
                context.SaveChanges();
            }

            help.Write($"Standardrechner ({programm.config.Standardrechner}): ");
            eingabe = Console.ReadLine();

            userSetting = context.Einstellungen
                .FirstOrDefault(us => us.BenutzerId == akt.Id && us.Schluessel == "Standardrechner");

            if (eingabe == "Basis" || eingabe == "Wissenschaftlich")
            {
                programm.config.Standardrechner = eingabe;
                userSetting.Wert = eingabe;
                context.SaveChanges();
            }

            help.Write($"Auto-Speichern (j/n, aktuell: {(programm.config.AutoSpeichern ? "j" : "n")}): ");
            eingabe = Console.ReadLine().ToLower();

            userSetting = context.Einstellungen
                .FirstOrDefault(us => us.BenutzerId == akt.Id && us.Schluessel == "AutoSpeichern");

            if (eingabe == "j" || eingabe == "n")
            {
                programm.config.AutoSpeichern = eingabe == "j";
                userSetting.Wert = programm.config.AutoSpeichern ? "j" : "n";
                context.SaveChanges();
            }

            help.Write($"Sprache ({programm.config.Sprache}): ");
            eingabe = Console.ReadLine();

            userSetting = context.Einstellungen
                .FirstOrDefault(us => us.BenutzerId == akt.Id && us.Schluessel == "Sprache");

            if (eingabe == "Deutsch" || eingabe == "Englisch" || eingabe == "Spanisch" || eingabe == "Italienisch" || eingabe == "Französisch")
            {
                programm.config.Sprache = eingabe;
                userSetting.Wert = eingabe;
                context.SaveChanges();
            }

            help.Write($"Zeige Zeitstempel (j/n, aktuell: {(programm.config.ZeigeZeitstempel ? "j" : "n")}): ");
            eingabe = Console.ReadLine().ToLower();

            userSetting = context.Einstellungen
                .FirstOrDefault(us => us.BenutzerId == akt.Id && us.Schluessel == "ZeigeZeitstempel");

            if (eingabe == "j" || eingabe == "n")
            {
                programm.config.ZeigeZeitstempel = eingabe == "j";
                userSetting.Wert = programm.config.ZeigeZeitstempel ? "j" : "n";
                context.SaveChanges();
            }

            var aktuellerBenutzer = programm.getAktBenutzer();
            string konfig = "Konfig";
            string KonfigOrdner = Path.Combine(aktuellerBenutzer.Name, konfig);
            if (!Directory.Exists(KonfigOrdner))
            {
                Directory.CreateDirectory(KonfigOrdner);
            }

            KonfigurationSpeichern(); // Erstellt die Datei, falls sie nicht existiert

            help.Write("Konfiguration aktualisiert!");
        }

        /// <summary>
        /// Speichert die aktuelle Konfiguration in eine JSON- und TOML-Datei
        /// </summary>
        public static void KonfigurationSpeichern()
        {
            try
            {
                string jsonString = JsonSerializer.Serialize(programm.config, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                SetzeKonfigDateiPfad();
                File.WriteAllText(programm.konfigJson, jsonString);

                string tomlString = Toml.FromModel(programm.config);
                File.WriteAllText(programm.konfigToml, tomlString);

                help.Write("Konfiguration gespeichert.");
            }
            catch (Exception ex)
            {
                help.Write($"Fehler beim Speichern der Konfiguration: {ex.Message}");
            }
        }

        /// <summary>
        /// Setzt die Pfade für die Konfigurationsdateien
        /// </summary>
        static void SetzeKonfigDateiPfad()
        {
            var akt = programm.getAktBenutzer();
            string konfigOrdner = $"Benutzer/{akt.Name}/Konfig";

            if (!Directory.Exists(Path.Combine(konfigOrdner)))
            {
                Directory.CreateDirectory(Path.Combine(konfigOrdner));
            }

            programm.konfigJson = Path.Combine(konfigOrdner, "config.json");
            programm.konfigToml = Path.Combine(konfigOrdner, "config.toml");
        }
    }
}
using System.Text;
using TaschenrechnerCore.Models;
using TaschenrechnerCore.Services;

namespace TaschenrechnerCore.Utils
{
    public class Hilfsfunktionen
    {
        private readonly BenutzerManagement _benutzerManagement;
        private readonly FlexibleLogger _logger;

        public Hilfsfunktionen(BenutzerManagement benutzer, FlexibleLogger logger)
        {
            _benutzerManagement = benutzer;
            _logger = logger;
        }

        public void WriteInfo(string input, bool noNewLine = false, bool ignoreTimeStamp = false)
        {
            _logger.Info(input, noNewLine, ignoreTimeStamp);
            Mischen();
        }

        public void WriteWarning(string input, bool noNewLine = false, bool ignoreTimeStamp = false)
        {
            _logger.Warn(input, noNewLine, ignoreTimeStamp);
            Mischen();
        }

        public void WriteError(string input, bool noNewLine = false, bool ignoreTimeStamp = false)
        {
            _logger.Error(input, noNewLine, ignoreTimeStamp);
            Mischen();
        }

        public void WriteDebug(string input, bool noNewLine = false, bool ignoreTimeStamp = false)
        {
            _logger.Debug(input, noNewLine, ignoreTimeStamp);
            Mischen();
        }

        /// <summary>
        /// Liest die Menüwahl des Nutzers ein und gibt die entsprechende Zahl zurück
        /// </summary>
        /// <returns></returns>
        public int MenuWahlEinlesen()
        {
            string eingabe = Console.ReadLine();
            _logger.Info($"Benutzer hat eingegeben: {eingabe}"); // Eingabe protokollieren
            int.TryParse(eingabe, out int zahl);
            return zahl;
        }

        public void Clear()
        {
            Console.Clear();
        }

        /// <summary>
        /// Mischt die Text- und Hintergrundfarbe
        /// </summary>
        public void Mischen()
        {
            Benutzer akt = _benutzerManagement.getBenutzer();

            using var context = new TaschenrechnerContext();
            if (akt != null)
            {
                var userSetting = context.Einstellungen
                    .FirstOrDefault(us => us.BenutzerId == akt.Id && us.Schluessel == "Thema");

                switch (userSetting?.Wert.ToLower())
                {
                    case "hell": Console.BackgroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.Black; break;
                    case "dunkel": Console.BackgroundColor = ConsoleColor.Black; Console.ForegroundColor = ConsoleColor.Gray; break;
                    case "grün": Console.ForegroundColor = ConsoleColor.Black; Console.BackgroundColor = ConsoleColor.Green; break;
                    case "gelb": Console.ForegroundColor = ConsoleColor.Black; Console.BackgroundColor = ConsoleColor.Yellow; break;
                    case "blau": Console.ForegroundColor = ConsoleColor.Black; Console.BackgroundColor = ConsoleColor.Blue; break;
                    case "rot": Console.ForegroundColor = ConsoleColor.Black; Console.BackgroundColor = ConsoleColor.Red; break;
                    case "lila": Console.ForegroundColor = ConsoleColor.White; Console.BackgroundColor = ConsoleColor.DarkMagenta; break;
                    case "matrix": Console.BackgroundColor = ConsoleColor.Black; Console.ForegroundColor = ConsoleColor.Green; break;
                    case "bunt":
                        int bg = 0, fg = 0;

                        while (bg == fg)
                        {
                            bg = new Random().Next(1, 17);
                            fg = new Random().Next(1, 17);
                        }

                        switch (bg)
                        {
                            case 1: Console.BackgroundColor = ConsoleColor.Black; break;
                            case 2: Console.BackgroundColor = ConsoleColor.DarkBlue; break;
                            case 3: Console.BackgroundColor = ConsoleColor.DarkGreen; break;
                            case 4: Console.BackgroundColor = ConsoleColor.DarkCyan; break;
                            case 5: Console.BackgroundColor = ConsoleColor.DarkRed; break;
                            case 6: Console.BackgroundColor = ConsoleColor.DarkMagenta; break;
                            case 7: Console.BackgroundColor = ConsoleColor.DarkYellow; break;
                            case 8: Console.BackgroundColor = ConsoleColor.Gray; break;
                            case 9: Console.BackgroundColor = ConsoleColor.DarkGray; break;
                            case 10: Console.BackgroundColor = ConsoleColor.Blue; break;
                            case 11: Console.BackgroundColor = ConsoleColor.Green; break;
                            case 12: Console.BackgroundColor = ConsoleColor.Cyan; break;
                            case 13: Console.BackgroundColor = ConsoleColor.Red; break;
                            case 14: Console.BackgroundColor = ConsoleColor.Magenta; break;
                            case 15: Console.BackgroundColor = ConsoleColor.Yellow; break;
                            case 16: Console.BackgroundColor = ConsoleColor.White; break;
                            default: bg = 0; break;
                        }
                        switch (fg)
                        {
                            case 1: Console.ForegroundColor = ConsoleColor.Black; break;
                            case 2: Console.ForegroundColor = ConsoleColor.DarkBlue; break;
                            case 3: Console.ForegroundColor = ConsoleColor.DarkGreen; break;
                            case 4: Console.ForegroundColor = ConsoleColor.DarkCyan; break;
                            case 5: Console.ForegroundColor = ConsoleColor.DarkRed; break;
                            case 6: Console.ForegroundColor = ConsoleColor.DarkMagenta; break;
                            case 7: Console.ForegroundColor = ConsoleColor.DarkYellow; break;
                            case 8: Console.ForegroundColor = ConsoleColor.Gray; break;
                            case 9: Console.ForegroundColor = ConsoleColor.DarkGray; break;
                            case 10: Console.ForegroundColor = ConsoleColor.Blue; break;
                            case 11: Console.ForegroundColor = ConsoleColor.Green; break;
                            case 12: Console.ForegroundColor = ConsoleColor.Cyan; break;
                            case 13: Console.ForegroundColor = ConsoleColor.Red; break;
                            case 14: Console.ForegroundColor = ConsoleColor.Magenta; break;
                            case 15: Console.ForegroundColor = ConsoleColor.Yellow; break;
                            case 16: Console.ForegroundColor = ConsoleColor.White; break;
                            default: fg = 0; break;
                        }
                        break;
                    default: Console.BackgroundColor = ConsoleColor.Black; Console.ForegroundColor = ConsoleColor.Gray; break;
                }
            }
        }

        /// <summary>
        /// Liest eine Zahl vom Nutzer ein und gibt sie zurück.
        /// </summary>
        /// <param name="nachricht"></param>
        /// <returns></returns>
        public double ZahlEinlesen(string nachricht)
        {
            while (true)
            {
                WriteInfo(nachricht); // Nachricht über den Logger ausgeben
                string eingabe = Console.ReadLine();

                if (double.TryParse(eingabe, out double zahl))
                {
                    _logger.Info($"Benutzer hat die Zahl {zahl} eingegeben."); // Eingabe protokollieren
                    return zahl;
                }

                WriteWarning("Bitte gib eine gültige Zahl ein!");
            }
        }

        public string Einlesen(string nachricht)
        {
            WriteInfo(nachricht); // Nachricht über den Logger ausgeben
            string eingabe = Console.ReadLine();

            _logger.Info($"Benutzer hat {eingabe} eingegeben."); // Eingabe protokollieren
            return eingabe;
        }

        public void setEncoding()
        {
            Console.OutputEncoding = Encoding.UTF8;
        }
    }
}

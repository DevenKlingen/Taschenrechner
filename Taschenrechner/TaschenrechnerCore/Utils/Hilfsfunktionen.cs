namespace TaschenrechnerCore.Utils;

public class Hilfsfunktionen
{
    public Program program = new Program();

    public void Write(string input)
    {
        Mischen();
        Console.WriteLine(input);
    }

    /// <summary>
    /// Liest die Menüwahl des Nutzers ein und gibt die entsprechende Zahl zurück
    /// </summary>
    /// <returns></returns>
    public int MenuWahlEinlesen()
    {
        string eingabe = Console.ReadLine();
        int.TryParse(eingabe, out int zahl);
        return zahl;
    }

    /// <summary>
    /// Mischt die Text- und Hintergrundfarbe
    /// </summary>
    public void Mischen()
    {
        Benutzer akt = program.getAktBenutzer();
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
            Write(nachricht);
            string eingabe = Console.ReadLine();

            if (double.TryParse(eingabe, out double zahl))
            {
                return zahl;
            }

            Write("Bitte gib eine gültige Zahl ein!");
        }
    }
}
using TaschenrechnerConsole;
using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;
using TaschenrechnerCore.Models;

namespace TaschenrechnerCore.Services;

public class StatistikMenu : IMenu
{
    static Hilfsfunktionen help = new Hilfsfunktionen();
    static Program program = new Program();
    static Statistiken stats = new Statistiken();
    static StatistikMonatsReport statsM = new StatistikMonatsReport();
    public void Show()
    {
        Benutzer aktuellerBenutzer = program.getAktBenutzer();
        bool statistikMenuAktiv = true;

        while (statistikMenuAktiv)
        {
            help.Mischen();
            help.Write("\n=== DATENBANK-MENÜ ===");
            help.Write("1. Benutzer Statistik anzeigen");
            help.Write("2. Monatsreport");
            help.Write("3. Wachstumstrend für einen Tag");
            help.Write("4. Zurück zum Hauptmenü");
            help.Write("Deine Wahl (1-4): ");
            int wahl = help.MenuWahlEinlesen();

            switch (wahl)
            {
                case 1:
                    stats.BenutzerStatistiken();
                    break;
                case 2:
                    statsM.MonatsReport();
                    break;
                case 3:
                    stats.Wachstumstrend();
                    break;
                case 4:
                    statistikMenuAktiv = false;
                    help.Write("Zurück zum Hauptmenü.");
                    break;
                default:
                    help.Write("Ungültige Wahl!");
                    break;
            }
            if (statistikMenuAktiv)
            {
                help.Write("\nDrücke Enter für Menü...");
                Console.ReadLine();
            }
        }
    }

    public string GetMenuTitle(int optionNumber)
    {
        return $"{optionNumber}. Statistik Menu";
    }
}
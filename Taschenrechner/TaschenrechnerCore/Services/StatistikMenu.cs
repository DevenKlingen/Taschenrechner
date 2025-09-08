using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class StatistikMenu : IMenu
{
    private readonly Hilfsfunktionen help;
    private readonly Statistiken stats;
    private readonly StatistikMonatsReport statsM;

    public StatistikMenu(
        Hilfsfunktionen help, 
        Statistiken stats, 
        StatistikMonatsReport statsM)
    {
        this.help = help;
        this.stats = stats;
        this.statsM = statsM;
    }

    public void Show()
    {
        bool statistikMenuAktiv = true;

        while (statistikMenuAktiv)
        {
            help.Mischen();
            help.Write("\n=== STATISTIK-MENÜ ===");
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
                help.Einlesen("\nDrücke Enter für Menü...");
            }
        }
    }

    public string GetMenuTitle(int optionNumber)
    {
        return $"{optionNumber}. Statistik-Menu";
    }
}
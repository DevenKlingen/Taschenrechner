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
            help.WriteInfo("\n=== STATISTIK-MENÜ ===");
            help.WriteInfo("1. Benutzer Statistik anzeigen");
            help.WriteInfo("2. Monatsreport");
            help.WriteInfo("3. Wachstumstrend für einen Tag");
            help.WriteInfo("0. Zurück zum Hauptmenü");
            int wahl = (int)help.ZahlEinlesen("Deine Wahl: ");

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
                case 0:
                    statistikMenuAktiv = false;
                    help.WriteInfo("Zurück zum Hauptmenü.");
                    break;
                default:
                    help.WriteWarning("Ungültige Wahl!");
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
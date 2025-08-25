using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class ListRechnerMenu : IMenu
{
    static Hilfsfunktionen help = new Hilfsfunktionen();
    static ListEinlesen listE = new ListEinlesen();
    static ListManipulation listM = new ListManipulation();

    public void Show()
    {
        bool listRechnerAktiv = true;

        List<int> liste1 = listE.ZahlenListeEinlesen("Liste 1");
        List<int> liste2 = listE.ZahlenListeEinlesen("Liste 2");

        while (listRechnerAktiv)
        {
            help.Mischen();

            help.Write("\n=== ZAHLEN-LISTEN OPERATIONEN ===");
            help.Write("1. Listen sortieren");
            help.Write("2. Duplikate entfernen");
            help.Write("3. Schnittmenge finden");
            help.Write("4. Vereinigung erstellen");
            help.Write("5. Zurück zum Rechenmenü");
            help.Write("Deine Wahl (1-5): ");
            int wahl = help.MenuWahlEinlesen();

            switch (wahl)
            {
                case 1:
                    listM.ListenSortieren(liste1, liste2);
                    break;
                case 2:
                    listM.DuplikateEntfernen(liste1, liste2);
                    break;
                case 3:
                    listM.Schnittmenge(liste1, liste2);
                    break;
                case 4:
                    listM.Vereinigung(liste1, liste2);
                    break;
                case 5:
                    listRechnerAktiv = false;
                    help.Write("Zurück zum Hauptmenü.");
                    break;
                default:
                    help.Write("Ungültige Wahl!");
                    break;
            }

            if (listRechnerAktiv)
            {
                help.Write("\nDrücke Enter für Menü...");
                Console.ReadLine();
            }
        }
    }

    public string GetMenuTitle(int optionTitle)
    {
        return $"{optionTitle}. ListRechner Menu";
    }
}
using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class ListRechnerMenu : IMenu
{
    private readonly Hilfsfunktionen _help;
    private readonly ListEinlesen _listEinlesen;
    private readonly ListManipulation _listManipulation;

    public ListRechnerMenu(
        Hilfsfunktionen help, 
        ListEinlesen listEinlesen, 
        ListManipulation listManipulation)
    {
        _help = help;
        _listEinlesen = listEinlesen;
        _listManipulation = listManipulation;
    }

    public void Show()
    {
        bool listRechnerAktiv = true;

        List<int> liste1 = _listEinlesen.ZahlenListeEinlesen("Liste 1");
        List<int> liste2 = _listEinlesen.ZahlenListeEinlesen("Liste 2");

        while (listRechnerAktiv)
        {
            _help.Mischen();

            _help.Write("\n=== ZAHLEN-LISTEN OPERATIONEN ===");
            _help.Write("1. Listen sortieren");
            _help.Write("2. Duplikate entfernen");
            _help.Write("3. Schnittmenge finden");
            _help.Write("4. Vereinigung erstellen");
            _help.Write("5. Zurück zum Rechenmenü");
            _help.Write("Deine Wahl (1-5): ");
            int wahl = _help.MenuWahlEinlesen();

            switch (wahl)
            {
                case 1:
                    _listManipulation.ListenSortieren(liste1, liste2);
                    break;
                case 2:
                    _listManipulation.DuplikateEntfernen(liste1, liste2);
                    break;
                case 3:
                    _listManipulation.Schnittmenge(liste1, liste2);
                    break;
                case 4:
                    _listManipulation.Vereinigung(liste1, liste2);
                    break;
                case 5:
                    listRechnerAktiv = false;
                    _help.Write("Zurück zum Hauptmenü.");
                    break;
                default:
                    _help.Write("Ungültige Wahl!");
                    break;
            }

            if (listRechnerAktiv)
            {
                _help.Einlesen("\nDrücke Enter für Menü...");
            }
        }
    }

    public string GetMenuTitle(int optionTitle)
    {
        return $"{optionTitle}. ListRechner Menu";
    }
}
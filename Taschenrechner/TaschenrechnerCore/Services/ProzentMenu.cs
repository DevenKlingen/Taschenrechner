using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class ProzentMenu : IMenu
{
    private readonly Hilfsfunktionen _help;
    private readonly ProzentRechnung _prozentRechnung;
    public ProzentMenu(Hilfsfunktionen help, ProzentRechnung prozentRechnung)
    {
        _help = help;
        _prozentRechnung = prozentRechnung;
    }

    public void Show()
    {
        bool prozentRechnerAktiv = true;
        while (prozentRechnerAktiv)
        {
            _help.Mischen();

            _help.Write("\n=== Prozentrechner ===");
            _help.Write("1. Prozentrechnung");
            _help.Write("2. Prozentualer Anteil");
            _help.Write("3. Grundwert");
            _help.Write("4. Zurück zum Rechenmenü");
            _help.Write("Deine Wahl (1-4): ");
            int wahl = _help.MenuWahlEinlesen();
            switch (wahl)
            {
                case 1:
                    _prozentRechnung.Prozentwert();
                    break;
                case 2:
                    _prozentRechnung.ProzentualerAnteil();
                    break;
                case 3:
                    _prozentRechnung.Grundwert();
                    break;
                case 4:
                    prozentRechnerAktiv = false;
                    _help.Write("Zurück zum Hauptmenü.");
                    break;
                default:
                    _help.Write("Ungültige Wahl!");
                    break;
            }

            if (prozentRechnerAktiv)
            {
                _help.Einlesen("\nDrücke Enter für Menü...");
            }
        }
    }

    public string GetMenuTitle(int optionTitle)
    {
        return $"{optionTitle}. Prozent Menu";
    }
}
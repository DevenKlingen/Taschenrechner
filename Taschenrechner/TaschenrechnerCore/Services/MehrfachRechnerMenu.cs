using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class MehrfachRechnerMenu : IMenu
{
    private readonly Hilfsfunktionen _help;
    private readonly Addition _addition;
    private readonly Subtraktion _subtraktion;
    private readonly Multiplikation _multiplikation;
    private readonly Division _division;
    
    public MehrfachRechnerMenu(
        Hilfsfunktionen help, 
        Addition addition, 
        Subtraktion subtraktion, 
        Multiplikation multiplikation, 
        Division division)
    {
        _help = help;
        _addition = addition;
        _subtraktion = subtraktion;
        _multiplikation = multiplikation;
        _division = division;
    }

    public void Show()
    {
        bool mehrfachBerechnungenAktiv = true;
        while (mehrfachBerechnungenAktiv)
        {
            _help.Mischen();
            _help.Write("\n=== MEHRFACH-BERECHNUNGEN ===");
            _help.Write("1. Addition");
            _help.Write("2. Subtraktion");
            _help.Write("3. Multiplikation");
            _help.Write("4. Division");
            _help.Write("5. Zurück zum Rechenmenü");
            _help.Write("Deine Wahl (1-5): ");
            int wahl = _help.MenuWahlEinlesen();
            switch (wahl)
            {
                case 1:
                    _addition.MehrfachAddition();
                    break;
                case 2:
                    _subtraktion.MehrfachSubtrahieren();
                    break;
                case 3:
                    _multiplikation.MehrfachMultiplizieren();
                    break;
                case 4:
                    _division.MehrfachDividieren();
                    break;
                case 5:
                    mehrfachBerechnungenAktiv = false;
                    _help.Write("Zurückzum Hauptmenü.");
                    break;
                default:
                    _help.Write("UngültigeWahl!");
                    break;
            }
            if (mehrfachBerechnungenAktiv)
            {
                _help.Einlesen("\nDrücke Enter fürMenü...");
            }
        }
    }

    public string GetMenuTitle(int optionTitle)
    {
        return $"{optionTitle}. MehfrachRechner";
    }
}
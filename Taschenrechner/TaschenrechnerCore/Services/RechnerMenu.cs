using System.Net;
using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class RechnerMenu : IMenu
{
    private readonly Hilfsfunktionen _help;
    private readonly Addition _addition;
    private readonly Subtraktion _subtraktion;
    private readonly Multiplikation _multiplikation;
    private readonly Division _division;
    private readonly WaehrungsRechner _waehrungsRechner;
    private readonly Potenzierer _potenzierer;
    private readonly ProzentMenu _ProzentMenu;
    private readonly DecimalRechner _decimalRechner;
    private readonly MatrixMenu _matrixMenu;
    private readonly ListRechnerMenu _listRechnerMenu;
    private readonly MehrfachRechnerMenu _mehrfachRechnerMenu;
    private readonly Fibonacci _fibonacci;
    private readonly PrimzahlenRechner _primzahlenRechner;
    private readonly Konstanten _konstanten;

    public RechnerMenu(
        Hilfsfunktionen help, 
        Addition addition, 
        Subtraktion subtraktion, 
        Multiplikation multiplikation, 
        Division division, 
        WaehrungsRechner waehrungsRechner, 
        Potenzierer potenzierer, 
        ProzentMenu prozentMenu, 
        DecimalRechner decimalRechner, 
        StatistikRechner statistikRechner, 
        MatrixMenu matrixMenu, 
        ListRechnerMenu listRechnerMenu, 
        MehrfachRechnerMenu mehrfachRechnerMenu, 
        Fibonacci fibonacci, 
        PrimzahlenRechner primzahlenRechner, 
        Konstanten konstanten)
    {
        _help = help;
        _addition = addition;
        _subtraktion = subtraktion;
        _multiplikation = multiplikation;
        _division = division;
        _waehrungsRechner = waehrungsRechner;
        _potenzierer = potenzierer;
        _ProzentMenu = prozentMenu;
        _decimalRechner = decimalRechner;
        _matrixMenu = matrixMenu;
        _listRechnerMenu = listRechnerMenu;
        _mehrfachRechnerMenu = mehrfachRechnerMenu;
        _fibonacci = fibonacci;
        _primzahlenRechner = primzahlenRechner;
        _konstanten = konstanten;
    }

    public void Show()
    {
        bool programmLaeuft = true;

        while (programmLaeuft)
        {
            _help.Mischen();

            _help.Write("\n=== RECHENMENÜ ===");
            _help.Write("Wähle eine Operation:");
            _help.Write("1. Addition");
            _help.Write("2. Subtraktion");
            _help.Write("3. Multiplikation");
            _help.Write("4. Division");
            _help.Write("5. Währungsrechner");
            _help.Write("6. Potenz");
            _help.Write("7. Prozentrechner");
            _help.Write("8. Zahl in Binär umwandeln");
            _help.Write("9. Zahl in Hexadezimal umwandeln");
            _help.Write("10. Matrix-Rechner");
            _help.Write("11. Listen-Rechner");
            _help.Write("12. Mehrfach-Berechnungen");
            _help.Write("13. Fibonacci-Zahlen");
            _help.Write("14. Primzahlen-Rechner");
            _help.Write("15. Suche im Dictionary");
            _help.Write("16. Zurück zum Hauptmenü");
            _help.Write("Deine Wahl (1-16): ");
            int wahl = _help.MenuWahlEinlesen();

            switch (wahl)
            {
                case 1:
                    _addition.Addieren();
                    break;
                case 2:
                    _subtraktion.Subtrahieren();
                    break;
                case 3:
                    _multiplikation.Multiplizieren();
                    break;
                case 4:
                    _division.Dividieren();
                    break;
                case 5:
                    _waehrungsRechner.WaehrungsRechnung();
                    break;
                case 6:
                    _potenzierer.Potenz();
                    break;
                case 7:
                    _ProzentMenu.Show();
                    break;
                case 8:
                    _decimalRechner.ToBinary();
                    break;
                case 9:
                    _decimalRechner.ToHexadecimal();
                    break;
                case 10:
                    _matrixMenu.Show();
                    break;
                case 11:
                    _listRechnerMenu.Show();
                    break;
                case 12:
                    _mehrfachRechnerMenu.Show();
                    break;
                case 13:
                    _fibonacci.FibonacciErstellen();
                    break;
                case 14:
                    _primzahlenRechner.PrimzahlenErmitteln();
                    break;
                case 15:
                    _konstanten.Suche();
                    break;
                case 16:
                    programmLaeuft = false;
                    _help.Write("Zurück zum Hauptmenü.");
                    break;
                default:
                    _help.Write("Ungültige Wahl!");
                    break;
            }

            if (programmLaeuft)
            {
                _help.Einlesen("\nDrücke Enter für Menü...");
            }
        }
    }

    public string GetMenuTitle(int optionTitle)
    {
        return $"{optionTitle}. Rechenmenu";
    }
}
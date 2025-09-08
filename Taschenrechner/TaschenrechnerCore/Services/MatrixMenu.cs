using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class MatrixMenu : IMenu
{
    private readonly Hilfsfunktionen _help;
    private readonly MatrixDeterminante _matrixDeterminante;
    private readonly Addition _addition;
    private readonly Multiplikation _multiplikation;

    public MatrixMenu(
        Hilfsfunktionen help, 
        MatrixDeterminante matrixDeterminante, 
        Addition addition, 
        Multiplikation multiplikation)
    {
        _help = help;
        _matrixDeterminante = matrixDeterminante;
        _addition = addition;
        _multiplikation = multiplikation;
    }

    public void Show()
    {
        bool matrixRechnerAktiv = true;

        while (matrixRechnerAktiv)
        {
            _help.Mischen();

            _help.Write("\n=== MATRIX-RECHNER (2x2) ===");
            _help.Write("1. Matrix-Addition");
            _help.Write("2. Matrix-Multiplikation");
            _help.Write("3. Determinante berechnen");
            _help.Write("4. Zurück zum Rechenmenü");
            _help.Write("Deine Wahl (1-4): ");
            int wahl = _help.MenuWahlEinlesen();

            switch (wahl)
            {
                case 1:
                    _addition.MatrixAddition();
                    break;
                case 2:
                    _multiplikation.MatrixMultiplikation();
                    break;
                case 3:
                    _matrixDeterminante.DeterminanteErmitteln();
                    break;
                case 4:
                    matrixRechnerAktiv = false;
                    _help.Write("Zurück zum Hauptmenü.");
                    break;
                default:
                    _help.Write("Ungültige Wahl!");
                    break;
            }

            if (matrixRechnerAktiv)
            {
                _help.Einlesen("\nDrücke Enter für Menü...");
            }
        }
    }

    public string GetMenuTitle(int optionTitle)
    {
        return $"{optionTitle}. Matrix Menu";
    }
}
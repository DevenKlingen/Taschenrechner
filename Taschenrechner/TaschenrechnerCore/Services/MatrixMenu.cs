using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class MatrixMenu : IMenu
{
    static Hilfsfunktionen help = new Hilfsfunktionen();
    static MatrixAusgabe matrixA = new MatrixAusgabe();
    static MatrixDeterminante matrixD = new MatrixDeterminante();
    static Addition a = new Addition();
    static Multiplikation m = new Multiplikation();

    public void Show()
    {
        bool matrixRechnerAktiv = true;

        while (matrixRechnerAktiv)
        {
            help.Mischen();

            help.Write("\n=== MATRIX-RECHNER (2x2) ===");
            help.Write("1. Matrix-Addition");
            help.Write("2. Matrix-Multiplikation");
            help.Write("3. Determinante berechnen");
            help.Write("4. Zurück zum Rechenmenü");
            help.Write("Deine Wahl (1-4): ");
            int wahl = help.MenuWahlEinlesen();

            switch (wahl)
            {
                case 1:
                    a.MatrixAddition();
                    break;
                case 2:
                    m.MatrixMultiplikation();
                    break;
                case 3:
                    matrixD.DeterminanteErmitteln();
                    break;
                case 4:
                    matrixRechnerAktiv = false;
                    help.Write("Zurück zum Hauptmenü.");
                    break;
                default:
                    help.Write("Ungültige Wahl!");
                    break;
            }

            if (matrixRechnerAktiv)
            {
                help.Write("\nDrücke Enter für Menü...");
                Console.ReadLine();
            }
        }
    }

    public string GetMenuTitle(int optionTitle)
    {
        return $"{optionTitle}. Matrix Menu";
    }
}
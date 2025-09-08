using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class MatrixLesen
{
    private readonly Hilfsfunktionen _help;
    
    public MatrixLesen(Hilfsfunktionen help)
    {  
        _help = help; 
    }

    /// <summary>
    /// Liest eine Matrix ein und prüft, ob die Eingabe valide ist
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public double[,] MatrixEinlesen(string name)
    {
        _help.Write($"Gib {name} ein:");
        double[,] matrix = new double[2, 2];

        for (int zeile = 0; zeile < 2; zeile++)
        {
            for (int spalte = 0; spalte < 2; spalte++)
            {
                matrix[zeile, spalte] = _help.ZahlEinlesen($"Element [{zeile + 1},{spalte + 1}]: ");
            }
        }

        return matrix;
    }
}
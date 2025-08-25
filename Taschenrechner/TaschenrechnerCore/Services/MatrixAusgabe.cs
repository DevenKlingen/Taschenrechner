using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class MatrixAusgabe
{
    static Hilfsfunktionen help = new Hilfsfunktionen();

    /// <summary>
    /// Gibt eine Matrix aus
    /// </summary>
    /// <param name="matrix"></param>
    /// <param name="name"></param>
    public void MatrixAusgeben(double[,] matrix, string name = "Matrix")
    {
        help.Write($"{name}:");
        for (int zeile = 0; zeile < 2; zeile++)
        {
            help.Write($"| {matrix[zeile, 0]:F1}  {matrix[zeile, 1]:F1} |");
        }
    }
}
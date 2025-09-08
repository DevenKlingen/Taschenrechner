using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class MatrixAusgabe
{
    private readonly Hilfsfunktionen _help;

    public MatrixAusgabe(Hilfsfunktionen help)
    {
        _help = help;
    }

    /// <summary>
    /// Gibt eine Matrix aus
    /// </summary>
    /// <param name="matrix"></param>
    /// <param name="name"></param>
    public void MatrixAusgeben(double[,] matrix, string name = "Matrix")
    {
        _help.Write($"{name}:");
        for (int zeile = 0; zeile < 2; zeile++)
        {
            _help.Write($"| {matrix[zeile, 0]:F1}  {matrix[zeile, 1]:F1} |");
        }
    }
}
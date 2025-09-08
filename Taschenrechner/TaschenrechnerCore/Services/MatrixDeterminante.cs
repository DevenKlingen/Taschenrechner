using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class MatrixDeterminante
{
    private readonly Hilfsfunktionen _help;
    private readonly MatrixLesen _matrixLesen;
    private readonly MatrixAusgabe _matrixAusgeben;

    public MatrixDeterminante(
        Hilfsfunktionen help, 
        MatrixLesen matrixLesen, 
        MatrixAusgabe matrixAusgeben)
    {
        _help = help;
        _matrixLesen = matrixLesen;
        _matrixAusgeben = matrixAusgeben;
    }

    /// <summary>
    /// Multipliziert zwei Matrizen miteinander
    /// </summary>
    public void DeterminanteErmitteln()
    {
        double[,] matrix = _matrixLesen.MatrixEinlesen("Matrix");

        double determinante = matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];

        _matrixAusgeben.MatrixAusgeben(matrix);
        _help.Write($"Determinante: {determinante}");
    }
}
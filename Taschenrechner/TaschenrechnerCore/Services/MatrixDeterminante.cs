using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class MatrixDeterminante
{
    static Hilfsfunktionen help = new Hilfsfunktionen();
    static MatrixLesen matrixL = new MatrixLesen();
    static MatrixAusgabe matrixA = new MatrixAusgabe();

    /// <summary>
    /// Multipliziert zwei Matrizen miteinander
    /// </summary>
    public void DeterminanteErmitteln()
    {
        double[,] matrix = matrixL.MatrixEinlesen("Matrix");

        double determinante = matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];

        matrixA.MatrixAusgeben(matrix);
        help.Write($"Determinante: {determinante}");
    }
}
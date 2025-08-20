namespace MeinTaschenrechner
{
    public class MatrixOperationen
    {
        static Hilfsfunktionen help = new Hilfsfunktionen();

        /// <summary>
        /// Zeigt das Matrix-Menü, wertet die Eingabe aus und führt die dazugehörige Funktion aus
        /// </summary>
        public void MatrixRechnerMenu()
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
                        MatrixAddition();
                        break;
                    case 2:
                        MatrixMultiplikation();
                        break;
                    case 3:
                        MatrixDeterminante();
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

        /// <summary>
        /// Addiert zwei Matrizen miteinander
        /// </summary>
        static void MatrixAddition()
        {
            double[,] matrixA = MatrixEinlesen("Matrix A");
            double[,] matrixB = MatrixEinlesen("Matrix B");

            double[,] ergebnis = new double[2, 2];

            for (int zeile = 0; zeile < 2; zeile++)
            {
                for (int spalte = 0; spalte < 2; spalte++)
                {
                    ergebnis[zeile, spalte] = matrixA[zeile, spalte] + matrixB[zeile, spalte];
                }
            }

            help.Write("\nErgebnis:");
            MatrixAusgeben(matrixA, "Matrix A");
            help.Write("+");
            MatrixAusgeben(matrixB, "Matrix B");
            help.Write("=");
            MatrixAusgeben(ergebnis, "Ergebnis");
        }

        /// <summary>
        /// Multipliziert zwei Matrizen miteinander
        /// </summary>
        static void MatrixMultiplikation()
        {
            double[,] matrixA = MatrixEinlesen("Matrix A");
            double[,] matrixB = MatrixEinlesen("Matrix B");
            double[,] ergebnis = new double[2, 2];

            for (int zeile = 0; zeile < 2; zeile++)
            {
                for (int spalte = 0; spalte < 2; spalte++)
                {
                    ergebnis[zeile, spalte] = matrixA[zeile, 0] * matrixB[0, spalte] + matrixA[zeile, 1] * matrixB[1, spalte];
                }
            }

            MatrixAusgeben(ergebnis, "Ergebnis der Matrix-Multiplikation");
        }

        /// <summary>
        /// Multipliziert zwei Matrizen miteinander
        /// </summary>
        static void MatrixDeterminante()
        {
            double[,] matrix = MatrixEinlesen("Matrix");

            double determinante = matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];

            MatrixAusgeben(matrix);
            help.Write($"Determinante: {determinante}");
        }

        /// <summary>
        /// Liest eine Matrix ein und prüft, ob die Eingabe valide ist
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static double[,] MatrixEinlesen(string name)
        {
            help.Write($"Gib {name} ein:");
            double[,] matrix = new double[2, 2];

            for (int zeile = 0; zeile < 2; zeile++)
            {
                for (int spalte = 0; spalte < 2; spalte++)
                {
                    matrix[zeile, spalte] = help.ZahlEinlesen($"Element [{zeile + 1},{spalte + 1}]: ");
                }
            }

            return matrix;
        }

        /// <summary>
        /// Gibt eine Matrix aus
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="name"></param>
        static void MatrixAusgeben(double[,] matrix, string name = "Matrix")
        {
            help.Write($"{name}:");
            for (int zeile = 0; zeile < 2; zeile++)
            {
                help.Write($"| {matrix[zeile, 0]:F1}  {matrix[zeile, 1]:F1} |");
            }
        }
    }
}
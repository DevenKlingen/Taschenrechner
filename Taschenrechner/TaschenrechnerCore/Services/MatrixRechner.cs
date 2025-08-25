using TaschenrechnerConsole;
using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;
using TaschenrechnerCore.Models;

namespace TaschenrechnerCore.Services;

public class MatrixRechner : BaseRechner
{
    static Program program = new Program();
    static Hilfsfunktionen help = new Hilfsfunktionen();
    static RechnerManager rechnerManager = new RechnerManager();
    

    public MatrixRechner() : base("Matrix-Rechner")
    {
    }

    public override double Berechnen(string operation, params double[] werte)
    {
        // Für Matrix-Operationen ist die Basis-Berechnen-Methode nicht ideal
        // Wir verwenden spezielle Matrix-Methoden
        throw new NotSupportedException("Verwende spezielle Matrix-Methoden für Berechnungen.");
    }

    public double[,] MatrixAddition(double[,] a, double[,] b)
    {
        int zeilen = a.GetLength(0);
        int spalten = a.GetLength(1);

        if (zeilen != b.GetLength(0) || spalten != b.GetLength(1))
            throw new ArgumentException("Matrizen müssen gleiche Dimensionen haben!");

        double[,] ergebnis = new double[zeilen, spalten];

        for (int i = 0; i < zeilen; i++)
        {
            for (int j = 0; j < spalten; j++)
            {
                ergebnis[i, j] = a[i, j] + b[i, j];
            }
        }

        return ergebnis;
    }

    public double[,] MatrixMultiplikation(double[,] a, double[,] b)
    {
        int aZeilen = a.GetLength(0);
        int aSpalten = a.GetLength(1);
        int bZeilen = b.GetLength(0);
        int bSpalten = b.GetLength(1);

        if (aSpalten != bZeilen)
            throw new ArgumentException("Spalten von A müssen gleich Zeilen von B sein!");

        double[,] ergebnis = new double[aZeilen, bSpalten];

        for (int i = 0; i < aZeilen; i++)
        {
            for (int j = 0; j < bSpalten; j++)
            {
                for (int k = 0; k < aSpalten; k++)
                {
                    ergebnis[i, j] += a[i, k] * b[k, j];
                }
            }
        }

        return ergebnis;
    }

    public void MatrixAusgeben(double[,] matrix, string name = "Matrix")
    {
        int zeilen = matrix.GetLength(0);
        int spalten = matrix.GetLength(1);

        Console.WriteLine($"{name} ({zeilen}x{spalten}):");
        for (int i = 0; i < zeilen; i++)
        {
            Console.Write("| ");
            for (int j = 0; j < spalten; j++)
            {
                Console.Write($"{matrix[i, j]:F2} ");
            }
            Console.WriteLine("|");
        }
    }

    public void ZeigeMatrixMenue()
    {
        Benutzer akt = program.getAktBenutzer();
        help.Mischen();
        Console.Clear();
        string aktueller = rechnerManager.AktuellerRechner?.RechnerTyp ?? "Kein Rechner aktiv";
        help.Write($"=== TASCHENRECHNER v2.0 ===");
        help.Write($"Aktueller Rechner: {aktueller}");
        help.Write($"Benutzer: {akt?.Name ?? "Nicht angemeldet"}");
        Console.WriteLine();
        help.Write("1. Mögliche Berechnungen ansehen");
        help.Write("2. Rechner wechseln");
        help.Write("3. Historie anzeigen");
        help.Write("4. Aktive Rechner anzeigen");
        help.Write("5. Datenbank-Historie");
        help.Write("6. Statistiken");
        help.Write("7. Einstellungen");
        help.Write("8. Benutzer-Management");
        help.Write("0. Beenden");
        Console.WriteLine();
    }

    public void MatrixBerechnen(string berechnung)
    {

        help.Write("Wie viele Zeilen und Spalten sollen die Matrizen haben?");

        int zeilenA;
        Console.Write("Anzahl der Zeilen (A): ");
        int.TryParse(Console.ReadLine(), out zeilenA);

        int spaltenA;
        Console.Write("Anzahl der Spalten (A) : ");
        int.TryParse(Console.ReadLine(), out spaltenA);

        double[,] matrixA = new double[zeilenA, spaltenA];

        int zeilenB;
        Console.Write("Anzahl der Zeilen (B): ");
        int.TryParse(Console.ReadLine(), out zeilenB);

        int spaltenB;
        Console.Write("Anzahl der Spalten (B) : ");
        int.TryParse(Console.ReadLine(), out spaltenB);

        if (berechnung.ToLower() == "multiplikation" && zeilenA != spaltenB)
        {
            help.Write("Für die Multiplikation müssen die Zeilen von Matrix A gleich den Spalten von Matrix B sein.");
            return;
        }

        int mengeA = zeilenA * spaltenA;
        help.Write($"Gib die Werte für die {mengeA} Elemente der Matrix A ein:");
        for (int i = 0; i < zeilenA; i++)
        {
            for (int j = 0; j < spaltenA; j++)
            {
                Console.Write($"Element [{i + 1}, {j + 1}]: ");
                double wert;
                while (!double.TryParse(Console.ReadLine(), out wert))
                {
                    help.Write("Ungültige Eingabe! Bitte eine Zahl eingeben.");
                }
                matrixA[i, j] = wert;
            }
        }

        int mengeB = zeilenB * spaltenB;
        double[,] matrixB = new double[zeilenB, spaltenB];
        help.Write($"Gib die Werte für die {mengeB} Elemente der Matrix B ein:");
        for (int i = 0; i < zeilenB; i++)
        {
            for (int j = 0; j < spaltenB; j++)
            {
                Console.Write($"Element [{i + 1}, {j + 1}]: ");
                double wert;
                while (!double.TryParse(Console.ReadLine(), out wert))
                {
                    help.Write("Ungültige Eingabe! Bitte eine Zahl eingeben.");
                }
                matrixB[i, j] = wert;
            }
        }

        switch (berechnung.ToLower())
        {
            case "addition":
                double[,] ergebnis = MatrixAddition(matrixA, matrixB);
                MatrixAusgeben(ergebnis, "Ergebnis der Addition");
                break;
            case "multiplikation":
                double[,] ergebnisMult = MatrixMultiplikation(matrixA, matrixB);
                MatrixAusgeben(ergebnisMult, "Ergebnis der Multiplikation");
                break;
            default:
                help.Write("Ungültige Berechnung!");
                break;
        }
    }
    
    public void ZeigeBerechnungen()
    {
        help.Mischen();
        Console.Clear();
        help.Write("=== Mögliche Matrix-Berechnungen ===");
        help.Write("1. Matrix Addition");
        help.Write("2. Matrix Multiplikation");
        help.Write("0. Zurück zum Hauptmenü");
        Console.WriteLine();

        int wahl = help.MenuWahlEinlesen();
        switch (wahl)
        {
            case 1:
                MatrixBerechnen("Addition");
                break;
            case 2:
                MatrixBerechnen("Multiplikation");
                break;
            case 0:
                help.Write("Zurück zum Hauptmenü.");
                break;
            default:
                help.Write("Ungültige Wahl!");
                break;
        }
    }
}
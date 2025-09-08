using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;
using TaschenrechnerCore.Models;

namespace TaschenrechnerCore.Services;

public class MatrixRechner : BaseRechner
{
    private readonly Hilfsfunktionen _help;
    private readonly RechnerManager _rechnerManager;
    private readonly BenutzerManagement _benutzerManagement;

    public MatrixRechner(
        Hilfsfunktionen help, 
        RechnerManager rechnerManager, 
        BenutzerManagement benutzerManagement,
        DatenbankBerechnungen datenbankBerechnungen) : base(benutzerManagement, datenbankBerechnungen, "Matrix-Rechner")
    {
        _help = help;
        _rechnerManager = rechnerManager;
        _benutzerManagement = benutzerManagement;
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

        _help.WriteInline($"{name} ({zeilen}x{spalten}):");
        for (int i = 0; i < zeilen; i++)
        {
            _help.WriteInline("| ");
            for (int j = 0; j < spalten; j++)
            {
                _help.WriteInline($"{matrix[i, j]:F2} ");
            }
            _help.Write("|");
        }
    }

    public void ZeigeMatrixMenue()
    {
        Benutzer akt = _benutzerManagement.getBenutzer();
        _help.Mischen();
        _help.Clear();
        string aktueller = _rechnerManager.AktuellerRechner?.RechnerTyp ?? "Kein Rechner aktiv";
        _help.Write($"\n=== TASCHENRECHNER v2.0 ===");
        _help.Write($"Aktueller Rechner: {aktueller}");
        _help.Write($"Benutzer: {akt?.Name ?? "Nicht angemeldet"}");
        _help.Write("");
        _help.Write("1. Mögliche Berechnungen ansehen");
        _help.Write("2. Rechner wechseln");
        _help.Write("3. Historie anzeigen");
        _help.Write("4. Aktive Rechner anzeigen");
        _help.Write("5. Datenbank-Historie");
        _help.Write("6. Statistiken");
        _help.Write("7. Einstellungen");
        _help.Write("8. Benutzer-Management");
        _help.Write("0. Beenden");
        _help.Write("");
    }

    public void MatrixBerechnen(string berechnung)
    {

        _help.Write("Wie viele Zeilen und Spalten sollen die Matrizen haben?");

        int zeilenA;
        int.TryParse(_help.Einlesen("Anzahl der Zeilen (A): "), out zeilenA);

        int spaltenA;
        int.TryParse(_help.Einlesen("Anzahl der Spalten (A): "), out spaltenA);

        double[,] matrixA = new double[zeilenA, spaltenA];

        int zeilenB;
        int.TryParse(_help.Einlesen("Anzahl der Zeilen (B): "), out zeilenB);

        int spaltenB;
        int.TryParse(_help.Einlesen("Anzahl der Spalten (B): "), out spaltenB);

        if (berechnung.ToLower() == "multiplikation" && zeilenA != spaltenB)
        {
            _help.Write("Für die Multiplikation müssen die Zeilen von Matrix A gleich den Spalten von Matrix B sein.");
            return;
        }

        int mengeA = zeilenA * spaltenA;
        _help.Write($"Gib die Werte für die {mengeA} Elemente der Matrix A ein:");
        for (int i = 0; i < zeilenA; i++)
        {
            for (int j = 0; j < spaltenA; j++)
            {
                double wert;
                while (!double.TryParse(_help.Einlesen($"Element [{i + 1}, {j + 1}]: "), out wert))
                {
                    _help.Write("Ungültige Eingabe! Bitte eine Zahl eingeben.");
                }
                matrixA[i, j] = wert;
            }
        }

        int mengeB = zeilenB * spaltenB;
        double[,] matrixB = new double[zeilenB, spaltenB];
        _help.Write($"Gib die Werte für die {mengeB} Elemente der Matrix B ein:");
        for (int i = 0; i < zeilenB; i++)
        {
            for (int j = 0; j < spaltenB; j++)
            {
                double wert;
                while (!double.TryParse(_help.Einlesen($"Element [{i + 1}, {j + 1}]: "), out wert))
                {
                    _help.Write("Ungültige Eingabe! Bitte eine Zahl eingeben.");
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
                _help.Write("Ungültige Berechnung!");
                break;
        }
    }
    
    public void ZeigeBerechnungen()
    {
        _help.Mischen();
        _help.Clear();
        _help.Write("\n=== Mögliche Matrix-Berechnungen ===");
        _help.Write("1. Matrix Addition");
        _help.Write("2. Matrix Multiplikation");
        _help.Write("0. Zurück zum Hauptmenü");
        _help.Write("");

        int wahl = _help.MenuWahlEinlesen();
        switch (wahl)
        {
            case 1:
                MatrixBerechnen("Addition");
                break;
            case 2:
                MatrixBerechnen("Multiplikation");
                break;
            case 0:
                _help.Write("Zurück zum Hauptmenü.");
                break;
            default:
                _help.Write("Ungültige Wahl!");
                break;
        }
    }
}
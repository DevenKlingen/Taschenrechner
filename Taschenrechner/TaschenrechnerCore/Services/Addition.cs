using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class Addition
{
    static Hilfsfunktionen help = new Hilfsfunktionen();
    static DatenbankBerechnungen datenbankB = new DatenbankBerechnungen();
    static HistorienBearbeitung historienB = new HistorienBearbeitung();
    static MatrixLesen matrixL = new MatrixLesen();
    static MatrixAusgabe matrixAus = new MatrixAusgabe();

    /// <summary>
    /// Addition zweier Zahlen
    /// </summary>
    public void Addieren()
    {
        double zahl1 = help.ZahlEinlesen("Gib die erste Zahl ein: ");
        double zahl2 = help.ZahlEinlesen("Gib die zweite Zahl ein: ");

        double ergebnis = zahl1 + zahl2;
        help.Write($"Ergebnis: {zahl1} + {zahl2} = {ergebnis}");

        double[] eingaben = { zahl1, zahl2 };
        historienB.BerechnungHinzufuegen("+", eingaben, ergebnis);

        datenbankB.BerechnungInDatenbankSpeichern("+", eingaben, ergebnis);
    }

    /// <summary>
    /// Addiert mindestens zwei Zahlen miteinander
    /// </summary>
    public void MehrfachAddition()
    {
        help.Write("\n===MEHRFACH-ADDITION ===");
        help.Write("Gibmehrere Zahlenein (beendemit 'fertig'):");

        List<double> zahlen = new List<double>();

        while (true)
        {
            help.Write($"Zahl {zahlen.Count + 1}: ");
            string eingabe = Console.ReadLine();

            if (eingabe.ToLower() == "fertig")
                break;

            if (int.TryParse(eingabe, out int zahl))
            {
                zahlen.Add(zahl);
            }
        }

        if (zahlen.Count < 2)
        {
            help.Write("Mindestens 2 Zahlen erforderlich!");
            return;
        }

        double summe = zahlen.Sum();

        string berechnung = $"{string.Join(" + ", zahlen)} = {summe}";
        help.Write($"Ergebnis: {berechnung}");

        historienB.BerechnungHinzufuegen("+", zahlen.ToArray(), summe);

        datenbankB.BerechnungInDatenbankSpeichern("+", zahlen.ToArray(), summe);
    }

    /// <summary>
    /// Addiert zwei Matrizen miteinander
    /// </summary>
    public void MatrixAddition()
    {
        double[,] matrixA = matrixL.MatrixEinlesen("Matrix A");
        double[,] matrixB = matrixL.MatrixEinlesen("Matrix B");

        double[,] ergebnis = new double[2, 2];

        for (int zeile = 0; zeile < 2; zeile++)
        {
            for (int spalte = 0; spalte < 2; spalte++)
            {
                ergebnis[zeile, spalte] = matrixA[zeile, spalte] + matrixB[zeile, spalte];
            }
        }

        help.Write("\nErgebnis:");
        matrixAus.MatrixAusgeben(matrixA, "Matrix A");
        help.Write("+");
        matrixAus.MatrixAusgeben(matrixB, "Matrix B");
        help.Write("=");
        matrixAus.MatrixAusgeben(ergebnis, "Ergebnis");
    }
}
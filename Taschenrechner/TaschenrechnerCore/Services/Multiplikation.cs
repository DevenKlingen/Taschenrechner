using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class Multiplikation
{
    static Hilfsfunktionen help = new Hilfsfunktionen();
    static HistorienBearbeitung historienB = new HistorienBearbeitung();
    static DatenbankBerechnungen datenbankB = new DatenbankBerechnungen();
    static MatrixLesen matrixL = new MatrixLesen();
    static MatrixAusgabe matrixAus = new MatrixAusgabe();

    /// <summary>
    /// Multiplikation zweier Zahlen
    /// </summary>
    public void Multiplizieren()
    {
        double zahl1 = help.ZahlEinlesen("Gib die erste Zahl ein: ");
        double zahl2 = help.ZahlEinlesen("Gib die zweite Zahl ein: ");

        double ergebnis = zahl1 * zahl2;
        help.Write($"Ergebnis: {zahl1} * {zahl2} = {ergebnis}");

        double[] eingaben = { zahl1, zahl2 };
        historienB.BerechnungHinzufuegen("*", eingaben, ergebnis);

        datenbankB.BerechnungInDatenbankSpeichern("*", eingaben, ergebnis);
    }

    /// <summary>
    /// Multipliziert mindestens zwei Zahlen miteinander
    /// </summary>
    public void MehrfachMultiplizieren()
    {
        help.Write("\n=== MEHRFACH-MULTIPLIKATION ===");
        help.Write("Gib mehrere Zahlen ein (beende mit 'fertig'):");

        List<double> zahlen = new List<double>();

        while (true)
        {
            help.Write($"Zahl {zahlen.Count + 1}: ");
            string eingabe = Console.ReadLine();
            if (eingabe.ToLower() == "fertig")
                break;
            if (double.TryParse(eingabe, out double zahl))
            {
                zahlen.Add(zahl);
            }
        }

        if (zahlen.Count < 2)
        {
            help.Write("Mindestens zwei Zahlen erforderlich!");
            return;
        }

        double ergebnis = 1;

        foreach (var zahl in zahlen)
        {
            ergebnis *= zahl;
        }

        string berechnung = $"{string.Join(" * ", zahlen)} = {ergebnis}";
        help.Write($"Ergebnis: {berechnung}");

        historienB.BerechnungHinzufuegen("*", zahlen.ToArray(), ergebnis);

        datenbankB.BerechnungInDatenbankSpeichern("*", zahlen.ToArray(), ergebnis);
    }
    
    /// <summary>
    /// Multipliziert zwei Matrizen miteinander
    /// </summary>
    public void MatrixMultiplikation()
    {
        double[,] matrixA = matrixL.MatrixEinlesen("Matrix A");
        double[,] matrixB = matrixL.MatrixEinlesen("Matrix B");
        double[,] ergebnis = new double[2, 2];

        for (int zeile = 0; zeile < 2; zeile++)
        {
            for (int spalte = 0; spalte < 2; spalte++)
            {
                ergebnis[zeile, spalte] = matrixA[zeile, 0] * matrixB[0, spalte] + matrixA[zeile, 1] * matrixB[1, spalte];
            }
        }

        matrixAus.MatrixAusgeben(ergebnis, "Ergebnis der Matrix-Multiplikation");
    }
}
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class Multiplikation
{
    private readonly Hilfsfunktionen _help;
    private readonly HistorienBearbeitung _historienBearbeitung;
    private readonly DatenbankBerechnungen _datenbankBerechnungen;
    private readonly MatrixLesen _matrixLesen;
    private readonly MatrixAusgabe _matrixAusgabe;
    private readonly BenutzerEinstellungen _benutzerEinstellungen;

    public Multiplikation(
        Hilfsfunktionen help, 
        HistorienBearbeitung historienB, 
        DatenbankBerechnungen datenbankB, 
        MatrixLesen matrixL, 
        MatrixAusgabe matrixAus,
        BenutzerEinstellungen benutzerEinstellungen)
    {
        _help = help;
        _historienBearbeitung = historienB;
        _datenbankBerechnungen = datenbankB;
        _matrixLesen = matrixL;
        _matrixAusgabe = matrixAus;
        _benutzerEinstellungen = benutzerEinstellungen;
    }



    /// <summary>
    /// Multiplikation zweier Zahlen
    /// </summary>
    public void Multiplizieren()
    {
        double zahl1 = _help.ZahlEinlesen("Gib die erste Zahl ein: ");
        double zahl2 = _help.ZahlEinlesen("Gib die zweite Zahl ein: ");

        double ergebnis = zahl1 * zahl2;
        _help.Write($"Ergebnis: {zahl1} * {zahl2} = {ergebnis}");

        double[] eingaben = { zahl1, zahl2 };
        _historienBearbeitung.BerechnungHinzufuegen(_benutzerEinstellungen, "*", eingaben, ergebnis);

        _datenbankBerechnungen.BerechnungInDatenbankSpeichern("*", eingaben, ergebnis);
    }

    /// <summary>
    /// Multipliziert mindestens zwei Zahlen miteinander
    /// </summary>
    public void MehrfachMultiplizieren()
    {
        _help.Write("\n=== MEHRFACH-MULTIPLIKATION ===");
        _help.Write("Gib mehrere Zahlen ein (beende mit 'fertig'):");

        List<double> zahlen = new List<double>();

        while (true)
        {
            string eingabe = _help.Einlesen($"Zahl {zahlen.Count + 1}: ");
            if (eingabe.ToLower() == "fertig")
                break;
            if (double.TryParse(eingabe, out double zahl))
            {
                zahlen.Add(zahl);
            }
        }

        if (zahlen.Count < 2)
        {
            _help.Write("Mindestens zwei Zahlen erforderlich!");
            return;
        }

        double ergebnis = 1;

        foreach (var zahl in zahlen)
        {
            ergebnis *= zahl;
        }

        string berechnung = $"{string.Join(" * ", zahlen)} = {ergebnis}";
        _help.Write($"Ergebnis: {berechnung}");

        _historienBearbeitung.BerechnungHinzufuegen(_benutzerEinstellungen, "*", zahlen.ToArray(), ergebnis);

        _datenbankBerechnungen.BerechnungInDatenbankSpeichern("*", zahlen.ToArray(), ergebnis);
    }
    
    /// <summary>
    /// Multipliziert zwei Matrizen miteinander
    /// </summary>
    public void MatrixMultiplikation()
    {
        double[,] matrixA = _matrixLesen.MatrixEinlesen("Matrix A");
        double[,] matrixB = _matrixLesen.MatrixEinlesen("Matrix B");
        double[,] ergebnis = new double[2, 2];

        for (int zeile = 0; zeile < 2; zeile++)
        {
            for (int spalte = 0; spalte < 2; spalte++)
            {
                ergebnis[zeile, spalte] = matrixA[zeile, 0] * matrixB[0, spalte] + matrixA[zeile, 1] * matrixB[1, spalte];
            }
        }

        _matrixAusgabe.MatrixAusgeben(ergebnis, "Ergebnis der Matrix-Multiplikation");
    }
}
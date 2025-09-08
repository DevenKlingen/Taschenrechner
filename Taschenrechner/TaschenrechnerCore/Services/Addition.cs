using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class Addition
{
    private readonly Hilfsfunktionen _help;
    private readonly DatenbankBerechnungen _datenbankBerechnungen;
    private readonly HistorienBearbeitung _historienBearbeitung;
    private readonly MatrixLesen _matrixLesen;
    private readonly MatrixAusgabe _matrixAusgabe;
    private readonly BenutzerEinstellungen _benutzerEinstellungen;

    public Addition(
        Hilfsfunktionen help, 
        DatenbankBerechnungen datenbankBerechnungen, 
        HistorienBearbeitung historienBearbeitung, 
        MatrixLesen matrixLesen, 
        MatrixAusgabe matrixAusgabe,
        BenutzerEinstellungen benutzerEinstellungen)
    {
        _help = help;
        _datenbankBerechnungen = datenbankBerechnungen;
        _historienBearbeitung = historienBearbeitung;
        _matrixLesen = matrixLesen;
        _matrixAusgabe = matrixAusgabe;
        _benutzerEinstellungen = benutzerEinstellungen;
    }

    /// <summary>
    /// Addition zweier Zahlen
    /// </summary>
    public void Addieren()
    {
        double zahl1 = _help.ZahlEinlesen("Gib die erste Zahl ein: ");
        double zahl2 = _help.ZahlEinlesen("Gib die zweite Zahl ein: ");

        double ergebnis = zahl1 + zahl2;
        _help.Write($"Ergebnis: {zahl1} + {zahl2} = {ergebnis}");

        double[] eingaben = { zahl1, zahl2 };
        _historienBearbeitung.BerechnungHinzufuegen(_benutzerEinstellungen, "+", eingaben, ergebnis);

        _datenbankBerechnungen.BerechnungInDatenbankSpeichern("+", eingaben, ergebnis);
    }

    /// <summary>
    /// Addiert mindestens zwei Zahlen miteinander
    /// </summary>
    public void MehrfachAddition()
    {
        _help.Write("\n===MEHRFACH-ADDITION ===");
        _help.Write("Gibmehrere Zahlenein (beendemit 'fertig'):");

        List<double> zahlen = new List<double>();

        while (true)
        {
            string eingabe = _help.Einlesen($"Zahl {zahlen.Count + 1}: ");

            if (eingabe.ToLower() == "fertig")
                break;

            if (int.TryParse(eingabe, out int zahl))
            {
                zahlen.Add(zahl);
            }
        }

        if (zahlen.Count < 2)
        {
            _help.Write("Mindestens 2 Zahlen erforderlich!");
            return;
        }

        double summe = zahlen.Sum();

        string berechnung = $"{string.Join(" + ", zahlen)} = {summe}";
        _help.Write($"Ergebnis: {berechnung}");

        _historienBearbeitung.BerechnungHinzufuegen(_benutzerEinstellungen, "+", zahlen.ToArray(), summe);

        _datenbankBerechnungen.BerechnungInDatenbankSpeichern("+", zahlen.ToArray(), summe);
    }

    /// <summary>
    /// Addiert zwei Matrizen miteinander
    /// </summary>
    public void MatrixAddition()
    {
        double[,] matrixA = _matrixLesen.MatrixEinlesen("Matrix A");
        double[,] matrixB = _matrixLesen.MatrixEinlesen("Matrix B");

        double[,] ergebnis = new double[2, 2];

        for (int zeile = 0; zeile < 2; zeile++)
        {
            for (int spalte = 0; spalte < 2; spalte++)
            {
                ergebnis[zeile, spalte] = matrixA[zeile, spalte] + matrixB[zeile, spalte];
            }
        }

        _help.Write("\nErgebnis:");
        _matrixAusgabe.MatrixAusgeben(matrixA, "Matrix A");
        _help.Write("+");
        _matrixAusgabe.MatrixAusgeben(matrixB, "Matrix B");
        _help.Write("=");
        _matrixAusgabe.MatrixAusgeben(ergebnis, "Ergebnis");
    }
}
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class Subtraktion
{
    private readonly Hilfsfunktionen _help;
    private readonly HistorienBearbeitung _historienBeabeitung;
    private readonly DatenbankBerechnungen _datenbankBerechnungen;
    private readonly BenutzerEinstellungen _benutzerEinstellungen;

    public Subtraktion(
        Hilfsfunktionen help, 
        HistorienBearbeitung historienB, 
        DatenbankBerechnungen datenbankB,
        BenutzerEinstellungen benutzerEinstellungen)
    {
        _help = help;
        _historienBeabeitung = historienB;
        _datenbankBerechnungen = datenbankB;
        _benutzerEinstellungen = benutzerEinstellungen;
    }



    /// <summary>
    /// Subtraktion zweier Zahlen
    /// </summary>
    public void Subtrahieren()
    {
        double zahl1 = _help.ZahlEinlesen("Gib die erste Zahl ein: ");
        double zahl2 = _help.ZahlEinlesen("Gib die zweite Zahl ein: ");

        double ergebnis = zahl1 - zahl2;
        _help.Write($"Ergebnis: {zahl1} - {zahl2} = {ergebnis}");

        double[] eingaben = { zahl1, zahl2 };
        _historienBeabeitung.BerechnungHinzufuegen(_benutzerEinstellungen, "-", eingaben, ergebnis);

        _datenbankBerechnungen.BerechnungInDatenbankSpeichern("-", eingaben, ergebnis);
    }

    /// <summary>
    /// Subtrahiert mindestens zwei Zahlen voneinander
    /// </summary>
    public void MehrfachSubtrahieren()
    {
        _help.Write("\n=== MEHRFACH-SUBTRAKTION ===");
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

        double ergebnis = zahlen[0];

        for (int i = 1; i < zahlen.Count; i++)
        {
            ergebnis -= zahlen[i];
        }

        string berechnung = $"{string.Join(" - ", zahlen)} = {ergebnis}";
        _help.Write($"Ergebnis: {berechnung}");

        _historienBeabeitung.BerechnungHinzufuegen(_benutzerEinstellungen, "-", zahlen.ToArray(), ergebnis);

        _datenbankBerechnungen.BerechnungInDatenbankSpeichern("-", zahlen.ToArray(), ergebnis);
    }
}

using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class Division
{
    private readonly Hilfsfunktionen _help;
    private readonly HistorienBearbeitung _historieBearbeitung;
    private readonly DatenbankBerechnungen _datenbankBerechnungen;
    private readonly BenutzerEinstellungen _benutzerEinstellungen;

    public Division(
        Hilfsfunktionen help,
        HistorienBearbeitung historienBearbeitung,
        DatenbankBerechnungen datenbankBerechnungen,
        BenutzerEinstellungen benutzerEinstellungen)
    {
        _help = help;
        _historieBearbeitung = historienBearbeitung;
        _datenbankBerechnungen = datenbankBerechnungen;
        _benutzerEinstellungen = benutzerEinstellungen;
    }

    /// <summary>
    /// Division zweier Zahlen
    /// </summary>
    public void Dividieren()
    {
        double zahl1 = _help.ZahlEinlesen("Gib die erste Zahl ein: ");
        double zahl2 = _help.ZahlEinlesen("Gib die zweite Zahl ein: ");

        if (zahl2 == 0)
        {
            _help.Write("Fehler: Division durch Null ist nicht möglich!");
        }
        else
        {
            double ergebnis = zahl1 / zahl2;
            _help.Write($"Ergebnis: {zahl1} / {zahl2} = {ergebnis}");

            double[] eingaben = { zahl1, zahl2 };
            _historieBearbeitung.BerechnungHinzufuegen(_benutzerEinstellungen, "/", eingaben, ergebnis);

            _datenbankBerechnungen.BerechnungInDatenbankSpeichern("/", eingaben, ergebnis);
        }
    }

    /// <summary>
    /// Dividiert mindestens zwei Zahlen durcheinander
    /// </summary>
    public void MehrfachDividieren()
    {
        _help.Write("\n=== MEHRFACH-DIVISION ===");
        _help.Write("Gib mehrere Zahlen ein (beende mit 'fertig'):");

        List<double> zahlen = new List<double>();

        while (true)
        {
            _help.Write($"Zahl {zahlen.Count + 1}: ");
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
            _help.Write("Mindestens zwei Zahlen erforderlich!");
            return;
        }

        double ergebnis = zahlen[0];

        for (int i = 1; i < zahlen.Count; i++)
        {
            if (zahlen[i] == 0)
            {
                _help.Write("Fehler: Division durch Null ist nicht möglich!");
                return;
            }
            ergebnis /= zahlen[i];
        }

        string berechnung = $"{string.Join(" / ", zahlen)} = {ergebnis}";
        _help.Write($"Ergebnis: {berechnung}");

        _historieBearbeitung.BerechnungHinzufuegen(_benutzerEinstellungen, "/", zahlen.ToArray(), ergebnis);

        _datenbankBerechnungen.BerechnungInDatenbankSpeichern("/", zahlen.ToArray(), ergebnis);
    }
}

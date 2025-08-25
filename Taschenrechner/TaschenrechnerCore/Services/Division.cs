using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class Division
{
    static Hilfsfunktionen help = new Hilfsfunktionen();
    static HistorienBearbeitung historieB = new HistorienBearbeitung();
    static DatenbankBerechnungen datenbankB = new DatenbankBerechnungen();

    /// <summary>
    /// Division zweier Zahlen
    /// </summary>
    public void Dividieren()
    {
        double zahl1 = help.ZahlEinlesen("Gib die erste Zahl ein: ");
        double zahl2 = help.ZahlEinlesen("Gib die zweite Zahl ein: ");

        if (zahl2 == 0)
        {
            help.Write("Fehler: Division durch Null ist nicht möglich!");
        }
        else
        {
            double ergebnis = zahl1 / zahl2;
            help.Write($"Ergebnis: {zahl1} / {zahl2} = {ergebnis}");

            double[] eingaben = { zahl1, zahl2 };
            historieB.BerechnungHinzufuegen("/", eingaben, ergebnis);

            datenbankB.BerechnungInDatenbankSpeichern("/", eingaben, ergebnis);
        }
    }

    /// <summary>
    /// Dividiert mindestens zwei Zahlen durcheinander
    /// </summary>
    public void MehrfachDividieren()
    {
        help.Write("\n=== MEHRFACH-DIVISION ===");
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

        double ergebnis = zahlen[0];

        for (int i = 1; i < zahlen.Count; i++)
        {
            if (zahlen[i] == 0)
            {
                help.Write("Fehler: Division durch Null ist nicht möglich!");
                return;
            }
            ergebnis /= zahlen[i];
        }

        string berechnung = $"{string.Join(" / ", zahlen)} = {ergebnis}";
        help.Write($"Ergebnis: {berechnung}");

        historieB.BerechnungHinzufuegen("/", zahlen.ToArray(), ergebnis);

        datenbankB.BerechnungInDatenbankSpeichern("/", zahlen.ToArray(), ergebnis);
    }
}

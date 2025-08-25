namespace TaschenrechnerCore.Services;

public class Subtraktion
{
    /// <summary>
    /// Subtraktion zweier Zahlen
    /// </summary>
    public void Subtrahieren()
    {
        double zahl1 = help.ZahlEinlesen("Gib die erste Zahl ein: ");
        double zahl2 = help.ZahlEinlesen("Gib die zweite Zahl ein: ");

        double ergebnis = zahl1 - zahl2;
        help.Write($"Ergebnis: {zahl1} - {zahl2} = {ergebnis}");

        string berechnung = $"{zahl1} - {zahl2} = {ergebnis}";
        historiemenu.HistorieHinzufuegen(berechnung);

        double[] eingaben = { zahl1, zahl2 };
        historiemenu.BerechnungHinzufuegen("-", eingaben, ergebnis);

        datenbankmenu.BerechnungInDatenbankSpeichern("-", eingaben, ergebnis);
    }

    /// <summary>
    /// Subtrahiert mindestens zwei Zahlen voneinander
    /// </summary>
    public void MehrfachSubtrahieren()
    {
        help.Write("\n=== MEHRFACH-SUBTRAKTION ===");
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
            ergebnis -= zahlen[i];
        }

        string berechnung = $"{string.Join(" - ", zahlen)} = {ergebnis}";
        help.Write($"Ergebnis: {berechnung}");

        historiemenu.BerechnungHinzufuegen("-", zahlen.ToArray(), ergebnis);

        datenbankmenu.BerechnungInDatenbankSpeichern("-", zahlen.ToArray(), ergebnis);

        historiemenu.HistorieHinzufuegen(berechnung);
    }
}

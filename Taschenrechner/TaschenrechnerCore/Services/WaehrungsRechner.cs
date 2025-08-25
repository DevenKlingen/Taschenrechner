namespace TaschenrechnerCore.Services;

public class WaehrungsRechner
{
    /// <summary>
    /// Rechnet Euro in Dollar mit einem Wechselkurs von 1.1 um
    /// </summary>
    public void WaehrungsRechnung()
    {
        help.Write("\n=== WÄHRUNGSRECHNER ===");
        help.Write("Betrag in Euro: ");
        bool umgerechnet = false;
        while (!umgerechnet)
        {
            if (decimal.TryParse(Console.ReadLine(), out decimal euro))
            {
                Console.OutputEncoding = Encoding.UTF8;
                decimal dollar = euro * 1.1m; // Beispiel-Wechselkurs
                help.Write($"{euro}€ = ${dollar}");

                string berechnung = $"{euro} + {1.1m} = {dollar}";
                historiemenu.HistorieHinzufuegen(berechnung);

                double euroDouble = (double)euro;
                double dollarDouble = (double)dollar;
                double[] eingaben = { euroDouble };
                historiemenu.BerechnungHinzufuegen("$", eingaben, dollarDouble);
                datenbankmenu.BerechnungInDatenbankSpeichern("$", eingaben, dollarDouble);
                umgerechnet = true;
            }
            else
            {
                help.Write("Ungültige Eingabe!");
            }
        }
    }
}

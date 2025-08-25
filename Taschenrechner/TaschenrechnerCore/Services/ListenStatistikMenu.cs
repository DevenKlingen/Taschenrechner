using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class ListenStatistikMenu
{
    static Hilfsfunktionen help = new Hilfsfunktionen();
    static ListenStatistik listS = new ListenStatistik();
    
    /// <summary>
    /// Erstellt eine Statistik für eingegebene Zahlen (Liste)
    /// </summary>
    public void StatsRechnerMenu()
    {
        help.Write("\n=== STATISTIK-RECHNER ===");
        help.Write("Gib mehrere Zahlen ein (beende mit 'fertig'):");

        List<double> zahlen = new List<double>();

        while (true)
        {
            help.Write($"Zahl {zahlen.Count + 1} (oder 'fertig'): ");
            string eingabe = Console.ReadLine();

            if (eingabe.ToLower() == "fertig")
                break;

            if (double.TryParse(eingabe, out double zahl))
            {
                zahlen.Add(zahl);
            }
            else
            {
                help.Write("Ungültige Eingabe!");
            }
        }

        if (zahlen.Count == 0)
        {
            help.Write("Keine Zahlen eingegeben!");
            return;
        }

        // Liste in Array umwandeln für die Statistik-Methoden
        double[] zahlenArray = zahlen.ToArray();

        help.Write($"\nAnzahl Werte: {zahlenArray.Length}");
        help.Write($"Minimum: {listS.FindeMinimum(zahlenArray)}");
        help.Write($"Maximum: {listS.FindeMaximum(zahlenArray)}");
        help.Write($"Summe: {listS.BerechneSumme(zahlenArray)}");
        help.Write($"Durchschnitt: {listS.BerechneDurchschnitt(zahlenArray):F2}");
    }
}
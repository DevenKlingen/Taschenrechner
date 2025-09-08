using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class ListenStatistikMenu
{
    private readonly Hilfsfunktionen _help;
    private readonly ListenStatistik _listStatistiken;
    
    public ListenStatistikMenu(
        Hilfsfunktionen help,
        ListenStatistik listenStatistik)
    {
        _help = help;
        _listStatistiken = listenStatistik;
    }

    /// <summary>
    /// Erstellt eine Statistik für eingegebene Zahlen (Liste)
    /// </summary>
    public void StatsRechnerMenu()
    {
        _help.Write("\n=== STATISTIK-RECHNER ===");
        _help.Write("Gib mehrere Zahlen ein (beende mit 'fertig'):");

        List<double> zahlen = new List<double>();

        while (true)
        {
            string eingabe = _help.Einlesen($"Zahl {zahlen.Count + 1} (oder 'fertig'): ");

            if (eingabe.ToLower() == "fertig")
                break;

            if (double.TryParse(eingabe, out double zahl))
            {
                zahlen.Add(zahl);
            }
            else
            {
                _help.Write("Ungültige Eingabe!");
            }
        }

        if (zahlen.Count == 0)
        {
            _help.Write("Keine Zahlen eingegeben!");
            return;
        }

        // Liste in Array umwandeln für die Statistik-Methoden
        double[] zahlenArray = zahlen.ToArray();

        _help.Write($"\nAnzahl Werte: {zahlenArray.Length}");
        _help.Write($"Minimum: {_listStatistiken.FindeMinimum(zahlenArray)}");
        _help.Write($"Maximum: {_listStatistiken.FindeMaximum(zahlenArray)}");
        _help.Write($"Summe: {_listStatistiken.BerechneSumme(zahlenArray)}");
        _help.Write($"Durchschnitt: {_listStatistiken.BerechneDurchschnitt(zahlenArray):F2}");
    }
}
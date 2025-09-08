using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class ListEinlesen
{
    private readonly Hilfsfunktionen _help;

    public ListEinlesen(Hilfsfunktionen help)
    {
        _help = help; 
    }
    
    /// <summary>
    /// Liest eine Liste ein und prüft, ob die Eingabe valide ist
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public List<int> ZahlenListeEinlesen(string name)
    {
        _help.Write($"\n{name} eingeben (beende mit 'fertig'):");
        List<int> zahlen = new List<int>();

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

        return zahlen;
    }
}
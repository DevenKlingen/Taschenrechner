using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class ListEinlesen
{
    static Hilfsfunktionen help = new Hilfsfunktionen();
    
    /// <summary>
    /// Liest eine Liste ein und prüft, ob die Eingabe valide ist
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public List<int> ZahlenListeEinlesen(string name)
    {
        help.Write($"\n{name} eingeben (beende mit 'fertig'):");
        List<int> zahlen = new List<int>();

        while (true)
        {
            help.Write($"Zahl {zahlen.Count + 1}: ");
            string eingabe = Console.ReadLine();

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
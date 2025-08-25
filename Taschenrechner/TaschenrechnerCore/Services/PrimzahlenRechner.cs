using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class PrimzahlenRechner
{
    static Hilfsfunktionen help = new Hilfsfunktionen();
    
    /// <summary>
    /// Berechnet alle Primzahlen bis n, wobei n vom Nutzer festgelegt wird
    /// </summary>
    public void PrimzahlenErmitteln()
    {
        help.Write("Gib eine Zahl ein: ");
        string eingabe = Console.ReadLine();

        long.TryParse(eingabe, out long zahl);
        List<long> zahlen = new List<long>();

        for (int i = 0; i < zahl; i++)
        {
            if (i < 2) // 0 und 1 sind keine Primzahlen
            {
                if (MathUtils.IstPrimzahl((int)i))
                {
                    zahlen.Add(i);
                }
            }
        }

        help.Write("Primzahlen bis " + zahl + ": " + string.Join(", ", zahlen));
    }
}
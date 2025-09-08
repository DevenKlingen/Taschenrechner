using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class PrimzahlenRechner
{
    private readonly Hilfsfunktionen _help;
    
    public PrimzahlenRechner(Hilfsfunktionen help)
    {
        _help = help;
    }

    /// <summary>
    /// Berechnet alle Primzahlen bis n, wobei n vom Nutzer festgelegt wird
    /// </summary>
    public void PrimzahlenErmitteln()
    {
        string eingabe = _help.Einlesen("Gib eine Zahl ein: ");

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

        _help.Write("Primzahlen bis " + zahl + ": " + string.Join(", ", zahlen));
    }
}
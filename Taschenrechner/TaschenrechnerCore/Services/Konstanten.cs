using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class Konstanten
{
    static Hilfsfunktionen help = new Hilfsfunktionen();
    static Dictionary<string, double> konstanten = new Dictionary<string, double>
    {
        ["pi"] = Math.PI,
        ["e"] = Math.E,
        ["phi"] = 1.618033988749,
        ["wurzel2"] = Math.Sqrt(2)
    };

    /// <summary>
    /// Durchsucht das Dictionary nach einer Konstanten
    /// </summary>
    public void Suche()
    {
        help.Write("Wonach willst du suchen? ");
        string input = Console.ReadLine();

        if (konstanten.ContainsKey(input) && input != null)
        {
            help.Write(konstanten[input].ToString());
        }
    }
}
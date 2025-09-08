using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class Konstanten
{
    private readonly Hilfsfunktionen _help;
    static Dictionary<string, double> konstanten = new Dictionary<string, double>
    {
        ["pi"] = Math.PI,
        ["e"] = Math.E,
        ["phi"] = 1.618033988749,
        ["wurzel2"] = Math.Sqrt(2)
    };

    public Konstanten(Hilfsfunktionen help)
    {
        _help = help;
    }

    /// <summary>
    /// Durchsucht das Dictionary nach einer Konstanten
    /// </summary>
    public void Suche()
    {
        string input = _help.Einlesen("Wonach willst du suchen? ");

        if (konstanten.ContainsKey(input) && input != null)
        {
            _help.Write(konstanten[input].ToString());
        }
    }
}
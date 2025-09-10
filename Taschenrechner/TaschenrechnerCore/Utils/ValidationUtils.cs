using TaschenrechnerCore.Interfaces;

namespace TaschenrechnerCore.Utils;

// Static Klasse für Eingabe-Validierung
public class ValidationUtils : IValidationService
{
    public static bool IstGueltigeZahl(string eingabe, out double zahl)
    {
        return double.TryParse(eingabe, out zahl);
    }

    public static bool IstImBereich(double zahl, double min, double max)
    {
        return zahl >= min && zahl <= max;
    }

    public static bool IstPositiv(double zahl)
    {
        return zahl > 0;
    }

    public static bool IstGanzzahl(double zahl)
    {
        return zahl == Math.Floor(zahl);
    }
    public bool isKonstante(string eingabe)
    {
        bool konst = false;
        string konstante = eingabe.ToLower();
        if (konstante == "pi" || konstante == "phi" || konstante == "e" || konstante == "wurzel2")
            konst = true;
        return konst;
    }
}
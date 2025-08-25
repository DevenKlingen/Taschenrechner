namespace TaschenrechnerCore.Utils;

// Static Klasse für Eingabe-Validierung
public static class ValidationUtils
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
}
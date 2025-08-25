namespace TaschenrechnerCore.Utils;

// Static Klasse für mathematische Hilfsfunktionen
public static class MathUtils
{
    // Static readonly Konstanten
    public static readonly double PI = Math.PI;
    public static readonly double E = Math.E;
    public static readonly double PHI = 1.6180339887; // Goldener Schnitt
    public static readonly double WURZEL_2 = Math.Sqrt(2);

    // Static Methoden
    public static double GradZuRadiant(double grad)
    {
        return grad * PI / 180.0;
    }

    public static double RadiantZuGrad(double radiant)
    {
        return radiant * 180.0 / PI;
    }

    public static long Fakultaet(int n)
    {
        if (n < 0)
            throw new ArgumentException("Fakultät nur für nicht-negative Zahlen!");

        if (n <= 1)
            return 1;

        long ergebnis = 1;
        for (int i = 2; i <= n; i++)
        {
            ergebnis *= i;
        }
        return ergebnis;
    }

    public static List<long> FibonacciSequenz(int anzahl)
    {
        if (anzahl <= 0)
            return new List<long>();

        var fibonacci = new List<long>();

        if (anzahl >= 1) fibonacci.Add(0);
        if (anzahl >= 2) fibonacci.Add(1);

        for (int i = 2; i < anzahl; i++)
        {
            fibonacci.Add(fibonacci[i - 1] + fibonacci[i - 2]);
        }

        return fibonacci;
    }

    public static bool IstPrimzahl(int zahl)
    {
        if (zahl <= 1) return false;
        if (zahl == 2) return true;
        if (zahl % 2 == 0) return false;

        for (int i = 3; i <= Math.Sqrt(zahl); i += 2)
        {
            if (zahl % i == 0)
                return false;
        }

        return true;
    }

    public static double BerechneDurchschnitt(params double[] werte)
    {
        if (werte == null || werte.Length == 0)
            throw new ArgumentException("Mindestens ein Wert erforderlich!");

        return werte.Average();
    }

    public static double BerechneStandardabweichung(params double[] werte)
    {
        if (werte == null || werte.Length < 2)
            throw new ArgumentException("Mindestens zwei Werte erforderlich!");

        double durchschnitt = werte.Average();
        double summeQuadrate = werte.Sum(x => Math.Pow(x - durchschnitt, 2));

        return Math.Sqrt(summeQuadrate / (werte.Length - 1));
    }
}
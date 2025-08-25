namespace TaschenrechnerCore.Services;

public class ListenStatistik
{
    /// <summary>
    /// Sucht das Minimum aus einem Array von Zahlen
    /// </summary>
    /// <param name="zahlen"></param>
    /// <returns></returns>
    public double FindeMinimum(double[] zahlen)
    {
        double min = 0;
        if (zahlen.Length > 0)
        {
            min = zahlen[0];
            int n = 0;
            while (n < zahlen.Length)
            {
                if (zahlen[n] < min)
                {
                    min = zahlen[n];
                }
                n++;
            }
        }

        return min;
    }

    /// <summary>
    /// Sucht das Maximum aus einem Array von Zahlen
    /// </summary>
    /// <param name="zahlen"></param>
    /// <returns></returns>
    public double FindeMaximum(double[] zahlen)
    {
        double max = 0;
        if (zahlen.Length > 0)
        {
            max = zahlen[0];
            int n = 0;
            while (n < zahlen.Length)
            {
                if (zahlen[n] > max)
                {
                    max = zahlen[n];
                }
                n++;
            }
        }

        return max;
    }

    /// <summary>
    /// Berechnet die Summe eines Arrays von Zahlen
    /// </summary>
    /// <param name="zahlen"></param>
    /// <returns></returns>
    public double BerechneSumme(double[] zahlen)
    {
        double summe = 0;

        foreach (double zahl in zahlen)
        {
            summe += zahl;
        }

        return summe;
    }

    /// <summary>
    /// Berechnet den Durchschnitt eines Arrays von Zahlen
    /// </summary>
    /// <param name="zahlen"></param>
    /// <returns></returns>
    public double BerechneDurchschnitt(double[] zahlen)
    {
        return BerechneSumme(zahlen) / zahlen.Length;
    }
}
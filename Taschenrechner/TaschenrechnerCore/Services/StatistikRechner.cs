using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class StatistikRechner : BaseRechner
{
    public StatistikRechner() : base("Statistik-Rechner")
    {
    }

    public override double Berechnen(string operation, params double[] werte)
    {
        if (!ValidiereEingaben(werte, 1))
            throw new ArgumentException("Mindestens ein Wert erforderlich!");

        double ergebnis;

        switch (operation.ToLower())
        {
            case "mittelwert":
            case "durchschnitt":
                ergebnis = MathUtils.BerechneDurchschnitt(werte);
                break;

            case "median":
                ergebnis = BerechneMedian(werte);
                break;

            case "standardabweichung":
                ergebnis = MathUtils.BerechneStandardabweichung(werte);
                break;

            case "varianz":
                double stdabw = MathUtils.BerechneStandardabweichung(werte);
                ergebnis = stdabw * stdabw;
                break;

            case "min":
                ergebnis = werte.Min();
                break;

            case "max":
                ergebnis = werte.Max();
                break;

            case "spannweite":
                ergebnis = werte.Max() - werte.Min();
                break;

            default:
                throw new NotSupportedException($"Statistik-Operation '{operation}' nicht unterstützt.");
        }

        BerechnungSpeichern(operation, werte, ergebnis);
        return ergebnis;
    }

    private double BerechneMedian(double[] werte)
    {
        var sortiert = werte.OrderBy(x => x).ToArray();
        int n = sortiert.Length;

        if (n % 2 == 0)
        {
            return (sortiert[n / 2 - 1] + sortiert[n / 2]) / 2.0;
        }
        else
        {
            return sortiert[n / 2];
        }
    }

    public void VollstaendigeStatistik(double[] werte)
    {
        Console.WriteLine("=== VOLLSTÄNDIGE STATISTIK ===");
        Console.WriteLine($"Anzahl Werte: {werte.Length}");
        Console.WriteLine($"Mittelwert: {Berechnen("mittelwert", werte):F2}");
        Console.WriteLine($"Median: {Berechnen("median", werte):F2}");
        Console.WriteLine($"Minimum: {Berechnen("min", werte):F2}");
        Console.WriteLine($"Maximum: {Berechnen("max", werte):F2}");
        Console.WriteLine($"Spannweite: {Berechnen("spannweite", werte):F2}");
        Console.WriteLine($"Standardabweichung: {Berechnen("standardabweichung", werte):F2}");
        Console.WriteLine($"Varianz: {Berechnen("varianz", werte):F2}");
    }
}
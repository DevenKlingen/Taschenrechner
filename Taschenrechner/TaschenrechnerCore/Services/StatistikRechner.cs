using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class StatistikRechner : BaseRechner, IRechner
{
    private readonly Hilfsfunktionen _help;

    public StatistikRechner(BenutzerManagement benutzerManagement, DatenbankBerechnungen datenbankBerechnungen, Hilfsfunktionen hilfsfunktionen)
        : base(benutzerManagement, datenbankBerechnungen, "Statistik-Rechner")
    {
        _help = hilfsfunktionen; // Speichere die Instanz der Hilfsfunktionen
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

        List<double> werteliste = new List<double>();
        foreach (var entry in werte)
        {
            werteliste.Add(entry);
        }

        BerechnungSpeichern(operation, werteliste, ergebnis);
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
        _help.WriteInfo("\n=== VOLLSTÄNDIGE STATISTIK ===");
        _help.WriteInfo($"Anzahl Werte: {werte.Length}");
        _help.WriteInfo($"Mittelwert: {Berechnen("mittelwert", werte):F2}");
        _help.WriteInfo($"Median: {Berechnen("median", werte):F2}");
        _help.WriteInfo($"Minimum: {Berechnen("min", werte):F2}");
        _help.WriteInfo($"Maximum: {Berechnen("max", werte):F2}");
        _help.WriteInfo($"Spannweite: {Berechnen("spannweite", werte):F2}");
        _help.WriteInfo($"Standardabweichung: {Berechnen("standardabweichung", werte):F2}");
        _help.WriteInfo($"Varianz: {Berechnen("varianz", werte):F2}");
    }
}
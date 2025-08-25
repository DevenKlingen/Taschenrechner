using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class WissenschaftlicherRechner : BaseRechner
{
    public WissenschaftlicherRechner() : base("Wissenschaftlich")
    {
    }

    public override double Berechnen(string operation, params double[] werte)
    {
        double ergebnis;

        switch (operation.ToLower())
        {
            // Basis-Operationen (mit mehr als 2 Werten)
            case "+":
                ergebnis = werte.Sum();
                break;

            case "*":
                ergebnis = werte.Aggregate(1.0, (acc, val) => acc * val);
                break;

            case "-":
                if (!ValidiereEingaben(werte, 2))
                    throw new ArgumentException("Subtraktion benötigt mindestens 2 Werte");
                ergebnis = werte[0] - werte.Skip(1).Sum();
                break;

            case "/":
                if (!ValidiereEingaben(werte, 2))
                    throw new ArgumentException("Division benötigt mindestens 2 Werte");
                if (werte.Skip(1).Any(v => v == 0))
                    throw new DivideByZeroException("Division durch Null nicht möglich!");
                ergebnis = werte[0] / werte.Skip(1).Aggregate(1.0, (acc, val) => acc * val);
                break;

            // Wissenschaftliche Funktionen
            case "sin":
                if (!ValidiereEingaben(werte, 1))
                    throw new ArgumentException("Sin benötigt 1 Wert");
                ergebnis = Math.Sin(werte[0]);
                break;

            case "cos":
                if (!ValidiereEingaben(werte, 1))
                    throw new ArgumentException("Cos benötigt 1 Wert");
                ergebnis = Math.Cos(werte[0]);
                break;

            case "sqrt":
            case "wurzel":
                if (!ValidiereEingaben(werte, 1))
                    throw new ArgumentException("Wurzel benötigt 1 Wert");
                if (werte[0] < 0)
                    throw new ArgumentException("Wurzel aus negativer Zahl nicht möglich!");
                ergebnis = Math.Sqrt(werte[0]);
                break;

            case "pow":
            case "potenz":
                if (!ValidiereEingaben(werte, 2))
                    throw new ArgumentException("Potenz benötigt 2 Werte");
                ergebnis = Math.Pow(werte[0], werte[1]);
                break;

            case "log":
                if (!ValidiereEingaben(werte, 1))
                    throw new ArgumentException("Log benötigt 1 Wert");
                if (werte[0] <= 0)
                    throw new ArgumentException("Logarithmus nur für positive Zahlen!");
                ergebnis = Math.Log10(werte[0]);
                break;

            case "ln":
                if (!ValidiereEingaben(werte, 1))
                    throw new ArgumentException("Ln benötigt 1 Wert");
                if (werte[0] <= 0)
                    throw new ArgumentException("Natürlicher Logarithmus nur für positive Zahlen!");
                ergebnis = Math.Log(werte[0]);
                break;

            default:
                // Fallback auf Basis-Rechner für einfache Operationen
                var basisRechner = new BasisRechner();
                return basisRechner.Berechnen(operation, werte);
        }

        BerechnungSpeichern(operation, werte, ergebnis);
        return ergebnis;
    }

    // Überschriebene Historie-Anzeige mit mehr Details
    public override void HistorieAnzeigen()
    {
        Console.WriteLine($"=== WISSENSCHAFTLICHE HISTORIE ===");

        if (historie.Count == 0)
        {
            Console.WriteLine("Keine Berechnungen vorhanden.");
            return;
        }

        var wissenschaftlicheFunktionen = historie
           .Where(h =>
           {
               // Prüfen, ob es sich um eine Grundrechenart handelt
               if (h.Operation == "+" || h.Operation == "-" || h.Operation == "*" || h.Operation == "/")
               {
                   // Zusätzliche Bedingung: Mehr als 2 Eingaben
                   return h.Eingaben.Count() > 2;
               }

               // Für andere Operationen nur die wissenschaftliche Prüfung
               return IsWissenschaftlicheFunktion(h.Operation);
           })
   .TakeLast(10);

        Console.WriteLine("Wissenschaftliche Funktionen:");
        foreach (var berechnung in wissenschaftlicheFunktionen)
        {
            string eingabenStr = berechnung.Operation.ToUpper() + "(" +
                               string.Join(", ", berechnung.Eingaben) + ")";
            Console.WriteLine($"[{berechnung.Zeitstempel:HH:mm:ss}] " +
                            $"{eingabenStr} = {berechnung.Ergebnis:F4}");
        }
    }

    private bool IsWissenschaftlicheFunktion(string operation)
    {
        string[] wissenschaftlich = { "sin", "cos", "sqrt", "wurzel", "pow", "potenz", "log", "ln" };
        return wissenschaftlich.Contains(operation.ToLower());
    }

    public double BerechneMitKonstante(string konstante)
    {
        switch (konstante.ToLower())
        {
            case "pi":
                return MathUtils.PI;
            case "e":
                return MathUtils.E;
            case "phi":
                return MathUtils.PHI;
            case "wurzel2":
                return MathUtils.WURZEL_2;
            default:
                throw new ArgumentException($"Unbekannte Konstante: {konstante}");
        }
    }

    public long BerechneFakultaet(int n)
    {
        long ergebnis = MathUtils.Fakultaet(n);
        BerechnungSpeichern("fakultät", new double[] { n }, ergebnis);
        return ergebnis;
    }

    public List<long> GeneriereFibonacci(int anzahl)
    {
        var fibonacci = MathUtils.FibonacciSequenz(anzahl);
        Console.WriteLine($"Fibonacci({anzahl}): [{string.Join(", ", fibonacci)}]");
        return fibonacci;
    }
}
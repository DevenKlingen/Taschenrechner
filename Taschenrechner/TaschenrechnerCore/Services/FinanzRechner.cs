using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class FinanzRechner : BaseRechner, IRechner
{
    Hilfsfunktionen _help;
    public FinanzRechner(BenutzerManagement benutzerManagement, DatenbankBerechnungen datenbankBerechnungen)
        : base(benutzerManagement, datenbankBerechnungen, "Finanz-Rechner")
    {
        _help = benutzerManagement.getHelp();
    }

    public override double Berechnen(string operation, params double[] werte)
    {
        double ergebnis;

        switch (operation.ToLower())
        {
            case "zinsen":
                // Einfache Zinsen: Kapital * Zinssatz * Zeit
                if (!ValidiereEingaben(werte, 3))
                    throw new ArgumentException("Zinsen: Kapital, Zinssatz(%), Zeit(Jahre)");
                ergebnis = werte[0] * (werte[1] / 100.0) * werte[2];
                break;

            case "zinseszinsen":
                // Zinseszinsen: Kapital * (1 + Zinssatz)^Zeit
                if (!ValidiereEingaben(werte, 3))
                    throw new ArgumentException("Zinseszinsen: Kapital, Zinssatz(%), Zeit(Jahre)");
                ergebnis = werte[0] * Math.Pow(1 + (werte[1] / 100.0), werte[2]);
                break;

            case "annuitaet":
                // Annuität: Kredit * (Zinssatz * (1+Zinssatz)^Laufzeit) / ((1+Zinssatz)^Laufzeit - 1)
                if (!ValidiereEingaben(werte, 3))
                    throw new ArgumentException("Annuität: Kreditsumme, Zinssatz(%), Laufzeit(Jahre)");

                double kreditsumme = werte[0];
                double zinssatz = werte[1] / 100.0;
                double laufzeit = werte[2];

                double faktor = Math.Pow(1 + zinssatz, laufzeit);
                ergebnis = kreditsumme * (zinssatz * faktor) / (faktor - 1);
                break;

            case "barwert":
                // Barwert: Zukunftswert / (1 + Zinssatz)^Zeit
                if (!ValidiereEingaben(werte, 3))
                    throw new ArgumentException("Barwert: Zukunftswert, Zinssatz(%), Zeit(Jahre)");
                ergebnis = werte[0] / Math.Pow(1 + (werte[1] / 100.0), werte[2]);
                break;

            case "KreditPlanErstellen":
                // Kreditplan erstellen ist keine Berechnung, sondern eine Ausgabe
                if (!ValidiereEingaben(werte, 3))
                    throw new ArgumentException("Kreditplan: Kreditsumme, Zinssatz(%), Laufzeit(Jahre)");
                KreditPlanErstellen(werte[0], werte[1], (int)werte[2]);
                return 0; // Keine Rückgabe für diese Methode
            default:
                throw new NotSupportedException($"Finanz-Operation '{operation}' nicht unterstützt.");
        }

        List<double> werteliste = new List<double>();
        foreach (var entry in werte)
        {
            werteliste.Add(entry);
        }

        BerechnungSpeichern(operation, werteliste, ergebnis);
        return ergebnis;
    }

    // Spezielle Methoden für Finanzberechnungen
    public void KreditPlanErstellen(double kreditsumme, double zinssatz, int laufzeitJahre)
    {
        _help.WriteInfo("\n=== TILGUNGSPLAN ===");
        _help.WriteInfo($"Kreditsumme: {kreditsumme:C}");
        _help.WriteInfo($"Zinssatz: {zinssatz:F2}%");
        _help.WriteInfo($"Laufzeit: {laufzeitJahre} Jahre\n");

        double annuitaet = Berechnen("annuitaet", kreditsumme, zinssatz, laufzeitJahre);
        double restschuld = kreditsumme;

        _help.WriteInfo($"Jährliche Annuität: {annuitaet:C}\n");
        _help.WriteInfo("Jahr\tZinsen\t\tTilgung\t\tRestschuld");
        _help.WriteInfo(new string('-', 50));

        for (int jahr = 1; jahr <= laufzeitJahre; jahr++)
        {
            double zinsenJahr = restschuld * (zinssatz / 100.0);
            double tilgungJahr = annuitaet - zinsenJahr;
            restschuld -= tilgungJahr;

            _help.WriteInfo($"{jahr}\t{zinsenJahr:C}\t{tilgungJahr:C}\t{Math.Max(0, restschuld):C}");
        }
    }
}
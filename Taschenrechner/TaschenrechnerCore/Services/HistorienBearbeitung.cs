using TaschenrechnerConsole;
using TaschenrechnerCore.Models;

namespace TaschenrechnerCore.Services;

public class HistorienBearbeitung
{
    static Program program = new Program();

    /// <summary>
    /// Fügt eine Berechnung zur Historie hinzu
    /// </summary>
    /// <param name="berechnung"></param>
    public void HistorieHinzufuegen(string berechnung)
    {
        string timestamp = DateTime.Now.ToString("HH:mm:ss");
        program.berechnungsHistorie.Add($"[{timestamp}] {berechnung}");
    }

    /// <summary>
    /// Fügt eine neue Berechnung zur Historie hinzu und aktualisiert die detaillierte Liste
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="eingaben"></param>
    /// <param name="ergebnis"></param>
    /// <param name="kommentar"></param>
    public void BerechnungHinzufuegen(string operation, double[] eingaben, double ergebnis, string kommentar = "")
    {
        var berechnung = new Berechnung
        {
            Zeitstempel = DateTime.Now,
            Operation = operation,
            Eingaben = eingaben.ToList(),
            Ergebnis = ergebnis,
            Kommentar = kommentar
        };

        program.detaillierteBerechnungen.Add(berechnung);

        // Alte string-basierte Historie auch aktualisieren
        HistorieHinzufuegen(berechnung.ToString());
    }
}
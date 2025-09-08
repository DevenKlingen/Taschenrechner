using TaschenrechnerCore.Models;

namespace TaschenrechnerCore.Services;

public class HistorienBearbeitung
{
    private readonly HistorieVerwaltung _historieVerwaltung;

    public HistorienBearbeitung(HistorieVerwaltung historieVerwaltung)
    {
        _historieVerwaltung = historieVerwaltung;
    }

    /// <summary>
    /// Fügt eine Berechnung zur Historie hinzu
    /// </summary>
    /// <param name="berechnung"></param>
    public void HistorieHinzufuegen(string berechnung)
    {
        string timestamp = DateTime.Now.ToString("HH:mm:ss");
        _historieVerwaltung._berechnungsHistorie.Add($"[{timestamp}] {berechnung}");
    }

    /// <summary>
    /// Fügt eine neue Berechnung zur Historie hinzu und aktualisiert die detaillierte Liste
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="eingaben"></param>
    /// <param name="ergebnis"></param>
    /// <param name="kommentar"></param>
    public void BerechnungHinzufuegen(BenutzerEinstellungen benutzerEinstellungen, string operation, double[] eingaben, double ergebnis, string kommentar = "")
    {
        var berechnung = new Berechnung
        {
            _benutzerEinstellungen = benutzerEinstellungen,
            Zeitstempel = DateTime.Now,
            Operation = operation,
            Eingaben = eingaben.ToList(),
            Ergebnis = ergebnis,
            Kommentar = kommentar,
        };

        _historieVerwaltung._detaillierteBerechnungen.Add(berechnung);

        // Alte string-basierte Historie auch aktualisieren
        HistorieHinzufuegen(berechnung.ToString());
    }
}
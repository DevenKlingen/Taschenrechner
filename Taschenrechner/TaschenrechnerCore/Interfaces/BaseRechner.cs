namespace TaschenrechnerCore.Interfaces;


using TaschenrechnerCore.Models;
using TaschenrechnerCore.Services;

public abstract class BaseRechner
{
    static DatenbankBerechnungen datenbankmenu = new();
    // Protected Felder - nur für abgeleitete Klassen sichtbar
    protected List<BerechnungErgebnis> historie;
    protected string rechnerTyp;

    // Private Felder - nur in dieser Klasse sichtbar
    private int maxHistorieGroesse;

    // Public Properties - von außen zugreifbar
    public string RechnerTyp
    {
        get { return rechnerTyp; }
    }

    public int AnzahlBerechnungen
    {
        get { return historie.Count; }
    }

    // Konstruktor
    protected BaseRechner(string typ, int maxHistorie = 100)
    {
        rechnerTyp = typ;
        maxHistorieGroesse = maxHistorie;
        historie = new List<BerechnungErgebnis>();
    }

    // Abstrakte Methode - muss in abgeleiteten Klassen implementiert werden
    public abstract double Berechnen(string operation, params double[] werte);

    // Virtuelle Methode - kann in abgeleiteten Klassen überschrieben werden
    public virtual void BerechnungSpeichern(string operation, double[] eingaben, double ergebnis)
    {
        Benutzer akt = program.getAktBenutzer();
        var berechnungErgebnis = new BerechnungErgebnis
        {
            Zeitstempel = DateTime.Now,
            Operation = operation,
            Eingaben = eingaben,
            Ergebnis = ergebnis,
            RechnerTyp = rechnerTyp
        };

        historie.Add(berechnungErgebnis);

        // Historie-Größe begrenzen (private Methode)
        HistorieBegrenzen();

        // In Datenbank speichern (falls Benutzer angemeldet)
        if (akt != null)
        {
            datenbankmenu.BerechnungInDatenbankSpeichern(operation, eingaben, ergebnis, "", rechnerTyp);
        }
    }

    // Private Hilfsmethode
    private void HistorieBegrenzen()
    {
        while (historie.Count > maxHistorieGroesse)
        {
            historie.RemoveAt(0); // Älteste Einträge entfernen
        }
    }

    // Public Methode für Historie-Anzeige
    public virtual void HistorieAnzeigen()
    {
        Console.WriteLine($"=== HISTORIE ({rechnerTyp}) ===");

        if (historie.Count == 0)
        {
            Console.WriteLine("Keine Berechnungen vorhanden.");
            return;
        }

        foreach (var berechnung in historie.TakeLast(10)) // Nur letzte 10
        {
            Console.WriteLine($"[{berechnung.Zeitstempel:HH:mm:ss}] " +
                            $"{string.Join($" {berechnung.Operation} ", berechnung.Eingaben)} = " +
                            $"{berechnung.Ergebnis:F2}");
        }
    }

    // Protected Methode für Eingabe-Validierung
    protected bool ValidiereEingaben(double[] werte, int mindestAnzahl)
    {
        if (werte == null || werte.Length < mindestAnzahl)
        {
            Console.WriteLine($"Mindestens {mindestAnzahl} Werte erforderlich!");
            return false;
        }

        return true;
    }
}
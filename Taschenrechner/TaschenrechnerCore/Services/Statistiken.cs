using TaschenrechnerCore.Utils;
using TaschenrechnerCore.Models;

namespace TaschenrechnerCore.Services;

public class Statistiken
{
    private readonly BenutzerManagement _benutzerManagement;
    private readonly Hilfsfunktionen _help;

    public Statistiken(
        BenutzerManagement benutzerManagement, 
        Hilfsfunktionen help)
    {
        _benutzerManagement = benutzerManagement;
        _help = help;
    }

    public void BenutzerStatistiken()
    {
        Benutzer aktuellerBenutzer = _benutzerManagement.getBenutzer();
        Benutzer akt = aktuellerBenutzer;
        
        if (aktuellerBenutzer == null)
        {
            _help.WriteWarning("Kein Benutzer angemeldet!");
            return;
        }

        using var context = new TaschenrechnerContext();

        var berechnungen = context.Berechnungen
            .Where(b => b.BenutzerId == akt.Id)
            .ToList();

        if (!berechnungen.Any())
        {
            _help.WriteWarning("Keine Berechnungen vorhanden für Statistiken.");
            return;
        }

        _help.WriteInfo("\n=== BENUTZER-STATISTIKEN ===");
        _help.WriteInfo($"Benutzer: {akt.Name}");
        _help.WriteInfo($"Mitglied seit: {akt.ErstelltAm:dd.MM.yyyy}");
        _help.WriteInfo($"Gesamt-Berechnungen: {berechnungen.Count}");

        // Operationen-Statistik
        var operationenStats = berechnungen
            .GroupBy(b => b.Operation)
            .Select(g => new { Operation = g.Key, Anzahl = g.Count() })
            .OrderByDescending(x => x.Anzahl)
            .ToList();

        _help.WriteInfo("\nHäufigste Operationen:");
        foreach (var stat in operationenStats)
        {
            _help.WriteInfo($"  {stat.Operation}: {stat.Anzahl} mal");
        }

        // Rechnertyp-Statistik
        var rechnerStats = berechnungen
            .GroupBy(b => b.Rechnertyp)
            .Select(g => new { Typ = g.Key, Anzahl = g.Count() })
            .OrderByDescending(x => x.Anzahl)
            .ToList();

        _help.WriteInfo("\nVerwendete Rechnertypen:");
        foreach (var stat in rechnerStats)
        {
            _help.WriteInfo($"  {stat.Typ}: {stat.Anzahl} mal");
        }

        // Tages-Aktivität (letzte 7 Tage)
        var heute = DateTime.Today;
        var vor7Tagen = heute.AddDays(-7);

        var tagesStats = berechnungen
            .Where(b => b.Zeitstempel >= vor7Tagen)
            .GroupBy(b => b.Zeitstempel.Date)
            .Select(g => new { Datum = g.Key, Anzahl = g.Count() })
            .OrderBy(x => x.Datum)
            .ToList();

        _help.WriteInfo("\nAktivität (letzte 7 Tage):");
        for (int i = 6; i >= 0; i--)
        {
            var datum = heute.AddDays(-i);
            var stat = tagesStats.FirstOrDefault(t => t.Datum == datum);
            int anzahl = stat?.Anzahl ?? 0;
            _help.WriteInfo($"  {datum:dd.MM.yyyy}: {anzahl} Berechnungen");
        }

        // Durchschnitte und Extreme
        var ergebnisse = berechnungen.Select(b => b.Ergebnis).ToList();
        if (ergebnisse.Any())
        {
            _help.WriteInfo("\nErgebnis-Statistiken:");
            _help.WriteInfo($"  Kleinster Wert: {ergebnisse.Min():F2}");
            _help.WriteInfo($"  Größter Wert: {ergebnisse.Max():F2}");
            _help.WriteInfo($"  Durchschnitt: {ergebnisse.Average():F2}");
        }

        // Berechnung der letzten 30 Tage
        var vor30Tagen = heute.AddDays(-30);
        if (vor30Tagen < akt.ErstelltAm)
        {
            vor30Tagen = akt.ErstelltAm;
        }

        // Liste aller Tage der letzten 30 Tage
        var alleTage = Enumerable.Range(0, (heute.AddDays(1) - vor30Tagen).Days + 1)
            .Select(offset => vor30Tagen.AddDays(offset).Date)
            .ToList();

        // Gruppiere die Berechnungen und zähle die Einträge pro Tag
        var durchschnittStats = berechnungen
            .Where(b => b.Zeitstempel >= vor30Tagen)
            .GroupBy(b => b.Zeitstempel.Date)
            .Select(g => new { Datum = g.Key, Anzahl = g.Count() })
            .ToList();

        // Fehlende Tage mit Anzahl = 0 hinzufügen
        var vollständigeStats = alleTage
            .GroupJoin(
                durchschnittStats,
                tag => tag,
                stat => stat.Datum,
                (tag, stats) => new { Datum = tag, Anzahl = stats.FirstOrDefault()?.Anzahl ?? 0 }
            )
            .ToList();

        // Durchschnitt berechnen
        var durchschnitt = vollständigeStats.Average(g => g.Anzahl);
        _help.WriteInfo($"Durchschnittliche Berechnungen pro Tag (letzte 30 Tage): {durchschnitt}");

        // Korrelation zwischen Tageszeit und Rechnertyp
        var korrelationStats = berechnungen;

        var korrelation = korrelationStats
            .GroupBy(b => new { b.Zeitstempel.Hour, b.Rechnertyp })
            .Select(g => new { g.Key.Hour, g.Key.Rechnertyp, Count = g.Count() })
            .OrderBy(x => x.Hour)
            .ToList();

        _help.WriteInfo("\nKorrelation zwischen Tageszeit und Rechnertyp:");

        foreach (var stat in korrelation)
        {
            _help.WriteInfo($"  {stat.Hour:00}:00 - {stat.Rechnertyp}: {stat.Count} mal");
        }
    }

    public void Wachstumstrend()
    {
        string eingabe = _help.Einlesen("Für welchen Tag möchtest du den Wachstumstrend bestimmen (dd.MM.yyyy)? ");

        if (!DateTime.TryParseExact(eingabe, "dd.MM.yyyy", null,
            System.Globalization.DateTimeStyles.None, out DateTime tag))
        {
            _help.WriteWarning("Ungültiges Format! Beispiel: 01.01.2025");
            return;
        }

        Benutzer akt = _benutzerManagement.getBenutzer();

        if (akt == null)
        {
            _help.WriteWarning("Kein Benutzer angemeldet!");
            return;
        }

        using var context = new TaschenrechnerContext();
        var berechnungen = context.Berechnungen
            .Where(b => b.BenutzerId == akt.Id && b.Zeitstempel.Date == tag.Date)
            .ToList();

        if (!berechnungen.Any())
        {
            _help.WriteWarning("Keine Berechnungen für diesen Tag gefunden.");
            return;
        }

        var anzahlBerechnungen = berechnungen.Count;
        var gesamtBerechnungen = context.Berechnungen
            .Where(b => b.BenutzerId == akt.Id && b.Zeitstempel.Date < tag.Date)
            .Count();

        if (gesamtBerechnungen == 0)
        {
            _help.WriteWarning("Keine vorherigen Berechnungen gefunden, Wachstumstrend kann nicht berechnet werden.");
            return;
        }

        var wachstum = (anzahlBerechnungen - gesamtBerechnungen) / (double)gesamtBerechnungen * 100;

        _help.WriteInfo($"Wachstumstrend für {tag:dd.MM.yyyy}: {wachstum:F2}%");
    }
}
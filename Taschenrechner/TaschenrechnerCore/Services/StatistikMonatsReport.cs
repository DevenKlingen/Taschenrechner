using TaschenrechnerCore.Utils;
using TaschenrechnerCore.Models;

namespace TaschenrechnerCore.Services;

public class StatistikMonatsReport
{
    private readonly Hilfsfunktionen _help;
    private readonly BenutzerManagement _benutzerManagement;

    public StatistikMonatsReport(
        Hilfsfunktionen help, 
        BenutzerManagement benutzerManagement)
    {
        _help = help;
        _benutzerManagement = benutzerManagement;
    }

    public void MonatsReport()
    {
        Benutzer akt = _benutzerManagement.getBenutzer();
        if (akt == null)
        {
            _help.WriteWarning("Kein Benutzer angemeldet!");
            return;
        }

        string eingabe = _help.Einlesen("Jahr und Monat (yyyy-mm): ");

        if (!DateTime.TryParseExact($"{eingabe}-01", "yyyy-MM-dd", null,
            System.Globalization.DateTimeStyles.None, out DateTime monat))
        {
            _help.WriteWarning("Ungültiges Format! Beispiel: 2025-01");
            return;
        }

        var monatsStart = new DateTime(monat.Year, monat.Month, 1);
        var monatsEnde = monatsStart.AddMonths(1);

        using var context = new TaschenrechnerContext();

        var berechnungen = context.Berechnungen
            .Where(b => b.BenutzerId == akt.Id &&
                       b.Zeitstempel >= monatsStart &&
                       b.Zeitstempel < monatsEnde)
            .OrderBy(b => b.Zeitstempel)
            .ToList();

        _help.WriteInfo($"\n=== MONATS-REPORT {monat:MMMM yyyy} ===");
        _help.WriteInfo($"Berechnungen gesamt: {berechnungen.Count}");

        if (!berechnungen.Any())
        {
            _help.WriteWarning("Keine Berechnungen in diesem Monat.");
            return;
        }

        // Tägliche Aufschlüsselung
        var tagegruppen = berechnungen
            .GroupBy(b => b.Zeitstempel.Day)
            .OrderBy(g => g.Key);

        _help.WriteInfo("\nTägliche Aufschlüsselung:");
        foreach (var gruppe in tagegruppen)
        {
            _help.WriteInfo($"  {gruppe.Key:00}.{monat.Month:00}: {gruppe.Count()} Berechnungen");
        }

        // Top-Operationen des Monats
        var topOps = berechnungen
            .GroupBy(b => b.Operation)
            .Select(g => new { Op = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(3);

        _help.WriteInfo("\nTop-Operationen:");
        foreach (var op in topOps)
        {
            _help.WriteInfo($"  {op.Op}: {op.Count} mal");
        }
    }
}
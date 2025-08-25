namespace TaschenrechnerCore.Services;

public class StatistikMonatsReport
{

    static void MonatsReport()
    {
        Benutzer akt = programm.getAktBenutzer();
        if (akt == null)
        {
            help.Write("Kein Benutzer angemeldet!");
            return;
        }

        help.Write("Jahr und Monat (yyyy-mm): ");
        string eingabe = Console.ReadLine();

        if (!DateTime.TryParseExact($"{eingabe}-01", "yyyy-MM-dd", null,
            System.Globalization.DateTimeStyles.None, out DateTime monat))
        {
            help.Write("Ungültiges Format! Beispiel: 2025-01");
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

        help.Write($"\n=== MONATS-REPORT {monat:MMMM yyyy} ===");
        help.Write($"Berechnungen gesamt: {berechnungen.Count}");

        if (!berechnungen.Any())
        {
            help.Write("Keine Berechnungen in diesem Monat.");
            return;
        }

        // Tägliche Aufschlüsselung
        var tagegruppen = berechnungen
            .GroupBy(b => b.Zeitstempel.Day)
            .OrderBy(g => g.Key);

        help.Write("\nTägliche Aufschlüsselung:");
        foreach (var gruppe in tagegruppen)
        {
            help.Write($"  {gruppe.Key:00}.{monat.Month:00}: {gruppe.Count()} Berechnungen");
        }

        // Top-Operationen des Monats
        var topOps = berechnungen
            .GroupBy(b => b.Operation)
            .Select(g => new { Op = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(3);

        help.Write("\nTop-Operationen:");
        foreach (var op in topOps)
        {
            help.Write($"  {op.Op}: {op.Count} mal");
        }
    }
}
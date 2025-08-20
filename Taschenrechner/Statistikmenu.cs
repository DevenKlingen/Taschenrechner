namespace MeinTaschenrechner
{
    internal class Statistikmenu
    {
        static Program programm = new Program();
        static Hilfsfunktionen help = new Hilfsfunktionen();

        public void StatistikMenu()
        {
            Benutzer aktuellerBenutzer = programm.getAktBenutzer();
            bool statistikMenuAktiv = true;

            while (statistikMenuAktiv)
            {
                help.Mischen();
                help.Write("\n=== DATENBANK-MENÜ ===");
                help.Write("1. Benutzer Statistik anzeigen");
                help.Write("2. Monatsreport");
                help.Write("3. Wachstumstrend für einen Tag");
                help.Write("4. Zurück zum Hauptmenü");
                help.Write("Deine Wahl (1-4): ");
                int wahl = help.MenuWahlEinlesen();

                switch (wahl)
                {
                    case 1:
                        BenutzerStatistiken();
                        break;
                    case 2:
                        MonatsReport();
                        break;
                    case 3:
                        Wachstumstrend();
                        break;
                    case 4:
                        statistikMenuAktiv = false;
                        help.Write("Zurück zum Hauptmenü.");
                        break;
                    default:
                        help.Write("Ungültige Wahl!");
                        break;
                }
                if (statistikMenuAktiv)
                {
                    help.Write("\nDrücke Enter für Menü...");
                    Console.ReadLine();
                }
            }
        }

        static void BenutzerStatistiken()
        {
            Benutzer akt = programm.getAktBenutzer();
            Benutzer aktuellerBenutzer = akt;

            if (aktuellerBenutzer == null)
            {
                help.Write("Kein Benutzer angemeldet!");
                return;
            }

            using var context = new TaschenrechnerContext();

            var berechnungen = context.Berechnungen
                .Where(b => b.BenutzerId == akt.Id)
                .ToList();

            if (!berechnungen.Any())
            {
                help.Write("Keine Berechnungen vorhanden für Statistiken.");
                return;
            }

            help.Write("=== BENUTZER-STATISTIKEN ===");
            help.Write($"Benutzer: {akt.Name}");
            help.Write($"Mitglied seit: {akt.ErstelltAm:dd.MM.yyyy}");
            help.Write($"Gesamt-Berechnungen: {berechnungen.Count}");

            // Operationen-Statistik
            var operationenStats = berechnungen
                .GroupBy(b => b.Operation)
                .Select(g => new { Operation = g.Key, Anzahl = g.Count() })
                .OrderByDescending(x => x.Anzahl)
                .ToList();

            help.Write("\nHäufigste Operationen:");
            foreach (var stat in operationenStats)
            {
                help.Write($"  {stat.Operation}: {stat.Anzahl} mal");
            }

            // Rechnertyp-Statistik
            var rechnerStats = berechnungen
                .GroupBy(b => b.Rechnertyp)
                .Select(g => new { Typ = g.Key, Anzahl = g.Count() })
                .OrderByDescending(x => x.Anzahl)
                .ToList();

            help.Write("\nVerwendete Rechnertypen:");
            foreach (var stat in rechnerStats)
            {
                help.Write($"  {stat.Typ}: {stat.Anzahl} mal");
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

            help.Write("\nAktivität (letzte 7 Tage):");
            for (int i = 6; i >= 0; i--)
            {
                var datum = heute.AddDays(-i);
                var stat = tagesStats.FirstOrDefault(t => t.Datum == datum);
                int anzahl = stat?.Anzahl ?? 0;
                help.Write($"  {datum:dd.MM.yyyy}: {anzahl} Berechnungen");
            }

            // Durchschnitte und Extreme
            var ergebnisse = berechnungen.Select(b => b.Ergebnis).ToList();
            if (ergebnisse.Any())
            {
                help.Write("\nErgebnis-Statistiken:");
                help.Write($"  Kleinster Wert: {ergebnisse.Min():F2}");
                help.Write($"  Größter Wert: {ergebnisse.Max():F2}");
                help.Write($"  Durchschnitt: {ergebnisse.Average():F2}");
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
            help.Write($"Durchschnittliche Berechnungen pro Tag (letzte 30 Tage): {durchschnitt}");

            // Korrelation zwischen Tageszeit und Rechnertyp
            var korrelationStats = berechnungen;

            var korrelation = korrelationStats
                .GroupBy(b => new { b.Zeitstempel.Hour, b.Rechnertyp })
                .Select(g => new { g.Key.Hour, g.Key.Rechnertyp, Count = g.Count() })
                .OrderBy(x => x.Hour)
                .ToList();

            help.Write("\nKorrelation zwischen Tageszeit und Rechnertyp:");

            foreach (var stat in korrelation)
            {
                help.Write($"  {stat.Hour:00}:00 - {stat.Rechnertyp}: {stat.Count} mal");
            }
        }

        static void Wachstumstrend()
        {
            help.Write("Für welchen Tag möchtest du den Wachstumstrend bestimmen (dd.MM.yyyy)? ");
            string eingabe = Console.ReadLine();

            if (!DateTime.TryParseExact(eingabe, "dd.MM.yyyy", null,
                System.Globalization.DateTimeStyles.None, out DateTime tag))
            {
                help.Write("Ungültiges Format! Beispiel: 01.01.2025");
                return;
            }

            Benutzer akt = programm.getAktBenutzer();

            if (akt == null)
            {
                help.Write("Kein Benutzer angemeldet!");
                return;
            }

            using var context = new TaschenrechnerContext();
            var berechnungen = context.Berechnungen
                .Where(b => b.BenutzerId == akt.Id && b.Zeitstempel.Date == tag.Date)
                .ToList();

            if (!berechnungen.Any())
            {
                help.Write("Keine Berechnungen für diesen Tag gefunden.");
                return;
            }

            var anzahlBerechnungen = berechnungen.Count;
            var gesamtBerechnungen = context.Berechnungen
                .Where(b => b.BenutzerId == akt.Id && b.Zeitstempel.Date < tag.Date)
                .Count();

            if (gesamtBerechnungen == 0)
            {
                help.Write("Keine vorherigen Berechnungen gefunden, Wachstumstrend kann nicht berechnet werden.");
                return;
            }

            var wachstum = (anzahlBerechnungen - gesamtBerechnungen) / (double)gesamtBerechnungen * 100;

            help.Write($"Wachstumstrend für {tag:dd.MM.yyyy}: {wachstum:F2}%");
        }

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
}
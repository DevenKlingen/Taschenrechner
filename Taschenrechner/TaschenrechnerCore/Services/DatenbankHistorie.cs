using TaschenrechnerConsole;
using TaschenrechnerCore.Utils;
using TaschenrechnerCore.Models;
using System.Text.Json;

namespace TaschenrechnerCore.Services;

public class DatenbankHistorie
{
    static Program program = new Program();
    static Hilfsfunktionen help = new Hilfsfunktionen();
    public void DatenbankHistorieAnzeigen()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Benutzer akt = program.getAktBenutzer();
        if (akt == null)
        {
            help.Write("Kein Benutzer angemeldet!");
            return;
        }

        using var context = new TaschenrechnerContext();

        var berechnungen = context.Berechnungen
            .Where(b => b.BenutzerId == akt.Id)
            .OrderByDescending(b => b.Zeitstempel)
            .Take(20) // Nur die letzten 20 anzeigen
            .ToList();

        if (!berechnungen.Any())
        {
            help.Write("Keine Berechnungen in der Datenbank gefunden.");
            return;
        }

        help.Write("=== DATENBANK-HISTORIE (letzte 20) ===");
        foreach (var berechnung in berechnungen)
        {
            try
            {
                double[] eingaben = JsonSerializer.Deserialize<double[]>(berechnung.Eingaben);
                if (berechnung.Operation == "$")
                {
                    help.Write($"[{berechnung.Zeitstempel:HH:mm:ss}] " + $"{eingaben[0]}€ = ${berechnung.Ergebnis.ToString($"F{program.config.Nachkommastellen}")} " + $"({berechnung.Rechnertyp})");
                }
                else if (berechnung.Operation == "/, *")
                {
                    help.Write($"[{berechnung.Zeitstempel:HH:mm:ss}] " + $"({eingaben[0]} / {eingaben[1]}) * {eingaben[2]} = {berechnung.Ergebnis.ToString($"F{program.config.Nachkommastellen}")} " + $"({berechnung.Rechnertyp})");
                }
                else if (berechnung.Operation == "*, /")
                {
                    help.Write($"[{berechnung.Zeitstempel:HH:mm:ss}] " + $"({eingaben[0]} * {eingaben[1]}) / {eingaben[2]} = {berechnung.Ergebnis.ToString($"F{program.config.Nachkommastellen}")} " + $"({berechnung.Rechnertyp})");
                }
                else
                {
                    string eingabenStr = string.Join($" {berechnung.Operation} ", eingaben);

                    help.Write($"[{berechnung.Zeitstempel:HH:mm:ss}] " + $"{eingabenStr} = {berechnung.Ergebnis.ToString($"F{program.config.Nachkommastellen}")} " + $"({berechnung.Rechnertyp})");
                }

                if (!string.IsNullOrEmpty(berechnung.Kommentar))
                {
                    help.Write($"    Kommentar: {berechnung.Kommentar}");
                }
            }
            catch (Exception ex)
            {
                help.Write($"Fehler beim Anzeigen einer Berechnung: {ex.Message}");
            }
        }
    }
}
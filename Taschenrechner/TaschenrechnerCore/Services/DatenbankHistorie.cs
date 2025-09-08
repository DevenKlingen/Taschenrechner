using TaschenrechnerCore.Utils;
using TaschenrechnerCore.Models;
using System.Text.Json;

namespace TaschenrechnerCore.Services;

public class DatenbankHistorie
{
    private readonly Hilfsfunktionen _help;
    private readonly BenutzerManagement _benutzerManagement;
    private readonly BenutzerEinstellungen _benutzerEinstellungen;

    public DatenbankHistorie(Hilfsfunktionen help, BenutzerManagement benutzerManagement, BenutzerEinstellungen benutzerEinstellungen)
    {
        _help = help;
        _benutzerManagement = benutzerManagement;
        _benutzerEinstellungen = benutzerEinstellungen;
    }

    public void DatenbankHistorieAnzeigen()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Benutzer akt = _benutzerManagement.getBenutzer();
        if (akt == null)
        {
            _help.Write("Kein Benutzer angemeldet!");
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
            _help.Write("Keine Berechnungen in der Datenbank gefunden.");
            return;
        }

        _help.Write("\n=== DATENBANK-HISTORIE (letzte 20) ===");
        foreach (var berechnung in berechnungen)
        {
            try
            {
                double[] eingaben = JsonSerializer.Deserialize<double[]>(berechnung.Eingaben);
                if (berechnung.Operation == "$")
                {
                    _help.Write($"[{berechnung.Zeitstempel:HH:mm:ss}] " + $"{eingaben[0]}€ = ${berechnung.Ergebnis.ToString($"F{_benutzerEinstellungen.getConfig().Nachkommastellen}")} " + $"({berechnung.Rechnertyp})");
                }
                else if (berechnung.Operation == "/, *")
                {
                    _help.Write($"[{berechnung.Zeitstempel:HH:mm:ss}] " + $"({eingaben[0]} / {eingaben[1]}) * {eingaben[2]} = {berechnung.Ergebnis.ToString($"F{_benutzerEinstellungen.getConfig().Nachkommastellen}")} " + $"({berechnung.Rechnertyp})");
                }
                else if (berechnung.Operation == "*, /")
                {
                    _help.Write($"[{berechnung.Zeitstempel:HH:mm:ss}] " + $"({eingaben[0]} * {eingaben[1]}) / {eingaben[2]} = {berechnung.Ergebnis.ToString($"F{_benutzerEinstellungen.getConfig().Nachkommastellen}")} " + $"({berechnung.Rechnertyp})");
                }
                else
                {
                    string eingabenStr = string.Join($" {berechnung.Operation} ", eingaben);

                    _help.Write($"[{berechnung.Zeitstempel:HH:mm:ss}] " + $"{eingabenStr} = {berechnung.Ergebnis.ToString($"F{_benutzerEinstellungen.getConfig().Nachkommastellen}")} " + $"({berechnung.Rechnertyp})");
                }

                if (!string.IsNullOrEmpty(berechnung.Kommentar))
                {
                    _help.Write($"    Kommentar: {berechnung.Kommentar}");
                }
            }
            catch (Exception ex)
            {
                _help.Write($"Fehler beim Anzeigen einer Berechnung: {ex.Message}");
            }
        }
    }
}
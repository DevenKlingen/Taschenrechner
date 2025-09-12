using TaschenrechnerCore.Utils;
using TaschenrechnerCore.Models;
using System.Text.Json;
using TaschenrechnerCore.Interfaces;

namespace TaschenrechnerCore.Services;

public class DatenbankHistorie : IHistorieManager
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

    public void HistorieAnzeigen()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Benutzer akt = _benutzerManagement.getBenutzer();
        if (akt == null)
        {
            _help.WriteInfo("Kein Benutzer angemeldet!");
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
            _help.WriteInfo("Keine Berechnungen in der Datenbank gefunden.");
            return;
        }

        _help.WriteInfo("\n=== DATENBANK-HISTORIE (letzte 20) ===");
        foreach (var berechnung in berechnungen)
        {
            try
            {
                double[] eingaben = JsonSerializer.Deserialize<double[]>(berechnung.Eingaben);
                if (berechnung.Operation == "$")
                {
                    _help.WriteInfo($"[{berechnung.Zeitstempel:HH:mm:ss}] " + $"{eingaben[0]}€ = ${berechnung.Ergebnis.ToString($"F{_benutzerEinstellungen.getConfig().Nachkommastellen}")} " + $"({berechnung.Rechnertyp})");
                }
                else if (berechnung.Operation == "/, *")
                {
                    _help.WriteInfo($"[{berechnung.Zeitstempel:HH:mm:ss}] " + $"({eingaben[0]} / {eingaben[1]}) * {eingaben[2]} = {berechnung.Ergebnis.ToString($"F{_benutzerEinstellungen.getConfig().Nachkommastellen}")} " + $"({berechnung.Rechnertyp})");
                }
                else if (berechnung.Operation == "*, /")
                {
                    _help.WriteInfo($"[{berechnung.Zeitstempel:HH:mm:ss}] " + $"({eingaben[0]} * {eingaben[1]}) / {eingaben[2]} = {berechnung.Ergebnis.ToString($"F{_benutzerEinstellungen.getConfig().Nachkommastellen}")} " + $"({berechnung.Rechnertyp})");
                }
                else
                {
                    string eingabenStr = string.Join($" {berechnung.Operation} ", eingaben);

                    _help.WriteInfo($"[{berechnung.Zeitstempel:HH:mm:ss}] " + $"{eingabenStr} = {berechnung.Ergebnis.ToString($"F{_benutzerEinstellungen.getConfig().Nachkommastellen}")} " + $"({berechnung.Rechnertyp})");
                }

                if (!string.IsNullOrEmpty(berechnung.Kommentar))
                {
                    _help.WriteInfo($"    Kommentar: {berechnung.Kommentar}");
                }
            }
            catch (Exception ex)
            {
                _help.WriteError($"Fehler beim Anzeigen einer Berechnung: {ex.Message}");
            }
        }
    }
    
    public void HistorieLöschen()
    {
        DatenbankReinigung _datenbankReinigung = new DatenbankReinigung(_help, _benutzerManagement);
        _datenbankReinigung.DatenbankBereinigen();
    }

    public void HistorieHinzufügen()
    {
        DatenbankBerechnungen _datenbankBerechnungen = new DatenbankBerechnungen(_benutzerManagement, _help);
        _datenbankBerechnungen.BerechnungenSuchen();
    }
}
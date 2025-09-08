using TaschenrechnerCore.Utils;
using TaschenrechnerCore.Models;
using System.Text.Json;

namespace TaschenrechnerCore.Services;

public class DatenbankExport
{
    private readonly Hilfsfunktionen _help;
    private readonly BenutzerManagement _benutzerManagement;

    public DatenbankExport(
        Hilfsfunktionen help, 
        BenutzerManagement benutzerManagement)
    {
        _help = help;
        _benutzerManagement = benutzerManagement;
    }

    public void DatenbankExportieren()
    {
        Benutzer akt = _benutzerManagement.getBenutzer();
        if (akt == null)
        {
            _help.Write("Kein Benutzer angemeldet!");
            return;
        }

        try
        {
            using var context = new TaschenrechnerContext();

            var berechnungen = context.Berechnungen
                .Where(b => b.BenutzerId == akt.Id)
                .OrderBy(b => b.Zeitstempel)
                .ToList();

            string exportDatei = $"export_{akt.Name}_{DateTime.Now:yyyyMMdd_HHmmss}.json";

            var exportData = new
            {
                Benutzer = akt.Name,
                ExportiertAm = DateTime.Now,
                AnzahlBerechnungen = berechnungen.Count,
                Berechnungen = berechnungen.Select(b => new
                {
                    b.Zeitstempel,
                    b.Operation,
                    Eingaben = JsonSerializer.Deserialize<double[]>(b.Eingaben),
                    b.Ergebnis,
                    b.Kommentar,
                    b.Rechnertyp
                })
            };

            string json = JsonSerializer.Serialize(exportData, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(exportDatei, json);
            _help.Write($"Datenbank exportiert nach: {exportDatei}");
        }
        catch (Exception ex)
        {
            _help.Write($"Fehler beim Export: {ex.Message}");
        }
    }
}
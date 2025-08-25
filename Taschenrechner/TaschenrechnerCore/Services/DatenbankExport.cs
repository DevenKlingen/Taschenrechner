using TaschenrechnerCore.Utils;
using TaschenrechnerCore.Models;
using System.Text.Json;

namespace TaschenrechnerCore.Services;

public class DatenbankExport
{
    static Hilfsfunktionen help = new Hilfsfunktionen();
    static BenutzerManagement benutzerManagement = new();
    public void DatenbankExportieren()
    {
        Benutzer akt = benutzerManagement.getBenutzer();
        if (akt == null)
        {
            help.Write("Kein Benutzer angemeldet!");
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
            help.Write($"Datenbank exportiert nach: {exportDatei}");
        }
        catch (Exception ex)
        {
            help.Write($"Fehler beim Export: {ex.Message}");
        }
    }
}
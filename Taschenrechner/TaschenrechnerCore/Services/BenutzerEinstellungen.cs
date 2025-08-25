using TaschenrechnerCore.Models;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class BenutzerEinstellungen
{
    static Hilfsfunktionen help = new();
    static BenutzerManagement benutzerManagement = new();
    public TaschenrechnerKonfiguration config = new TaschenrechnerKonfiguration();
    public void StandardEinstellungenErstellen(int benutzerId, TaschenrechnerContext context)
    {
        // Anzahl der benötigten IDs (entspricht der Anzahl der Standard-Einstellungen)
        int benötigteIds = 6;

        // Hole alle existierenden IDs aus der Tabelle
        var existingIds = context.Einstellungen.Select(e => e.Id).ToHashSet();

        // Berechne die kleinstmöglichen freien IDs
        var freieIds = new List<int>();
        int aktuelleId = 1;

        while (freieIds.Count < benötigteIds)
        {
            if (!existingIds.Contains(aktuelleId))
            {
                freieIds.Add(aktuelleId);
            }
            aktuelleId++;
        }

        var vorhandeneEinstellungen = context.Einstellungen
            .Where(e => e.BenutzerId == benutzerId)
            .Select(e => e.Schluessel)
            .ToHashSet();

        // Erstelle die Standard-Einstellungen mit den berechneten IDs
        var standardEinstellungen = new List<Einstellung>
            {
                new Einstellung { Id = freieIds[0], BenutzerId = benutzerId, Schluessel = "Nachkommastellen", Wert = "2" },
                new Einstellung { Id = freieIds[1], BenutzerId = benutzerId, Schluessel = "Thema", Wert = "Hell" },
                new Einstellung { Id = freieIds[2], BenutzerId = benutzerId, Schluessel = "Standardrechner", Wert = "Basis" },
                new Einstellung { Id = freieIds[3], BenutzerId = benutzerId, Schluessel = "AutoSpeichern", Wert = "true" },
                new Einstellung { Id = freieIds[4], BenutzerId = benutzerId, Schluessel = "Sprache", Wert = "Deutsch" },
                new Einstellung { Id = freieIds[5], BenutzerId = benutzerId, Schluessel = "ZeigeZeitstempel", Wert = "true" }
            };

        // Füge die Einstellungen zur Datenbank hinzu
        var fehlendeEinstellungen = standardEinstellungen
            .Where(e => !vorhandeneEinstellungen.Contains(e.Schluessel))
            .ToList();

        // Nur fehlende Einstellungen hinzufügen
        if (fehlendeEinstellungen.Any())
        {
            context.Einstellungen.AddRange(fehlendeEinstellungen);
            context.SaveChanges();
            help.Write($"Es wurden {fehlendeEinstellungen.Count} fehlende Standardeinstellungen hinzugefügt.");
        }
        else
        {
            help.Write("Alle Standardeinstellungen sind bereits vorhanden.");
        }
    }

    public void BenutzereinstellungenLaden()
    {
        var aktuellerBenutzer = benutzerManagement.getBenutzer();
        if (aktuellerBenutzer == null)
        {
            help.Write("Kein Benutzer angemeldet! Konnte keine Einstellungen laden!");
            return;
        }
        using var context = new TaschenrechnerContext();

        var einstellungen = context.Einstellungen
            .Where(e => e.BenutzerId == aktuellerBenutzer.Id)
            .ToList();

        // Einstellungen in das config-Objekt laden
        foreach (var einstellung in einstellungen)
        {
            switch (einstellung.Schluessel)
            {
                case "Nachkommastellen":
                    if (int.TryParse(einstellung.Wert, out int stellen))
                        config.Nachkommastellen = stellen;
                    break;
                case "Thema":
                        config.Thema = einstellung.Wert;
                    break;
                case "AutoSpeichern":
                    if (bool.TryParse(einstellung.Wert, out bool autoSave))
                        config.AutoSpeichern = autoSave;
                    break;
                case "Sprache":
                    config.Sprache = einstellung.Wert;
                    break;
                case "Standardrechner":
                    config.Standardrechner = einstellung.Wert;
                    break;
                case "ZeigeZeitstempel":
                    if (bool.TryParse(einstellung.Wert, out bool zeigeZeitstempel))
                        config.ZeigeZeitstempel = zeigeZeitstempel;
                    break;

            }
        }

        help.Write("Benutzereinstellungen geladen.");
    }
}
using TaschenrechnerCore.Utils;
using System.Text.Json;
using TaschenrechnerCore.Services;
using Tomlyn;
using TaschenrechnerCore.Models;

namespace TaschenrechnerCore;

public class KonfigVerwaltung
{
    private string _konfigJson = "config.json";
    private string _konfigToml = "config.toml";

    private Hilfsfunktionen _help;
    private readonly KonfigBearbeiten _konfigBearbeiten;
    private readonly BenutzerEinstellungen _benutzerEinstellungen;
    private readonly BenutzerManagement _benutzerManagement;

    public KonfigVerwaltung(
        KonfigBearbeiten konfigBearbeiten,
        BenutzerEinstellungen benutzerEinstellungen,
        BenutzerManagement benutzerManagement)
    {
        _konfigBearbeiten = konfigBearbeiten;
        _benutzerEinstellungen = benutzerEinstellungen;
        _benutzerManagement = benutzerManagement;
    }

    public void setHilfe(Hilfsfunktionen help)
    {
        _help = help;
    }

    /// <summary>
    /// Lädt die Konfiguration aus einer Datei (TOML oder JSON)
    /// </summary>
    public void KonfigurationLaden()
    {
        try
        {
            _konfigBearbeiten.SetzeKonfigDateiPfad();
            if (File.Exists(_konfigToml))
            {
                string tomlString = File.ReadAllText(_konfigToml);
                _benutzerEinstellungen.setConfig(Toml.ToModel<TaschenrechnerKonfiguration>(tomlString));
                _help.Write("Konfiguration aus TOML geladen.");
            }
            else if (File.Exists(_konfigJson))
            {
                string jsonString = File.ReadAllText(_konfigJson);
                _benutzerEinstellungen.setConfig(JsonSerializer.Deserialize<TaschenrechnerKonfiguration>(jsonString));
                _help.Write("Konfiguration aus JSON geladen.");
            }
            else
            {
                _help.Write("Verwende Standard-Konfiguration.");
                KonfigurationSpeichern(); // Erstelle Standard-Datei
            }
        }
        catch (Exception ex)
        {
            _help.Write($"Fehler beim Laden der Konfiguration: {ex.Message}");
            _benutzerEinstellungen.setConfig(new TaschenrechnerKonfiguration()); // Fallback
        }

        var akt = _benutzerManagement.getBenutzer();
        using var context = new TaschenrechnerContext();
        var userSetting = context.Einstellungen
                .FirstOrDefault(us => us.BenutzerId == akt.Id && us.Schluessel == "Thema");
        _help.Mischen();
    }

    /// <summary>
    /// Speichert die aktuelle Konfiguration in eine JSON- und TOML-Datei
    /// </summary>
    public void KonfigurationSpeichern()
    {
        try
        {
            string jsonString = JsonSerializer.Serialize(_benutzerEinstellungen.getConfig(), new JsonSerializerOptions
            {
                WriteIndented = true
            });
            _konfigBearbeiten.SetzeKonfigDateiPfad();
            File.WriteAllText(_konfigJson, jsonString);

            string tomlString = Toml.FromModel(_benutzerEinstellungen.getConfig());
            File.WriteAllText(_konfigToml, tomlString);

            _help.Write("Konfiguration gespeichert.");
        }
        catch (Exception ex)
        {
            _help.Write($"Fehler beim Speichern der Konfiguration: {ex.Message}");
        }
    }

    /// <summary>
    /// Zeigt die aktuelle Konfiguration an
    /// </summary>
    public void KonfigurationAnzeigen()
    {
        _help.Write("\n=== AKTUELLE KONFIGURATION ===");
        _help.Write($"Thema: {_benutzerEinstellungen.getConfig().Thema}");
        _help.Write($"Nachkommastellen: {_benutzerEinstellungen.getConfig().Nachkommastellen}");
        _help.Write($"Standardrechner: {_benutzerEinstellungen.getConfig().Standardrechner}");
        _help.Write($"Auto-Speichern: {_benutzerEinstellungen.getConfig().AutoSpeichern}");
        _help.Write($"Sprache: {_benutzerEinstellungen.getConfig().Sprache}");
        _help.Write($"Zeitstempel anzeigen: {_benutzerEinstellungen.getConfig().ZeigeZeitstempel}");
    }

    public string getJsonPath()
    {
        return _konfigJson;
    }

    public string getTomlPath()
    {
        return _konfigToml;
    }

    public void SetzeKonfigDateiPfad(string konfigOrdner)
    {
        _konfigJson = Path.Combine(konfigOrdner, "config.json");
        _konfigToml = Path.Combine(konfigOrdner, "config.toml");
    }
}
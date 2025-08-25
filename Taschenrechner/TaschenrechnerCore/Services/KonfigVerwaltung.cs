using TaschenrechnerCore.Utils;
using System.Text.Json;
using TaschenrechnerCore.Services;
using Tomlyn;
using TaschenrechnerCore.Models;

namespace TaschenrechnerCore;

public class KonfigVerwaltung
{
    public string konfigJson = "config.json";
    public string konfigToml = "config.toml";
    static Hilfsfunktionen help = new Hilfsfunktionen();
    static KonfigBearbeiten konfigB = new KonfigBearbeiten();
    static BenutzerEinstellungen benutzerEinstellungen = new();
    static BenutzerManagement benutzerManagement = new();

    /// <summary>
    /// Lädt die Konfiguration aus einer Datei (TOML oder JSON)
    /// </summary>
    public void KonfigurationLaden()
    {
        try
        {
            konfigB.SetzeKonfigDateiPfad();
            if (File.Exists(konfigToml))
            {
                string tomlString = File.ReadAllText(konfigToml);
                benutzerEinstellungen.config = Toml.ToModel<TaschenrechnerKonfiguration>(tomlString);
                help.Write("Konfiguration aus TOML geladen.");
            }
            else if (File.Exists(konfigJson))
            {
                string jsonString = File.ReadAllText(konfigJson);
                benutzerEinstellungen.config = JsonSerializer.Deserialize<TaschenrechnerKonfiguration>(jsonString);
                help.Write("Konfiguration aus JSON geladen.");
            }
            else
            {
                help.Write("Verwende Standard-Konfiguration.");
                KonfigurationSpeichern(); // Erstelle Standard-Datei
            }
        }
        catch (Exception ex)
        {
            help.Write($"Fehler beim Laden der Konfiguration: {ex.Message}");
            benutzerEinstellungen.config = new TaschenrechnerKonfiguration(); // Fallback
        }

        var akt = benutzerManagement.getBenutzer();
        using var context = new TaschenrechnerContext();
        var userSetting = context.Einstellungen
                .FirstOrDefault(us => us.BenutzerId == akt.Id && us.Schluessel == "Thema");
        switch (userSetting?.Wert.ToLower())
        {
            case "hell":
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
                benutzerEinstellungen.config.Thema = "Hell";
                break;
            case "dunkel":
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                benutzerEinstellungen.config.Thema = "Dunkel";
                break;
            case "grün":
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Green;
                benutzerEinstellungen.config.Thema = "Grün";
                break;
            case "gelb":
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Yellow;
                benutzerEinstellungen.config.Thema = "Gelb";
                break;
            case "blau":
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Blue;
                benutzerEinstellungen.config.Thema = "Blau";
                break;
            case "rot":
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Red;
                benutzerEinstellungen.config.Thema = "Rot";
                break;
            case "lila":
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.DarkMagenta;
                benutzerEinstellungen.config.Thema = "Lila";
                break;
            case "matrix":
                Console.ForegroundColor = ConsoleColor.Green;
                Console.BackgroundColor = ConsoleColor.Black;
                benutzerEinstellungen.config.Thema = "Matrix";
                break;
            default:
                help.Write("Ungültige Wahl!");
                break;
        }
    }

    /// <summary>
    /// Speichert die aktuelle Konfiguration in eine JSON- und TOML-Datei
    /// </summary>
    public void KonfigurationSpeichern()
    {
        try
        {
            string jsonString = JsonSerializer.Serialize(benutzerEinstellungen.config, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            konfigB.SetzeKonfigDateiPfad();
            File.WriteAllText(konfigJson, jsonString);

            string tomlString = Toml.FromModel(benutzerEinstellungen.config);
            File.WriteAllText(konfigToml, tomlString);

            help.Write("Konfiguration gespeichert.");
        }
        catch (Exception ex)
        {
            help.Write($"Fehler beim Speichern der Konfiguration: {ex.Message}");
        }
    }

    /// <summary>
    /// Zeigt die aktuelle Konfiguration an
    /// </summary>
    public void KonfigurationAnzeigen()
    {
        help.Write("=== AKTUELLE KONFIGURATION ===");
        help.Write($"Thema: {benutzerEinstellungen.config.Thema}");
        help.Write($"Nachkommastellen: {benutzerEinstellungen.config.Nachkommastellen}");
        help.Write($"Standardrechner: {benutzerEinstellungen.config.Standardrechner}");
        help.Write($"Auto-Speichern: {benutzerEinstellungen.config.AutoSpeichern}");
        help.Write($"Sprache: {benutzerEinstellungen.config.Sprache}");
        help.Write($"Zeitstempel anzeigen: {benutzerEinstellungen.config.ZeigeZeitstempel}");
    }

    public string getJsonPath()
    {
        return konfigJson;
    }

    public string getTomlPath()
    {
        return konfigToml;
    }

    public void SetzeKonfigDateiPfad(string konfigOrdner)
    {
        konfigJson = Path.Combine(konfigOrdner, "config.json");
        konfigToml = Path.Combine(konfigOrdner, "config.toml");
    }
}
using TaschenrechnerCore.Utils;
using TaschenrechnerCore.Models;

namespace TaschenrechnerCore.Services;

public class KonfigBearbeiten
{
    private readonly Hilfsfunktionen _help;
    private KonfigVerwaltung _konfigVerwaltung;
    private readonly BenutzerManagement _benutzerManagement;
    private readonly BenutzerEinstellungen _benutzerEinstellungen;

    public KonfigBearbeiten(
        Hilfsfunktionen help,
        BenutzerManagement benutzerManagement, 
        BenutzerEinstellungen benutzerEinstellungen)
    {
        _help = help;
        _benutzerManagement = benutzerManagement;
        _benutzerEinstellungen = benutzerEinstellungen;
    }

    public void setKonfigVerwaltung(KonfigVerwaltung konfigVerwaltung)
    {
        _konfigVerwaltung = konfigVerwaltung;
    }

    /// <summary>
    /// Erlaubt dem Nutzer, die Konfiguration zu ändern
    /// </summary>
    public void KonfigurationAendern()
    {
        _help.Mischen();
        Benutzer akt = _benutzerManagement.getBenutzer();

        using var context = new TaschenrechnerContext();
        var userSetting = context.Einstellungen
            .FirstOrDefault(us => us.BenutzerId == akt.Id && us.Schluessel == "Thema");

        _help.Write("\n=== KONFIGURATION ÄNDERN ===");

        string eingabe = _help.Einlesen($"Thema ({_benutzerEinstellungen.getConfig().Thema}): ").ToLower();

        _help.Mischen();

        eingabe = _help.Einlesen($"Nachkommastellen ({_benutzerEinstellungen.getConfig().Nachkommastellen}): ");

        userSetting = context.Einstellungen
            .FirstOrDefault(us => us.BenutzerId == akt.Id && us.Schluessel == "Nachkommastellen");

        if (int.TryParse(eingabe, out int stellen) && stellen >= 0 && stellen <= 10)
        {
            _benutzerEinstellungen.getConfig().Nachkommastellen = stellen;
            userSetting.Wert = stellen.ToString();
            context.SaveChanges();
        }

        eingabe = _help.Einlesen($"Standardrechner ({_benutzerEinstellungen.getConfig().Standardrechner}): ");

        userSetting = context.Einstellungen
            .FirstOrDefault(us => us.BenutzerId == akt.Id && us.Schluessel == "Standardrechner");

        if (eingabe == "Basis" || eingabe == "Wissenschaftlich")
        {
            _benutzerEinstellungen.getConfig().Standardrechner = eingabe;
            userSetting.Wert = eingabe;
            context.SaveChanges();
        }

        eingabe = _help.Einlesen($"Auto-Speichern (j/n, aktuell: {(_benutzerEinstellungen.getConfig().AutoSpeichern ? "j" : "n")}): ").ToLower();

        userSetting = context.Einstellungen
            .FirstOrDefault(us => us.BenutzerId == akt.Id && us.Schluessel == "AutoSpeichern");

        if (eingabe == "j" || eingabe == "n")
        {
            _benutzerEinstellungen.getConfig().AutoSpeichern = eingabe == "j";
            userSetting.Wert = _benutzerEinstellungen.getConfig().AutoSpeichern ? "j" : "n";
            context.SaveChanges();
        }

        eingabe = _help.Einlesen($"Sprache ({_benutzerEinstellungen.getConfig().Sprache}): ");

        userSetting = context.Einstellungen
            .FirstOrDefault(us => us.BenutzerId == akt.Id && us.Schluessel == "Sprache");

        if (eingabe == "Deutsch" || eingabe == "Englisch" || eingabe == "Spanisch" || eingabe == "Italienisch" || eingabe == "Französisch")
        {
            _benutzerEinstellungen.getConfig().Sprache = eingabe;
            userSetting.Wert = eingabe;
            context.SaveChanges();
        }

        eingabe = _help.Einlesen($"Zeige Zeitstempel (j/n, aktuell: {(_benutzerEinstellungen.getConfig().ZeigeZeitstempel ? "j" : "n")}): ").ToLower();

        userSetting = context.Einstellungen
            .FirstOrDefault(us => us.BenutzerId == akt.Id && us.Schluessel == "ZeigeZeitstempel");

        if (eingabe == "j" || eingabe == "n")
        {
            _benutzerEinstellungen.getConfig().ZeigeZeitstempel = eingabe == "j";
            userSetting.Wert = _benutzerEinstellungen.getConfig().ZeigeZeitstempel ? "j" : "n";
            context.SaveChanges();
        }

        var aktuellerBenutzer = _benutzerManagement.getBenutzer();
        string konfig = "Konfig";
        string KonfigOrdner = Path.Combine(aktuellerBenutzer.Name, konfig);
        if (!Directory.Exists(KonfigOrdner))
        {
            Directory.CreateDirectory(KonfigOrdner);
        }

        _konfigVerwaltung.KonfigurationSpeichern(); // Erstellt die Datei, falls sie nicht existiert

        _help.Write("Konfiguration aktualisiert!");
    }

    /// <summary>
    /// Setzt die Pfade für die Konfigurationsdateien
    /// </summary>
    public void SetzeKonfigDateiPfad()
    {
        var akt = _benutzerManagement.getBenutzer();
        string konfigOrdner = $"Benutzer/{akt.Name}/Konfig";

        if (!Directory.Exists(Path.Combine(konfigOrdner)))
        {
            Directory.CreateDirectory(Path.Combine(konfigOrdner));
        }

        _konfigVerwaltung.SetzeKonfigDateiPfad(konfigOrdner);
    }
}
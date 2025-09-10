using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Models;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class BenutzerManagement : IBenutzerService
{
    private Hilfsfunktionen _help;
    private BenutzerEinstellungen _benutzerEinstellungen;
    private Benutzer _aktuellerBenutzer;

    private TaschenrechnerContext DbContext;

    public void setHelp(Hilfsfunktionen help)
    {
        _help = help;
    }

    public Hilfsfunktionen getHelp()
    {
        return _help;
    }
    public void setBenutzerEinstellungen(BenutzerEinstellungen benutzerEinstellungen)
    {
        _benutzerEinstellungen = benutzerEinstellungen;
    }

    public void BenutzerAnmelden()
    {
        DbContext = new TaschenrechnerContext();
        DbContext.Database.EnsureCreated();

        _help.WriteInfo("\n=== BENUTZER-ANMELDUNG ===");
        string name = _help.Einlesen("Benutzername: ")?.Trim();

        if (string.IsNullOrEmpty(name))
        {
            _help.WriteInfo("Ungültiger Benutzername!");
            return;
        }

        // Benutzer suchen
        var benutzer = DbContext.Benutzer.FirstOrDefault(u => u.Name == name);

        if (benutzer == null)
        {
            // Neuen Benutzer erstellen
            _help.WriteInfo($"Benutzer '{name}' nicht gefunden.");

            if (_help.Einlesen("Neuen Benutzer erstellen? (j/n): ")?.ToLower() == "j")
            {
                benutzer = BenutzerErstellen(name, DbContext);
                _aktuellerBenutzer = benutzer;
            }
        }
        else
        {
            _aktuellerBenutzer = benutzer;
            _benutzerEinstellungen.StandardEinstellungenErstellen(_aktuellerBenutzer.Id, DbContext);
        }
        try
        {
            _help.WriteInfo($"Angemeldet als: {_aktuellerBenutzer.Name}");

        }
        catch (Exception ex)
        {
            _help.WriteError("ACHTUNG! " + ex);
        }

        // Benutzereinstellungen laden
        _benutzerEinstellungen.BenutzereinstellungenLaden();
    }

    public Benutzer BenutzerErstellen(string name, TaschenrechnerContext context)
    {
        string email = _help.Einlesen("Email (optional): ")?.Trim();

        var neuerBenutzer = new Benutzer
        {
            Name = name,
            Email = string.IsNullOrEmpty(email) ? "" : email,
            ErstelltAm = DateTime.Now
        };

        try
        {
            context.Benutzer.Add(neuerBenutzer);
            context.SaveChanges();

            // Standard-Einstellungen erstellen
            _benutzerEinstellungen.StandardEinstellungenErstellen(neuerBenutzer.Id, context);

            _help.WriteInfo($"Benutzer '{name}' erfolgreich erstellt!");
            return neuerBenutzer;
        }
        catch (Exception ex)
        {
            _help.WriteError($"Fehler beim Erstellen des Benutzers: {ex.Message}");
            return null;
        }
    }

    public void BenutzerWechseln()
    {
        BenutzerAnmelden();
    }

    public void BenutzerLöschen()
    {
        _help.WriteInfo("Welchen Benutzer möchtest du löschen?");
        string name = _help.Einlesen("Benutzername: ")?.Trim();

        if (string.IsNullOrEmpty(name))
        {
            _help.WriteInfo("Ungültiger Benutzername!");
            return;
        }

        var benutzer = DbContext.Benutzer.FirstOrDefault(u => u.Name == name);

        if (benutzer == null)
        {
            _help.WriteInfo($"Benutzer '{name}' nicht gefunden.");
            return;
        }

        string bestaetigung = _help.Einlesen($"Bist du sicher, dass du den Benutzer '{benutzer.Name}' und alle zugehörigen Daten löschen möchtest? (j/n): ")?.ToLower();

        if (bestaetigung != "j")
        {
            _help.WriteInfo("Löschung abgebrochen.");
            return;
        }

        try
        {
            // Alle Berechnungen des Benutzers löschen
            var berechnungen = DbContext.Berechnungen.Where(b => b.BenutzerId == benutzer.Id).ToList();
            DbContext.Berechnungen.RemoveRange(berechnungen);
            // Benutzer löschen
            DbContext.Benutzer.Remove(benutzer);
            DbContext.SaveChanges();
            _help.WriteInfo($"Benutzer '{benutzer.Name}' und alle zugehörigen Daten wurden gelöscht.");
        }
        catch (Exception ex)
        {
            _help.WriteError($"Fehler beim Löschen des Benutzers: {ex.Message}");
        }
    }

    public Benutzer getBenutzer()
    {
        return _aktuellerBenutzer;
    }
}
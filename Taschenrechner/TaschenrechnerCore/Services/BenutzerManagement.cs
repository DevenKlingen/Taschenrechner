using TaschenrechnerCore.Models;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class BenutzerManagement
{
    private Hilfsfunktionen _help;
    private BenutzerEinstellungen _benutzerEinstellungen;
    private Benutzer _aktuellerBenutzer;
    private KonfigVerwaltung _konfigVerwaltung;

    private TaschenrechnerContext DbContext;

    public void setKonfigVerwaltung(KonfigVerwaltung konfigVerwaltung)
    {
        _konfigVerwaltung = konfigVerwaltung;
    }

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

        _help.Write("\n=== BENUTZER-ANMELDUNG ===");
        string name = _help.Einlesen("Benutzername: ")?.Trim();

        if (string.IsNullOrEmpty(name))
        {
            _help.Write("Ungültiger Benutzername!");
            return;
        }

        // Benutzer suchen
        var benutzer = DbContext.Benutzer.FirstOrDefault(u => u.Name == name);

        if (benutzer == null)
        {
            // Neuen Benutzer erstellen
            _help.Write($"Benutzer '{name}' nicht gefunden.");

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
            _help.Write($"Angemeldet als: {_aktuellerBenutzer.Name}");

            string benutzerOrdner = Path.Join("Benutzer", _aktuellerBenutzer.Name);
            _konfigVerwaltung.KonfigurationLaden();

            if (!Directory.Exists(benutzerOrdner))
            {
                Directory.CreateDirectory(benutzerOrdner);
            }
        }
        catch (Exception ex)
        {
            _help.Write("ACHTUNG! " + ex);
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

            _help.Write($"Benutzer '{name}' erfolgreich erstellt!");
            return neuerBenutzer;
        }
        catch (Exception ex)
        {
            _help.Write($"Fehler beim Erstellen des Benutzers: {ex.Message}");
            return null;
        }
    }

    public void BenutzerWechseln()
    {
        BenutzerAnmelden();
    }

    public void BenutzerLöschen()
    {
        _help.Write("Welchen Benutzer möchtest du löschen?");
        string name = _help.Einlesen("Benutzername: ")?.Trim();

        if (string.IsNullOrEmpty(name))
        {
            _help.Write("Ungültiger Benutzername!");
            return;
        }

        var benutzer = DbContext.Benutzer.FirstOrDefault(u => u.Name == name);

        if (benutzer == null)
        {
            _help.Write($"Benutzer '{name}' nicht gefunden.");
            return;
        }

        string bestaetigung = _help.Einlesen($"Bist du sicher, dass du den Benutzer '{benutzer.Name}' und alle zugehörigen Daten löschen möchtest? (j/n): ")?.ToLower();

        if (bestaetigung != "j")
        {
            _help.Write("Löschung abgebrochen.");
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
            _help.Write($"Benutzer '{benutzer.Name}' und alle zugehörigen Daten wurden gelöscht.");
        }
        catch (Exception ex)
        {
            _help.Write($"Fehler beim Löschen des Benutzers: {ex.Message}");
        }
    }

    public Benutzer getBenutzer()
    {
        return _aktuellerBenutzer;
    }
}
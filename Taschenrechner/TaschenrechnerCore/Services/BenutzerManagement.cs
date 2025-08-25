using TaschenrechnerCore.Models;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class BenutzerManagement
{
    static Hilfsfunktionen help = new();
    static BenutzerEinstellungen benutzerE = new();
    protected Benutzer aktuellerBenutzer = null;
    private static TaschenrechnerContext DbContext;
    static KonfigVerwaltung konfigV = new();

    public void BenutzerAnmelden()
    {
        DbContext = new TaschenrechnerContext();
        DbContext.Database.EnsureCreated();

        help.Write("=== BENUTZER-ANMELDUNG ===");
        help.Write("Benutzername: ");
        string name = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(name))
        {
            help.Write("Ungültiger Benutzername!");
            return;
        }

        // Benutzer suchen
        var benutzer = DbContext.Benutzer.FirstOrDefault(u => u.Name == name);

        if (benutzer == null)
        {
            // Neuen Benutzer erstellen
            help.Write($"Benutzer '{name}' nicht gefunden.");
            help.Write("Neuen Benutzer erstellen? (j/n): ");

            if (Console.ReadLine()?.ToLower() == "j")
            {
                benutzer = BenutzerErstellen(name, DbContext);
                aktuellerBenutzer = benutzer;
            }
        }
        else
        {
            aktuellerBenutzer = benutzer;
            benutzerE.StandardEinstellungenErstellen(aktuellerBenutzer.Id, DbContext);
        }
        try
        {
            help.Write($"Angemeldet als: {aktuellerBenutzer.Name}");

            string benutzerOrdner = Path.Join("Benutzer", aktuellerBenutzer.Name);
            konfigV.KonfigurationLaden();

            if (!Directory.Exists(benutzerOrdner))
            {
                Directory.CreateDirectory(benutzerOrdner);
            }
        }
        catch (Exception ex)
        {
            help.Write("ACHTUNG! " + ex);
        }
        // Benutzereinstellungen laden
        benutzerE.BenutzereinstellungenLaden();
    }

    static Benutzer BenutzerErstellen(string name, TaschenrechnerContext context)
    {
        help.Write("Email (optional): ");
        string email = Console.ReadLine()?.Trim();

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
            benutzerE.StandardEinstellungenErstellen(neuerBenutzer.Id, context);

            help.Write($"Benutzer '{name}' erfolgreich erstellt!");
            return neuerBenutzer;
        }
        catch (Exception ex)
        {
            help.Write($"Fehler beim Erstellen des Benutzers: {ex.Message}");
            return null;
        }
    }

    public void BenutzerWechseln()
    {
        BenutzerAnmelden();
    }

    public void BenutzerLöschen()
    {
        help.Write("Welchen Benutzer möchtest du löschen?");
        help.Write("Benutzername: ");
        string name = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(name))
        {
            help.Write("Ungültiger Benutzername!");
            return;
        }

        var benutzer = DbContext.Benutzer.FirstOrDefault(u => u.Name == name);

        if (benutzer == null)
        {
            help.Write($"Benutzer '{name}' nicht gefunden.");
            return;
        }

        help.Write($"Bist du sicher, dass du den Benutzer '{benutzer.Name}' und alle zugehörigen Daten löschen möchtest? (j/n): ");
        string bestaetigung = Console.ReadLine()?.ToLower();

        if (bestaetigung != "j")
        {
            help.Write("Löschung abgebrochen.");
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
            help.Write($"Benutzer '{benutzer.Name}' und alle zugehörigen Daten wurden gelöscht.");
        }
        catch (Exception ex)
        {
            help.Write($"Fehler beim Löschen des Benutzers: {ex.Message}");
        }
    }

    public Benutzer getBenutzer()
    {
        return aktuellerBenutzer;
    }
}
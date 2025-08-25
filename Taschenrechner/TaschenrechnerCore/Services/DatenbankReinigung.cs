using TaschenrechnerCore.Utils;
using TaschenrechnerCore.Models;

namespace TaschenrechnerCore.Services;

public class DatenbankReinigung
{
    static Hilfsfunktionen help = new Hilfsfunktionen();
    static BenutzerManagement benutzerManagement = new();

    public void DatenbankBereinigen()
    {
        Benutzer akt = benutzerManagement.getBenutzer();
        if (akt == null)
        {
            help.Write("Kein Benutzer angemeldet!");
            return;
        }

        help.Write("=== DATENBANK BEREINIGEN ===");
        help.Write("Warnung: Diese Aktion löscht alte Berechnungen unwiderruflich!");

        int tage = (int)help.ZahlEinlesen("Berechnungen älter als wie viele Tage löschen? ");

        var grenzDatum = DateTime.Now.AddDays(-tage);

        using var context = new TaschenrechnerContext();

        var zuLoeschendeEintraege = context.Berechnungen
            .Where(b => b.BenutzerId == akt.Id &&
                       b.Zeitstempel < grenzDatum)
            .ToList();

        help.Write($"{zuLoeschendeEintraege.Count} Berechnungen würden gelöscht werden.");
        help.Write("Fortfahren? (ja/nein): ");

        if (Console.ReadLine()?.ToLower() == "ja")
        {
            try
            {
                context.Berechnungen.RemoveRange(zuLoeschendeEintraege);
                context.SaveChanges();
                help.Write($"{zuLoeschendeEintraege.Count} Berechnungen gelöscht.");
            }
            catch (Exception ex)
            {
                help.Write($"Fehler beim Löschen: {ex.Message}");
            }
        }
        else
        {
            help.Write("Bereinigung abgebrochen.");
        }
    }
}
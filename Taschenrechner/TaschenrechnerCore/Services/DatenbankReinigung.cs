using TaschenrechnerCore.Utils;
using TaschenrechnerCore.Models;

namespace TaschenrechnerCore.Services;

public class DatenbankReinigung
{
    private readonly Hilfsfunktionen _help;
    private readonly BenutzerManagement _benutzerManagement;

    public DatenbankReinigung(
        Hilfsfunktionen help, 
        BenutzerManagement benutzerManagement)
    {
        _help = help;
        _benutzerManagement = benutzerManagement;
    }

    public void DatenbankBereinigen()
    {
        Benutzer akt = _benutzerManagement.getBenutzer();
        if (akt == null)
        {
            _help.Write("Kein Benutzer angemeldet!");
            return;
        }

        _help.Write("\n=== DATENBANK BEREINIGEN ===");
        _help.Write("Warnung: Diese Aktion löscht alte Berechnungen unwiderruflich!");

        int tage = (int)_help.ZahlEinlesen("Berechnungen älter als wie viele Tage löschen? ");

        var grenzDatum = DateTime.Now.AddDays(-tage);

        using var context = new TaschenrechnerContext();

        var zuLoeschendeEintraege = context.Berechnungen
            .Where(b => b.BenutzerId == akt.Id &&
                       b.Zeitstempel < grenzDatum)
            .ToList();

        _help.Write($"{zuLoeschendeEintraege.Count} Berechnungen würden gelöscht werden.");
        _help.Write("Fortfahren? (ja/nein): ");

        if (Console.ReadLine()?.ToLower() == "ja")
        {
            try
            {
                context.Berechnungen.RemoveRange(zuLoeschendeEintraege);
                context.SaveChanges();
                _help.Write($"{zuLoeschendeEintraege.Count} Berechnungen gelöscht.");
            }
            catch (Exception ex)
            {
                _help.Write($"Fehler beim Löschen: {ex.Message}");
            }
        }
        else
        {
            _help.Write("Bereinigung abgebrochen.");
        }
    }
}
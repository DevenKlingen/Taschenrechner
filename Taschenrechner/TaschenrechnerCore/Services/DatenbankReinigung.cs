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
            _help.WriteWarning("Kein Benutzer angemeldet!");
            return;
        }

        _help.WriteInfo("\n=== DATENBANK BEREINIGEN ===");
        _help.WriteWarning("Warnung: Diese Aktion löscht alte Berechnungen unwiderruflich!");

        int tage = (int)_help.ZahlEinlesen("Berechnungen älter als wie viele Tage löschen? ");

        var grenzDatum = DateTime.Now.AddDays(-tage);

        using var context = new TaschenrechnerContext();

        var zuLoeschendeEintraege = context.Berechnungen
            .Where(b => b.BenutzerId == akt.Id &&
                       b.Zeitstempel < grenzDatum)
            .ToList();

        _help.WriteInfo($"{zuLoeschendeEintraege.Count} Berechnungen würden gelöscht werden.");
        _help.WriteInfo("Fortfahren? (ja/nein): ");

        if (Console.ReadLine()?.ToLower() == "ja")
        {
            try
            {
                context.Berechnungen.RemoveRange(zuLoeschendeEintraege);
                context.SaveChanges();
                _help.WriteInfo($"{zuLoeschendeEintraege.Count} Berechnungen gelöscht.");
            }
            catch (Exception ex)
            {
                _help.WriteError($"Fehler beim Löschen: {ex.Message}");
            }
        }
        else
        {
            _help.WriteInfo("Bereinigung abgebrochen.");
        }
    }
}
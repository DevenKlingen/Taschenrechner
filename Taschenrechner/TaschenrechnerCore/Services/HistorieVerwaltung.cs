using TaschenrechnerCore.Utils;
using TaschenrechnerCore.Models;

namespace TaschenrechnerCore.Services;

public class HistorieVerwaltung
{
    private const string HISTORIE_DATEI = "berechnungen.txt";
 
    private readonly BenutzerManagement _benutzerManagement;
    private readonly Hilfsfunktionen _help;
    private HistorienExport _historieExport;

    public List<Berechnung> _detaillierteBerechnungen;
    public List<string> _berechnungsHistorie;

    public HistorieVerwaltung(
        BenutzerManagement benutzerManagement, 
        Hilfsfunktionen help)
    {
        _benutzerManagement = benutzerManagement;
        _help = help;
        _detaillierteBerechnungen = new List<Berechnung>();
        _berechnungsHistorie = new List<string>();
    }

    public void setHistorienExport(HistorienExport historienExport)
    {
        _historieExport = historienExport;
    }

    /// <summary>
    /// Speichert die Historie in einer Textdatei
    /// </summary>
    public void HistorieSpeichern()
    {
        try
        {
            var akt = _benutzerManagement.getBenutzer();
            string pfad = Path.Join("Benutzer", akt.Name, "Backups", HISTORIE_DATEI);

            // Prüfe die Größe der Datei im Backup-Ordner
            if (File.Exists(pfad) && new FileInfo(pfad).Length > 1_000_000)
            {
                _historieExport.HistorieAlsZipExportieren(pfad);
                return;
            }

            File.WriteAllLines(pfad, _berechnungsHistorie);
            _help.Write($"Historie gespeichert in {pfad}");
        }
        catch (Exception ex)
        {
            _help.Write($"Fehler beim Speichern: {ex.Message}");
        }
    }

    /// <summary>
    /// Löscht die Historie und alle dazugehörigen dateien
    /// </summary>
    public void HistorieLöschen()
    {
        try
        {
            // Benutzerverzeichnis ermitteln
            var akt = _benutzerManagement.getBenutzer();
            string benutzerVerzeichnis = Path.Join("Benutzer", akt.Name, "Backups");

            // Allerelevanten Dateinamen/Pattern
            string[] muster = new[]
            {
                    "berechnungen*.txt",
                    "berechnungen*.xml",
                    "berechnungen*.csv",
                    "berechnungen*.zip"
                };

            int geloescht = 0;
            foreach (var pattern in muster)
            {
                string[] dateien = Directory.GetFiles(benutzerVerzeichnis, pattern);
                foreach (var datei in dateien)
                {
                    try
                    {
                        File.Delete(datei);
                        _help.Write($"Datei gelöscht:{Path.GetFileName(datei)}");
                        geloescht++;
                    }
                    catch (Exception ex)
                    {
                        _help.Write($"Fehler beim Löschen von{datei}: {ex.Message}");
                    }
                }
            }

            _berechnungsHistorie.Clear();
            if (geloescht == 0)
                _help.Write("Keine Historie-Dateien gefunden.");
            else
                _help.Write("Alle Historie-Dateien wurden gelöscht!");

            string eingabe = _help.Einlesen("Möchtest du auch die Datenbankhistorie löschen? (j/n)")?.ToLower();

            if (eingabe == "j")
            {
                using var context = new TaschenrechnerContext();
                var berechnungen = context.Berechnungen.ToList();
                context.Berechnungen.RemoveRange(berechnungen);
                context.SaveChanges();
                _help.Write("Datenbankhistorie gelöscht.");
            }
            else if (eingabe != "n")
            {
                _help.Write("Ungültige Eingabe! Es wird keine Datenbankhistorie gelöscht.");
            }
        }
        catch (Exception ex)
        {
            _help.Write($"Fehler beim Löschen:{ex.Message}");
        }
    }

    /// <summary>
    /// Lädt die Historie aus einer Datei
    /// </summary>
    public void HistorieLaden()
    {
        try
        {
            if (File.Exists(HISTORIE_DATEI))
            {
                string[] zeilen = File.ReadAllLines(HISTORIE_DATEI);
                _berechnungsHistorie.AddRange(zeilen);
                _help.Write($"{zeilen.Length} Einträge aus Historie geladen.");
            }
            else
            {
                _help.Write("Keine gespeicherteHistorie gefunden.");
            }
        }
        catch (Exception ex)
        {
            _help.Write($"Fehler beim Laden: {ex.Message}");
        }
    }

    public string getHistorieDatei()
    {
        return HISTORIE_DATEI;
    }
}
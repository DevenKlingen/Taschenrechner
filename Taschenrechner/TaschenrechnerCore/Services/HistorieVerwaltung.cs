using TaschenrechnerCore.Utils;
using TaschenrechnerCore.Models;

namespace TaschenrechnerCore.Services;

public class HistorieVerwaltung
{
    static string historieDatei = "berechnungen.txt";
    static BenutzerManagement benutzerManagement = new();
    static Hilfsfunktionen help = new Hilfsfunktionen();
    static HistorienExport historieE = new HistorienExport();
    public List<Berechnung> detaillierteBerechnungen = new List<Berechnung>();
    public List<string> berechnungsHistorie = new List<string>();

    /// <summary>
    /// Speichert die Historie in einer Textdatei
    /// </summary>
    public void HistorieSpeichern()
    {
        try
        {
            var akt = benutzerManagement.getBenutzer();
            string pfad = Path.Join("Benutzer", akt.Name, "Backups", historieDatei);

            // Prüfe die Größe der Datei im Backup-Ordner
            if (File.Exists(pfad) && new FileInfo(pfad).Length > 1_000_000)
            {
                historieE.HistorieAlsZipExportieren(pfad);
                return;
            }

            File.WriteAllLines(pfad, berechnungsHistorie);
            help.Write($"Historie gespeichert in {pfad}");
        }
        catch (Exception ex)
        {
            help.Write($"Fehler beim Speichern: {ex.Message}");
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
            var akt = benutzerManagement.getBenutzer();
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
                        help.Write($"Datei gelöscht:{Path.GetFileName(datei)}");
                        geloescht++;
                    }
                    catch (Exception ex)
                    {
                        help.Write($"Fehler beim Löschen von{datei}: {ex.Message}");
                    }
                }
            }

            berechnungsHistorie.Clear();
            if (geloescht == 0)
                help.Write("Keine Historie-Dateien gefunden.");
            else
                help.Write("Alle Historie-Dateien wurden gelöscht!");

            help.Write("Möchtest du auch die Datenbankhistorie löschen? (j/n)");
            string eingabe = Console.ReadLine()?.ToLower();

            if (eingabe == "j")
            {
                using var context = new TaschenrechnerContext();
                var berechnungen = context.Berechnungen.ToList();
                context.Berechnungen.RemoveRange(berechnungen);
                context.SaveChanges();
                help.Write("Datenbankhistorie gelöscht.");
            }
            else if (eingabe != "n")
            {
                help.Write("Ungültige Eingabe! Es wird keine Datenbankhistorie gelöscht.");
            }
        }
        catch (Exception ex)
        {
            help.Write($"Fehler beim Löschen:{ex.Message}");
        }
    }

    /// <summary>
    /// Lädt die Historie aus einer Datei
    /// </summary>
    public void HistorieLaden()
    {
        try
        {
            if (File.Exists(historieDatei))
            {
                string[] zeilen = File.ReadAllLines(historieDatei);
                berechnungsHistorie.AddRange(zeilen);
                help.Write($"{zeilen.Length} Einträge aus Historie geladen.");
            }
            else
            {
                help.Write("Keine gespeicherteHistorie gefunden.");
            }
        }
        catch (Exception ex)
        {
            help.Write($"Fehler beim Laden: {ex.Message}");
        }
    }

    public string getHistorieDatei()
    {
        return historieDatei;
    }
}
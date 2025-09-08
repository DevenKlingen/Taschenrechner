using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class Backup
{
    private readonly BenutzerManagement _benutzerManagement;
    private readonly Hilfsfunktionen _help;
    private readonly HistorienExport _historienExport;

    public Backup(
        BenutzerManagement benutzerManagement,
        Hilfsfunktionen help,
        HistorienExport historienExport)
    {
        _benutzerManagement = benutzerManagement;
        _help = help;
        _historienExport = historienExport;
    }

    /// <summary>
    /// Erstellt ein Backup der aktuellen Historie und speichert es in einem Backup-Ordner
    /// </summary>
    public void BackupErstellen()
    {
        try
        {
            var akt = _benutzerManagement.getBenutzer();

            string? datenbankBackupWahl = _help.Einlesen("Möchtest du auch ein Datenbank-Backup erstellen? (j/n)")?.Trim().ToLower();
            if (datenbankBackupWahl == "j")
            {
                DatenbankBackup();
            }
            else if (datenbankBackupWahl != "n")
            {
                _help.Write("Ungültige Eingabe! Es wird kein Datenbank-Backup erstellt.");
            }

            string zeitstempel = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string backupOrdner = Path.Join("Benutzer", akt.Name, "Backups");

            _help.Write($"Backupordner: {backupOrdner}");

            // Backup-Ordner erstellen, falls nicht vorhanden
            if (!Directory.Exists(backupOrdner))
            {
                Directory.CreateDirectory(backupOrdner);
            }

            string[] zuSicherndeDateien = {
            Path.Combine(akt.Name, "Backups", "berechnungen.txt")
            };

            foreach (string datei in zuSicherndeDateien)
            {
                if (File.Exists(datei))
                {
                    string dateiName = Path.GetFileNameWithoutExtension(datei);
                    string erweiterung = Path.GetExtension(datei);
                    string backupDatei = Path.Combine(backupOrdner, $"{dateiName}_{zeitstempel}{erweiterung}");

                    File.Copy(datei, backupDatei);
                    _help.Write($"Backup erstellt: {backupDatei}");
                }
            }

            // XML-Export als zusätzliches Backup
            _historienExport.HistorieAlsXMLExportieren();
            string xmlDatei = Path.Combine(akt.Name, "Backups", "berechnungen.xml");
            string xmlBackup = Path.Combine(backupOrdner, $"berechnungen_{zeitstempel}.xml");
            if (File.Exists(xmlDatei))
            {
                File.Copy(xmlDatei, xmlBackup);
            }
        }
        catch (Exception ex)
        {
            _help.Write($"Fehler beim Backup: {ex.Message}");
        }
    }

    public void DatenbankBackup()
    {
        try
        {
            string quellDatei = "taschenrechner.db";
            string backupDatei = $"backup_taschenrechner_{DateTime.Now:yyyyMMdd_HHmmss}.db";

            if (File.Exists(quellDatei))
            {
                File.Copy(quellDatei, backupDatei);
                _help.Write($"Datenbank-Backup erstellt: {backupDatei}");

                // Dateigröße anzeigen
                var info = new FileInfo(backupDatei);
                _help.Write($"Backup-Größe: {info.Length / 1024.0:F1} KB");
            }
            else
            {
                _help.Write("Keine Datenbank-Datei gefunden!");
            }
        }
        catch (Exception ex)
        {
            _help.Write($"Fehler beim Backup: {ex.Message}");
        }
    }
}
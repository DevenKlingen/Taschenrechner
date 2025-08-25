using TaschenrechnerConsole;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class Backup
{
    static Program program = new Program();
    static Hilfsfunktionen help = new Hilfsfunktionen();
    static HistorienExport historienEx = new HistorienExport();

    /// <summary>
    /// Erstellt ein Backup der aktuellen Historie und speichert es in einem Backup-Ordner
    /// </summary>
    public void BackupErstellen()
    {
        try
        {
            var akt = program.getAktBenutzer();

            help.Write("Möchtest du auch ein Datenbank-Backup erstellen? (j/n)");
            string? datenbankBackupWahl = Console.ReadLine()?.Trim().ToLower();
            if (datenbankBackupWahl == "j")
            {
                DatenbankBackup();
            }
            else if (datenbankBackupWahl != "n")
            {
                help.Write("Ungültige Eingabe! Es wird kein Datenbank-Backup erstellt.");
            }

            string zeitstempel = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string backupOrdner = Path.Join("Benutzer", akt.Name, "Backups");

            Console.WriteLine($"Backupordner: {backupOrdner}");

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
                    help.Write($"Backup erstellt: {backupDatei}");
                }
            }

            // XML-Export als zusätzliches Backup
            historienEx.HistorieAlsXMLExportieren();
            string xmlDatei = Path.Combine(program.benutzer, "Backups", "berechnungen.xml");
            string xmlBackup = Path.Combine(backupOrdner, $"berechnungen_{zeitstempel}.xml");
            if (File.Exists(xmlDatei))
            {
                File.Copy(xmlDatei, xmlBackup);
            }
        }
        catch (Exception ex)
        {
            help.Write($"Fehler beim Backup: {ex.Message}");
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
                help.Write($"Datenbank-Backup erstellt: {backupDatei}");

                // Dateigröße anzeigen
                var info = new FileInfo(backupDatei);
                help.Write($"Backup-Größe: {info.Length / 1024.0:F1} KB");
            }
            else
            {
                help.Write("Keine Datenbank-Datei gefunden!");
            }
        }
        catch (Exception ex)
        {
            help.Write($"Fehler beim Backup: {ex.Message}");
        }
    }
}
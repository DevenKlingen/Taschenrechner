using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class Backup
{
    private readonly Hilfsfunktionen _help;

    public Backup(Hilfsfunktionen help)
    {
        _help = help;
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
                _help.WriteInfo($"Datenbank-Backup erstellt: {backupDatei}");

                // Dateigröße anzeigen
                var info = new FileInfo(backupDatei);
                _help.WriteInfo($"Backup-Größe: {info.Length / 1024.0:F1} KB");
            }
            else
            {
                _help.WriteWarning("Keine Datenbank-Datei gefunden!");
            }
        }
        catch (Exception ex)
        {
            _help.WriteError($"Fehler beim Backup: {ex.Message}");
        }
    }
}
using TaschenrechnerConsole;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class BackupsVerwalten
{
    static Program program = new Program();
    static Hilfsfunktionen help = new Hilfsfunktionen();
    static KonfigVerwaltung konfigV = new KonfigVerwaltung();
    static HistorienBearbeitung historieB = new HistorienBearbeitung();

    /// <summary>
    /// Zeigt alle verfügbaren Backups im Backup-Ordner an
    /// </summary>
    public void BackupsAnzeigen()
    {
        try
        {
            var akt = program.getAktBenutzer();
            string backup = "Backups";
            string backupOrdner = Path.Join("Benutzer", akt.Name, backup);

            if (!Directory.Exists(backupOrdner))
            {
                help.Write("Kein Backup-Ordner vorhanden.");
                return;
            }

            string[] backupDateien = Directory.GetFiles(backupOrdner);

            if (backupDateien.Length == 0)
            {
                help.Write("Keine Backups vorhanden.");
                return;
            }

            help.Write("=== VERFÜGBARE BACKUPS ===");
            for (int i = 0; i < backupDateien.Length; i++)
            {
                FileInfo info = new FileInfo(backupDateien[i]);
                help.Write($"{i + 1}. {info.Name} ({info.LastWriteTime:dd.MM.yyyy HH:mm})");
            }
        }
        catch (Exception ex)
        {
            help.Write($"Fehler beim Anzeigen der Backups: {ex.Message}");
        }
    }

    /// <summary>
    /// Löscht alle Backups im Backup-Ordner
    /// </summary>
    public void BackupsLöschen()
    {
        try
        {
            var akt = program.getAktBenutzer();
            string backup = "Backups";
            string backupOrdner = Path.Join("Benutzer", akt.Name, backup);

            if (!Directory.Exists(backupOrdner))
            {
                help.Write("Kein Backup-Ordner vorhanden.");
                return;
            }
            foreach (string datei in Directory.GetFiles(backupOrdner))
            {
                File.Delete(datei);
            }
            help.Write("Alle Backups wurden gelöscht.");
        }
        catch (Exception ex)
        {
            help.Write($"Fehler beim Löschen der Backups: {ex.Message}");
        }
    }

    /// <summary>
    /// Stellt ein ausgewähltes Backup wieder her, indem es die Historie-Datei ersetzt
    /// </summary>
    public void BackupWiederherstellen()
    {
        try
        {
            var akt = program.getAktBenutzer();
            string backup = "Backups";
            string backupOrdner = Path.Join("Benutzer", akt.Name, backup);

            if (!Directory.Exists(backupOrdner))
            {
                help.Write("Kein Backup-Ordner vorhanden.");
                return;
            }
            string[] backupDateien = Directory.GetFiles(backupOrdner);
            if (backupDateien.Length == 0)
            {
                help.Write("Keine Backups vorhanden.");
                return;
            }
            help.Write("=== VERFÜGBARE BACKUPS ZUM WIEDERHERSTELLEN ===");
            for (int i = 0; i < backupDateien.Length; i++)
            {
                FileInfo info = new FileInfo(backupDateien[i]);
                help.Write($"{i + 1}. {info.Name} ({info.LastWriteTime:dd.MM.yyyy HH:mm})");
            }
            help.Write("Wähle ein Backup zum Wiederherstellen (1-" + backupDateien.Length + "): ");
            int wahl = help.MenuWahlEinlesen();
            if (wahl < 1 || wahl > backupDateien.Length)
            {
                help.Write("Ungültige Wahl!");
                return;
            }
            string gewaehltesBackup = backupDateien[wahl - 1];
            File.Copy(gewaehltesBackup, program.historieDatei, true);
            konfigV.KonfigurationLaden(); // Konfiguration neu laden
            historieB.HistorieHinzufuegen($"Backup wiederhergestellt: {gewaehltesBackup}");

            help.Write($"Backup wiederhergestellt: {gewaehltesBackup}");
        }
        catch (Exception ex)
        {
            help.Write($"Fehler beim Wiederherstellen des Backups: {ex.Message}");
        }
    }
}
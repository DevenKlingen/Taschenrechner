using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class BackupsVerwalten
{
    private readonly Hilfsfunktionen _help;
    private readonly KonfigVerwaltung _konfigVerwaltung;
    private readonly HistorienBearbeitung _historienBearbeitung;
    private readonly BenutzerManagement _benutzerManagement;
    private readonly HistorieVerwaltung _historieVerwaltung;

    public BackupsVerwalten(
        Hilfsfunktionen help,
        KonfigVerwaltung konfigVerwaltung,
        HistorienBearbeitung historienBearbeitung,
        BenutzerManagement benutzerManagement,
        HistorieVerwaltung historieVerwaltung)
    {
        _help = help;
        _konfigVerwaltung = konfigVerwaltung;
        _historienBearbeitung = historienBearbeitung;
        _benutzerManagement = benutzerManagement;
        _historieVerwaltung = historieVerwaltung;
    }

    /// <summary>
    /// Zeigt alle verfügbaren Backups im Backup-Ordner an
    /// </summary>
    public void BackupsAnzeigen()
    {
        try
        {
            var akt = _benutzerManagement.getBenutzer();
            string backup = "Backups";
            string backupOrdner = Path.Join("Benutzer", akt.Name, backup);

            if (!Directory.Exists(backupOrdner))
            {
                _help.Write("Kein Backup-Ordner vorhanden.");
                return;
            }

            string[] backupDateien = Directory.GetFiles(backupOrdner);

            if (backupDateien.Length == 0)
            {
                _help.Write("Keine Backups vorhanden.");
                return;
            }

            _help.Write("\n=== VERFÜGBARE BACKUPS ===");
            for (int i = 0; i < backupDateien.Length; i++)
            {
                FileInfo info = new FileInfo(backupDateien[i]);
                _help.Write($"{i + 1}. {info.Name} ({info.LastWriteTime:dd.MM.yyyy HH:mm})");
            }
        }
        catch (Exception ex)
        {
            _help.Write($"Fehler beim Anzeigen der Backups: {ex.Message}");
        }
    }

    /// <summary>
    /// Löscht alle Backups im Backup-Ordner
    /// </summary>
    public void BackupsLöschen()
    {
        try
        {
            var akt = _benutzerManagement.getBenutzer();
            string backup = "Backups";
            string backupOrdner = Path.Join("Benutzer", akt.Name, backup);

            if (!Directory.Exists(backupOrdner))
            {
                _help.Write("Kein Backup-Ordner vorhanden.");
                return;
            }
            foreach (string datei in Directory.GetFiles(backupOrdner))
            {
                File.Delete(datei);
            }
            _help.Write("Alle Backups wurden gelöscht.");
        }
        catch (Exception ex)
        {
            _help.Write($"Fehler beim Löschen der Backups: {ex.Message}");
        }
    }

    /// <summary>
    /// Stellt ein ausgewähltes Backup wieder her, indem es die Historie-Datei ersetzt
    /// </summary>
    public void BackupWiederherstellen()
    {
        try
        {
            var akt = _benutzerManagement.getBenutzer();
            string backup = "Backups";
            string backupOrdner = Path.Join("Benutzer", akt.Name, backup);

            if (!Directory.Exists(backupOrdner))
            {
                _help.Write("Kein Backup-Ordner vorhanden.");
                return;
            }
            string[] backupDateien = Directory.GetFiles(backupOrdner);
            if (backupDateien.Length == 0)
            {
                _help.Write("Keine Backups vorhanden.");
                return;
            }
            _help.Write("\n=== VERFÜGBARE BACKUPS ZUM WIEDERHERSTELLEN ===");
            for (int i = 0; i < backupDateien.Length; i++)
            {
                FileInfo info = new FileInfo(backupDateien[i]);
                _help.Write($"{i + 1}. {info.Name} ({info.LastWriteTime:dd.MM.yyyy HH:mm})");
            }
            _help.Write("Wähle ein Backup zum Wiederherstellen (1-" + backupDateien.Length + "): ");
            int wahl = _help.MenuWahlEinlesen();
            if (wahl < 1 || wahl > backupDateien.Length)
            {
                _help.Write("Ungültige Wahl!");
                return;
            }
            string gewaehltesBackup = backupDateien[wahl - 1];
            File.Copy(gewaehltesBackup, _historieVerwaltung.getHistorieDatei(), true);
            _konfigVerwaltung.KonfigurationLaden(); // Konfiguration neu laden
            _historienBearbeitung.HistorieHinzufuegen($"Backup wiederhergestellt: {gewaehltesBackup}");

            _help.Write($"Backup wiederhergestellt: {gewaehltesBackup}");
        }
        catch (Exception ex)
        {
            _help.Write($"Fehler beim Wiederherstellen des Backups: {ex.Message}");
        }
    }
}
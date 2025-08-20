namespace MeinTaschenrechner
{
    internal class Backupmenu
    {
        static Program programm = new Program();
        static Hilfsfunktionen help = new Hilfsfunktionen();
        static Historiemenu historiemenu = new Historiemenu();
        static Konfigmenu konfigmenu = new Konfigmenu();
        static Datenbankmenu datenbankmenu = new Datenbankmenu();

        /// <summary>
        /// Zeigt das Menü für die Backup-Funktionen an und wertet die Eingabe aus
        /// </summary>
        public void BackupMenu()
        {
            bool BackupMenuAktiv = true;
            while (BackupMenuAktiv)
            {
                help.Mischen();

                help.Write("\n=== BACKUPS ===");
                help.Write("1. Backup erstellen");
                help.Write("2. Backups anzeigen");
                help.Write("3. Backup wiederherstellen");
                help.Write("4. Backups löschen");
                help.Write("5. Zurück zum Hauptmenü");
                help.Write("Deine Wahl (1-4): ");
                int wahl = help.MenuWahlEinlesen();
                switch (wahl)
                {
                    case 1:
                        BackupErstellen();
                        break;
                    case 2:
                        BackupsAnzeigen();
                        break;
                    case 3:
                        BackupWiederherstellen();
                        break;
                    case 4:
                        BackupsLöschen();
                        break;
                    case 5:
                        BackupMenuAktiv = false;
                        help.Write("Zurück zum Hauptmenü.");
                        break;
                    default:
                        help.Write("Ungültige Wahl!");
                        break;
                }

                if (BackupMenuAktiv)
                {
                    help.Write("\nDrücke Enter für Menü...");
                    Console.ReadLine();
                }
            }
        }

        /// <summary>
        /// Erstellt ein Backup der aktuellen Historie und speichert es in einem Backup-Ordner
        /// </summary>
        public void BackupErstellen()
        {
            try
            {
                var akt = programm.getAktBenutzer();

                help.Write("Möchtest du auch ein Datenbank-Backup erstellen? (j/n)");
                string? datenbankBackupWahl = Console.ReadLine()?.Trim().ToLower();
                if (datenbankBackupWahl == "j")
                {
                    datenbankmenu.DatenbankBackup();
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
            Path.Combine(programm.benutzer, "Backups", "berechnungen.txt")
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
                historiemenu.HistorieAlsXMLExportieren();
                string xmlDatei = Path.Combine(programm.benutzer, "Backups", "berechnungen.xml");
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

        /// <summary>
        /// Zeigt alle verfügbaren Backups im Backup-Ordner an
        /// </summary>
        static void BackupsAnzeigen()
        {
            try
            {
                var akt = programm.getAktBenutzer();
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
        /// Stellt ein ausgewähltes Backup wieder her, indem es die Historie-Datei ersetzt
        /// </summary>
        static void BackupWiederherstellen()
        {
            try
            {
                var akt = programm.getAktBenutzer();
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
                File.Copy(gewaehltesBackup, programm.historieDatei, true);
                konfigmenu.KonfigurationLaden(); // Konfiguration neu laden
                historiemenu.HistorieHinzufuegen($"Backup wiederhergestellt: {gewaehltesBackup}");

                help.Write($"Backup wiederhergestellt: {gewaehltesBackup}");
            }
            catch (Exception ex)
            {
                help.Write($"Fehler beim Wiederherstellen des Backups: {ex.Message}");
            }
        }

        /// <summary>
        /// Löscht alle Backups im Backup-Ordner
        /// </summary>
        static void BackupsLöschen()
        {
            try
            {
                var akt = programm.getAktBenutzer();
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
    }
}
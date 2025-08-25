using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class BackupMenu : IMenu
{
    static Hilfsfunktionen help = new Hilfsfunktionen();
    static BackupsVerwalten backupsVerwalten = new BackupsVerwalten();
    static Backup backup = new Backup();
    public void Show()
    {
        bool BackupMenuAktiv = true;
        while (BackupMenuAktiv)
        {
            help.Write("\n=== BACKUPS ===");
            help.Write("1. Backup erstellen");
            help.Write("2. Backups anzeigen");
            help.Write("3. Backup wiederherstellen");
            help.Write("4. Backups löschen");
            help.Write("0. Zurück zum Hauptmenü");
            help.Write("Deine Wahl (1-4): ");
            int wahl = help.MenuWahlEinlesen();
            switch (wahl)
            {
                case 1:
                    backup.BackupErstellen();
                    break;
                case 2:
                    backupsVerwalten.BackupsAnzeigen();
                    break;
                case 3:
                    backupsVerwalten.BackupWiederherstellen();
                    break;
                case 4:
                    backupsVerwalten.BackupsLöschen();
                    break;
                case 0:
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

    public string GetMenuTitle(int optionTitle)
    {
        return $"{optionTitle}. Backup-Menü";
    }
}
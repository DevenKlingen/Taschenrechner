using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class BackupMenu : IMenu
{
    private readonly Hilfsfunktionen _help;
    private readonly BackupsVerwalten _backupsVerwalten;
    private readonly Backup _backup;

    public BackupMenu(
        Hilfsfunktionen help,
        BackupsVerwalten backupsVerwalten,
        Backup backup)
    {
        _help = help;
        _backup = backup;
        _backupsVerwalten = backupsVerwalten;
    }

    public void Show()
    {
        bool BackupMenuAktiv = true;
        while (BackupMenuAktiv)
        {
            _help.Write("\n=== BACKUPS ===");
            _help.Write("1. Backup erstellen");
            _help.Write("2. Backups anzeigen");
            _help.Write("3. Backup wiederherstellen");
            _help.Write("4. Backups löschen");
            _help.Write("0. Zurück zum Hauptmenü");
            _help.Write("Deine Wahl (1-4): ");
            int wahl = _help.MenuWahlEinlesen();
            switch (wahl)
            {
                case 1:
                    _backup.BackupErstellen();
                    break;
                case 2:
                    _backupsVerwalten.BackupsAnzeigen();
                    break;
                case 3:
                    _backupsVerwalten.BackupWiederherstellen();
                    break;
                case 4:
                    _backupsVerwalten.BackupsLöschen();
                    break;
                case 0:
                    BackupMenuAktiv = false;
                    _help.Write("Zurück zum Hauptmenü.");
                    break;
                default:
                    _help.Write("Ungültige Wahl!");
                    break;
            }

            if (BackupMenuAktiv)
            {
                _help.Einlesen("\nDrücke Enter für Menü...");
            }
        }
    }

    public string GetMenuTitle(int optionTitle)
    {
        return $"{optionTitle}. Backup-Menü";
    }
}
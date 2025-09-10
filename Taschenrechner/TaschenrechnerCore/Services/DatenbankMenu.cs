using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class DatenbankMenu : IMenu
{
    private readonly Hilfsfunktionen _help;
    private readonly DatenbankHistorie _datenbankHistorie;
    private readonly DatenbankBerechnungen _datenbankBerechnungen;
    private readonly DatenbankReinigung _datenbankReinigung;
    private readonly Backup _backup;
    private readonly BenutzerManagement _benutzerManagement;

    public DatenbankMenu(
        Hilfsfunktionen help, 
        DatenbankHistorie datenbankHistorie, 
        DatenbankBerechnungen datenbankBerechnungen, 
        DatenbankReinigung datenbankReinigung, 
        BenutzerManagement benutzerManagement,
        Backup backup)
    {
        _help = help;
        _datenbankHistorie = datenbankHistorie;
        _datenbankBerechnungen = datenbankBerechnungen;
        _datenbankReinigung = datenbankReinigung;
        _benutzerManagement = benutzerManagement;
        _backup = backup;
    }

    public void Show()
    {
        bool datenbankMenuAktiv = true;
        while (datenbankMenuAktiv)
        {
            _help.Mischen();
            _help.WriteInfo("\n=== DATENBANK-MENÜ ===");
            _help.WriteInfo("1. Datenbank-Historie anzeigen");
            _help.WriteInfo("2. Berechnungen suchen");
            _help.WriteInfo("3. Datenbank bereinigen");
            _help.WriteInfo("4. Datenbank Backup");
            _help.WriteInfo("5. Import-Export-Service");
            _help.WriteInfo("0. Zurück zum Hauptmenü");
            int wahl = (int)_help.ZahlEinlesen("Deine Wahl: ");

            switch (wahl)
            {
                case 1:
                    _datenbankHistorie.HistorieAnzeigen();
                    break;
                case 2:
                    _datenbankBerechnungen.BerechnungenSuchen();
                    break;
                case 3:
                    _datenbankReinigung.DatenbankBereinigen();
                    break;
                case 4:
                    _backup.DatenbankBackup();
                    break;
                case 5:
                    ImportExportMenu _importExportMenu = new ImportExportMenu(_help, new ImportExportService(_help, _benutzerManagement, _datenbankBerechnungen));
                    _importExportMenu.Show();
                    break;
                case 0:
                    datenbankMenuAktiv = false;
                    _help.WriteInfo("Zurück zum Hauptmenü.");
                    break;
                default:
                    _help.WriteInfo("Ungültige Wahl!");
                    break;
            }
            if (datenbankMenuAktiv)
            {
                _help.WriteInfo("\nDrücke Enter für Menü...");
                Console.ReadLine();
            }
        }
    }

    public string GetMenuTitle(int optionTitle)
    {
        return $"{optionTitle}. Datenbank-Menu";
    }
}
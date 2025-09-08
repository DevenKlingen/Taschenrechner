using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class DatenbankMenu : IMenu
{
    private readonly Hilfsfunktionen _help;
    private readonly DatenbankHistorie _datenbankHistorie;
    private readonly DatenbankBerechnungen _datenbankBerechnungen;
    private readonly DatenbankExport _datenbankExport;
    private readonly DatenbankReinigung _datenbankReinigung;
    private readonly Backup _backup;

    public DatenbankMenu(
        Hilfsfunktionen help, 
        DatenbankHistorie datenbankHistorie, 
        DatenbankBerechnungen datenbankBerechnungen, 
        DatenbankExport datenbankExport, 
        DatenbankReinigung datenbankReinigung, 
        Backup backup)
    {
        _help = help;
        _datenbankHistorie = datenbankHistorie;
        _datenbankBerechnungen = datenbankBerechnungen;
        _datenbankExport = datenbankExport;
        _datenbankReinigung = datenbankReinigung;
        _backup = backup;
    }

    public void Show()
    {
        bool datenbankMenuAktiv = true;
        while (datenbankMenuAktiv)
        {
            _help.Mischen();
            _help.Write("\n=== DATENBANK-MENÜ ===");
            _help.Write("1. Datenbank-Historie anzeigen");
            _help.Write("2. Berechnungen suchen");
            _help.Write("3. Datenbank exportieren");
            _help.Write("4. Datenbank bereinigen");
            _help.Write("5. Datenbank Backup");
            _help.Write("6. Zurück zum Hauptmenü");
            _help.Write("Deine Wahl (1-6): ");
            int wahl = _help.MenuWahlEinlesen();
            switch (wahl)
            {
                case 1:
                    _datenbankHistorie.DatenbankHistorieAnzeigen();
                    break;
                case 2:
                    _datenbankBerechnungen.BerechnungenSuchen();
                    break;
                case 3:
                    _datenbankExport.DatenbankExportieren();
                    break;
                case 4:
                    _datenbankReinigung.DatenbankBereinigen();
                    break;
                case 5:
                    _backup.DatenbankBackup();
                    break;
                case 6:
                    datenbankMenuAktiv = false;
                    _help.Write("Zurück zum Hauptmenü.");
                    break;
                default:
                    _help.Write("Ungültige Wahl!");
                    break;
            }
            if (datenbankMenuAktiv)
            {
                _help.Write("\nDrücke Enter für Menü...");
                Console.ReadLine();
            }
        }
    }

    public string GetMenuTitle(int optionTitle)
    {
        return $"{optionTitle}. Datenbank-Menu";
    }
}
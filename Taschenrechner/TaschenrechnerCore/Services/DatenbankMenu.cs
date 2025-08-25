using TaschenrechnerConsole;
using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class DatenbankMenu : IMenu
{
    static Program program = new Program();
    static Hilfsfunktionen help = new Hilfsfunktionen();
    static DatenbankHistorie datenbankH = new DatenbankHistorie();
    static DatenbankBerechnungen datenbankB = new DatenbankBerechnungen();
    static DatenbankExport datenbankE = new DatenbankExport();
    static DatenbankReinigung datenbankR = new DatenbankReinigung();
    static Backup backup = new Backup();

    public void Show()
    {
        bool datenbankMenuAktiv = true;
        while (datenbankMenuAktiv)
        {
            help.Mischen();
            help.Write("\n=== DATENBANK-MENÜ ===");
            help.Write("1. Datenbank-Historie anzeigen");
            help.Write("2. Berechnungen suchen");
            help.Write("3. Datenbank exportieren");
            help.Write("4. Datenbank bereinigen");
            help.Write("5. Datenbank Backup");
            help.Write("6. Zurück zum Hauptmenü");
            help.Write("Deine Wahl (1-6): ");
            int wahl = help.MenuWahlEinlesen();
            switch (wahl)
            {
                case 1:
                    datenbankH.DatenbankHistorieAnzeigen();
                    break;
                case 2:
                    datenbankB.BerechnungenSuchen();
                    break;
                case 3:
                    datenbankE.DatenbankExportieren();
                    break;
                case 4:
                    datenbankR.DatenbankBereinigen();
                    break;
                case 5:
                    backup.DatenbankBackup();
                    break;
                case 6:
                    datenbankMenuAktiv = false;
                    help.Write("Zurück zum Hauptmenü.");
                    break;
                default:
                    help.Write("Ungültige Wahl!");
                    break;
            }
            if (datenbankMenuAktiv)
            {
                help.Write("\nDrücke Enter für Menü...");
                Console.ReadLine();
            }
        }
    }

    public string GetMenuTitle(int optionTitle)
    {
        return $"{optionTitle}. Datenbank Menu";
    }
}
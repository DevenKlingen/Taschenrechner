using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class HistorienMenu : IMenu
{
    private readonly Hilfsfunktionen _help;
    private readonly HistorieImport _historieImport;
    private readonly HistorienExport _historieExport;
    private readonly HistorieVerwaltung _historieVerwaltung;
    private readonly HistorieZeigen _historieZeigen;

    public HistorienMenu(Hilfsfunktionen help, HistorieImport historieI, HistorienExport historieE, HistorieVerwaltung historieV, HistorieZeigen historieZ)
    {
        _help = help;
        _historieImport = historieI;
        _historieExport = historieE;
        _historieVerwaltung = historieV;
        _historieZeigen = historieZ;
    }



    /// <summary>
    /// Zeigt das Historiemenü an, wertet die Eingabe aus und führt die dazugehörige Funktion aus
    /// </summary>
    public void Show()
    {
        bool historieMenuAktiv = true;
        while (historieMenuAktiv)
        {
            _help.Mischen();

            _help.Write("\n=== HISTORIE ===");
            _help.Write("1. Historie ansehen");
            _help.Write("2. Historie speichern");
            _help.Write("3. Historie löschen");
            _help.Write("4. Als XML Exportieren");
            _help.Write("5. Von XML Importieren");
            _help.Write("6. Als CSV Exportieren");
            _help.Write("7. Zurück zum Hauptmenü");
            _help.Write("Deine Wahl (1-7): ");
            int wahl = _help.MenuWahlEinlesen();
            switch (wahl)
            {
                case 1:
                    _historieZeigen.HistorieAnzeigen();
                    break;
                case 2:
                    _historieVerwaltung.HistorieSpeichern();
                    break;
                case 3:
                    _historieVerwaltung.HistorieLöschen();
                    break;
                case 4:
                    _historieExport.HistorieAlsXMLExportieren();
                    break;
                case 5:
                    _historieImport.HistorieVonXMLImportieren();
                    break;
                case 6:
                    _historieExport.HistorieAlsCSVExportieren();
                    break;
                case 7:
                    historieMenuAktiv = false;
                    _help.Write("Zurück zum Hauptmenü.");
                    break;
                default:
                    _help.Write("Ungültige Wahl!");
                    break;
            }

            if (historieMenuAktiv)
            {
                _help.Einlesen("\nDrücke Enter für Menü...");
            }
        }
    }

    public string GetMenuTitle(int optionTitle)
    {
        return $"{optionTitle}. Historien-Menu";
    }
}
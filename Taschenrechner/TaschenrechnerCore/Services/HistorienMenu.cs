using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class HistorienMenu : IMenu
{
    static Hilfsfunktionen help = new Hilfsfunktionen();
    static HistorieImport historieI = new HistorieImport();
    static HistorienExport historieE = new HistorienExport();
    static HistorieVerwaltung historieV = new HistorieVerwaltung();
    static HistorieZeigen historieZ = new HistorieZeigen();

    /// <summary>
    /// Zeigt das Historiemenü an, wertet die Eingabe aus und führt die dazugehörige Funktion aus
    /// </summary>
    public void Show()
    {
        bool historieMenuAktiv = true;
        while (historieMenuAktiv)
        {
            help.Mischen();

            help.Write("\n=== HISTORIE ===");
            help.Write("1. Historie ansehen");
            help.Write("2. Historie speichern");
            help.Write("3. Historie löschen");
            help.Write("4. Als XML Exportieren");
            help.Write("5. Von XML Importieren");
            help.Write("6. Als CSV Exportieren");
            help.Write("7. Zurück zum Hauptmenü");
            help.Write("Deine Wahl (1-7): ");
            int wahl = help.MenuWahlEinlesen();
            switch (wahl)
            {
                case 1:
                    historieZ.HistorieAnzeigen();
                    break;
                case 2:
                    historieV.HistorieSpeichern();
                    break;
                case 3:
                    historieV.HistorieLöschen();
                    break;
                case 4:
                    historieE.HistorieAlsXMLExportieren();
                    break;
                case 5:
                    historieI.HistorieVonXMLImportieren();
                    break;
                case 6:
                    historieE.HistorieAlsCSVExportieren();
                    break;
                case 7:
                    historieMenuAktiv = false;
                    help.Write("Zurück zum Hauptmenü.");
                    break;
                default:
                    help.Write("Ungültige Wahl!");
                    break;
            }

            if (historieMenuAktiv)
            {
                help.Write("\nDrücke Enter für Menü...");
                Console.ReadLine();
            }
        }
    }

    public string GetMenuTitle(int optionTitle)
    {
        return $"{optionTitle}. Historien Menu";
    }
}
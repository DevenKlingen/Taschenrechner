using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services
{
    public class ImportExportMenu :IMenu
    {
        ImportExportService _service;
        Hilfsfunktionen _help;
        public ImportExportMenu(Hilfsfunktionen help, ImportExportService service) 
        {
            _help = help;
            _service = service;
        }

        public void Show()
        {
            bool importExportMenuAktiv = true;
            while (importExportMenuAktiv)
            {
                _help.Clear();
                _help.WriteInfo("\n=== IMPORT-EXPORT-MENU ===");
                _help.WriteInfo("1. Datenbank in Textdatei exportieren");
                _help.WriteInfo("2. Datenbank zu XML exportieren");
                _help.WriteInfo("3. Datenbank aus XML importieren");
                _help.WriteInfo("0. Zurück zum Datenbank-Menu");
                //TODO: Menu hier einfügen
                int wahl = (int)_help.ZahlEinlesen("Deine Wahl: ");

                switch (wahl)
                {
                    case 1:
                        _service.DatenbankExportieren();
                        break;
                    case 2:
                        _service.DatenbankZuXML();
                        break;
                    case 3:
                        _service.DatenbankVonXML();
                        break;
                    case 0:
                        importExportMenuAktiv = false;
                        _help.WriteInfo("Zurück zum Datenbank-Menu.");
                        break;
                    default:
                        _help.WriteInfo("Ungültige Wahl!");
                        break;
                }

                _help.Einlesen("Drücke Enter um zum Menu zurückzukehren... ");
            }
        }

        public string GetMenuTitle(int optionNumber)
        {
            return $"{optionNumber}. Import-Export-Menu";
        }
    }
}

using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Models;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services
{
    public class EinstellungsMenu : IMenu
    {
        private BenutzerManagement _benutzerManagement;
        private Hilfsfunktionen _help;
        private EinstellungsBearbeitung _einstellungsBearbeitung;

        public EinstellungsMenu(BenutzerManagement benutzerManagement, Hilfsfunktionen help)
        {
            _benutzerManagement = benutzerManagement;
            _help = help;
            _einstellungsBearbeitung = new EinstellungsBearbeitung(_help, _benutzerManagement);
        }

        public void Show()
        {
            while (true)
            {
                _help.WriteInfo("\n--- Einstellungsmenü ---");
                _help.WriteInfo("1. Einstellungen anzeigen");
                _help.WriteInfo("2. Einstellungen bearbeiten");
                _help.WriteInfo("0. Zurück zum Hauptmenü");

                int wahl = (int)_help.ZahlEinlesen("Bitte wählen: ");

                switch (wahl) // Menü mit Switch case
                {
                    case 1:
                        _einstellungsBearbeitung.ZeigeEinstellungen();
                        break;
                    case 2:
                        _einstellungsBearbeitung.EinstellungenBearbeiten(); // Einstellungen Bearbeiten
                        break;
                    case 0:
                        return;
                    default:
                        _help.WriteWarning("Ungültige Auswahl.");
                        break;
                }
            }
        }
        
        public string GetMenuTitle(int optionNumber)
        {
            return $"{optionNumber}. EinstellungsMenu";
        }
    }
}

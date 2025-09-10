using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services
{
    public  class RechnerMenu :IMenu
    {
        private Hilfsfunktionen _help;
        private string aktueller;
        private MatrixRechner _matrixRechner;
        private Operationen _operationen;
        private RechnerManager _rechnerManager;
        public RechnerMenu(Hilfsfunktionen help, string aktuellerRechner, MatrixRechner matrixRechner, Operationen operationen, RechnerManager rechnerManager) 
        { 
            _help = help; 
            aktueller = aktuellerRechner;
            _matrixRechner = matrixRechner;
            _operationen = operationen;
            _rechnerManager = rechnerManager;
        }

        public void Show()
        {
            bool rechnerMenuAktiv = true;
            while (rechnerMenuAktiv)
            {
                _help.Mischen();
                _help.WriteInfo("\n=== RECHNER-MENÜ ===");
                _help.WriteInfo("1. Berechnung durchführen");
                _help.WriteInfo("2. Rechner wechseln");
                _help.WriteInfo("3. Aktive Rechner anzeigen");
                _help.WriteInfo("0. Zurück zum Hauptmenü");
                int wahl = (int)_help.ZahlEinlesen("Deine Wahl: ");

                switch (wahl)
                {
                    case 1: // Berechnung durchführen
                        if (aktueller.ToLower() == "matrix-rechner")
                        { 
                            _matrixRechner.ZeigeBerechnungen();
                        }
                        else
                        {
                            _operationen.BerechnungDurchfuehren();
                        }
                        break;
                    case 2: // Rechner wechseln
                        _rechnerManager.RechnerWechseln();
                        break;
                    case 4: // Aktive Rechner anzeigen
                        _rechnerManager.ZeigeAktiveRechner();
                        break;
                    case 0:
                        rechnerMenuAktiv = false;
                        _help.WriteInfo("Zurück zum Hauptmenü.");
                        break;
                    default:
                        _help.WriteInfo("Ungültige Wahl!");
                        break;
                }
                if (rechnerMenuAktiv)
                {
                    _help.WriteInfo("\nDrücke Enter für Menü...");
                    Console.ReadLine();
                }
            }
        }


        public string GetMenuTitle(int optionTitle)
        {
            return $"{optionTitle}. Rechner-Menu";
        }
        //Rechner Wechseln, Berechnung durchführen, Aktive rechner anzeigen
    }
}

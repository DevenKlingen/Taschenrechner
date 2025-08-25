using SQLitePCL;
using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class ProzentMenu : IMenu
{
    static Hilfsfunktionen help = new Hilfsfunktionen();
    static ProzentRechnung prozentR = new ProzentRechnung();

    public void Show()
    {
        bool prozentRechnerAktiv = true;
        while (prozentRechnerAktiv)
        {
            help.Mischen();

            help.Write("\n=== Prozentrechner ===");
            help.Write("1. Prozentrechnung");
            help.Write("2. Prozentualer Anteil");
            help.Write("3. Grundwert");
            help.Write("4. Zurück zum Rechenmenü");
            help.Write("Deine Wahl (1-4): ");
            int wahl = help.MenuWahlEinlesen();
            switch (wahl)
            {
                case 1:
                    prozentR.Prozentwert();
                    break;
                case 2:
                    prozentR.ProzentualerAnteil();
                    break;
                case 3:
                    prozentR.Grundwert();
                    break;
                case 4:
                    prozentRechnerAktiv = false;
                    help.Write("Zurück zum Hauptmenü.");
                    break;
                default:
                    help.Write("Ungültige Wahl!");
                    break;
            }

            if (prozentRechnerAktiv)
            {
                help.Write("\nDrücke Enter für Menü...");
                Console.ReadLine();
            }
        }
    }

    public string GetMenuTitle(int optionTitle)
    {
        return $"{optionTitle}. Prozent Menu";
    }
}
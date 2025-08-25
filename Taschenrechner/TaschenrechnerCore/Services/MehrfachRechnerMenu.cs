using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class MehrfachRechnerMenu : IMenu
{
    static Hilfsfunktionen help = new Hilfsfunktionen();
    static Addition a = new Addition();
    static Subtraktion s = new Subtraktion();
    static Multiplikation m = new Multiplikation();
    static Division d = new Division();
    
    public void Show()
    {
        bool mehrfachBerechnungenAktiv = true;
        while (mehrfachBerechnungenAktiv)
        {
            help.Mischen();
            help.Write("\n=== MEHRFACH-BERECHNUNGEN ===");
            help.Write("1. Addition");
            help.Write("2. Subtraktion");
            help.Write("3. Multiplikation");
            help.Write("4. Division");
            help.Write("5. Zurück zum Rechenmenü");
            help.Write("Deine Wahl (1-5): ");
            int wahl = help.MenuWahlEinlesen();
            switch (wahl)
            {
                case 1:
                    a.MehrfachAddition();
                    break;
                case 2:
                    s.MehrfachSubtrahieren();
                    break;
                case 3:
                    m.MehrfachMultiplizieren();
                    break;
                case 4:
                    d.MehrfachDividieren();
                    break;
                case 5:
                    mehrfachBerechnungenAktiv = false;
                    help.Write("Zurückzum Hauptmenü.");
                    break;
                default:
                    help.Write("UngültigeWahl!");
                    break;
            }
            if (mehrfachBerechnungenAktiv)
            {
                help.Write("\nDrücke Enter fürMenü...");
                Console.ReadLine();
            }
        }
    }

    public string GetMenuTitle(int optionTitle)
    {
        return $"{optionTitle}. MehfrachRechner";
    }
}
using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class RechnerMenu : IMenu
{
    static Hilfsfunktionen help = new Hilfsfunktionen();
    static Addition a = new Addition();
    static Subtraktion s = new Subtraktion();
    static Multiplikation m = new Multiplikation();
    static Division d = new Division();
    static WaehrungsRechner wR = new WaehrungsRechner();
    static Potenzierer p = new Potenzierer();
    static ProzentMenu pR = new ProzentMenu();
    static DecimalRechner decimalR = new DecimalRechner();
    static StatistikMenu statistikM = new StatistikMenu();
    static MatrixMenu matrixR = new MatrixMenu();
    public void Show()
    {
        bool programmLaeuft = true;

        while (programmLaeuft)
        {
            help.Mischen();

            help.Write("\n=== RECHENMENÜ ===");
            help.Write("Wähle eine Operation:");
            help.Write("1. Addition");
            help.Write("2. Subtraktion");
            help.Write("3. Multiplikation");
            help.Write("4. Division");
            help.Write("5. Währungsrechner");
            help.Write("6. Potenz");
            help.Write("7. Prozentrechner");
            help.Write("8. Zahl in Binär umwandeln");
            help.Write("9. Zahl in Hexadezimal umwandeln");
            help.Write("10. Statistik-Rechner");
            help.Write("11. Matrix-Rechner");
            help.Write("12. Listen-Rechner");
            help.Write("13. Mehrfach-Berechnungen");
            help.Write("14. Fibonacci-Zahlen");
            help.Write("15. Primzahlen-Rechner");
            help.Write("16. Suche im Dictionary");
            help.Write("17. Zurück zum Hauptmenü");
            help.Write("Deine Wahl (1-17): ");
            int wahl = help.MenuWahlEinlesen();

            switch (wahl)
            {
                case 1:
                    a.Addieren();
                    break;
                case 2:
                    s.Subtrahieren();
                    break;
                case 3:
                    m.Multiplizieren();
                    break;
                case 4:
                    d.Dividieren();
                    break;
                case 5:
                    wR.WaehrungsRechnung();
                    break;
                case 6:
                    p.Potenz();
                    break;
                case 7:
                    pR.Show();
                    break;
                case 8:
                    decimalR.ToBinary();
                    break;
                case 9:
                    decimalR.ToHexadecimal();
                    break;
                case 10:
                    statistikM.Show();
                    break;
                case 11:
                    matrixrechner.MatrixRechnerMenu();
                    break;
                case 12:
                    listrechner.ListRechnerMenu();
                    break;
                case 13:
                    mehrfachrechner.MehrfachBerechnungenMenu();
                    break;
                case 14:
                    Fibonacci();
                    break;
                case 15:
                    PrimzahlenRechner();
                    break;
                case 16:
                    grundrechenArten.Suche();
                    break;
                case 17:
                    programmLaeuft = false;
                    help.Write("Zurück zum Hauptmenü.");
                    break;
                default:
                    help.Write("Ungültige Wahl!");
                    break;
            }

            if (programmLaeuft)
            {
                help.Write("\nDrücke Enter für Menü...");
                Console.ReadLine();
            }
        }
    }

    public string GetMenuTitle(int optionTitle)
    {
        return $"{optionTitle}. Rechenmenu";
    }
}

using TaschenrechnerConsole;
using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class BenutzerMenu : IMenu
{
    static Program program = new Program();
    static Hilfsfunktionen help = new Hilfsfunktionen();

    public void Show()
    {
        help.Write("=== BENUTZER-MANAGEMENT ===");
        help.Write("1. Benutzer wechseln");
        help.Write("2. Benutzer löschen");
        help.Write("3. Zurück zum Hauptmenü");
        int wahl = (int)help.ZahlEinlesen("Deine Wahl: ");
        switch (wahl)
        {
            case 1:
                program.BenutzerWechseln();
                break;
            case 2:
                program.BenutzerLöschen();
                break;
            case 3:
                return; // Zurück zum Hauptmenü
            default:
                help.Write("Ungültige Wahl!");
                break;
        }
    }
    public string GetMenuTitle(int optionTitle)
    {
        return $"{optionTitle}. BenutzerMenu";
    }

}
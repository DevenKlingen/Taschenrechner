using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class BenutzerMenu : IMenu
{
    private readonly BenutzerManagement _benutzerManagement;
    private readonly Hilfsfunktionen _help;

    public BenutzerMenu(
        BenutzerManagement benutzerManagement, 
        Hilfsfunktionen help)
    {
        _benutzerManagement = benutzerManagement;
        _help = help;
    }
    public void Show()
    {
        _help.WriteInfo("\n=== BENUTZER-MANAGEMENT ===");
        _help.WriteInfo("1. Benutzer wechseln");
        _help.WriteInfo("2. Benutzer löschen");
        _help.WriteInfo("0. Zurück zum Hauptmenü");
        int wahl = (int)_help.ZahlEinlesen("Deine Wahl: ");

        switch (wahl)
        {
            case 1:
                _benutzerManagement.BenutzerWechseln();
                break;
            case 2:
                _benutzerManagement.BenutzerLöschen();
                break;
            case 0:
                return; // Zurück zum Hauptmenü
            default:
                _help.WriteInfo("Ungültige Wahl!");
                break;
        }
    }
    public string GetMenuTitle(int optionTitle)
    {
        return $"{optionTitle}. BenutzerMenu";
    }

}
using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class KonfigMenu : IMenu
{
    private readonly Hilfsfunktionen _help;
    private readonly KonfigBearbeiten _konfigBearbeiten;
    private readonly KonfigVerwaltung _konfigVerwaltung;

    public KonfigMenu(
        Hilfsfunktionen help, 
        KonfigBearbeiten konfigBearbeiten, 
        KonfigVerwaltung konfigVerwaltung)
    {
        _help = help;
        _konfigBearbeiten = konfigBearbeiten;
        _konfigVerwaltung = konfigVerwaltung;
    }

    /// <summary>
    /// Zeigt das Konfiurationsmenü an, wrtet die Eingabe aus und führt die dazugehörige Funktion aus
    /// </summary>
    public void Show()
    {
        bool konfigurierung = true;
        while (konfigurierung)
        {
            _help.Mischen();

            _help.Write("\n=== KONFIGURATIONSMENU ===");
            _help.Write("1. Konfiguration laden");
            _help.Write("2. Konfiguration anzeigen");
            _help.Write("3. Konfiguration bearbeiten");
            _help.Write("4. Konfiguration speichern");
            _help.Write("5. Zurück zum Hauptmenü");
            _help.Write("Deine Wahl (1-5): ");
            int wahl = _help.MenuWahlEinlesen();
            switch (wahl)
            {
                case 1:
                    _konfigVerwaltung.KonfigurationLaden();
                    break;
                case 2:
                    _konfigVerwaltung.KonfigurationAnzeigen();
                    break;
                case 3:
                    _konfigBearbeiten.KonfigurationAendern();
                    break;
                case 4:
                    _konfigVerwaltung.KonfigurationSpeichern();
                    break;
                case 5:
                    konfigurierung = false;
                    _help.Write("Zurück zum Hauptmenü.");
                    break;
                default:
                    _help.Write("Ungültige Wahl!");
                    break;
            }

            if (konfigurierung)
            {
                _help.Einlesen("\nDrücke Enter für Menü...");
            }
        }
    }

    public string GetMenuTitle(int optionTitle)
    {
        return $"{optionTitle}. Konfigurations-Menu";
    }
}
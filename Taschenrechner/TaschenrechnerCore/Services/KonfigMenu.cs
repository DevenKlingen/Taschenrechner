using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class KonfigMenu : IMenu
{
    static Hilfsfunktionen help = new Hilfsfunktionen();
    static KonfigBearbeiten konfigB = new KonfigBearbeiten();
    static KonfigVerwaltung konfigV = new KonfigVerwaltung();

    /// <summary>
    /// Zeigt das Konfiurationsmenü an, wrtet die Eingabe aus und führt die dazugehörige Funktion aus
    /// </summary>
    public void Show()
    {
        bool konfigurierung = true;
        while (konfigurierung)
        {
            help.Mischen();

            help.Write("\n=== KONFIGURATIONSMENU ===");
            help.Write("1. Konfiguration laden");
            help.Write("2. Konfiguration anzeigen");
            help.Write("3. Konfiguration bearbeiten");
            help.Write("4. Konfiguration speichern");
            help.Write("5. Zurück zum Hauptmenü");
            help.Write("Deine Wahl (1-5): ");
            int wahl = help.MenuWahlEinlesen();
            switch (wahl)
            {
                case 1:
                    konfigV.KonfigurationLaden();
                    break;
                case 2:
                    konfigV.KonfigurationAnzeigen();
                    break;
                case 3:
                    konfigB.KonfigurationAendern();
                    break;
                case 4:
                    konfigV.KonfigurationSpeichern();
                    break;
                case 5:
                    konfigurierung = false;
                    help.Write("Zurück zum Hauptmenü.");
                    break;
                default:
                    help.Write("Ungültige Wahl!");
                    break;
            }

            if (konfigurierung)
            {
                help.Write("\nDrücke Enter für Menü...");
                Console.ReadLine();
            }
        }
    }

    public string GetMenuTitle(int optionTitle)
    {
        return $"{optionTitle}. Konfigurations-Menu";
    }
}
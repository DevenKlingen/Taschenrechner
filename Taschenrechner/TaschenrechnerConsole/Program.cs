using TaschenrechnerCore.Services;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerConsole;

public class Program
{
    static BenutzerManagement benutzerManagement = new();
    static Hilfsfunktionen help = new();
    static HistorieVerwaltung historieVerwaltung = new();
    static BenutzerEinstellungen benutzerEinstellungen = new();
    static Backup backup = new();
    static RechnerMenu rechnerMenu = new();
    static HistorienMenu historienMenu = new();
    static KonfigMenu konfigMenu = new();
    static BackupMenu backupMenu = new();
    static DatenbankMenu datenbankMenu = new();
    static StatistikMenu statistikMenu = new();
    static OptimierterRechner optRechner = new();

    static void Main(string[] args)
    {
        bool programmLaeuft = true;

        benutzerManagement.BenutzerAnmelden();

        help.Write("Lade gespeicherte Historie...");
        historieVerwaltung.HistorieLaden();

        if (benutzerEinstellungen.config.AutoSpeichern)
        {
            backup.BackupErstellen();
        }

        while (programmLaeuft)
        {
            help.Mischen();

            help.Write("Willkommen zum Taschenrechner!");
            help.Write("Wähle eine Operation:");
            help.Write("1. Rechenmenü");
            help.Write("2. Historie-Optionen");
            help.Write("3. Konfiguration bearbeiten");
            help.Write("4. Backups verwalten");
            help.Write("5. Benutzer Wechseln");
            help.Write("6. Benutzer Löschen");
            help.Write("7. Datenbank optionen");
            help.Write("8. Benutzer-Statistik anzeigen");
            help.Write("9. Optimierter Rechner");
            help.Write("10. Beenden");
            help.Write("Deine Wahl (1-10): ");
            int wahl = help.MenuWahlEinlesen();

            switch (wahl)
            {
                case 1:
                    rechnerMenu.Show();
                    break;
                case 2:
                    historienMenu.Show();
                    break;
                case 3:
                    konfigMenu.Show();
                    break;
                case 4:
                    backupMenu.Show();
                    break;
                case 5:
                    benutzerManagement.BenutzerWechseln();
                    break;
                case 6:
                    benutzerManagement.BenutzerLöschen();
                    break;
                case 7:
                    datenbankMenu.Show();
                    break;
                case 8:
                    statistikMenu.Show();
                    break;
                case 9:
                    optRechner.Start();
                    break;
                case 10:
                    help.Write("Speichere Historie...");
                    historieVerwaltung.HistorieSpeichern();

                    programmLaeuft = false;
                    help.Write("Auf Wiedersehen!");
                    break;
                default:
                    help.Write("Ungültige Wahl!");
                    break;
            }

            if (programmLaeuft)
            {
                help.Write("\nDrücke Enter für Hauptmenü...");
                Console.ReadLine();
            }
        }
    }
}
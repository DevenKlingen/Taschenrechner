using TaschenrechnerCore.Models;
using TaschenrechnerCore.Services;
using TaschenrechnerCore.Enums;
using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;
using Microsoft.EntityFrameworkCore;


namespace TaschenrechnerConsole;

public class Program
{
    public List<string> berechnungsHistorie = new List<string>();
    public string historieDatei = "berechnungen.txt";

    public string konfigJson = "config.json";
    public string konfigToml = "config.toml";

    public TaschenrechnerKonfiguration config = new TaschenrechnerKonfiguration();

    public List<Berechnung> detaillierteBerechnungen = new List<Berechnung>();

    public string benutzer = "";

    static Benutzer aktuellerBenutzer = null;

    private static TaschenrechnerContext DbContext;


    static void Main(string[] args)
    {
        DbContext = new TaschenrechnerContext();
        DbContext.Database.EnsureCreated();

        bool programmLaeuft = true;

        program.BenutzerAnmelden();

        help.Write("Lade gespeicherte Historie...");
        historiemenu.HistorieLaden();

        if (program.config.AutoSpeichern)
        {
            backupmenu.BackupErstellen();
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
                    grundrechnung.RechenMenu();
                    break;
                case 2:
                    historiemenu.HistorieMenu();
                    break;
                case 3:
                    konfigmenu.KonfigMenu();
                    break;
                case 4:
                    backupmenu.BackupMenu();
                    break;
                case 5:
                    program.BenutzerWechseln();
                    break;
                case 6:
                    program.BenutzerLöschen();
                    break;
                case 7:
                    datenbankmenu.DatenbankMenu();
                    break;
                case 8:
                    statistikmenu.StatistikMenu();
                    break;
                case 9:
                    optRechner.Start();
                    break;
                case 10:
                    help.Write("Speichere Historie...");
                    historiemenu.HistorieSpeichern();

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
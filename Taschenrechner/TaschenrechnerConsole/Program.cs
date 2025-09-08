using TaschenrechnerCore;
using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Services;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerConsole;

public class ConsoleLogTarget : ILogTarget
{
    public void WriteLog(string message)
    {
        Console.WriteLine(message);
    }
}

public class Program
{
    static void Main(string[] args)
    {

        BenutzerManagement _benutzerManagement = new BenutzerManagement();

        var logger = new FlexibleLogger(new ConsoleLogTarget());

        Hilfsfunktionen _help = new Hilfsfunktionen(_benutzerManagement, logger);
        _benutzerManagement.setHelp(_help);

        BenutzerEinstellungen _benutzerEinstellungen = new BenutzerEinstellungen(_help, _benutzerManagement, new TaschenrechnerCore.Models.TaschenrechnerKonfiguration());
        _benutzerManagement.setBenutzerEinstellungen(_benutzerEinstellungen);

        KonfigBearbeiten _konfigBearbeiten = new KonfigBearbeiten(_help, _benutzerManagement, _benutzerEinstellungen);

        KonfigVerwaltung _konfigVerwaltung = new KonfigVerwaltung(_konfigBearbeiten, _benutzerEinstellungen, _benutzerManagement);
        _konfigBearbeiten.setKonfigVerwaltung(_konfigVerwaltung);
        _benutzerManagement.setKonfigVerwaltung(_konfigVerwaltung);
        _konfigVerwaltung.setHilfe(_help);

        HistorieVerwaltung _historieVerwaltung = new HistorieVerwaltung(_benutzerManagement, _help);

        HistorienExport _historienExport = new HistorienExport(_help, _benutzerManagement, _historieVerwaltung);
        _historieVerwaltung.setHistorienExport(_historienExport);

        Backup _backup = new Backup(_benutzerManagement, _help, _historienExport);

        HistorienBearbeitung _historienBearbeitung = new HistorienBearbeitung(_historieVerwaltung);

        BackupsVerwalten _backupsVerwalten = new BackupsVerwalten(_help, _konfigVerwaltung, _historienBearbeitung, _benutzerManagement, _historieVerwaltung);

        BackupMenu _backupMenu = new BackupMenu(_help, _backupsVerwalten, _backup);

        DatenbankHistorie _datenbankHistorie = new DatenbankHistorie(_help, _benutzerManagement, _benutzerEinstellungen);

        DatenbankBerechnungen _datenbankBerechnungen = new DatenbankBerechnungen(_benutzerManagement, _help);

        DatenbankExport _datenbankExport = new DatenbankExport(_help, _benutzerManagement);

        DatenbankReinigung _datenbankReinigung = new DatenbankReinigung(_help, _benutzerManagement);

        DatenbankMenu _datenbankMenu = new DatenbankMenu(_help, _datenbankHistorie, _datenbankBerechnungen, _datenbankExport, _datenbankReinigung, _backup);

        HistorieImport _historieImport = new HistorieImport(_help, _benutzerManagement, _historieVerwaltung, _benutzerEinstellungen);

        HistorieZeigen _historieZeigen = new HistorieZeigen(_historieVerwaltung, _help, _benutzerEinstellungen);

        HistorienMenu _historienMenu = new HistorienMenu(_help, _historieImport, _historienExport, _historieVerwaltung, _historieZeigen);

        KonfigMenu _konfigMenu = new KonfigMenu(_help, _konfigBearbeiten, _konfigVerwaltung);

        MatrixLesen _matrixLesen = new MatrixLesen(_help);

        MatrixAusgabe _matrixAusgabe = new MatrixAusgabe(_help);

        Addition _addition = new Addition(_help, _datenbankBerechnungen, _historienBearbeitung, _matrixLesen, _matrixAusgabe, _benutzerEinstellungen);

        Subtraktion _subtraktion = new Subtraktion(_help, _historienBearbeitung, _datenbankBerechnungen, _benutzerEinstellungen);

        Multiplikation _multiplikation = new Multiplikation(_help, _historienBearbeitung, _datenbankBerechnungen, _matrixLesen, _matrixAusgabe, _benutzerEinstellungen);

        Division _division = new Division(_help, _historienBearbeitung, _datenbankBerechnungen, _benutzerEinstellungen);

        WaehrungsRechner _waehrungsRechner = new WaehrungsRechner(_help, _historienBearbeitung, _datenbankBerechnungen, _benutzerEinstellungen);

        Potenzierer _potenzierer = new Potenzierer(_help, _historienBearbeitung, _datenbankBerechnungen, _benutzerEinstellungen);

        ProzentRechnung _prozentRechnung = new ProzentRechnung(_help, _historienBearbeitung, _datenbankBerechnungen, _benutzerEinstellungen);

        ProzentMenu _prozentMenu = new ProzentMenu(_help, _prozentRechnung);

        DecimalRechner _decimalRechner = new DecimalRechner(_historienBearbeitung, _help, _datenbankBerechnungen, _benutzerEinstellungen);

        MatrixDeterminante _matrixDeterminante = new MatrixDeterminante(_help, _matrixLesen, _matrixAusgabe);

        MatrixMenu _matrixMenu = new MatrixMenu(_help, _matrixDeterminante, _addition, _multiplikation);

        ListEinlesen _listEinlesen = new ListEinlesen(_help);

        ListManipulation _listManipulation = new ListManipulation(_help);

        ListRechnerMenu _listRechnerMenu = new ListRechnerMenu(_help, _listEinlesen, _listManipulation);

        MehrfachRechnerMenu _mehrfachRechnerMenu = new MehrfachRechnerMenu(_help, _addition, _subtraktion, _multiplikation, _division);

        Fibonacci _fibonacci = new Fibonacci(_help);

        PrimzahlenRechner _primzahlenRechner = new PrimzahlenRechner(_help);

        Konstanten _konstanten = new Konstanten(_help);

        Statistiken _statistiken = new Statistiken(_benutzerManagement, _help);

        StatistikMonatsReport _statistikMonatsReport = new StatistikMonatsReport(_help, _benutzerManagement);

        StatistikMenu _statistikMenu = new StatistikMenu(_help, _statistiken, _statistikMonatsReport);

        RechnerMenu _rechnerMenu = new RechnerMenu(_help, _addition, _subtraktion, _multiplikation, _division, _waehrungsRechner, _potenzierer, _prozentMenu, _decimalRechner, _statistikMenu, _matrixMenu, _listRechnerMenu, _mehrfachRechnerMenu, _fibonacci, _primzahlenRechner, _konstanten);

        RechnerManager _rechnerManager = new RechnerManager(_help);

        MatrixRechner _matrixRechner = new MatrixRechner(_help, _rechnerManager, _benutzerManagement, _datenbankBerechnungen);

        BenutzerMenu _benutzerMenu = new BenutzerMenu(_benutzerManagement, _help);

        WissenschaftlicherRechner _wissenschaftsRechner = new WissenschaftlicherRechner(_benutzerManagement, _datenbankBerechnungen);

        OptimierterRechner _optRechner = new OptimierterRechner(_rechnerManager, _help, _konfigVerwaltung, _historieVerwaltung, _konfigBearbeiten, _datenbankHistorie, _statistikMenu, _matrixRechner, _benutzerMenu, _backup, _wissenschaftsRechner, _benutzerManagement, _benutzerEinstellungen);

        RechnerFactory.setRechnerManager(_rechnerManager);
        RechnerFactory.setHelp(_help);
        RechnerFactory.setBenutzerManagement(_benutzerManagement);
        RechnerFactory.setDatenbankBerechnungen(_datenbankBerechnungen);


        List<IMenu> menus = new List<IMenu>{
            _rechnerMenu, 
            _historienMenu, 
            _konfigMenu, 
            _backupMenu, 
            _datenbankMenu, 
            _statistikMenu
        };

        bool programmLaeuft = true;

        _benutzerManagement.BenutzerAnmelden();

        _help.Write("Lade gespeicherte Historie...");
        _historieVerwaltung.HistorieLaden();

        if (_benutzerEinstellungen.getConfig().AutoSpeichern)
        {
            _backup.BackupErstellen();
        }

        while (programmLaeuft)
        {
            _help.Mischen();
            int i;
            _help.Write("\nWillkommen zum Taschenrechner!");
            for (i = 1; i <= menus.Count(); i++)
            {
                _help.Write(menus[i - 1].GetMenuTitle(i));
            }

            _help.Write($"{i}. Benutzer Wechseln");
            i++;
            _help.Write($"{i}. Benutzer Löschen");
            i++;
            _help.Write($"{i}. Optimierter Rechner");
            _help.Write("0. Beenden");

            int.TryParse(_help.ZahlEinlesen("Deine Wahl: ").ToString(), out int wahl);
            
            switch (wahl)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                    menus[wahl-1].Show();
                    break;
                case 7:
                    _benutzerManagement.BenutzerWechseln();
                    break;
                case 8:
                    _benutzerManagement.BenutzerLöschen();
                    break;
                case 9:
                    _optRechner.Start();
                    break;
                case 0:
                    _help.Write("Speichere Historie...");
                    _historieVerwaltung.HistorieSpeichern();

                    programmLaeuft = false;
                    _help.Write("Auf Wiedersehen!");
                    break;
                default:
                    _help.Write("Ungültige Wahl!");
                    break;
            }

            if (programmLaeuft)
            {
                _help.Write("\nDrücke Enter für Hauptmenü...");
                Console.ReadLine();
            }
        }
    }
}
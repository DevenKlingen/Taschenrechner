using TaschenrechnerCore.Enums;
using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Models;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class OptimierterRechner
{
    static void Main(string[] args)
    {

        BenutzerManagement _benutzerManagement = new BenutzerManagement();
        FlexibleLogger _dateiLogger = new FlexibleLogger(new DateiLogTarget(), _benutzerManagement);
        FlexibleLogger _logger = new FlexibleLogger(new ConsoleLogTarget(), _benutzerManagement);
        Hilfsfunktionen _help = new Hilfsfunktionen(_benutzerManagement, _logger, _dateiLogger);
        _benutzerManagement.setHelp( _help );

        DatenbankBerechnungen _datenbankBerechnungen = new DatenbankBerechnungen(_benutzerManagement, _help);
        RechnerManager _rechnerManager = new RechnerManager(_help, _benutzerManagement, _datenbankBerechnungen);
        
        MatrixRechner _matrixRechner = new MatrixRechner(_help, _rechnerManager, _benutzerManagement, _datenbankBerechnungen);

        WissenschaftlicherRechner _wissenschaftlicherRechner = new WissenschaftlicherRechner(_benutzerManagement, _datenbankBerechnungen);
        TaschenrechnerKonfiguration _config = new TaschenrechnerKonfiguration();
        BenutzerEinstellungen _benutzerEinstellungen = new BenutzerEinstellungen(_help, _benutzerManagement, _config);
        _benutzerManagement.setBenutzerEinstellungen(_benutzerEinstellungen);
        ValidationUtils _validation = new ValidationUtils();
        Operationen _operationen = new Operationen(_help, _benutzerManagement, _rechnerManager, _wissenschaftlicherRechner, _benutzerEinstellungen, _validation);

        DatenbankHistorie _datenbankHistorie = new DatenbankHistorie(_help, _benutzerManagement, _benutzerEinstellungen);
        ImportExportService _datenbankExport = new ImportExportService(_help, _benutzerManagement, _datenbankBerechnungen);
        DatenbankReinigung _datenbankReinigung = new DatenbankReinigung(_help, _benutzerManagement);
        Backup _backup = new Backup(_help);
        DatenbankMenu _datenbankMenu = new DatenbankMenu(_help, _datenbankHistorie, _datenbankBerechnungen, _datenbankReinigung, _benutzerManagement, _backup);

        Statistiken _statistiken = new Statistiken(_benutzerManagement, _help);
        StatistikMonatsReport _statistikMonatsReport = new StatistikMonatsReport(_help, _benutzerManagement);
        StatistikMenu _statistikMenu = new StatistikMenu(_help, _statistiken, _statistikMonatsReport);

        EinstellungsMenu _einstellungsMenu = new EinstellungsMenu(_benutzerManagement, _help);

        BenutzerMenu _benutzerMenu = new BenutzerMenu(_benutzerManagement, _help);


        _help.WriteInfo("=== TASCHENRECHNER v2.0 (OOP) ===");

        // Neues Feature: Rechner-Auswahl beim Start
        var akt = _benutzerManagement.getBenutzer();
        if (akt == null)
        {
            _benutzerManagement.BenutzerAnmelden();
        }

        using var context = new TaschenrechnerContext();

        var userSetting = context.Einstellungen
                .FirstOrDefault(e => e.BenutzerId == _benutzerManagement.getBenutzer().Id && e.Schluessel == "Standardrechner");

        // Standard-Rechner basierend auf Konfiguration
        switch (userSetting.Wert.ToLower())
        {
            case "basis":
                _rechnerManager.WechsleZuRechner(RechnerTyp.Basis);
                _help.WriteInfo("Basisrechner ausgewählt");
                break;
            case "wissenschaftlich":
                _rechnerManager.WechsleZuRechner(RechnerTyp.Wissenschaftlich);
                _help.WriteInfo("Wissenschaftlicher rechner ausgewählt");
                break;
            case "finanz":
                _rechnerManager.WechsleZuRechner(RechnerTyp.Finanz);
                _help.WriteInfo("Finanzrechner ausgewählt");
                break;
            case "matrix":
                _rechnerManager.WechsleZuRechner(RechnerTyp.Matrix);
                _help.WriteInfo("Matrixrechner ausgewählt");
                break;
            case "statistik":
                _rechnerManager.WechsleZuRechner(RechnerTyp.Statistik);
                _help.WriteInfo("StatistikRechner ausgewählt");
                break;
            default:
                _help.WriteWarning("Ungültiger Standardrechner in der Konfiguration! Wechsle zu Basis-Rechner.");
                _rechnerManager.WechsleZuRechner(RechnerTyp.Basis);
                break;
        }

        RechnerFactory.Initialisiere(_help, _rechnerManager, _benutzerManagement, _datenbankBerechnungen);

        RechnerMenu _rechnerMenu = new RechnerMenu(_help, _rechnerManager.AktuellerRechner.RechnerTyp, _matrixRechner, _operationen, _rechnerManager);

        List<IMenu> menus = [_rechnerMenu, _datenbankMenu, _einstellungsMenu, _benutzerMenu, _statistikMenu];

        bool programmLaeuft = true;

        while (programmLaeuft)
        {
            for(int i = 1; i <= menus.Count; i++) 
                {
                    _help.WriteInfo(menus[i-1].GetMenuTitle(i));
                }

            int wahl = (int)_help.ZahlEinlesen("Deine Wahl: ");
            
            try
            {
                menus[wahl - 1].Show();
            }
            catch (Exception ex)
            {
                _help.WriteError($"Fehler: {ex.Message}");
            }

            if (programmLaeuft)
            {
                _help.Einlesen("\nDrücke Enter für Hauptmenü...");
            }
        }

        // Cleanup //TODO: Autospeichern einführen
        if (_benutzerEinstellungen.getConfig().AutoSpeichern)
        {
        }

        _rechnerManager.SchliesseAlleRechner();
        _help.WriteInfo("Auf Wiedersehen!");
    }
}
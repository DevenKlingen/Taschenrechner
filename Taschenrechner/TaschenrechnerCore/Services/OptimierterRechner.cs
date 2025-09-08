using TaschenrechnerCore.Models;
using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;
using TaschenrechnerCore.Enums;

namespace TaschenrechnerCore.Services;

public class OptimierterRechner
{
    private readonly RechnerManager _rechnerManager;
    private readonly Hilfsfunktionen _help;
    private readonly KonfigVerwaltung _konfigVerwaltung;
    private readonly HistorieVerwaltung _historieVerwaltung;
    private readonly KonfigBearbeiten _konfigBearbeiten;
    private readonly DatenbankHistorie _datenbankHistorie;
    private readonly StatistikMenu _statistikMenu;
    private readonly MatrixRechner _matrixRechner;
    private readonly BenutzerMenu _benutzerMemi;
    private readonly Backup _backup;
    private readonly WissenschaftlicherRechner _wissenschaftsRechner;
    private readonly BenutzerManagement _benutzerManagement;
    private readonly BenutzerEinstellungen _benutzerEinstellungen;

    public OptimierterRechner(
        RechnerManager rechnerManager, 
        Hilfsfunktionen help, 
        KonfigVerwaltung konfigVerwaltung, 
        HistorieVerwaltung historieVerwaltung,
        KonfigBearbeiten konfigBearbeiten, 
        DatenbankHistorie datenbankHistorie, 
        StatistikMenu statistikMenu, 
        MatrixRechner matrixRechner, 
        BenutzerMenu benutzerMenu, 
        Backup backup, 
        WissenschaftlicherRechner wissenschaftsRechner, 
        BenutzerManagement benutzerManagement, 
        BenutzerEinstellungen benutzerEinstellungen)
    {
        _rechnerManager = rechnerManager;
        _help = help;
        _konfigVerwaltung = konfigVerwaltung;
        _historieVerwaltung = historieVerwaltung;
        _konfigBearbeiten = konfigBearbeiten;
        _datenbankHistorie = datenbankHistorie;
        _statistikMenu = statistikMenu;
        _matrixRechner = matrixRechner;
        _benutzerMemi = benutzerMenu;
        _backup = backup;
        _wissenschaftsRechner = wissenschaftsRechner;
        _benutzerManagement = benutzerManagement;
        _benutzerEinstellungen = benutzerEinstellungen;
    }

    public void Start()
    {
        Benutzer akt = _benutzerManagement.getBenutzer();
        _help.Write("\n=== TASCHENRECHNER v2.0 (OOP) ===");

        // Bestehende Funktionen
        _historieVerwaltung.HistorieLaden();
        _konfigVerwaltung.KonfigurationLaden();

        // Neues Feature: Rechner-Auswahl beim Start
        if (akt == null)
        {
            _benutzerManagement.BenutzerAnmelden();
        }
        using var context = new TaschenrechnerContext();

        var userSetting = context.Einstellungen
                .FirstOrDefault(us => us.BenutzerId == akt.Id && us.Schluessel == "Standardrechner");

        // Standard-Rechner basierend auf Konfiguration
        switch (userSetting?.Wert.ToLower())
        {
            case "basis":
                _rechnerManager.WechsleZuRechner(RechnerTyp.Basis);
                _help.Write("Basisrechner ausgewählt");
                break;
            case "wissenschaftlich":
                _rechnerManager.WechsleZuRechner(RechnerTyp.Wissenschaftlich);
                _help.Write("Wissenschaftlicher rechner ausgewählt");
                break;
            case "finanz":
                _rechnerManager.WechsleZuRechner(RechnerTyp.Finanz);
                _help.Write("Finanzrechner ausgewählt");
                break;
            case "matrix":
                _rechnerManager.WechsleZuRechner(RechnerTyp.Matrix);
                _help.Write("Matrixrechner ausgewählt");
                break;
            case "statistik":
                _rechnerManager.WechsleZuRechner(RechnerTyp.Statistik);
                _help.Write("StatistikRechner ausgewählt");
                break;
            default:
                _help.Write("Ungültiger Standardrechner in der Konfiguration! Wechsle zu Basis-Rechner.");
                _rechnerManager.WechsleZuRechner(RechnerTyp.Basis);
                break;
        }

        bool programmLaeuft = true;

        while (programmLaeuft)
        {
            string aktueller = _rechnerManager.AktuellerRechner.RechnerTyp;
            if (aktueller.ToLower() == "matrix-rechner")
            {
                _matrixRechner.ZeigeMatrixMenue();
            }
            else
            {
                ZeigeErweitertesMenue();
            }
            int wahl = (int)_help.ZahlEinlesen("Deine Wahl: ");
            try
            {
                switch (wahl)
                {
                    case 1: // Berechnung durchführen
                        if (aktueller.ToLower() == "matrix-rechner")
                        {
                            _matrixRechner.ZeigeBerechnungen();
                        }
                        else
                        {
                            BerechnungDurchfuehren();
                        }
                        break;
                    case 2: // Rechner wechseln
                        RechnerWechseln();
                        break;
                    case 3: // Historie anzeigen
                        if (_rechnerManager.AktuellerRechner != null)
                            _rechnerManager.AktuellerRechner.HistorieAnzeigen();
                        else
                            _help.Write("Kein Rechner aktiv!");
                        break;
                    case 4: // Aktive Rechner anzeigen
                        _rechnerManager.ZeigeAktiveRechner();
                        break;
                    case 5: // Datenbank-Historie
                        _datenbankHistorie.DatenbankHistorieAnzeigen();
                        break;
                    case 6: // Statistiken
                        _statistikMenu.Show();
                        break;
                    case 7: // Einstellungen
                        _konfigBearbeiten.KonfigurationAendern();
                        break;
                    case 8: // Benutzer-Management
                        _benutzerMemi.Show();
                        break;
                    case 0:
                        programmLaeuft = false;
                        break;
                    default:
                        _help.Write("Ungültige Wahl!");
                        break;
                }
            }
            catch (Exception ex)
            {
                _help.Write($"Fehler: {ex.Message}");
            }

            if (programmLaeuft)
            {
                _help.Einlesen("\nDrücke Enter für Hauptmenü...");
            }
        }

        // Cleanup
        if (_benutzerEinstellungen.getConfig().AutoSpeichern)
        {
            _konfigVerwaltung.KonfigurationSpeichern();
            _backup.BackupErstellen();
        }

        _rechnerManager.SchliesseAlleRechner();
        _help.Write("Auf Wiedersehen!");
    }

    public void ZeigeErweitertesMenue()
    {
        Benutzer akt = _benutzerManagement.getBenutzer();
        _help.Mischen();
        _help.Clear();
        string aktueller = _rechnerManager.AktuellerRechner?.RechnerTyp ?? "Kein Rechner aktiv";
        _help.Write($"\n=== TASCHENRECHNER v2.0 ===");
        _help.Write($"Aktueller Rechner: {aktueller}");
        _help.Write($"Benutzer: {akt?.Name ?? "Nicht angemeldet"}");
        _help.Write("");
        _help.Write("1. Berechnung durchführen");
        _help.Write("2. Rechner wechseln");
        _help.Write("3. Historie anzeigen");
        _help.Write("4. Aktive Rechner anzeigen");
        _help.Write("5. Datenbank-Historie");
        _help.Write("6. Statistiken");
        _help.Write("7. Einstellungen");
        _help.Write("8. Benutzer-Management");
        _help.Write("0. Beenden");
        _help.Write("");
    }

    public void BerechnungDurchfuehren()
    {
        string aktueller = _rechnerManager.AktuellerRechner.RechnerTyp;

        if (_rechnerManager.AktuellerRechner == null)
        {
            _help.Write("Kein Rechner aktiv! Wechsle zuerst zu einem Rechner.");
            return;
        }

        _help.Write($"=== {_rechnerManager.AktuellerRechner.RechnerTyp} ===");

        // Operationen je nach Rechner-Typ anzeigen
        ZeigeVerfuegbareOperationen(_rechnerManager.AktuellerRechner);

        string operation = _help.Einlesen("Operation: ");

        if (string.IsNullOrEmpty(operation))
            return;

        List<double> werte = new List<double>();
        _help.Write("Gib Werte ein (beende mit 'fertig'):");

        bool zahlenSamm = true;
        int wertAnzahl = 0;

        while (zahlenSamm)
        {
            if (wertAnzahl == 2 && aktueller.ToLower() == "basis-rechner")
            {
                zahlenSamm = false;
                break;
            }

            string eingabe = _help.Einlesen($"Wert {werte.Count + 1}: ");

            if (eingabe?.ToLower() == "fertig")
            {
                zahlenSamm = false;
                break;
            }

            if (double.TryParse(eingabe, out double wert))
            {
                werte.Add(wert);
                wertAnzahl++;
            }
            else if (isKonstante(eingabe))
            {
                double ergebnis;
                // Konstante direkt ersetzen
                ergebnis = _wissenschaftsRechner.BerechneMitKonstante(eingabe);
                werte.Add(ergebnis);
            }
            else
            {
                _help.Write("Ungültige Eingabe!");
            }
        }

        if (werte.Count == 0)
        {
            _help.Write("Keine Werte eingegeben!");
            return;
        }

        try
        {
            double ergebnis = _rechnerManager.AktuellerRechner.Berechnen(operation, werte.ToArray());
            _help.Write($"Ergebnis: {FormatUtils.FormatiereZahl(ergebnis, _benutzerEinstellungen.getConfig().Nachkommastellen)}");
        }
        catch (Exception ex)
        {
            _help.Write($"Berechnungsfehler: {ex.Message}");
        }
    }

    public void RechnerWechseln()
    {
        _help.Write("=== RECHNER WECHSELN ===");
        RechnerFactory.ZeigeVerfuegbareRechner();

        int wahl = (int)_help.ZahlEinlesen("Rechner wählen (Nummer): ");

        var verfuegbareTypen = RechnerFactory.GetVerfuegbareRechnerTypen();

        if (wahl >= 1 && wahl <= verfuegbareTypen.Count)
        {
            string typName = verfuegbareTypen[wahl - 1];
            RechnerTyp typ = (RechnerTyp)Enum.Parse(typeof(RechnerTyp), typName);
            _rechnerManager.WechsleZuRechner(typ);
        }
        else
        {
            _help.Write("Ungültige Wahl!");
        }
    }

    public void ZeigeVerfuegbareOperationen(BaseRechner rechner)
    {
        _help.Write("Verfügbare Operationen:");

        switch (rechner.RechnerTyp)
        {
            case "Basis-Rechner":
                _help.Write("  +, -, *, /");
                break;
            case "Wissenschaftlich":
                _help.Write("  +, -, *, /, sin, cos, sqrt, pow, log, ln");
                break;
            case "Finanz-Rechner":
                _help.Write("  zinsen, zinseszinsen, annuitaet, barwert, Kreditplan erstellen");
                break;
            case "Statistik-Rechner":
                _help.Write("  mittelwert, median, standardabweichung, min, max, spannweite");
                break;
        }
    }

    static bool isKonstante(string eingabe)
    {
        bool konst = false;
        string konstante = eingabe.ToLower();
        if (konstante == "pi" || konstante == "phi" || konstante == "e" || konstante == "wurzel2")
            konst = true;
        return konst;
    }
}
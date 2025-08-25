using TaschenrechnerCore.Models;
using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;
using TaschenrechnerCore.Enums;

namespace TaschenrechnerCore.Services;

public class OptimierterRechner
{
    static RechnerManager rechnerManager = new RechnerManager();
    static Hilfsfunktionen help = new Hilfsfunktionen();
    static KonfigVerwaltung konfigV = new KonfigVerwaltung();
    static HistorieVerwaltung historieV = new HistorieVerwaltung();
    static HistorieZeigen historieZ = new HistorieZeigen();
    static KonfigBearbeiten konfigB = new KonfigBearbeiten();
    static DatenbankHistorie datenbankH = new DatenbankHistorie();
    static StatistikMenu statistikM = new StatistikMenu();
    static MatrixRechner matrixR = new MatrixRechner();
    static BenutzerMenu benutzerM = new BenutzerMenu();
    static Backup backup = new Backup();
    static WissenschaftlicherRechner wissenschaftsRechner = new WissenschaftlicherRechner();
    static BenutzerManagement benutzerManagement = new();
    static BenutzerEinstellungen benutzerEinstellungen = new();

    public void Start()
    {
        Benutzer akt = benutzerManagement.getBenutzer();
        help.Write("=== TASCHENRECHNER v2.0 (OOP) ===");

        // Bestehende Funktionen
        historieV.HistorieLaden();
        konfigV.KonfigurationLaden();

        // Neues Feature: Rechner-Auswahl beim Start
        if (akt == null)
        {
            benutzerManagement.BenutzerAnmelden();
        }
        using var context = new TaschenrechnerContext();

        var userSetting = context.Einstellungen
                .FirstOrDefault(us => us.BenutzerId == akt.Id && us.Schluessel == "Standardrechner");

        // Standard-Rechner basierend auf Konfiguration
        switch (userSetting?.Wert.ToLower())
        {
            case "basis":
                rechnerManager.WechsleZuRechner(RechnerTyp.Basis);
                help.Write("Basisrechner ausgewählt");
                break;
            case "wissenschaftlich":
                rechnerManager.WechsleZuRechner(RechnerTyp.Wissenschaftlich);
                help.Write("Wissenschaftlicher rechner ausgewählt");
                break;
            case "finanz":
                rechnerManager.WechsleZuRechner(RechnerTyp.Finanz);
                help.Write("Finanzrechner ausgewählt");
                break;
            case "matrix":
                rechnerManager.WechsleZuRechner(RechnerTyp.Matrix);
                help.Write("Matrixrechner ausgewählt");
                break;
            case "statistik":
                rechnerManager.WechsleZuRechner(RechnerTyp.Statistik);
                help.Write("StatistikRechner ausgewählt");
                break;
            default:
                help.Write("Ungültiger Standardrechner in der Konfiguration! Wechsle zu Basis-Rechner.");
                rechnerManager.WechsleZuRechner(RechnerTyp.Basis);
                break;
        }

        bool programmLaeuft = true;

        while (programmLaeuft)
        {
            string aktueller = rechnerManager.AktuellerRechner.RechnerTyp;
            if (aktueller.ToLower() == "matrix-rechner")
            {
                matrixR.ZeigeMatrixMenue();
            }
            else
            {
                ZeigeErweitertesMenue();
            }
            int wahl = (int)help.ZahlEinlesen("Deine Wahl: ");
            try
            {
                switch (wahl)
                {
                    case 1: // Berechnung durchführen
                        if (aktueller.ToLower() == "matrix-rechner")
                        {
                            matrixR.ZeigeBerechnungen();
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
                        if (rechnerManager.AktuellerRechner != null)
                            rechnerManager.AktuellerRechner.HistorieAnzeigen();
                        else
                            help.Write("Kein Rechner aktiv!");
                        break;
                    case 4: // Aktive Rechner anzeigen
                        rechnerManager.ZeigeAktiveRechner();
                        break;
                    case 5: // Datenbank-Historie
                        datenbankH.DatenbankHistorieAnzeigen();
                        break;
                    case 6: // Statistiken
                        statistikM.Show();
                        break;
                    case 7: // Einstellungen
                        konfigB.KonfigurationAendern();
                        break;
                    case 8: // Benutzer-Management
                        benutzerM.Show();
                        break;
                    case 0:
                        programmLaeuft = false;
                        break;
                    default:
                        help.Write("Ungültige Wahl!");
                        break;
                }
            }
            catch (Exception ex)
            {
                help.Write($"Fehler: {ex.Message}");
            }

            if (programmLaeuft)
            {
                help.Write("\nDrücke Enter für Hauptmenü...");
                Console.ReadLine();
            }
        }

        // Cleanup
        if (benutzerEinstellungen.config.AutoSpeichern)
        {
            konfigV.KonfigurationSpeichern();
            backup.BackupErstellen();
        }

        rechnerManager.SchliesseAlleRechner();
        help.Write("Auf Wiedersehen!");
    }

    static void ZeigeErweitertesMenue()
    {
        Benutzer akt = benutzerManagement.getBenutzer();
        help.Mischen();
        Console.Clear();
        string aktueller = rechnerManager.AktuellerRechner?.RechnerTyp ?? "Kein Rechner aktiv";
        help.Write($"=== TASCHENRECHNER v2.0 ===");
        help.Write($"Aktueller Rechner: {aktueller}");
        help.Write($"Benutzer: {akt?.Name ?? "Nicht angemeldet"}");
        Console.WriteLine();
        help.Write("1. Berechnung durchführen");
        help.Write("2. Rechner wechseln");
        help.Write("3. Historie anzeigen");
        help.Write("4. Aktive Rechner anzeigen");
        help.Write("5. Datenbank-Historie");
        help.Write("6. Statistiken");
        help.Write("7. Einstellungen");
        help.Write("8. Benutzer-Management");
        help.Write("0. Beenden");
        Console.WriteLine();
    }

    static void BerechnungDurchfuehren()
    {
        string aktueller = rechnerManager.AktuellerRechner.RechnerTyp;

        if (rechnerManager.AktuellerRechner == null)
        {
            help.Write("Kein Rechner aktiv! Wechsle zuerst zu einem Rechner.");
            return;
        }

        help.Write($"=== {rechnerManager.AktuellerRechner.RechnerTyp} ===");

        // Operationen je nach Rechner-Typ anzeigen
        ZeigeVerfuegbareOperationen(rechnerManager.AktuellerRechner);

        help.Write("Operation: ");
        string operation = Console.ReadLine();

        if (string.IsNullOrEmpty(operation))
            return;

        List<double> werte = new List<double>();
        help.Write("Gib Werte ein (beende mit 'fertig'):");

        bool zahlenSamm = true;
        int wertAnzahl = 0;

        while (zahlenSamm)
        {
            if (wertAnzahl == 2 && aktueller.ToLower() == "basis-rechner")
            {
                zahlenSamm = false;
                break;
            }

            help.Write($"Wert {werte.Count + 1}: ");
            string eingabe = Console.ReadLine();

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
                ergebnis = wissenschaftsRechner.BerechneMitKonstante(eingabe);
                werte.Add(ergebnis);
            }
            else
            {
                help.Write("Ungültige Eingabe!");
            }
        }

        if (werte.Count == 0)
        {
            help.Write("Keine Werte eingegeben!");
            return;
        }

        try
        {
            double ergebnis = rechnerManager.AktuellerRechner.Berechnen(operation, werte.ToArray());
            help.Write($"Ergebnis: {FormatUtils.FormatiereZahl(ergebnis, benutzerEinstellungen.config.Nachkommastellen)}");
        }
        catch (Exception ex)
        {
            help.Write($"Berechnungsfehler: {ex.Message}");
        }
    }

    static void RechnerWechseln()
    {
        help.Write("=== RECHNER WECHSELN ===");
        RechnerFactory.ZeigeVerfuegbareRechner();

        int wahl = (int)help.ZahlEinlesen("Rechner wählen (Nummer): ");

        var verfuegbareTypen = RechnerFactory.GetVerfuegbareRechnerTypen();

        if (wahl >= 1 && wahl <= verfuegbareTypen.Count)
        {
            string typName = verfuegbareTypen[wahl - 1];
            RechnerTyp typ = (RechnerTyp)Enum.Parse(typeof(RechnerTyp), typName);
            rechnerManager.WechsleZuRechner(typ);
        }
        else
        {
            help.Write("Ungültige Wahl!");
        }
    }

    static void ZeigeVerfuegbareOperationen(BaseRechner rechner)
    {
        help.Write("Verfügbare Operationen:");

        switch (rechner.RechnerTyp)
        {
            case "Basis-Rechner":
                help.Write("  +, -, *, /");
                break;
            case "Wissenschaftlich":
                help.Write("  +, -, *, /, sin, cos, sqrt, pow, log, ln");
                break;
            case "Finanz-Rechner":
                help.Write("  zinsen, zinseszinsen, annuitaet, barwert, Kreditplan erstellen");
                break;
            case "Statistik-Rechner":
                help.Write("  mittelwert, median, standardabweichung, min, max, spannweite");
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
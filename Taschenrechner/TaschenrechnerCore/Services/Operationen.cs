using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Models;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services
{
    public class Operationen
    {
        private ValidationUtils _validationUtils;
        private Hilfsfunktionen _help;
        private BenutzerManagement _benutzerManagement;
        private RechnerManager _rechnerManager;
        private WissenschaftlicherRechner _wissenschaftlicherRechner;
        private BenutzerEinstellungen _benutzerEinstellungen;

        public Operationen(
            Hilfsfunktionen help, 
            BenutzerManagement benutzerManagement, 
            RechnerManager rechnerManager, 
            WissenschaftlicherRechner wissenschaftlicherRechner, 
            BenutzerEinstellungen benutzerEinstellungen,
            ValidationUtils validation) 
        {
            _help = help;
            _benutzerEinstellungen = benutzerEinstellungen;
            _benutzerManagement = benutzerManagement;
            _rechnerManager = rechnerManager;
            _wissenschaftlicherRechner = wissenschaftlicherRechner;
            _validationUtils = validation;
        }

        public void ZeigeVerfuegbareOperationen(BaseRechner rechner)
        {
            _help.WriteInfo("Verfügbare Operationen:");

            switch (rechner.RechnerTyp)
            {
                case "Basis-Rechner":
                    _help.WriteInfo("  +, -, *, /");
                    break;
                case "Wissenschaftlich":
                    _help.WriteInfo("  +, -, *, /, sin, cos, sqrt, pow, log, ln");
                    break;
                case "Finanz-Rechner":
                    _help.WriteInfo("  zinsen, zinseszinsen, annuitaet, barwert, Kreditplan erstellen");
                    break;
                case "Statistik-Rechner":
                    _help.WriteInfo("  mittelwert, median, standardabweichung, min, max, spannweite");
                    break;
            }
        }

        public void BerechnungDurchfuehren()
        {
            string aktueller = _rechnerManager.AktuellerRechner.RechnerTyp;

            if (_rechnerManager.AktuellerRechner == null)
            {
                _help.WriteWarning("Kein Rechner aktiv! Wechsle zuerst zu einem Rechner.");
                return;
            }

            _help.WriteInfo($"=== {_rechnerManager.AktuellerRechner.RechnerTyp} ===");

            // Operationen je nach Rechner-Typ anzeigen
            ZeigeVerfuegbareOperationen(_rechnerManager.AktuellerRechner);

            string operation = _help.Einlesen("Operation: ");

            if (string.IsNullOrEmpty(operation))
                return;

            List<double> werte = new List<double>();
            _help.WriteInfo("Gib Werte ein (beende mit 'fertig'):");

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
                else if (_validationUtils.isKonstante(eingabe))
                {
                    double ergebnis;
                    // Konstante direkt ersetzen
                    ergebnis = _wissenschaftlicherRechner.BerechneMitKonstante(eingabe);
                    werte.Add(ergebnis);
                }
                else
                {
                    _help.WriteWarning("Ungültige Eingabe!");
                }
            }

            if (werte.Count == 0)
            {
                _help.WriteWarning("Keine Werte eingegeben!");
                return;
            }

            try
            {
                double ergebnis = _rechnerManager.AktuellerRechner.Berechnen(operation, werte.ToArray());
                _help.WriteInfo($"Ergebnis: {FormatUtils.FormatiereZahl(ergebnis, _benutzerEinstellungen.getConfig().Nachkommastellen)}");
            }
            catch (Exception ex)
            {
                _help.WriteError($"Berechnungsfehler: {ex.Message}");
            }
        }
    }
}

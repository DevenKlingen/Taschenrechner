using TaschenrechnerCore.Utils;
using TaschenrechnerCore.Models;
using System.Text;
using System.Text.Json;

namespace TaschenrechnerCore.Services;

public class HistorieZeigen
{
    private readonly HistorieVerwaltung _historieVerwaltung;
    private readonly Hilfsfunktionen _help;
    private readonly BenutzerEinstellungen _benutzerEinstellungen;

    public HistorieZeigen(
        HistorieVerwaltung historieVerwaltung, 
        Hilfsfunktionen help, 
        BenutzerEinstellungen benutzerEinstellungen)
    {
        _historieVerwaltung = historieVerwaltung;
        _help = help;
        _benutzerEinstellungen = benutzerEinstellungen;
    }



    /// <summary>
    /// Zeigt die aktuelle Historie an
    /// </summary>
    public void HistorieAnzeigen()
    {
        double.TryParse(_help.Einlesen("\nMöchtest du die Historie aus der Datei (1) oder aus der DB (2)?"), out double wahl);
        if (wahl == 1)
        {
            _help.Write("\n=== BERECHNUNGSHISTORIE ===");
            if (_historieVerwaltung._berechnungsHistorie.Count == 0)
            {
                _help.Write("Keine Berechnungen durchgeführt.");
                return;
            }
            else
            {
                foreach (var eintrag in _historieVerwaltung._berechnungsHistorie)
                {
                    _help.Write(eintrag);
                }
            }
        }
        else if (wahl == 2)
        {
            bool dbHistorie = true;
            int seite = 1;
            int eintraege = 10;

            while (dbHistorie)
            {
                _help.Write("\n=== BERECHNUNGSHISTORIE AUS DER DB ===");
                _help.Write("Vorige Seite: <");
                _help.Write("Nächste Seite: >");
                _help.Write("Beenden: 0");
                _help.Write("Seite " + seite + ":");

                using var context = new TaschenrechnerContext();

                var berechnungen = context.Berechnungen
                    .OrderBy(b => b.Zeitstempel)
                    .Skip((seite - 1) * eintraege)
                    .Take(eintraege)
                    .ToList();

                if (berechnungen.Count == 0)
                {
                    _help.Write("Keine Berechnungen in der Datenbank gefunden.");
                    dbHistorie = false;
                    continue;
                }

                foreach (var berechnung in berechnungen)
                {
                    try
                    {
                        double[] eingaben = JsonSerializer.Deserialize<double[]>(berechnung.Eingaben);
                        if (berechnung.Operation == "$")
                        {
                            _help.setEncoding();
                            _help.Write($"[{berechnung.Zeitstempel:HH:mm:ss}] " + $"{eingaben[0]}€ = ${berechnung.Ergebnis.ToString($"F{_benutzerEinstellungen.getConfig().Nachkommastellen}")} " + $"({berechnung.Rechnertyp})");
                        }
                        else if (berechnung.Operation == "/, *")
                        {
                            _help.Write($"[{berechnung.Zeitstempel:HH:mm:ss}] " + $"({eingaben[0]} / {eingaben[1]}) * {eingaben[2]} = {berechnung.Ergebnis.ToString($"F{_benutzerEinstellungen.getConfig().Nachkommastellen}")} " + $"({berechnung.Rechnertyp})");
                        }
                        else if (berechnung.Operation == "*, /")
                        {
                            _help.Write($"[{berechnung.Zeitstempel:HH:mm:ss}] " + $"({eingaben[0]} * {eingaben[1]}) / {eingaben[2]} = {berechnung.Ergebnis.ToString($"F{_benutzerEinstellungen.getConfig().Nachkommastellen}")} " + $"({berechnung.Rechnertyp})");
                        }
                        else
                        {
                            string eingabenStr = string.Join($" {berechnung.Operation} ", eingaben);

                            _help.Write($"[{berechnung.Zeitstempel:HH:mm:ss}] " + $"{eingabenStr} = {berechnung.Ergebnis.ToString($"F{_benutzerEinstellungen.getConfig().Nachkommastellen}")} " + $"({berechnung.Rechnertyp})");
                        }

                        if (!string.IsNullOrEmpty(berechnung.Kommentar))
                        {
                            _help.Write($"    Kommentar: {berechnung.Kommentar}");
                        }
                    }
                    catch (Exception ex)
                    {
                        _help.Write($"Fehler beim Anzeigen einer Berechnung: {ex.Message}");
                    }
                }

                string input = _help.Einlesen("");
                if (input == "<")
                {
                    if (seite > 1)
                    {
                        seite--;
                    }
                }
                else if (input == ">")
                {
                    seite++;
                }
                else if (input == "0")
                {
                    dbHistorie = false;
                    _help.Write("Zurück zum Hauptmenü.");
                }
                else
                {
                    _help.Write("Ungültige Eingabe!");
                }
            }
        }
        else
        {
            _help.Write("Ungültige Wahl!");
        }
    }
}
using TaschenrechnerConsole;
using TaschenrechnerCore.Utils;
using TaschenrechnerCore.Models;
using System.Text;
using System.Text.Json;

namespace TaschenrechnerCore.Services;

public class HistorieZeigen
{
    static Program program = new Program();
    static Hilfsfunktionen help = new Hilfsfunktionen();

    /// <summary>
    /// Zeigt die aktuelle Historie an
    /// </summary>
    public void HistorieAnzeigen()
    {
        help.Write("\nMöchtest du die Historie aus der Datei (1) oder aus der DB (2)?");
        double.TryParse(Console.ReadLine(), out double wahl);
        if (wahl == 1)
        {
            help.Write("\n=== BERECHNUNGSHISTORIE ===");
            if (program.berechnungsHistorie.Count == 0)
            {
                help.Write("Keine Berechnungen durchgeführt.");
                return;
            }
            else
            {
                foreach (var eintrag in program.berechnungsHistorie)
                {
                    help.Write(eintrag);
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
                help.Write("\n=== BERECHNUNGSHISTORIE AUS DER DB ===");
                help.Write("Vorige Seite: <");
                help.Write("Nächste Seite: >");
                help.Write("Beenden: 0");
                help.Write("Seite " + seite + ":");

                using var context = new TaschenrechnerContext();

                var berechnungen = context.Berechnungen
                    .OrderBy(b => b.Zeitstempel)
                    .Skip((seite - 1) * eintraege)
                    .Take(eintraege)
                    .ToList();

                if (berechnungen.Count == 0)
                {
                    help.Write("Keine Berechnungen in der Datenbank gefunden.");
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
                            Console.OutputEncoding = Encoding.UTF8;
                            help.Write($"[{berechnung.Zeitstempel:HH:mm:ss}] " + $"{eingaben[0]}€ = ${berechnung.Ergebnis.ToString($"F{program.config.Nachkommastellen}")} " + $"({berechnung.Rechnertyp})");
                        }
                        else if (berechnung.Operation == "/, *")
                        {
                            help.Write($"[{berechnung.Zeitstempel:HH:mm:ss}] " + $"({eingaben[0]} / {eingaben[1]}) * {eingaben[2]} = {berechnung.Ergebnis.ToString($"F{program.config.Nachkommastellen}")} " + $"({berechnung.Rechnertyp})");
                        }
                        else if (berechnung.Operation == "*, /")
                        {
                            help.Write($"[{berechnung.Zeitstempel:HH:mm:ss}] " + $"({eingaben[0]} * {eingaben[1]}) / {eingaben[2]} = {berechnung.Ergebnis.ToString($"F{program.config.Nachkommastellen}")} " + $"({berechnung.Rechnertyp})");
                        }
                        else
                        {
                            string eingabenStr = string.Join($" {berechnung.Operation} ", eingaben);

                            help.Write($"[{berechnung.Zeitstempel:HH:mm:ss}] " + $"{eingabenStr} = {berechnung.Ergebnis.ToString($"F{program.config.Nachkommastellen}")} " + $"({berechnung.Rechnertyp})");
                        }

                        if (!string.IsNullOrEmpty(berechnung.Kommentar))
                        {
                            help.Write($"    Kommentar: {berechnung.Kommentar}");
                        }
                    }
                    catch (Exception ex)
                    {
                        help.Write($"Fehler beim Anzeigen einer Berechnung: {ex.Message}");
                    }
                }

                help.Write("");
                string input = Console.ReadLine();
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
                    help.Write("Zurück zum Hauptmenü.");
                }
                else
                {
                    help.Write("Ungültige Eingabe!");
                }
            }
        }
        else
        {
            help.Write("Ungültige Wahl!");
        }
    }
}
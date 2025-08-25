using TaschenrechnerCore.Utils;
using TaschenrechnerCore.Models;
using System.Text.Json;

namespace TaschenrechnerCore.Services;

public class DatenbankBerechnungen
{
    static BenutzerManagement benutzerManagement = new();
    static Hilfsfunktionen help = new Hilfsfunktionen();

    public void BerechnungInDatenbankSpeichern(string operation, double[] eingaben, double ergebnis, string kommentar = "", string rechnertyp = "Basis")
    {
        Benutzer akt = benutzerManagement.getBenutzer();
        if (akt == null)
        {
            help.Write("Kein Benutzer angemeldet!");
            return;
        }

        try
        {
            using var context = new TaschenrechnerContext();

            var berechnungDB = new BerechnungDB
            {
                BenutzerId = akt.Id,
                Operation = operation,
                Eingaben = JsonSerializer.Serialize(eingaben), // Array als JSON speichern
                Ergebnis = ergebnis,
                Kommentar = kommentar,
                Rechnertyp = rechnertyp,
                Zeitstempel = DateTime.Now
            };

            context.Berechnungen.Add(berechnungDB);
            context.SaveChanges();

            help.Write("Berechnung in Datenbank gespeichert.");
        }
        catch (Exception ex)
        {
            help.Write($"Fehler beim Speichern in Datenbank: {ex.Message}");
        }
    }

    public void BerechnungenSuchen()
    {
        Benutzer akt = benutzerManagement.getBenutzer();
        if (akt == null)
        {
            help.Write("Kein Benutzer angemeldet!");
            return;
        }

        help.Write("=== BERECHNUNGEN SUCHEN ===");
        help.Write("1. Nach Operation suchen (+, -, *, /)");
        help.Write("2. Nach Datum suchen");
        help.Write("3. Nach Rechnertyp suchen");
        help.Write("4. Nach Ergebnis-Bereich suchen");

        int wahl = (int)help.ZahlEinlesen("Deine Wahl (1-4): ");

        using var context = new TaschenrechnerContext();
        IQueryable<BerechnungDB> query = context.Berechnungen
            .Where(b => b.BenutzerId == akt.Id);

        switch (wahl)
        {
            case 1:
                help.Write("Operation eingeben (+, -, *, /, etc.): ");
                string operation = Console.ReadLine();
                query = query.Where(b => b.Operation == operation);
                break;

            case 2:
                help.Write("Datum (yyyy-mm-dd): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime datum))
                {
                    var startDatum = datum.Date;
                    var endDatum = startDatum.AddDays(1);
                    query = query.Where(b => b.Zeitstempel >= startDatum && b.Zeitstempel < endDatum);
                }
                break;

            case 3:
                help.Write("Rechnertyp (Basis, Wissenschaftlich, Finanz): ");
                string rechnertyp = Console.ReadLine();
                query = query.Where(b => b.Rechnertyp.Contains(rechnertyp));
                break;

            case 4:
                double min = help.ZahlEinlesen("Minimum: ");
                double max = help.ZahlEinlesen("Maximum: ");
                query = query.Where(b => b.Ergebnis >= min && b.Ergebnis <= max);
                break;

            default:
                help.Write("Ungültige Wahl!");
                return;
        }

        var ergebnisse = query.OrderByDescending(b => b.Zeitstempel).ToList();

        help.Write($"\n{ergebnisse.Count} Ergebnisse gefunden:");
        foreach (var berechnung in ergebnisse)
        {
            double[] eingaben = JsonSerializer.Deserialize<double[]>(berechnung.Eingaben);
            string eingabenStr = string.Join($" {berechnung.Operation} ", eingaben);

            help.Write($"[{berechnung.Zeitstempel:dd.MM.yyyy HH:mm}] " +
                  $"{eingabenStr} = {berechnung.Ergebnis} ({berechnung.Rechnertyp})");
        }
    }
}
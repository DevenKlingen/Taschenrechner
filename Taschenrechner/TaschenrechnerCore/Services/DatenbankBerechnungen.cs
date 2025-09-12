using TaschenrechnerCore.Utils;
using TaschenrechnerCore.Models;
using System.Text.Json;

namespace TaschenrechnerCore.Services;

public class DatenbankBerechnungen
{
    private readonly BenutzerManagement _benutzerManagement;
    private readonly Hilfsfunktionen _help;

    public DatenbankBerechnungen(
        BenutzerManagement benutzerManagement,
        Hilfsfunktionen help)
    {
        _benutzerManagement = benutzerManagement;
        _help = help;
    }

    public void BerechnungInDatenbankSpeichern(string operation, List<double> eingaben, double ergebnis, string kommentar = "", string rechnertyp = "Basis")
    {
        Benutzer akt = _benutzerManagement.getBenutzer();
        if (akt == null)
        {
            _help.WriteInfo("Kein Benutzer angemeldet!");
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
        }
        catch (Exception ex)
        {
            _help.WriteError($"Fehler beim Speichern in Datenbank: {ex.Message}");
        }
    }

    public void BerechnungenSuchen()
    {
        Benutzer akt = _benutzerManagement.getBenutzer();
        if (akt == null)
        {
            _help.WriteInfo("Kein Benutzer angemeldet!");
            return;
        }

        _help.WriteInfo("\n=== BERECHNUNGEN SUCHEN ===");
        _help.WriteInfo("1. Nach Operation suchen (+, -, *, /)");
        _help.WriteInfo("2. Nach Datum suchen");
        _help.WriteInfo("3. Nach Rechnertyp suchen");
        _help.WriteInfo("4. Nach Ergebnis-Bereich suchen");

        int wahl = (int)_help.ZahlEinlesen("Deine Wahl (1-4): ");

        using var context = new TaschenrechnerContext();
        IQueryable<BerechnungDB> query = context.Berechnungen
            .Where(b => b.BenutzerId == akt.Id);

        switch (wahl)
        {
            case 1:
                string operation = _help.Einlesen("Operation eingeben (+, -, *, /, etc.): ");
                query = query.Where(b => b.Operation == operation);
                break;

            case 2:
                if (DateTime.TryParse(_help.Einlesen("Datum (yyyy-mm-dd): "), out DateTime datum))
                {
                    var startDatum = datum.Date;
                    var endDatum = startDatum.AddDays(1);
                    query = query.Where(b => b.Zeitstempel >= startDatum && b.Zeitstempel < endDatum);
                }
                break;

            case 3:
                string rechnertyp = _help.Einlesen("Rechnertyp (Basis, Wissenschaftlich, Finanz): ");
                query = query.Where(b => b.Rechnertyp.Contains(rechnertyp));
                break;

            case 4:
                double min = _help.ZahlEinlesen("Minimum: ");
                double max = _help.ZahlEinlesen("Maximum: ");
                query = query.Where(b => b.Ergebnis >= min && b.Ergebnis <= max);
                break;

            default:
                _help.WriteInfo("Ungültige Wahl!");
                return;
        }

        var ergebnisse = query.OrderByDescending(b => b.Zeitstempel).ToList();

        _help.WriteInfo($"\n{ergebnisse.Count} Ergebnisse gefunden:");
        foreach (var berechnung in ergebnisse)
        {
            double[] eingaben = JsonSerializer.Deserialize<double[]>(berechnung.Eingaben);
            string eingabenStr = string.Join($" {berechnung.Operation} ", eingaben);

            _help.WriteInfo($"[{berechnung.Zeitstempel:dd.MM.yyyy HH:mm}] " +
                  $"{eingabenStr} = {berechnung.Ergebnis} ({berechnung.Rechnertyp})");
        }
    }
}
using TaschenrechnerCore.Utils;
using System.Text;

namespace TaschenrechnerCore.Services;

public class WaehrungsRechner
{
    private readonly Hilfsfunktionen _help;
    private readonly HistorienBearbeitung _historienBearbeitung;
    private readonly DatenbankBerechnungen _datenbankBerechnungen;
    private readonly BenutzerEinstellungen _benutzerEinstellungen;
    public WaehrungsRechner(
        Hilfsfunktionen help, 
        HistorienBearbeitung historienB, 
        DatenbankBerechnungen datenbankB,
        BenutzerEinstellungen benutzerEinstellungen)
    {
        _help = help;
        _historienBearbeitung = historienB;
        _datenbankBerechnungen = datenbankB;
        _benutzerEinstellungen = benutzerEinstellungen;
    }



    /// <summary>
    /// Rechnet Euro in Dollar mit einem Wechselkurs von 1.1 um
    /// </summary>
    public void WaehrungsRechnung()
    {
        _help.Write("\n=== WÄHRUNGSRECHNER ===");
        bool umgerechnet = false;
        while (!umgerechnet)
        {
            if (decimal.TryParse(_help.Einlesen("Betrag in Euro: "), out decimal euro))
            {
                _help.setEncoding();
                decimal dollar = euro * 1.1m; // Beispiel-Wechselkurs
                _help.Write($"{euro}€ = ${dollar}");

                double euroDouble = (double)euro;
                double dollarDouble = (double)dollar;
                double[] eingaben = { euroDouble };
                _historienBearbeitung.BerechnungHinzufuegen(_benutzerEinstellungen, "$", eingaben, dollarDouble);
                _datenbankBerechnungen.BerechnungInDatenbankSpeichern("$", eingaben, dollarDouble);
                umgerechnet = true;
            }
            else
            {
                _help.Write("Ungültige Eingabe!");
            }
        }
    }
}

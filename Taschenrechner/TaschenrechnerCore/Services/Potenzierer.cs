using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class Potenzierer
{
    private readonly Hilfsfunktionen _help;
    private readonly HistorienBearbeitung _historienBearbeitung;
    private readonly DatenbankBerechnungen _datenbankBerechnungen;
    private readonly BenutzerEinstellungen _benutzerEinstellungen;

    public Potenzierer(
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
    /// Rechnet eine Potenz einer Zahl aus
    /// </summary>
    public void Potenz()
    {
        double zahl = _help.ZahlEinlesen("Gib die erste Zahl ein: ");
        double potenz = _help.ZahlEinlesen("Gib die Potenz ein: ");

        double ergebnis = Math.Pow(zahl, potenz);
        _help.Write($"Ergebnis: {zahl} ^ {potenz} = {ergebnis}");

        double[] eingaben = { zahl, potenz };
        _historienBearbeitung.BerechnungHinzufuegen(_benutzerEinstellungen, "^", eingaben, ergebnis);

        _datenbankBerechnungen.BerechnungInDatenbankSpeichern("^", eingaben, ergebnis);
    }
}

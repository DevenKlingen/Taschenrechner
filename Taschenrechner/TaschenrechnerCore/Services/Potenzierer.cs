using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class Potenzierer
{
    static Hilfsfunktionen help = new Hilfsfunktionen();
    static HistorienBearbeitung historienB = new HistorienBearbeitung();
    static DatenbankBerechnungen datenbankB = new DatenbankBerechnungen();

    /// <summary>
    /// Rechnet eine Potenz einer Zahl aus
    /// </summary>
    public void Potenz()
    {
        double zahl = help.ZahlEinlesen("Gib die erste Zahl ein: ");
        double potenz = help.ZahlEinlesen("Gib die Potenz ein: ");

        double ergebnis = Math.Pow(zahl, potenz);
        help.Write($"Ergebnis: {zahl} ^ {potenz} = {ergebnis}");

        double[] eingaben = { zahl, potenz };
        historienB.BerechnungHinzufuegen("^", eingaben, ergebnis);

        datenbankB.BerechnungInDatenbankSpeichern("^", eingaben, ergebnis);
    }
}

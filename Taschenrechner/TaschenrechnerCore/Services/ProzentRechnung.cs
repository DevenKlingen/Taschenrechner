using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class ProzentRechnung
{
    static Hilfsfunktionen help = new Hilfsfunktionen();
    static HistorienBearbeitung historienB = new HistorienBearbeitung();
    static DatenbankBerechnungen datenbankB = new DatenbankBerechnungen();

    /// <summary>
    /// Berechnet den Prozentwert anhand des gegebenen Grundwertes und Prozentsatzes
    /// </summary>
    public void Prozentwert()
    {
        double grundwert = help.ZahlEinlesen("Gib den Grundwert ein: ");
        double prozentsatz = help.ZahlEinlesen("Gib den Prozentsatz ein: ");
        double prozentwert = (prozentsatz / 100) * grundwert;

        help.Write($"Der Prozentwert von {grundwert} bei {prozentsatz}% ist {prozentwert}");

        double[] eingaben = { prozentsatz, 100, grundwert };
        historienB.BerechnungHinzufuegen("/, *", eingaben, prozentwert);

        datenbankB.BerechnungInDatenbankSpeichern("/, *", eingaben, prozentwert);
    }

    /// <summary>
    /// Berechnet den Prozentsatz anhand des gegebenen Grundwertes und Prozentwertes
    /// </summary>
    public void ProzentualerAnteil()
    {
        double grundwert = help.ZahlEinlesen("Gib den Grundwert ein: ");
        double prozentwert = help.ZahlEinlesen("Gib den Prozentwert ein: ");
        double prozentsatz = (prozentwert / grundwert) * 100;

        help.Write($"Der Prozentsatz von {prozentwert} bei {grundwert} ist {prozentsatz}%");

        double[] eingaben = { prozentwert, grundwert, 100 };
        historienB.BerechnungHinzufuegen("/, *", eingaben, prozentsatz);

        datenbankB.BerechnungInDatenbankSpeichern("/, *", eingaben, prozentsatz);
    }

    /// <summary>
    /// Berechnet den Grundwert anhand des gegebenen Prozentwertes und Prozentsatzes
    /// </summary>
    public void Grundwert()
    {
        double prozentwert = help.ZahlEinlesen("Gib den Prozentwert ein: ");
        double prozentsatz = help.ZahlEinlesen("Gib den Prozentsatz ein: ");
        double grundwert = (prozentwert * 100) / prozentsatz;

        help.Write($"Der Grundwert von {prozentwert} bei {prozentsatz}% ist {grundwert}");

        double[] eingaben = { prozentwert, 100, prozentsatz };
        historienB.BerechnungHinzufuegen("*, /", eingaben, grundwert);

        datenbankB.BerechnungInDatenbankSpeichern("*, /", eingaben, grundwert);
    }
}
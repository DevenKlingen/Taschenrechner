using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class ProzentRechnung
{
    private readonly Hilfsfunktionen _help;
    private readonly HistorienBearbeitung _historienBearbeitung;
    private readonly DatenbankBerechnungen _datenbankBerechnungen;
    private readonly BenutzerEinstellungen _benutzerEinstellungen;

    public ProzentRechnung(
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
    /// Berechnet den Prozentwert anhand des gegebenen Grundwertes und Prozentsatzes
    /// </summary>
    public void Prozentwert()
    {
        double grundwert = _help.ZahlEinlesen("Gib den Grundwert ein: ");
        double prozentsatz = _help.ZahlEinlesen("Gib den Prozentsatz ein: ");
        double prozentwert = (prozentsatz / 100) * grundwert;

        _help.Write($"Der Prozentwert von {grundwert} bei {prozentsatz}% ist {prozentwert}");

        double[] eingaben = { prozentsatz, 100, grundwert };
        _historienBearbeitung.BerechnungHinzufuegen(_benutzerEinstellungen, "/, *", eingaben, prozentwert);

        _datenbankBerechnungen.BerechnungInDatenbankSpeichern("/, *", eingaben, prozentwert);
    }

    /// <summary>
    /// Berechnet den Prozentsatz anhand des gegebenen Grundwertes und Prozentwertes
    /// </summary>
    public void ProzentualerAnteil()
    {
        double grundwert = _help.ZahlEinlesen("Gib den Grundwert ein: ");
        double prozentwert = _help.ZahlEinlesen("Gib den Prozentwert ein: ");
        double prozentsatz = (prozentwert / grundwert) * 100;

        _help.Write($"Der Prozentsatz von {prozentwert} bei {grundwert} ist {prozentsatz}%");

        double[] eingaben = { prozentwert, grundwert, 100 };
        _historienBearbeitung.BerechnungHinzufuegen(_benutzerEinstellungen, "/, *", eingaben, prozentsatz);

        _datenbankBerechnungen.BerechnungInDatenbankSpeichern("/, *", eingaben, prozentsatz);
    }

    /// <summary>
    /// Berechnet den Grundwert anhand des gegebenen Prozentwertes und Prozentsatzes
    /// </summary>
    public void Grundwert()
    {
        double prozentwert = _help.ZahlEinlesen("Gib den Prozentwert ein: ");
        double prozentsatz = _help.ZahlEinlesen("Gib den Prozentsatz ein: ");
        double grundwert = (prozentwert * 100) / prozentsatz;

        _help.Write($"Der Grundwert von {prozentwert} bei {prozentsatz}% ist {grundwert}");

        double[] eingaben = { prozentwert, 100, prozentsatz };
        _historienBearbeitung.BerechnungHinzufuegen(_benutzerEinstellungen, "*, /", eingaben, grundwert);

        _datenbankBerechnungen.BerechnungInDatenbankSpeichern("*, /", eingaben, grundwert);
    }
}
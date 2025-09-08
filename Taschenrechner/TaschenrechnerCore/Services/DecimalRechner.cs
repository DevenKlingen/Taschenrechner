using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class DecimalRechner
{
    private readonly HistorienBearbeitung _historieBearbeitung;
    private readonly Hilfsfunktionen _help;
    private readonly DatenbankBerechnungen _datenbankBerechnungen;
    private readonly BenutzerEinstellungen _benutzerEinstellungen;

    public DecimalRechner(
        HistorienBearbeitung historieBearbeitung, 
        Hilfsfunktionen help, 
        DatenbankBerechnungen datenbankBerechnungen,
        BenutzerEinstellungen benutzerEinstellungen)
    {
        _historieBearbeitung = historieBearbeitung;
        _help = help;
        _datenbankBerechnungen = datenbankBerechnungen;
        _benutzerEinstellungen = benutzerEinstellungen;
    }



    /// <summary>
    /// Wandelt eine Dezimalzahl in die dazugehörige Binärzahl um
    /// </summary>
    public void ToBinary()
    {
        _help.Write("Gib eine Zahl ein: ");
        while (true)
        {
            if (int.TryParse(_help.Einlesen("Deine Zahl: "), out int zahl))
            {
                string binär = Convert.ToString(zahl, 2);
                _help.Write($"Die Binärdarstellung von {zahl} ist {binär}");

                double.TryParse(binär, out double bin);
                double[] eingaben = { zahl };
                _historieBearbeitung.BerechnungHinzufuegen(_benutzerEinstellungen, "binary", eingaben, bin);
                _datenbankBerechnungen.BerechnungInDatenbankSpeichern("binary", eingaben, bin);
                break;
            }
            else
            {
                _help.Write("Ungültige Eingabe!");
            }
        }
    }

    /// <summary>
    /// Wandelt eine Dezimalzahl in die dazugehörige Hexadezimalzahl um
    /// </summary>
    public void ToHexadecimal()
    {
        _help.Write("Gib eine Zahl ein: ");
        while (true)
        {
            if (int.TryParse(_help.Einlesen("Deine Zahl: "), out int zahl))
            { 
                string hexadezimal = Convert.ToString(zahl, 16).ToUpper();
                _help.Write($"Die Hexadezimaldarstellung von {zahl} ist {hexadezimal}");

                double.TryParse(hexadezimal, out double hex);
                double[] eingaben = { zahl };
                _historieBearbeitung.BerechnungHinzufuegen(_benutzerEinstellungen,"hexadecimal", eingaben, hex);
                _datenbankBerechnungen.BerechnungInDatenbankSpeichern("hexadecimal", eingaben, hex);
                break;
            }
            else
            {
                _help.Write("Ungültige Eingabe!");
            }
        }
    }
}
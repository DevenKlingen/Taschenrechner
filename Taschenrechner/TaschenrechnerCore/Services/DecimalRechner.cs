using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class DecimalRechner
{
    static HistorienBearbeitung historieB = new HistorienBearbeitung();
    static Hilfsfunktionen help = new Hilfsfunktionen();
    static DatenbankBerechnungen datenbankB = new DatenbankBerechnungen();

    /// <summary>
    /// Wandelt eine Dezimalzahl in die dazugehörige Binärzahl um
    /// </summary>
    public void ToBinary()
    {
        help.Write("Gib eine Zahl ein: ");
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int zahl))
            {
                string binär = Convert.ToString(zahl, 2);
                help.Write($"Die Binärdarstellung von {zahl} ist {binär}");

                double.TryParse(binär, out double bin);
                double[] eingaben = { zahl };
                historieB.BerechnungHinzufuegen("binary", eingaben, bin);
                datenbankB.BerechnungInDatenbankSpeichern("binary", eingaben, bin);
                break;
            }
            else
            {
                help.Write("Ungültige Eingabe!");
            }
        }
    }

    /// <summary>
    /// Wandelt eine Dezimalzahl in die dazugehörige Hexadezimalzahl um
    /// </summary>
    public void ToHexadecimal()
    {
        help.Write("Gib eine Zahl ein: ");
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int zahl))
            {
                string hexadezimal = Convert.ToString(zahl, 16).ToUpper();
                help.Write($"Die Hexadezimaldarstellung von {zahl} ist {hexadezimal}");

                double.TryParse(hexadezimal, out double hex);
                double[] eingaben = { zahl };
                historieB.BerechnungHinzufuegen("hexadecimal", eingaben, hex);
                datenbankB.BerechnungInDatenbankSpeichern("hexadecimal", eingaben, hex);
                break;
            }
            else
            {
                help.Write("Ungültige Eingabe!");
            }
        }
    }
}
namespace TaschenrechnerCore.Models;

public class BerechnungErgebnis
{
    public DateTime Zeitstempel { get; set; }
    public required string Operation { get; set; }
    public required double[] Eingaben { get; set; }
    public double Ergebnis { get; set; }
    public required string RechnerTyp { get; set; }
}
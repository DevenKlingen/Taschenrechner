namespace TaschenrechnerCore.Models;

public class Berechnung
{
    public Program program = new Program();
    public DateTime Zeitstempel { get; set; }
    public string Operation { get; set; }
    public List<double> Eingaben { get; set; } = new List<double>();
    public double Ergebnis { get; set; }
    public string Kommentar { get; set; } = "";

    public override string ToString()
    {
        string zeitString = program.config.ZeigeZeitstempel
            ? $"[{Zeitstempel:HH:mm:ss}] "
            : "";
        return $"{zeitString}{string.Join($" {Operation} ", Eingaben)} = {Ergebnis.ToString($"F{program.config.Nachkommastellen}")}";
    }
}
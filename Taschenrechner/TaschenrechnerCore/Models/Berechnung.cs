using TaschenrechnerCore.Services;

namespace TaschenrechnerCore.Models;

public class Berechnung
{
    static BenutzerEinstellungen benutzerEinstellungen = new();
    public DateTime Zeitstempel { get; set; }
    public string Operation { get; set; }
    public List<double> Eingaben { get; set; } = new List<double>();
    public double Ergebnis { get; set; }
    public string Kommentar { get; set; } = "";

    public override string ToString()
    {
        string zeitString = benutzerEinstellungen.config.ZeigeZeitstempel
            ? $"[{Zeitstempel:HH:mm:ss}] "
            : "";
        return $"{zeitString}{string.Join($" {Operation} ", Eingaben)} = {Ergebnis.ToString($"F{benutzerEinstellungen.config.Nachkommastellen}")}";
    }
}
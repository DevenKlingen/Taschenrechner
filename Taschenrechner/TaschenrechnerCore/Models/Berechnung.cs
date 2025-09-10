using TaschenrechnerCore.Services;

namespace TaschenrechnerCore.Models;

public class Berechnung
{
    public BenutzerEinstellungen _benutzerEinstellungen { get; set; }
    public DateTime Zeitstempel { get; set; }
    public string Operation { get; set; }
    public List<double> Eingaben { get; set; } = new List<double>();
    public double Ergebnis { get; set; }
    public string Kommentar { get; set; } = "";
    public string RechnerTyp { get; set; } = "";

    public void setBenutzerEinstellungen(BenutzerEinstellungen benutzerEinstellungen)
    {
        _benutzerEinstellungen = benutzerEinstellungen;
    }
    public override string ToString()
    {
        string zeitString = _benutzerEinstellungen.getConfig().ZeigeZeitstempel
            ? $"[{Zeitstempel:HH:mm:ss}] "
            : "";
        return $"{zeitString}{string.Join($" {Operation} ", Eingaben)} = {Ergebnis.ToString($"F{_benutzerEinstellungen.getConfig().Nachkommastellen}")}";
    }
}
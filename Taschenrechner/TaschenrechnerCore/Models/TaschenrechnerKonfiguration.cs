namespace TaschenrechnerCore.Models;

// Konfiguration für den Taschenrechner
public class TaschenrechnerKonfiguration
{
    public string Thema { get; set; } = "Hell";
    public int Nachkommastellen { get; set; } = 2;
    public string Standardrechner { get; set; } = "Basis";
    public bool AutoSpeichern { get; set; } = true;
    public string Sprache { get; set; } = "Deutsch";
    public bool ZeigeZeitstempel { get; set; } = true;
}

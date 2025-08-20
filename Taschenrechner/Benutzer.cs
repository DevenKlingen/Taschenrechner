using System.ComponentModel.DataAnnotations;

namespace MeinTaschenrechner
{

    public class Benutzer
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public DateTime ErstelltAm { get; set; } = DateTime.Now;

        [MaxLength(100)]
        public string Email { get; set; }

        public List<BerechnungDB> Berechnungen { get; set; } = new List<BerechnungDB>();
    }

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
}
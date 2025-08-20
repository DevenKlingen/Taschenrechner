using System.ComponentModel.DataAnnotations;

namespace MeinTaschenrechner
{
    public class BerechnungDB
    {
        public int Id { get; set; }

        public DateTime Zeitstempel { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(10)]
        public string Operation { get; set; } //Rechenzeichen wie +, -, *, /

        [Required]
        public string Eingaben { get; set; } // JSON-String der Eingaben

        public double Ergebnis { get; set; }

        [MaxLength(200)]
        public string Kommentar { get; set; } // Optionaler Kommentar zur Berechnung

        [Required]
        [MaxLength(50)]
        public string Rechnertyp { get; set; } // Basis, Wissenschaftlich, Finanz

        public int BenutzerId { get; set; }
        public Benutzer Benutzer { get; set; }
    }
}

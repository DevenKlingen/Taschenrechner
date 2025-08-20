using System.ComponentModel.DataAnnotations;

namespace MeinTaschenrechner
{
    public class Einstellung
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Schluessel { get; set; } // z.B. "Nachkommastellen", "Thema"

        [Required]
        public string Wert { get; set; }

        public int BenutzerId { get; set; }
        public Benutzer Benutzer { get; set; }
    }
}
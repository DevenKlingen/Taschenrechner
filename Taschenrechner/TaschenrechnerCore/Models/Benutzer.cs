namespace TaschenrechnerCore.Models;

using System.ComponentModel.DataAnnotations;

public class Benutzer
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public required string Name { get; set; }

    public DateTime ErstelltAm { get; set; } = DateTime.Now;

    [MaxLength(100)]
    public string? Email { get; set; }

    public List<BerechnungDB> Berechnungen { get; set; } = new List<BerechnungDB>();
}
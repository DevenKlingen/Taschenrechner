using Microsoft.EntityFrameworkCore;

namespace MeinTaschenrechner
{
    public class TaschenrechnerContext : DbContext
    {
        public DbSet<Benutzer> Benutzer { get; set; }
        public DbSet<BerechnungDB> Berechnungen { get; set; }
        public DbSet<Einstellung> Einstellungen { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=C:\\temp\\taschenrechner.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Beziehnungen definieren
            modelBuilder.Entity<BerechnungDB>()
                .HasOne(b => b.Benutzer)
                .WithMany(u => u.Berechnungen)
                .HasForeignKey(b => b.BenutzerId);

            modelBuilder.Entity<Einstellung>()
                .HasOne(e => e.Benutzer)
                .WithMany()
                .HasForeignKey(e => e.BenutzerId);

            // Indizes für bessere Performance
            modelBuilder.Entity<BerechnungDB>()
                .HasIndex(b => b.Zeitstempel);

            modelBuilder.Entity<Benutzer>()
                .HasIndex(u => u.Name)
                .IsUnique();
        }
    }
}
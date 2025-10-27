using Microsoft.EntityFrameworkCore;
using spaV1.Models;

namespace spaV1.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<RendezVous> RendezVous { get; set; }
        public DbSet<Paiement> Paiements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ensure decimal properties use an explicit SQL type to avoid precision/scale warnings
            modelBuilder.Entity<Service>()
                .Property(s => s.Prix)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Paiement>()
                .Property(p => p.Montant)
                .HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);
        }
    }
}

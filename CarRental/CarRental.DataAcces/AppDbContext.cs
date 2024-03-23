using CarRental.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarRental.DataAcces
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Rental> Rentals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().
                HasMany(c => c.Rentals).
                WithOne(r => r.Client);

            modelBuilder.Entity<Vehicle>().
                HasMany(v => v.Rentals).
                WithOne(r => r.Vehicle);

            modelBuilder.Entity<Rental>().
                HasOne(r => r.Client).
                WithMany(c => c.Rentals);

            modelBuilder.Entity<Rental>().
                HasOne(r => r.Vehicle).
                WithMany(v => v.Rentals);
        }
    }
}
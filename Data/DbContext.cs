using Microsoft.EntityFrameworkCore;
using background_jobs.Entities;


namespace background_jobs.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Coin> Coins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coin>()
                .Property(c => c.Price)
                .HasPrecision(18, 2);
        }
    }
}
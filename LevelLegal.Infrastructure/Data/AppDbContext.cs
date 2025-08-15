using Microsoft.EntityFrameworkCore;
using LevelLegal.Domain.Entities;

namespace LevelLegal.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Matter> Matters { get; set; } = null!;
        public DbSet<Evidence> Evidence { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Matter>()
                .HasMany(m => m.EvidenceItems)
                .WithOne(e => e.Matter)
                .HasForeignKey(e => e.MatterId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using ChromeHistoryAnalyzer.Models;

namespace ChromeHistoryAnalyzer.Data
{
    public class ChromeHistoryDbContext : DbContext
    {
        public ChromeHistoryDbContext(DbContextOptions<ChromeHistoryDbContext> options)
            : base(options)
        {
        }

        public DbSet<ChromeUrl> Urls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChromeUrl>(entity =>
            {
                entity.ToTable("urls");
                entity.HasKey(e => e.Id);
            });
        }
    }
}
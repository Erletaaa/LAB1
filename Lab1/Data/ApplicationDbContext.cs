using GarageMarketProject.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GarageMarketProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public ApplicationDbContext() { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Favorite> Favorites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().ToTable("Categories");
            modelBuilder.Entity<Comment>().ToTable("Comments");
            modelBuilder.Entity<Follow>().ToTable("Follows");
            modelBuilder.Entity<Favorite>().ToTable("Favorites");
        }
    }
}

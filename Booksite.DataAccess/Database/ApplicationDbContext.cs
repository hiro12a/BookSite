using Book.Models;
using Microsoft.EntityFrameworkCore;

namespace BookSite.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
        }

        public DbSet<Category> Categories { get; set; }

        // Add data to the database without having to manually input it
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOder = 1},
                new Category { Id = 2, Name = "SciFi", DisplayOder = 2},
                new Category { Id = 3, Name = "History", DisplayOder = 3}
                );
        }
    }
}

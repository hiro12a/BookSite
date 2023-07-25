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
        public DbSet<Product> Products { get; set; }

        // Add data to the database without having to manually input it
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { CatId = 1, Name = "Action", DisplayOder = 1},
                new Category { CatId = 2, Name = "SciFi", DisplayOder = 2},
                new Category { CatId = 3, Name = "History", DisplayOder = 3}
                );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = 1,
                    Title = "Fortune Teller",
                    Author = "John Doe",
                    Description = "A fortune teller discovered a huge secrete that will change his life forver",
                    ISBN = "SD839201784",
                    ListPrice = 99,
                    Price = 90,
                    Price50 = 85,
                    Price100 = 80,
                    CategoryId = 1,
                    ImageUrl = ""
                },
                new Product
                {
                    ProductId = 2,
                    Title = "Miss fortune",
                    Author = "Thomas Wayne",
                    Description = "Read about how this young woman came from being poor to being rich",
                    ISBN = "DJ849273891",
                    ListPrice = 40,
                    Price = 30,
                    Price50 = 25,
                    Price100 = 20,
                    CategoryId = 2,
                    ImageUrl = ""
                },
                new Product
                {
                    ProductId = 3,
                    Title = "Lazy Bum",
                    Author = "Neko Sorry",
                    Description = "A book on how to be a lazy bum",
                    ISBN = "SK829363432",
                    ListPrice = 60,
                    Price = 50,
                    Price50 = 45,
                    Price100 = 40,
                    CategoryId = 3,
                    ImageUrl = ""
                },
                new Product
                {
                    ProductId = 4,
                    Title = "Stinky Hero",
                    Author = "Man Two",
                    Description = "A story of how a man who doesn't like to shower became a hero",
                    ISBN = "SD8764928487",
                    ListPrice = 50,
                    Price = 40,
                    Price50 = 35,
                    Price100 = 30,
                    CategoryId = 1,
                    ImageUrl = ""
                },
                new Product
                {
                    ProductId = 5,
                    Title = "Little Man",
                    Author = "Nick Enry",
                    Description = "Even though he is small, he has the strongest will of them all",
                    ISBN = "MX456482165",
                    ListPrice = 75,
                    Price = 65,
                    Price50 = 60,
                    Price100 = 55,
                    CategoryId = 2,
                    ImageUrl = ""
                }
                );
        }
    }
}

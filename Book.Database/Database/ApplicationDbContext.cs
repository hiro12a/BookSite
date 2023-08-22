using Book.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookSite.Database
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Company> Companys { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<ImageManager> ImageManagers { get; set; }

        // Add data to the database without having to manually input it
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { CatId = 1, Name = "Action", DisplayOder = 1},
                new Category { CatId = 2, Name = "SciFi", DisplayOder = 2},
                new Category { CatId = 3, Name = "History", DisplayOder = 3}
                );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = 1,
                    Title = "Apocalypse Gachapon",
                    Author = "Xuan Huang",
                    Description = "At the moment when life ceased to exist, Ye Zhongming returned to ten years ago, to that afternoon when the apocalypse began.\r\n\r\nWas it heaven’s favor or another punishment? Did he really have to re-live the cruel and frigid end of the world? Ye Zhongming decided to survive, not for anything else, but for those comrades who had fought and died together, for his unwavering lover! And he wanted to find answers.",
                    ListPrice = 25,
                    Price = 40,
                    Price50 = 35,
                    Price100 = 30,
                    CategoryId = 1,
                },
                new Product
                {
                    ProductId = 2,
                    Title = "Ex Rank Supporting Roles Replay in a Prestigious School",
                    Author = "Koi Wol Wol",
                    Description = "When I cleared the final chapter of a game that was a national failure, I became an unnamed side character in it.\r\n\r\nA non-standard, unmeasurable, EX rank side character.",
                    ListPrice = 40,
                    Price = 30,
                    Price50 = 25,
                    Price100 = 20,
                    CategoryId = 2,
                },
                new Product
                {
                    ProductId = 3,
                    Title = "He Came From the Grave",
                    Author = "Tu Yue Guan",
                    Description = "In 20XX, a few tomb robbers in the Northwest Desert entered an underground tomb and accidentally discovered that there was a young male corpse that had not decayed for a thousand years, and the wall of the tomb was engraved with Xu Fu Dongdu* looking for longevity- the epitome of medicine. Everyone was overjoyed, and brought out the things in the tomb…",
                    ListPrice = 60,
                    Price = 50,
                    Price50 = 45,
                    Price100 = 40,
                    CategoryId = 3,
            
                },
                new Product
                {
                    ProductId = 4,
                    Title = "Hell King",
                    Author = "Kim Nam Jae",
                    Description = "Yong Muryun, the Pope of the Demon Church who had been betrayed and killed. He has come back from hell in order to punish those who had killed him.",
                    ListPrice = 50,
                    Price = 40,
                    Price50 = 35,
                    Price100 = 30,
                    CategoryId = 1,
          
                },
                new Product
                {
                    ProductId = 5,
                    Title = "Kuma Kuma Kuma Bear",
                    Author = "Kumanano",
                    Description = "Yuna, a 15 years old girl started playing the world’s first VRMMO. She has earned billions of yen in stocks. She confines herself in her house playing the game without going to school. Today, a huge update has arrived. She obtains a non-transferable rare bear outfit. But the equipment is so embarrassing that she can’t wear it even in the game.",
                    ListPrice = 75,
                    Price = 65,
                    Price50 = 60,
                    Price100 = 55,
                    CategoryId = 2,
             
                },
                new Product
                {
                    ProductId = 6,
                    Title = "Lord of Glory",
                    Author = "Mei Changsheng",
                    Description = "This is the story of a man who has succeeded in evolving his mortal body and soul into that of a God.",
                    ListPrice = 75,
                    Price = 55,
                    Price50 = 50,
                    Price100 = 45,
                    CategoryId = 3,

                },
                new Product
                {
                    ProductId = 7,
                    Title = "Paper Rose",
                    Author = "Lin Di Er",
                    Description = "Nurse Bai Yan and Mayor’s assistant Kang Jian suddenly got married, but they realized that this was not a Cinderella and Prince storyline. He already had a lover in his arms, and there were political enemies watching from behind. Her mother-in-law regarded her as a thorn in her eye, while her father-in-law and her mother were old acquaintances. It was all very confusing.",
                    ListPrice = 65,
                    Price = 60,
                    Price50 = 55,
                    Price100 = 45,
                    CategoryId = 3,

                },
                new Product
                {
                    ProductId = 8,
                    Title = "VIP",
                    Author = "Toika",
                    Description = "The blacksmith Anvil created weapons alone in a ruined world. He came across a community, a hero universe, where there are heroes of every dimension.l",
                    ListPrice = 55,
                    Price = 45,
                    Price50 = 40,
                    Price100 = 35,
                    CategoryId = 1,

                }
                );
        }
    }
}

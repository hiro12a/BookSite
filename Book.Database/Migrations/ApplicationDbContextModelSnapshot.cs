﻿// <auto-generated />
using BookSite.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Book.Database.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Book.Models.Category", b =>
                {
                    b.Property<int>("CatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CatId"));

                    b.Property<int>("DisplayOder")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("CatId");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            CatId = 1,
                            DisplayOder = 1,
                            Name = "Action"
                        },
                        new
                        {
                            CatId = 2,
                            DisplayOder = 2,
                            Name = "SciFi"
                        },
                        new
                        {
                            CatId = 3,
                            DisplayOder = 3,
                            Name = "History"
                        });
                });

            modelBuilder.Entity("Book.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductId"));

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ISBN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("ListPrice")
                        .HasColumnType("float");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<double>("Price100")
                        .HasColumnType("float");

                    b.Property<double>("Price50")
                        .HasColumnType("float");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProductId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            ProductId = 1,
                            Author = "John Doe",
                            CategoryId = 1,
                            Description = "A fortune teller discovered a huge secrete that will change his life forver",
                            ISBN = "SD839201784",
                            ImageUrl = "",
                            ListPrice = 99.0,
                            Price = 90.0,
                            Price100 = 80.0,
                            Price50 = 85.0,
                            Title = "Fortune Teller"
                        },
                        new
                        {
                            ProductId = 2,
                            Author = "Thomas Wayne",
                            CategoryId = 2,
                            Description = "Read about how this young woman came from being poor to being rich",
                            ISBN = "DJ849273891",
                            ImageUrl = "",
                            ListPrice = 40.0,
                            Price = 30.0,
                            Price100 = 20.0,
                            Price50 = 25.0,
                            Title = "Miss fortune"
                        },
                        new
                        {
                            ProductId = 3,
                            Author = "Neko Sorry",
                            CategoryId = 3,
                            Description = "A book on how to be a lazy bum",
                            ISBN = "SK829363432",
                            ImageUrl = "",
                            ListPrice = 60.0,
                            Price = 50.0,
                            Price100 = 40.0,
                            Price50 = 45.0,
                            Title = "Lazy Bum"
                        },
                        new
                        {
                            ProductId = 4,
                            Author = "Man Two",
                            CategoryId = 1,
                            Description = "A story of how a man who doesn't like to shower became a hero",
                            ISBN = "SD8764928487",
                            ImageUrl = "",
                            ListPrice = 50.0,
                            Price = 40.0,
                            Price100 = 30.0,
                            Price50 = 35.0,
                            Title = "Stinky Hero"
                        },
                        new
                        {
                            ProductId = 5,
                            Author = "Nick Enry",
                            CategoryId = 2,
                            Description = "Even though he is small, he has the strongest will of them all",
                            ISBN = "MX456482165",
                            ImageUrl = "",
                            ListPrice = 75.0,
                            Price = 65.0,
                            Price100 = 55.0,
                            Price50 = 60.0,
                            Title = "Little Man"
                        });
                });

            modelBuilder.Entity("Book.Models.Product", b =>
                {
                    b.HasOne("Book.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });
#pragma warning restore 612, 618
        }
    }
}
using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace Infrastructure
{
   
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Book>? books { get; set; }
        public DbSet<Author>? authors { get; set; }
        public DbSet<Rating>? ratings { get; set; }
        public DbSet<Sell>? sells { get; set; }
        public DbSet<Category>? categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Author>()
                .HasMany(e => e.Books)
                .WithOne(e => e.Author)
                .HasForeignKey(e => e.AuthorId)
                .IsRequired();
            modelBuilder.Entity<Category>()
                .HasMany(e => e.Books)
                .WithOne(e => e.Category)
                .HasForeignKey(e => e.CategoryId)
                .IsRequired();
            modelBuilder.Entity<Book>()
                .HasMany(e => e.Ratings)
                .WithOne(e => e.Book)
                .HasForeignKey(e => e.BookId)
                .IsRequired();
            modelBuilder.Entity<Book>()
                .HasMany(e => e.Sells)
                .WithOne(e => e.Book)
                .HasForeignKey(e => e.BookId)
                .IsRequired();
            modelBuilder.Entity<AppUser>()
               .HasMany(e => e.Ratings)
               .WithOne(e => e.User)
               .HasForeignKey(e => e.UserId)
               .IsRequired();
            modelBuilder.Entity<AppUser>()
               .HasMany(e => e.Sells)
               .WithOne(e => e.User)
               .HasForeignKey(e => e.UserId)
               .IsRequired();

            //// Seed authors
            //modelBuilder.Entity<Author>().HasData(
            //    new Author { Id = Guid.NewGuid(), Name = "J.K. Rowling", Description = "British author known for the Harry Potter series." },
            //    new Author { Id = Guid.NewGuid(), Name = "George R.R. Martin", Description = "American author known for A Song of Ice and Fire series (Game of Thrones)." },
            //    new Author { Id = Guid.NewGuid(), Name = "Stephen King", Description = "American author known for horror and suspense novels." }
            //    // Add more authors as needed
            //);

            //// Seed categories
            //modelBuilder.Entity<Category>().HasData(
            //    new Category { Id = Guid.NewGuid(), Name = "Fantasy", Description = "Books belonging to the fantasy genre." },
            //    new Category { Id = Guid.NewGuid(), Name = "Adventure", Description = "Books belonging to the adventure genre." },
            //    new Category { Id = Guid.NewGuid(), Name = "Horror", Description = "Books belonging to the horror genre." }
            //    // Add more categories as needed
            //);

            //// Seed books
            //modelBuilder.Entity<Book>().HasData(
            //    // Author: J.K. Rowling
            //    new Book
            //    {
            //        Id = Guid.NewGuid(),
            //        Title = "Harry Potter and the Philosopher's Stone",
            //        Description = "The first book in the Harry Potter series.",
            //        Price = 19.99f,
            //        EditionDate = new DateTime(1997, 6, 26),
            //        ImageUrl = "https://example.com/harry_potter_1.jpg",
            //        stockQuantity = 100,
            //        DocUrl = "https://example.com/harry_potter_1.pdf",
            //        AuthorId = authors.FirstOrDefault(a => a.Name == "J.K. Rowling")?.Id ?? Guid.NewGuid(),
            //        CategoryId = categories.FirstOrDefault(c => c.Name == "Fantasy")?.Id ?? Guid.NewGuid()
            //    },
            //    new Book
            //    {
            //        Id = Guid.NewGuid(),
            //        Title = "Harry Potter and the Chamber of Secrets",
            //        Description = "The second book in the Harry Potter series.",
            //        Price = 21.99f,
            //        EditionDate = new DateTime(1998, 7, 2),
            //        ImageUrl = "https://example.com/harry_potter_2.jpg",
            //        stockQuantity = 80,
            //        DocUrl = "https://example.com/harry_potter_2.pdf",
            //        AuthorId = authors.FirstOrDefault(a => a.Name == "J.K. Rowling")?.Id ?? Guid.NewGuid(),
            //        CategoryId = categories.FirstOrDefault(c => c.Name == "Fantasy")?.Id ?? Guid.NewGuid()
            //    },
            //    // Add more books by J.K. Rowling
            //    // ... 

            //    // Author: George R.R. Martin
            //    new Book
            //    {
            //        Id = Guid.NewGuid(),
            //        Title = "A Game of Thrones",
            //        Description = "The first book in the A Song of Ice and Fire series.",
            //        Price = 24.99f,
            //        EditionDate = new DateTime(1996, 8, 6),
            //        ImageUrl = "https://example.com/got_1.jpg",
            //        stockQuantity = 120,
            //        DocUrl = "https://example.com/got_1.pdf",
            //        AuthorId = authors.FirstOrDefault(a => a.Name == "George R.R. Martin")?.Id ?? Guid.NewGuid(),
            //        CategoryId = categories.FirstOrDefault(c => c.Name == "Fantasy")?.Id ?? Guid.NewGuid()
            //    },
            //    new Book
            //    {
            //        Id = Guid.NewGuid(),
            //        Title = "A Clash of Kings",
            //        Description = "The second book in the A Song of Ice and Fire series.",
            //        Price = 26.99f,
            //        EditionDate = new DateTime(1998, 11, 16),
            //        ImageUrl = "https://example.com/got_2.jpg",
            //        stockQuantity = 90,
            //        DocUrl = "https://example.com/got_2.pdf",
            //        AuthorId = authors.FirstOrDefault(a => a.Name == "George R.R. Martin")?.Id ?? Guid.NewGuid(),
            //        CategoryId = categories.FirstOrDefault(c => c.Name == "Fantasy")?.Id ?? Guid.NewGuid()
            //    },
            //    // Add more books by George R.R. Martin
            //    // ... 

            //    // Author: Stephen King
            //    new Book
            //    {
            //        Id = Guid.NewGuid(),
            //        Title = "It",
            //        Description = "A horror novel about a menacing clown.",
            //        Price = 18.99f,
            //        EditionDate = new DateTime(1986, 9, 15),
            //        ImageUrl = "https://example.com/it.jpg",
            //        stockQuantity = 70,
            //        DocUrl = "https://example.com/it.pdf",
            //        AuthorId = authors.FirstOrDefault(a => a.Name == "Stephen King")?.Id ?? Guid.NewGuid(),
            //        CategoryId = categories.FirstOrDefault(c => c.Name == "Horror")?.Id ?? Guid.NewGuid()
            //    },
            //    new Book
            //    {
            //        Id = Guid.NewGuid(),
            //        Title = "The Shining",
            //        Description = "A horror novel set in an isolated hotel.",
            //        Price = 17.99f,
            //        EditionDate = new DateTime(1977, 1, 28),
            //        ImageUrl = "https://example.com/the_shining.jpg",
            //        stockQuantity = 60,
            //        DocUrl = "https://example.com/the_shining.pdf",
            //        AuthorId = authors.FirstOrDefault(a => a.Name == "Stephen King")?.Id ?? Guid.NewGuid(),
            //        CategoryId = categories.FirstOrDefault(c => c.Name == "Horror")?.Id ?? Guid.NewGuid()
            //    }
            //);

        }

    }
}



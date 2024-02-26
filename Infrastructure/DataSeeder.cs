using Core.Entities;

namespace Infrastructure
{

    public static class DataSeeder
    {
        public static void SeedData(DataContext context)
        {
            if (!context.authors.Any())
            {
                // Seed authors
                var authors = new List<Author>
            {
                new Author
                {
                    Id = Guid.NewGuid(),
                    Name = "J.K. Rowling",
                    Description = "British author known for the Harry Potter series.",
                },
                new Author
                {
                    Id = Guid.NewGuid(),
                    Name = "George R.R. Martin",
                    Description = "American author known for A Song of Ice and Fire series (Game of Thrones).",
                },
                new Author {
                    Id = Guid.NewGuid(),
                    Name = "Stephen King",
                    Description = "American author known for horror and suspense novels."
                }
            };

                context.authors.AddRange(authors);
                context.SaveChanges();
            }

            if (!context.categories.Any())
            {
                // Seed categories
                var categories = new List<Category>
            {
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Fantasy",
                    Description = "Books belonging to the fantasy genre."
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Adventure",
                    Description = "Books belonging to the adventure genre."
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Horror",
                    Description = "Books belonging to the horror genre."
                }
                // Add more categories as needed
            };

                context.categories.AddRange(categories);
                context.SaveChanges();
            }

            if (!context.books.Any())
            {
                // Seed books
                var books = new List<Book>
            {
                            new Book
                            {
                                Id = Guid.NewGuid(),
                                Title = "Harry Potter and the Philosopher's Stone",
                                Description = "The first book in the Harry Potter series.",
                                Price = 19.99f,
                                EditionDate = new DateTime(1997, 6, 26),
                                ImageUrl = "https://example.com/harry_potter_1.jpg",
                                stockQuantity = 100,
                                DocUrl = "https://example.com/harry_potter_1.pdf",
                                AuthorId = context.authors.FirstOrDefault(a => a.Name == "J.K. Rowling")?.Id ?? Guid.NewGuid(),
                                CategoryId = context.categories.FirstOrDefault(c => c.Name == "Fantasy")?.Id ?? Guid.NewGuid()
                            },
                            new Book
                            {
                                Id = Guid.NewGuid(),
                                Title = "Harry Potter and the Chamber of Secrets",
                                Description = "The second book in the Harry Potter series.",
                                Price = 21.99f,
                                EditionDate = new DateTime(1998, 7, 2),
                                ImageUrl = "https://example.com/harry_potter_2.jpg",
                                stockQuantity = 80,
                                DocUrl = "https://example.com/harry_potter_2.pdf",
                                AuthorId = context.authors.FirstOrDefault(a => a.Name == "J.K. Rowling")?.Id ?? Guid.NewGuid(),
                                CategoryId = context.categories.FirstOrDefault(c => c.Name == "Fantasy")?.Id ?? Guid.NewGuid()
                            },
                            // Add more books by J.K. Rowling
                            // ... 

                            // Author: George R.R. Martin
                            new Book
                            {
                                Id = Guid.NewGuid(),
                                Title = "A Game of Thrones",
                                Description = "The first book in the A Song of Ice and Fire series.",
                                Price = 24.99f,
                                EditionDate = new DateTime(1996, 8, 6),
                                ImageUrl = "https://example.com/got_1.jpg",
                                stockQuantity = 120,
                                DocUrl = "https://example.com/got_1.pdf",
                                AuthorId = context.authors.FirstOrDefault(a => a.Name == "George R.R. Martin")?.Id ?? Guid.NewGuid(),
                                CategoryId = context.categories.FirstOrDefault(c => c.Name == "Fantasy")?.Id ?? Guid.NewGuid()
                            },
                            new Book
                            {
                                Id = Guid.NewGuid(),
                                Title = "A Clash of Kings",
                                Description = "The second book in the A Song of Ice and Fire series.",
                                Price = 26.99f,
                                EditionDate = new DateTime(1998, 11, 16),
                                ImageUrl = "https://example.com/got_2.jpg",
                                stockQuantity = 90,
                                DocUrl = "https://example.com/got_2.pdf",
                                AuthorId = context.authors.FirstOrDefault(a => a.Name == "George R.R. Martin")?.Id ?? Guid.NewGuid(),
                                CategoryId = context.categories.FirstOrDefault(c => c.Name == "Fantasy")?.Id ?? Guid.NewGuid()
                            },
                            // Add more books by George R.R. Martin
                            // ... 

                            // Author: Stephen King
                            new Book
                            {
                                Id = Guid.NewGuid(),
                                Title = "It",
                                Description = "A horror novel about a menacing clown.",
                                Price = 18.99f,
                                EditionDate = new DateTime(1986, 9, 15),
                                ImageUrl = "https://example.com/it.jpg",
                                stockQuantity = 70,
                                DocUrl = "https://example.com/it.pdf",
                                AuthorId = context.authors.FirstOrDefault(a => a.Name == "Stephen King")?.Id ?? Guid.NewGuid(),
                                CategoryId = context.categories.FirstOrDefault(c => c.Name == "Horror")?.Id ?? Guid.NewGuid()
                            },
                            new Book
                            {
                                Id = Guid.NewGuid(),
                                Title = "The Shining",
                                Description = "A horror novel set in an isolated hotel.",
                                Price = 17.99f,
                                EditionDate = new DateTime(1977, 1, 28),
                                ImageUrl = "https://example.com/the_shining.jpg",
                                stockQuantity = 60,
                                DocUrl = "https://example.com/the_shining.pdf",
                                AuthorId = context.authors.FirstOrDefault(a => a.Name == "Stephen King")?.Id ?? Guid.NewGuid(),
                                CategoryId = context.categories.FirstOrDefault(c => c.Name == "Horror")?.Id ?? Guid.NewGuid()
                            }
                    };

                context.books.AddRange(books);
                context.SaveChanges();
            }
        }
    }

}

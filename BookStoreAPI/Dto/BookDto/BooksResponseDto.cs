﻿using Core.Entities;

namespace BookStoreAPI.Dto.BookDto
{
    public class BooksResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public int Rate { get; set; }
        public DateTime EditionDate { get; set; }
        public string ImageUrl { get; set; }
        public string AuthorName { get; set; }
    }
}

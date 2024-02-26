namespace Core.Entities
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public DateTime EditionDate { get; set; } 
        public string ImageUrl { get; set; }
        public int stockQuantity { get; set; }
        public string DocUrl { get; set; }
        public Guid AuthorId { get; set; }
        public Author Author { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<Rating> Ratings { get; } = new List<Rating>();
        public ICollection<Sell> Sells { get; } = new List<Sell>();
    }
}
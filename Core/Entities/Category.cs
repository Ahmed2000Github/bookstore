namespace Core.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Book> Books { get; } = new List<Book>();
    }
}

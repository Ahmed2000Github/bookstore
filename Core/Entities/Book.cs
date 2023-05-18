namespace Core.Entities
{
    public class Book
    {
        public Guid id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public float price { get; set; }
        public int rate { get; set; }
        public DateOnly editionDate { get; set; }
        public string imageUrl { get; set; }
        public string docUrl { get; set; }
    }
}
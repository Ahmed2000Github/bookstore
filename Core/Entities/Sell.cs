namespace Core.Entities
{
    public class Sell
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
    }
}
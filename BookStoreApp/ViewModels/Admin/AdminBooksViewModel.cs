namespace BookStoreApp.ViewModels.Admin
{
    public class AdminBooksViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public int Rate { get; set; }
        public DateTime EditionDate { get; set; }
        public string ImageUrl { get; set; }
        public string AuthorName { get; set; }
    }
}

namespace BookStoreAPI.Dto.BookDto
{
    public class BookSearchParamsDto
    {
        public string? Title { get; set; }
        public float? Price { get; set; }
        public string? EditionDate { get; set; }
        public string? AuthorName { get; set; }
    }
}

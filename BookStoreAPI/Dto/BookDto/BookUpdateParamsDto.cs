using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Dto.BookDto
{
    public class BookUpdateParamsDto
    {
        [Required]
        public Guid BookId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public float? Price { get; set; }
        public string? EditionDate { get; set; }
        public IFormFile? Blanket { get; set; }
        public IFormFile? Document { get; set; }
    }
}

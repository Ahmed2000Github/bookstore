using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Dto.BookDto
{
    public class BookParamsDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public string EditionDate { get; set; }
        [Required]
        public IFormFile Blanket { get; set; }
        [Required]
        public IFormFile Document { get; set; }
        [Required]
        public Guid  AuthorId { get; set; }
    }
}

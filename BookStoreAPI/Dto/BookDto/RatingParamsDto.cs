using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Dto.BookDto
{
    public class RatingParamsDto
    {
        [Required]
        public Guid BookId { get; set; }
        [Required]
        public int Rate { get; set; }
    }
}

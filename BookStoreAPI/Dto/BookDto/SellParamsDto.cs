using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Dto.BookDto
{
    public class SellParamsDto
    {
        [Required]
        public Guid BookId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}

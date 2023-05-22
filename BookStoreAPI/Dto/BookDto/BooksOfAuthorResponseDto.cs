using Core.Entities;

namespace BookStoreAPI.Dto.BookDto
{
    public class BooksOfAuthorResponseDto
    {
        public int Total { get; set; }
        public ICollection<BooksResponseDto> Books { get; set; }
    }
}

namespace BookStoreAPI.Dto.AuthorDto
{
    public class AuthorResponseDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}

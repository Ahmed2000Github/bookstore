using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Dto.UserDto
{
    public class VerifyEmailParamsDto
    {
        [Required]
        public string Key { get; set; }
        [Required]
        public string Email { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Dto.UserDto
{
    public class AdminLoginParamsDto
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

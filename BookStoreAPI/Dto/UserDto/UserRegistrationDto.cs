using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Dto.UserDto
{
    public class UserRegistrationParamsDto
    {
        [Required,MinLength(4)]
        public string UserName { get; set; }
        [Required, MinLength(4)]
        public string Password { get; set; }
        [Required,EmailAddress]
        public string Email { get; set; }
    }
}

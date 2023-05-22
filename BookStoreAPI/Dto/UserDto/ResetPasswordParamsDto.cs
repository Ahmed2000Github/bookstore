using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Dto.UserDto
{
    public class ResetPasswordParamsDto
    {
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string Email { get; set; }
    }
}

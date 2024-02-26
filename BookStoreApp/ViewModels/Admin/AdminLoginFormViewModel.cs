using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.ViewModels.Admin
{
    public class AdminLoginFormViewModel
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

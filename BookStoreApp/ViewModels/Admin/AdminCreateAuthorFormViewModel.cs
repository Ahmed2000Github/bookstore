using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.ViewModels.Admin
{
    public class AdminCreateAuthorFormViewModel
    {
        [Required]
        [MaxLength(20), MinLength(4)]
        public string? Name { get; set; }
        [Required]
        [MaxLength(300), MinLength(4)]
        public string? Description { get; set; }
    }
}

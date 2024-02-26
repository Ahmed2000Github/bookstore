using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.ViewModels.Admin
{
    public class AdminBookFormViewModel
    {
        [Required]
        [MinLength(4), MaxLength(20)]
        public string Title { get; set; }
        [Required]
        [MinLength(4), MaxLength(300)]
        public string Description { get; set; }
        [Required]
        [Range(0.0, 1000.0, ErrorMessage = "Value must be between 0 and 1000.")]
        public float Price { get; set; }
        [Required]
        public DateTime EditionDate { get; set; } = DateTime.Now;
        [Required]
        public AdminAuthorViewModel Author { get; set; }
    }
}

using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.ViewModels.Admin
{
    public class AdminCreateBookFormViewModel
    {
        [Required]
        [MinLength(4), MaxLength(20)]
        public string Title { get; set; }
        [Required]
        [MinLength(4), MaxLength(300)]
        public string Description { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public DateTime EditionDate { get; set; }
        [Required]
        public IBrowserFile Image { get; set; }
        [Required]
        public IBrowserFile Document { get; set; }
        [Required]
        public string AuthorId { get; set; }
    }
}

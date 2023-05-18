using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Dto
{
    public class AuthorParamsDto{
        [MinLength(4),MaxLength(20)]
        public string? Name { get; set; }
        [MinLength(4), MaxLength(300)]
        public string? Description { get; set; }
    }
}

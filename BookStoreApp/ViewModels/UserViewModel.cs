using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string ImageUrl { get; set; }
        public string Email { get; set; }
    }
}

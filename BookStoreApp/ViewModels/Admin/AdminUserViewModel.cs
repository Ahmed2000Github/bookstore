using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.ViewModels.Admin
{
    public class AdminUserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string ImageUrl { get; set; }
        public string Email { get; set; }
    }
}

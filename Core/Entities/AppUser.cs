

using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    public class AppUser : IdentityUser
    {
        public string AvatarUrl { get; set; }
        public ICollection<Rating> Ratings { get; } = new List<Rating>();
        public ICollection<Sell> Sells { get; } = new List<Sell>();
    }
}
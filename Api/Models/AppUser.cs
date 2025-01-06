using Microsoft.AspNetCore.Identity;

namespace Api.Models
{
    public class AppUser : IdentityUser
    {
        public List<Portafolio> Portafolios { get; set; } = new List<Portafolio>();
    }
}

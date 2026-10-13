using Microsoft.AspNetCore.Identity;

namespace MvcMovie.Models
{
    public class ApplicationUser:IdentityUser
    {
        //to add additional properties to the user, you can add them here. For example, you can add a Adress property:
        public string? Address { get; set; }
    }
}

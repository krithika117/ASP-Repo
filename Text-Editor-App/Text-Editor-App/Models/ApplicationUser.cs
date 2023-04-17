using Microsoft.AspNetCore.Identity;

namespace Text_Editor_App.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
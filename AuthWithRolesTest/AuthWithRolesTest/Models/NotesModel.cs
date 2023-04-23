using AuthWithRolesTest.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AuthWithRolesTest.Models
{
    public class NotesModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public string UserId { get; set; }

        public IdentityUser User;
    }
}

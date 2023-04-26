using Microsoft.AspNetCore.Identity;

namespace LiveTextEditorApp.Models
{
    public class DocumentModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; } 
        public string UserId { get; set; }

        public IdentityUser User;
    }
}

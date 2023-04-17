using Microsoft.EntityFrameworkCore;

namespace Text_Editor_App.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<DocumentArea> Documents { get; set; }
    }

}

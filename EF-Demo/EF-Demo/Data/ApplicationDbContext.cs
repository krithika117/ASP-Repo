using EF_Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace EF_Demo.Data
{
    public class ApplicationDbContext :  DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<StudentModel> Students { get; set; }

    }
}

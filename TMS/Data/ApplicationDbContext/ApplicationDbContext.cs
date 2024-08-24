using Microsoft.EntityFrameworkCore;

namespace TMS.Data.ApplicationDbContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<TaskDetails> TaskDetails { get; set; }
    }
}

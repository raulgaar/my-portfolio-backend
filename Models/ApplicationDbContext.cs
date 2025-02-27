using Microsoft.EntityFrameworkCore;

namespace my_portfolio_backend.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
            {
            }

            public DbSet<Project> Projects { get; set; }
            public DbSet<User> Users { get; set; }
    }
}
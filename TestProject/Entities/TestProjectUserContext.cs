using Microsoft.EntityFrameworkCore;

namespace TestProject
{
    public class TestProjectUserContext : DbContext
    {
        public TestProjectUserContext(DbContextOptions<TestProjectUserContext> options)
           : base(options)
        {
        }

        public DbSet<ApplicationUser> Users { get; set; }
    }
}
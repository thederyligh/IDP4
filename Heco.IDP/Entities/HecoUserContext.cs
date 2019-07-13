using Microsoft.EntityFrameworkCore;

namespace Heco.IDP.Entities
{
    public class HecoUserContext : DbContext
    {
        public HecoUserContext(DbContextOptions<HecoUserContext> options)
           : base(options)
        {
        }

        public DbSet<ApplicationUser> Users { get; set; }
    }
}
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Contexts
{
    public class TokenDbContext : DbContext
    {
        public TokenDbContext(DbContextOptions<TokenDbContext> dbContext)
            : base(dbContext)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User>? Users { get; set; }
    }
}

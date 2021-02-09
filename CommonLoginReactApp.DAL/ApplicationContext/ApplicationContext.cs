using CommonLoginReactApp.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CommonLoginReactApp.DAL.ApplicationContext
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> dbContext)
            : base(dbContext)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
using System;
using System.Linq;
using System.Threading.Tasks;
using CommonLoginReactApp.DAL.Entities;
using CommonLoginReactApp.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CommonLoginReactApp.DAL.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly ApplicationContext.ApplicationContext applicationContext;

        public AccountRepository(UserManager<ApplicationUser> userManager, ApplicationContext.ApplicationContext applicationDbContext)
        {
            this.userManager = userManager;
            this.applicationContext = applicationDbContext;
        }

        public async Task<ApplicationUser> GetByIdAsync(long id)
        {
            var user = await this.applicationContext.Users.FindAsync(id);
            return user;
        }

        public async Task<ApplicationUser> GetByEmailAsync(string email)
        {
            var user = await this.applicationContext.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
            return user;
        }

        public async Task<bool> CreateRoleAsync(string roleName)
        {
            var role = new ApplicationRole() { Name = roleName };
            await this.applicationContext.Roles.AddAsync(role);
            var result = await this.applicationContext.SaveChangesAsync();
            return result > 0;
        }
    }
}
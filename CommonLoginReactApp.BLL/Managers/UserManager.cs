using System.Threading.Tasks;
using CommonLoginReactApp.BLL.Interfaces;
using CommonLoginReactApp.DAL.Constants;
using CommonLoginReactApp.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace CommonLoginReactApp.BLL.Managers
{
    public class UserManager : IUserManager
    {
        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly UserManager<ApplicationUser> userManager;

        private readonly RoleManager<ApplicationRole> roleManager;

        public UserManager(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<bool> CreateAsync(ApplicationUser applicationUser, string password)
        {
            var result = await this.userManager.CreateAsync(applicationUser, password);
            return result.Succeeded;
        }

        public async Task<bool> LogInAsync(ApplicationUser applicationUser, string password)
        {
            var result = await this.signInManager.CheckPasswordSignInAsync(applicationUser, password, lockoutOnFailure: false);
            return result.Succeeded;
        }

        public Task LogOutAsync()
        {
            return Task.Run(() =>
            {
                return this.signInManager.SignOutAsync();
            });
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser applicationUser, string password)
        {
            var result = await this.signInManager.CheckPasswordSignInAsync(applicationUser, password, lockoutOnFailure: false);
            return result.Succeeded;
        }

        public async Task<bool> AddToRoleAsync(ApplicationUser applicationUser)
        {
            var role = await this.roleManager.FindByNameAsync(RoleConstants.User);
            if (role == null)
            {
                var userRole = new ApplicationRole()
                {
                    Name = RoleConstants.User
                };
                await this.roleManager.CreateAsync(userRole);
            }

            var result = await this.userManager.AddToRoleAsync(applicationUser, RoleConstants.User);
            return result.Succeeded;
        }
    }
}
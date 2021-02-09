using System.Threading.Tasks;
using CommonLoginReactApp.DAL.Entities;

namespace CommonLoginReactApp.BLL.Interfaces
{
    public interface IUserManager
    {
        Task<bool> CreateAsync(ApplicationUser applicationUser, string password);

        Task<bool> LogInAsync(ApplicationUser applicationUser, string password);

        Task LogOutAsync();

        Task<bool> CheckPasswordAsync(ApplicationUser applicationUser, string password);

        Task<bool> AddToRoleAsync(ApplicationUser applicationUser);
    }
}
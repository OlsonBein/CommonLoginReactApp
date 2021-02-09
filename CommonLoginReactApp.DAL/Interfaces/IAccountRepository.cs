using System.Threading.Tasks;
using CommonLoginReactApp.DAL.Entities;

namespace CommonLoginReactApp.DAL.Interfaces
{
    public interface IAccountRepository
    {
        Task<ApplicationUser> GetByIdAsync(long id);

        Task<ApplicationUser> GetByEmailAsync(string email);

        Task<bool> CreateRoleAsync(string roleName);
    }
}
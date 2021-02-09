using System.Threading.Tasks;
using CommonLoginReactApp.BLL.Models;

namespace CommonLoginReactApp.BLL.Interfaces
{
    public interface IAccountService
    {
        Task<UserModel> RegisterAsync(RegistrationModel model);

        Task<UserModel> LogInAsync(LoginModel model, string password);

        Task LogOutAsync();

        Task<UserModel> GetUserByIdAsync(long id);
    }
}
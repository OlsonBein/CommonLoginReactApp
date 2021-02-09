using System.Threading.Tasks;
using AutoMapper;
using CommonLoginReactApp.BLL.Interfaces;
using CommonLoginReactApp.BLL.Models;
using CommonLoginReactApp.DAL.Constants;
using CommonLoginReactApp.DAL.Entities;
using CommonLoginReactApp.DAL.Interfaces;

namespace CommonLoginReactApp.BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository accountRepository;

        private readonly IUserManager userManager;

        public AccountService(IAccountRepository accountRepository, IUserManager userManager)
        {
            this.accountRepository = accountRepository;
            this.userManager = userManager;
        }

        public async Task<UserModel> RegisterAsync(RegistrationModel model)
        {
            var response = new UserModel();
            if (model == null)
            {
                response.Errors.Add(ErrorConstants.ModelIsNull);
                return response;
            }

            var existingUser = await this.accountRepository.GetByEmailAsync(model.Email);
            if (existingUser != null)
            {
                response.Errors.Add(ErrorConstants.UserWithSameEmailExists);
                return response;
            }

            var config = new MapperConfiguration(cfg => cfg.CreateMap<RegistrationModel, ApplicationUser>()
                .ForMember("UserName", opt => opt.MapFrom(x => x.Login)));
            var mapper = new AutoMapper.Mapper(config);
            var applicationUser = mapper.Map<RegistrationModel, ApplicationUser>(model);
            var result = await this.userManager.CreateAsync(applicationUser, model.Password);
            if (!result)
            {
                response.Errors.Add(ErrorConstants.IncorrectInput);
                return response;
            }

            await this.userManager.AddToRoleAsync(applicationUser);
            return response;
        }

        public async Task<UserModel> LogInAsync(LoginModel model, string password)
        {
            var response = new UserModel();
            if (model == null)
            {
                response.Errors.Add(ErrorConstants.ModelIsNull);
                return response;
            }

            var existingUser = await this.accountRepository.GetByEmailAsync(model.Email);
            if (existingUser == null)
            {
                response.Errors.Add(ErrorConstants.IncorrectInput);
                return response;
            }

            var result = await this.userManager.LogInAsync(existingUser, password);
            if (!result)
            {
                response.Errors.Add(ErrorConstants.IncorrectPassword);
                return response;
            }

            var config = new MapperConfiguration(cfg => cfg.CreateMap<ApplicationUser, UserModel>()
                .ForMember("Login", opt => opt.MapFrom(x => x.UserName)));
            var mapper = new AutoMapper.Mapper(config);
            response = mapper.Map<ApplicationUser, UserModel>(existingUser);
            return response;
        }

        public Task LogOutAsync()
        {
            return Task.Run(() =>
            {
                return this.userManager.LogOutAsync();
            });
        }

        public async Task<UserModel> GetUserByIdAsync(long id)
        {
            var response = new UserModel();
            var existingUser = await this.accountRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                response.Errors.Add(ErrorConstants.ImpossibleToFindUser);
                return response;
            }

            var config = new MapperConfiguration(cfg => cfg.CreateMap<ApplicationUser, UserModel>()
                .ForMember("Login", opt => opt.MapFrom(x => x.UserName)));
            var mapper = new AutoMapper.Mapper(config);
            response = mapper.Map<ApplicationUser, UserModel>(existingUser);
            return response;
        }
    }
}
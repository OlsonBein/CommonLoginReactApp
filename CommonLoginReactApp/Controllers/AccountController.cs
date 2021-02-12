using System.Threading.Tasks;
using CommonLoginReactApp.BLL.Interfaces;
using CommonLoginReactApp.BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommonLoginReactApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService service)
        {
            this.accountService = service;
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Registration(RegistrationModel model)
        {
            var result = await this.accountService.RegisterAsync(model);
            return this.Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var applicationUser = await this.accountService.LogInAsync(model, model.Password);
            return this.Ok(applicationUser);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await this.accountService.LogOutAsync();
            return this.Ok();
        }
    }
}
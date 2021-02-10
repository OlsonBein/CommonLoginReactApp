using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CommonLoginReactApp.BLL.Interfaces;
using CommonLoginReactApp.BLL.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace CommonLoginReactApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;
        private readonly IHttpClientFactory httpClientFactory;

        public AccountController(IAccountService service, IHttpClientFactory httpClientFactory)
        {
            this.accountService = service;
            this.httpClientFactory = httpClientFactory;
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
            if (applicationUser.Errors.Any())
            {
                return this.Ok(applicationUser);
            }

            var authClient = this.httpClientFactory.CreateClient();
            var discoveryDocument = await authClient.GetDiscoveryDocumentAsync("https://localhost:44331/");
            var tokenResponse = await authClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = "client_id",
                ClientSecret = "secret",
                Scope = "apiscope"
            });
            authClient.SetBearerToken(tokenResponse.AccessToken);
            this.HttpContext.Response.Cookies.Append("accessToken", tokenResponse.AccessToken);
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
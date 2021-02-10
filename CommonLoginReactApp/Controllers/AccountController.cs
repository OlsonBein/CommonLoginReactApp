using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CommonLoginReactApp.BLL.Interfaces;
using CommonLoginReactApp.BLL.Models;
using CommonLoginReactApp.DAL.Constants;
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
            if (applicationUser.Errors.Any())
            {
                return this.Ok(applicationUser);
            }

            var id = this.CreateClaims(applicationUser.Email);
            await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
            return this.Ok(applicationUser);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return this.Ok();
        }

        private ClaimsIdentity CreateClaims(string email)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, RoleConstants.User)
            };
            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            return id;
        }
    }
}
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommonLoginReactApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet("secret")]
        public async Task<IActionResult> Secret()
        {
            var authClient = new HttpClient();
            var discoveryDocument = await authClient.GetDiscoveryDocumentAsync("https://localhost:10001/");
            var tokenResponse = await authClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = "client_id",
                ClientSecret = "client_secret",
                Scope = "HomeAPI"
            });
            if (tokenResponse.IsError)
            {
                return this.Ok(tokenResponse.Error);
            }

            var homeClient = new HttpClient();
            homeClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await homeClient.GetAsync("https://localhost:44331/api/home/closedEndpoint");
            if (!response.IsSuccessStatusCode)
            {
                return this.Ok(response.StatusCode);
            }

            var message = await response.Content.ReadAsStringAsync();
            return this.Ok(message);
        }

        [HttpGet("closedEndpoint")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public string ClosedEndpoint()
        {
            return "Congratulations, you got here!";
        }
    }
}
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetSecret()
        {
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

            var response = await authClient.GetAsync("https://localhost:44331/api/home/closedEndpoint");
            var message = await response.Content.ReadAsStringAsync();
            return this.Ok(message);
        }

        [HttpGet("closedEndpoint")]
        public string ClosedEndpoint()
        {
            return "Congratulations, you got here!";
        }
    }
}
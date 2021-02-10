using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommonLoginReactApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        [Authorize]
        [HttpGet("closedEndpoint")]
        public IActionResult Index()
        {
            return this.Ok("Congratulations, you got here!");
        }
    }
}
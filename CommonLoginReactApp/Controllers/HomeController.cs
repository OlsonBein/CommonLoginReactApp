using Microsoft.AspNetCore.Mvc;

namespace CommonLoginReactApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        [HttpGet("closedEndpoint")]
        public IActionResult Index()
        {
            return this.Ok("Congratulations, you got here!");
        }
    }
}
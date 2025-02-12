using Microsoft.AspNetCore.Mvc;

namespace my_portfolio_backend.Controllers
{
    public class HomeController : Controller
    {
        [Route("Home/Error")]
        public IActionResult Error()
        {
            return View();  // Esto devolverá la vista Error.cshtml
        }
    }
}

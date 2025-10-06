using Microsoft.AspNetCore.Mvc;

namespace WriteAndReadWebApplicationMVC.Controllers
{
    public class ReadController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace WriteAndReadWebApplicationMVC.Controllers
{
    public class WriteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

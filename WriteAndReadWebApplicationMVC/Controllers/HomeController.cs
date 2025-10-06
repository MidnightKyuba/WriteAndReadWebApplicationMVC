using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using WriteAndReadWebApplicationMVC.Models;
using WriteAndReadWebApplicationMVC.Services;
using WriteAndReadWebApplicationMVC.Services.Interfaces;

namespace WriteAndReadWebApplicationMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, IUserService userService)
        {
            _logger = logger;
            this._userService = userService;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("_Logged") == null)
            {
                HttpContext.Session.SetString("_Logged", "False");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            if (HttpContext.Session.GetString("_Logged") == null)
            {
                HttpContext.Session.SetString("_Logged", "False");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Login(string? message = null)
        {
            if (HttpContext.Session.GetString("_Logged") == null)
            {
                HttpContext.Session.SetString("_Logged", "False");
            }
            if (HttpContext.Session.GetString("_Logged") == "False")
            {
                if (message != null)
                {
                    ViewData["Message"] = message;
                }
                return View();
            }
            else
            {
                return this.Index();
            }
        }

        [HttpGet]
        public IActionResult Register(string? message = null)
        {
            if (HttpContext.Session.GetString("_Logged") == null)
            {
                HttpContext.Session.SetString("_Logged", "False");
            }
            if (HttpContext.Session.GetString("_Logged") == "False")
            {
                if (message != null)
                {
                    ViewData["Message"] = message;
                }
                return View();
            }
            else
            {
                return this.Index();
            }
        }

        [HttpPost]
        public IActionResult Login() 
        {
            try
            {
                User user = _userService.GetUser(HttpContext.Request.Form["login"]);
                if (user != null)
                {
                    if (user.password == Tools.GetHash(HttpContext.Request.Form["password"]))
                    {
                        foreach(Block block in user.blocks) 
                        {
                            if(block.blockEnd <= DateTime.Now) 
                            {
                                ViewData["Message"] = "Konto zablokowane";
                                return View();
                            }
                        }
                        HttpContext.Session.SetString("_CurrentUser", JsonSerializer.Serialize(user));
                        HttpContext.Session.SetString("_Logged", "True");
                        return this.Index();
                    }
                    else 
                    {
                        ViewData["Message"] = "Login lub has�o s� nieprawid�owe";
                        return View();
                    }
                }
                else
                {
                    ViewData["Message"] = "Login lub has�o s� nieprawid�owe";
                    return View();
                }
            }
            catch (Exception ex) 
            {
                ViewData["Message"] = ex.Message;
                return View();
            }
        }

        [HttpPost]
        public IActionResult Register() 
        {
            string login = HttpContext.Request.Form["login"];
            string email = HttpContext.Request.Form["email"];
            string password = HttpContext.Request.Form["password"];
            string password2 = HttpContext.Request.Form["password2"];
            string country = HttpContext.Request.Form["country"];
            string street = HttpContext.Request.Form["street"];
            string city = HttpContext.Request.Form["city"];
            try
            {
                if (!this._userService.CheckIfLoginExist(login)) 
                {
                    if (!this._userService.CheckIfEmailExist(email)) 
                    {
                        if(password == password2) 
                        {
                            if (password.Length > 7) 
                            {
                                if (int.TryParse(HttpContext.Request.Form["postcode"], out int postcode)) 
                                {
                                    if (10000 <= postcode && postcode <= 99999)
                                    {
                                        if (DateTime.TryParse(HttpContext.Request.Form["birthday"], out DateTime birthday))
                                        {
                                            User user = new User(login, email, Tools.GetHash(password), country, street, city, postcode.ToString(), birthday);
                                            int newId = this._userService.Register(user);
                                            if(newId != 0) 
                                            {
                                                User currentUser = this._userService.GetUser(newId);
                                                HttpContext.Session.SetString("_CurrentUser", JsonSerializer.Serialize(currentUser));
                                                HttpContext.Session.SetString("_Logged", "True");
                                                return this.Index();
                                            }
                                            else 
                                            {
                                                ViewData["Message"] = "Nie uda�o si� zarejestrowa� u�ytkownika";
                                                return View();
                                            }
                                        }
                                        else
                                        {
                                            ViewData["Message"] = "Podana warto�� w dacie urodzeni nie jest datom";
                                            return View();
                                        }
                                    }
                                    else
                                    {
                                        ViewData["Message"] = "Kod pocztowy nie sk�ada si� z 5 cyfr";
                                        return View();
                                    }
                                }
                                else 
                                {
                                    ViewData["Message"] = "Kod pocztowy nie jest liczbom";
                                    return View();
                                }
                            }
                            else 
                            {
                                ViewData["Message"] = "Has�o musi mi�� przynajmniej osiem znak�w";
                                return View();
                            }
                        }
                        else 
                        {
                            ViewData["Message"] = "Has�a nie s� takie same";
                            return View();
                        }
                    }
                    else
                    {
                        ViewData["Message"] = "Email jest ju� zaj�ty";
                        return View();
                    }
                }
                else 
                {
                    ViewData["Message"] = "Login jest ju� zaj�ty";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message;
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
    }
}

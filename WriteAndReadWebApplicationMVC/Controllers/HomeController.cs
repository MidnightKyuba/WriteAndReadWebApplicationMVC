using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
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
            return View("Index");
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
                return Redirect("/home/index");
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
                return Redirect("/home/index");
            }
        }

        [HttpPost]
        public IActionResult Login() 
        {
            try
            {
                string login = HttpContext.Request.Form["login"];
                if (login == null || login.Length == 0) 
                {
                    ViewData["Message"] = "Pole login nie mo¿e byæ pustê";
                    return View();
                }
                string password = HttpContext.Request.Form["Password"];
                if (password == null || password.Length == 0)
                {
                    ViewData["Message"] = "Pole has³o nie mo¿e byæ pustê";
                    return View();
                }
                User user = _userService.GetUser(login);
                if (user != null)
                {
                    if (user.password == Tools.GetHash(password))
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
                        return Redirect("/home/index");
                    }
                    else 
                    {
                        ViewData["Message"] = "Login lub has³o s¹ nieprawid³owe";
                        return View();
                    }
                }
                else
                {
                    ViewData["Message"] = "Login lub has³o s¹ nieprawid³owe";
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
                if (login == null || login.Length == 0)
                {
                    ViewData["Message"] = "Pole login nie mo¿e byæ pustê";
                    return View();
                }
                if (email == null || email.Length == 0)
                {
                    ViewData["Message"] = "Pole email nie mo¿e byæ pustê";
                    return View();
                }
                if (password == null || password.Length == 0)
                {
                    ViewData["Message"] = "Pole has³o nie mo¿e byæ pustê";
                    return View();
                }
                if (password2 == null || password2.Length == 0)
                {
                    ViewData["Message"] = "Pole powtórz has³o nie mo¿e byæ pustê";
                    return View();
                }
                if (country == null || country.Length == 0)
                {
                    ViewData["Message"] = "Pole kraj nie mo¿e byæ pustê";
                    return View();
                }
                if (street == null || street.Length == 0)
                {
                    ViewData["Message"] = "Pole ulica nie mo¿e byæ pustê";
                    return View();
                }
                if (city == null || city.Length == 0)
                {
                    ViewData["Message"] = "Pole miasto nie mo¿e byæ pustê";
                    return View();
                }
                if (!this._userService.CheckIfLoginExist(login)) 
                {
                    if (!this._userService.CheckIfEmailExist(email)) 
                    {
                        if(password == password2) 
                        {
                            Regex regex = new Regex("^[a-zA-Z0-9!@#$%^&*]{8,}$");
                            if (regex.IsMatch(password)) 
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
                                                return Redirect("/home/index");
                                            }
                                            else 
                                            {
                                                ViewData["Message"] = "Nie uda³o siê zarejestrowaæ u¿ytkownika";
                                                return View();
                                            }
                                        }
                                        else
                                        {
                                            ViewData["Message"] = "Podana wartoœæ w dacie urodzeni nie jest datom";
                                            return View();
                                        }
                                    }
                                    else
                                    {
                                        ViewData["Message"] = "Kod pocztowy nie sk³ada siê z 5 cyfr";
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
                                ViewData["Message"] = "Has³o musi miêæ przynajmniej osiem znaków (liter, cyfr lub znaków specjalnych !,@,#,$,%,^,&,*)";
                                return View();
                            }
                        }
                        else 
                        {
                            ViewData["Message"] = "Has³a nie s¹ takie same";
                            return View();
                        }
                    }
                    else
                    {
                        ViewData["Message"] = "Email jest ju¿ zajêty";
                        return View();
                    }
                }
                else 
                {
                    ViewData["Message"] = "Login jest ju¿ zajêty";
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

        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout() 
        {
            HttpContext.Session.Clear();
            return Redirect("/home/Index");
        }
        
    }
}

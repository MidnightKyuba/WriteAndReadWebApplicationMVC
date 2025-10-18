using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using WriteAndReadWebApplicationMVC.Models;
using WriteAndReadWebApplicationMVC.Services.Interfaces;

namespace WriteAndReadWebApplicationMVC.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;

        public ProfileController(IUserService userService) 
        {
            this._userService = userService;
        }

        [HttpGet]
        public IActionResult Index(string? message = null)
        {
            if (HttpContext.Session.GetString("_Logged") == null)
            {
                HttpContext.Session.SetString("_Logged", "False");
            }
            if (HttpContext.Session.GetString("_Logged") == "False") 
            {
                return Redirect("/home/index");
            }
            else 
            {
                if(message != null) 
                {
                    ViewData["Message"] = message;
                }
                return View();
            }
        }
        
        [HttpGet]
        public IActionResult Edit(string? message = null)
        {
            if (HttpContext.Session.GetString("_Logged") == null)
            {
                HttpContext.Session.SetString("_Logged", "False");
            }
            if (HttpContext.Session.GetString("_Logged") == "False")
            {
                return Redirect("/home/index");
            }
            else
            {
                if (message != null)
                {
                    ViewData["Message"] = message;
                }
                return View();
            }
        }

        [HttpPost]
        public IActionResult Edit() 
        {
            if (HttpContext.Session.GetString("_Logged") == null)
            {
                HttpContext.Session.SetString("_Logged", "False");
            }
            if (HttpContext.Session.GetString("_Logged") == "False")
            {
                return Redirect("/home/index");
            }
            else
            {
                User user = JsonSerializer.Deserialize<User>(HttpContext.Session.GetString("_CurrentUser"));
                string newLogin = HttpContext.Request.Form["login"];
                string newEmail = HttpContext.Request.Form["email"];
                string newCountry = HttpContext.Request.Form["country"];
                string newStreet = HttpContext.Request.Form["street"];
                string newCity = HttpContext.Request.Form["city"];
                if(newLogin == null || newLogin.Length == 0) 
                {
                    ViewData["Message"] = "Pole login nie może być pustę";
                    return View();
                }
                if (newEmail == null || newEmail.Length == 0)
                {
                    ViewData["Message"] = "Pole email nie może być pustę";
                    return View();
                }
                if (newCountry == null || newCountry.Length == 0)
                {
                    ViewData["Message"] = "Pole kraj nie może być pustę";
                    return View();
                }
                if (newStreet == null || newStreet.Length == 0)
                {
                    ViewData["Message"] = "Pole ulica nie może być pustę";
                    return View();
                }
                if (newCity == null || newCity.Length == 0)
                {
                    ViewData["Message"] = "Pole miasto nie może być pustę";
                    return View();
                }
                if (user.login != newLogin) 
                {
                    if (!this._userService.CheckIfLoginExist(newLogin))
                    {
                        user.login = newLogin;
                    }
                    else
                    {
                        ViewData["Message"] = "Login zajęty przez innego użytkownika";
                        return View();
                    }
                }
                if (user.email != newEmail) 
                {
                    if (!this._userService.CheckIfEmailExist(newEmail))
                    {
                        user.email = newEmail;
                    }
                    else
                    {
                        ViewData["Message"] = "Adres email zajęty przez innego użytkownika";
                        return View();
                    }
                }
                user.country = newCountry;
                user.city = newCity;
                user.street = newStreet;
                if (int.TryParse(HttpContext.Request.Form["postcode"], out int newPostcode))
                {
                    if (newPostcode < 100000 && newPostcode >= 10000)
                    {
                        user.postcode = newPostcode.ToString();
                    }
                    else
                    {
                        ViewData["Message"] = "Kod pocztowy powinien być liczbom pomiędzy 10000 lub 99999";
                        return View();
                    }
                }
                else
                {
                    ViewData["Message"] = "Kod pocztowy powinien być liczbom";
                    return View();
                }
                if (DateTime.TryParse(HttpContext.Request.Form["birthday"], out DateTime newBirthday))
                {
                    user.birthDate = newBirthday;                
                }
                else
                {
                    ViewData["Message"] = "Data urodzenia musi być datą";
                    return View();
                }
                this._userService.ChangeUserData(user);
                user = this._userService.GetUser(user.id);
                if (user.login == newLogin && user.email == newEmail 
                    && user.country == newCountry && user.city == newCity 
                    && user.street == newStreet && user.postcode == newPostcode.ToString() 
                    && user.birthDate == newBirthday)
                {
                    HttpContext.Session.SetString("_CurrentUser", JsonSerializer.Serialize(user));
                    return Redirect("/profile/index?message=Zmieniono dane");
                }
                else
                {
                    HttpContext.Session.SetString("_CurrentUser", JsonSerializer.Serialize(user));
                    ViewData["Message"] = "Nie udało się zmienić danych";
                    return View();
                }  
            }
        }

        [HttpGet]
        public IActionResult EditPassword(string? message = null)
        {
            if (HttpContext.Session.GetString("_Logged") == null)
            {
                HttpContext.Session.SetString("_Logged", "False");
            }
            if (HttpContext.Session.GetString("_Logged") == "False")
            {
                return Redirect("/home/index");
            }
            else
            {
                if (message != null)
                {
                    ViewData["Message"] = message;
                }
                return View();
            }
        }

        [HttpPost]
        public IActionResult EditPassword()
        {
            if (HttpContext.Session.GetString("_Logged") == null)
            {
                HttpContext.Session.SetString("_Logged", "False");
            }
            if (HttpContext.Session.GetString("_Logged") == "False")
            {
                return Redirect("/home/index");
            }
            else
            {
                string newPassword = HttpContext.Request.Form["newPassword"];
                string newPassword2 = HttpContext.Request.Form["newPassword2"];
                string oldPassword = HttpContext.Request.Form["oldPassword"];
                if (newPassword == newPassword2)
                {
                    Regex regex = new Regex("^[a-zA-Z0-9!@#$%^&*]{8,}$");
                    if (regex.IsMatch(newPassword)) 
                    {
                        User user = JsonSerializer.Deserialize<User>(HttpContext.Session.GetString("_CurrentUser"));
                        if (user.password == Tools.GetHash(oldPassword)) 
                        {
                            user.password = Tools.GetHash(newPassword);
                            this._userService.ChangeUserData(user);
                            user = this._userService.GetUser(user.id);
                            if (user.password == newPassword)
                            {
                                HttpContext.Session.SetString("_CurrentUser", JsonSerializer.Serialize(user));
                                ViewData["Message"] = "Hasło zmienione";
                                return Redirect("/profile/index?message=Hasło zmienione");
                            }
                            else
                            {
                                HttpContext.Session.SetString("_CurrentUser", JsonSerializer.Serialize(user));
                                ViewData["Message"] = "Nie udało się zmienić hasła";
                                return View();
                            }
                        }
                        else 
                        {
                            ViewData["Message"] = "Stare hasło jest nieprawidłowe";
                            return View();
                        }
                    }
                    else 
                    {
                        ViewData["Message"] = "Nowe hasło musi mięć przynajmniej osiem znaków (liter, cyfr lub znaków specjalnych !,@,#,$,%,^,&,*)";
                        return View();
                    }
                }
                else
                {
                    ViewData["Message"] = "Nowe hasło nie jest tako samo w obydwu polach";
                    return View();
                }
            }
        }
    }
}

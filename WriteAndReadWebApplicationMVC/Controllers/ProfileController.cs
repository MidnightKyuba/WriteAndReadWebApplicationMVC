using Microsoft.AspNetCore.Mvc;
using WriteAndReadWebApplicationMVC.Models;
using WriteAndReadWebApplicationMVC.Services;
using WriteAndReadWebApplicationMVC.Services.Interfaces;
using System.Text.Json;

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
        public IActionResult Index()
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
                return View();
            }
        }

        [HttpPost]
        public IActionResult ChangeLogin() 
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
                string newlogin = HttpContext.Request.Form["login"];
                if (!this._userService.CheckIfLoginExist(newlogin)) 
                {
                    User user = JsonSerializer.Deserialize<User>(HttpContext.Session.GetString("_CurrentUser"));
                    user.login = newlogin;
                    this._userService.ChangeUserData(user);
                    user = this._userService.GetUser(user.id);
                    if(user.login == newlogin) 
                    {
                        HttpContext.Session.SetString("_CurrentUser",JsonSerializer.Serialize(user));
                        ViewData["Message"] = "Login zmieniony";
                        return View("Index");
                    }
                    else 
                    {
                        HttpContext.Session.SetString("_CurrentUser", JsonSerializer.Serialize(user));
                        ViewData["Message"] = "Nie udało się zmienić loginu";
                        return View("Index");
                    }
                }
                else 
                {
                    ViewData["Message"] = "Login zajęty przez innego użytkownika";
                    return View("Index");
                }
            }
        }

        [HttpPost]
        public IActionResult ChangeEmail()
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
                string newemail = HttpContext.Request.Form["email"];
                if (!this._userService.CheckIfEmailExist(newemail))
                {
                    User user = JsonSerializer.Deserialize<User>(HttpContext.Session.GetString("_CurrentUser"));
                    user.email = newemail;
                    this._userService.ChangeUserData(user);
                    user = this._userService.GetUser(user.id);
                    if (user.email == newemail)
                    {
                        HttpContext.Session.SetString("_CurrentUser", JsonSerializer.Serialize(user));
                        ViewData["Message"] = "Adres email zmieniony";
                        return View("Index");
                    }
                    else
                    {
                        HttpContext.Session.SetString("_CurrentUser", JsonSerializer.Serialize(user));
                        ViewData["Message"] = "Nie udało się zmienić adresu email";
                        return View("Index");
                    }
                }
                else
                {
                    ViewData["Message"] = "Adres email zajęty przez innego użytkownika";
                    return View("Index");
                }
            }
        }

        [HttpPost]
        public IActionResult ChangePassword()
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
                    if(newPassword.Length > 7) 
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
                                return View("Index");
                            }
                            else
                            {
                                HttpContext.Session.SetString("_CurrentUser", JsonSerializer.Serialize(user));
                                ViewData["Message"] = "Nie udało się zmienić hasła";
                                return View("Index");
                            }
                        }
                        else 
                        {
                            ViewData["Message"] = "Stare hasło jest nieprawidłowe";
                            return View("Index");
                        }
                    }
                    else 
                    {
                        ViewData["Message"] = "Nowe hasło musi być dłuższe niż siedem znaków";
                        return View("Index");
                    }
                }
                else
                {
                    ViewData["Message"] = "Nowe hasło nie jest tako samo w obydwu polach";
                    return View("Index");
                }
            }
        }

        [HttpPost]
        public IActionResult ChangeAdrress()
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
                string newCountry = HttpContext.Request.Form["country"];
                string newCity = HttpContext.Request.Form["city"];
                string newStreet = HttpContext.Request.Form["street"];
                if (int.TryParse(HttpContext.Request.Form["postcode"],out int newPostcode))
                {
                    if( newPostcode < 100000 && newPostcode >= 10000) 
                    {
                        User user = JsonSerializer.Deserialize<User>(HttpContext.Session.GetString("_CurrentUser"));
                        user.country = newCountry;
                        user.city = newCity;
                        user.street = newStreet;
                        user.postcode = newPostcode.ToString();
                        this._userService.ChangeUserData(user);
                        user = this._userService.GetUser(user.id);
                        if (user.country == newCountry && user.city == newCity && user.street == newStreet && user.postcode == newPostcode.ToString())
                        {
                            HttpContext.Session.SetString("_CurrentUser", JsonSerializer.Serialize(user));
                            ViewData["Message"] = "Adres zmieniony";
                            return View("Index");
                        }
                        else
                        {
                            HttpContext.Session.SetString("_CurrentUser", JsonSerializer.Serialize(user));
                            ViewData["Message"] = "Nie udało się zmienić adresu";
                            return View("Index");
                        }
                    }
                    else 
                    {
                        ViewData["Message"] = "Kod pocztowy powinien być liczbom pomiędzy 10000 lub 99999";
                        return View("Index");
                    }
                }
                else
                {
                    ViewData["Message"] = "Kod pocztowy powinien być 5 cyfrową liczbom";
                    return View("Index");
                }
            }
        }

        [HttpPost]
        public IActionResult ChangeBirthday()
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
                if (DateTime.TryParse(HttpContext.Request.Form["birthday"],out DateTime newBirthday))
                {
                    User user = JsonSerializer.Deserialize<User>(HttpContext.Session.GetString("_CurrentUser"));
                    user.birthDate = newBirthday;
                    this._userService.ChangeUserData(user);
                    user = this._userService.GetUser(user.id);
                    if (user.birthDate == newBirthday)
                    {
                        HttpContext.Session.SetString("_CurrentUser", JsonSerializer.Serialize(user));
                        ViewData["Message"] = "Data urodzenia zmieniona";
                        return View("Index");
                    }
                    else
                    {
                        HttpContext.Session.SetString("_CurrentUser", JsonSerializer.Serialize(user));
                        ViewData["Message"] = "Nie udało się zmienić daty urodzenia";
                        return View("Index");
                    }
                }
                else
                {
                    ViewData["Message"] = "Data urodzenia musi być datą";
                    return View("Index");
                }
            }
        }
    }
}

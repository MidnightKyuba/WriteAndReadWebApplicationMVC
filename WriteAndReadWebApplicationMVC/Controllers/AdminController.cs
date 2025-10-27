using Microsoft.AspNetCore.Mvc;
using WriteAndReadWebApplicationMVC.Models;
using WriteAndReadWebApplicationMVC.Services.Interfaces;
using System.Text.Json;
using System.Diagnostics.Eventing.Reader;

namespace WriteAndReadWebApplicationMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IUserService _userService;

        public AdminController(IAdminService adminService, IUserService userService) 
        {
            this._adminService = adminService;
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
            User user = JsonSerializer.Deserialize<User>(HttpContext.Session.GetString("_CurrentUser"));
            if(user.admin == true)
            {
                List<User> users = this._adminService.GetAllUsers();
                if (message != null) 
                {
                    ViewData["Message"] = message;
                }
                ViewData["usersList"] = JsonSerializer.Serialize(users);
                return View("Index");
            }
            else 
            {
                return Redirect("/home/index");
            }
        }

        [HttpPost]
        public IActionResult ChangeUserLevel(string userIdString) 
        {
            if (HttpContext.Session.GetString("_Logged") == null)
            {
                HttpContext.Session.SetString("_Logged", "False");
            }
            if (HttpContext.Session.GetString("_Logged") == "False")
            {
                return Redirect("/home/index");
            }
            User user = JsonSerializer.Deserialize<User>(HttpContext.Session.GetString("_CurrentUser"));
            if (user.admin == true)
            {
                if (int.TryParse(userIdString, out int userId))
                {
                    try 
                    {
                        this._adminService.ChangeUserLevel(userId);
                        return this.Index("Zmieniono uprawnienia użytkownika");
                    }
                    catch (Exception ex) 
                    {
                        return this.Index(ex.Message);
                    }
                }
                else 
                {
                    return this.Index("Id użytkownika nie jest liczbom");
                }
            }
            else
            {
                return Redirect("/home/index");
            }
        }
        [HttpPost]
        public IActionResult DeleteUser(string userIdString)
        {
            if (HttpContext.Session.GetString("_Logged") == null)
            {
                HttpContext.Session.SetString("_Logged", "False");
            }
            if (HttpContext.Session.GetString("_Logged") == "False")
            {
                return Redirect("/home/index");
            }
            User user = JsonSerializer.Deserialize<User>(HttpContext.Session.GetString("_CurrentUser"));
            if (user.admin == true)
            {
                if (int.TryParse(userIdString, out int userId))
                {
                    try
                    {
                        this._adminService.DeleteUser(userId);
                        return this.Index("Usunięto użytkownika");
                    }
                    catch (Exception ex)
                    {
                        return this.Index(ex.Message);
                    }
                }
                else
                {
                    return this.Index("Id użytkownika nie jest liczbom");
                }
            }
            else
            {
                return Redirect("/home/index");
            }
        }
        [HttpPost]
        public IActionResult CreateBlock(string userIdString, string endTimeString)
        {
            if (HttpContext.Session.GetString("_Logged") == null)
            {
                HttpContext.Session.SetString("_Logged", "False");
            }
            if (HttpContext.Session.GetString("_Logged") == "False")
            {
                return Redirect("/home/index");
            }
            User user = JsonSerializer.Deserialize<User>(HttpContext.Session.GetString("_CurrentUser"));
            if (user.admin == true)
            {
                if (int.TryParse(userIdString, out int userId))
                {
                    if (DateTime.TryParse(endTimeString, out DateTime endTime))
                    {
                        try
                        {
                            User userToBlock = this._userService.GetUser(userId);
                            Block block = new Block(userToBlock, DateTime.Now, endTime);
                            this._adminService.CreateBlock(block);
                            return this.Index("Zablokowano użytkownika");
                        }
                        catch (Exception ex)
                        {
                            return this.Index(ex.Message);
                        }
                    }
                    else
                    {
                        return this.Index("Zakończenie blokady musi być datą");
                    }
                }
                else
                {
                    return this.Index("Id użytkownika nie jest liczbom");
                }
            }
            else
            {
                return Redirect("/home/index");
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using WriteAndReadWebApplicationMVC.Models;
using WriteAndReadWebApplicationMVC.Services.Interfaces;

namespace WriteAndReadWebApplicationMVC.Controllers
{
    public class WriteController : Controller
    {
        private readonly IBookService _bookService;

        public WriteController(IBookService bookService)
        {
            _bookService = bookService;
        }
        public IActionResult Index(int bookStartId = 1)
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
                User currentUser = JsonSerializer.Deserialize<User>(HttpContext.Session.GetString("_CurrentUser"));
                List<Book> booksToView = this._bookService.GetAllMyBooks(currentUser.id, bookStartId);
                ViewData["BooksList"] = JsonSerializer.Serialize(booksToView);
                ViewData["bookStartId"] = bookStartId;
                return View("Index");
            }
        }

        [HttpGet]
        public IActionResult CreateBook(string? message = null) 
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
        public IActionResult CreateBook()
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
                string title = HttpContext.Request.Form["title"];
                string description = HttpContext.Request.Form["description"];
                if(title.Length > 0) 
                {
                    if(description.Length > 0) 
                    {
                        Book book = new Book(user.id,title,description, DateTime.Now, DateTime.Now);
                        int result = this._bookService.CreateBook(book);
                        if(result > 0) 
                        {
                            return Redirect("/write/index");
                        }
                        else 
                        {
                            ViewData["Message"] = "Nie udało się zapisać książki w bazie danych";
                            return View();
                        }
                    }
                    else 
                    {
                        ViewData["Message"] = "Opis nie może być pusty";
                        return View();
                    }
                }
                else 
                {
                    ViewData["Message"] = "Tytuł nie może być pusty";
                    return View();
                }
            }
        }

        [HttpGet]
        public IActionResult EditBook(int bookId, string? message = null)
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
                Book book = this._bookService.GetBook(bookId);
                ViewData["Book"] = JsonSerializer.Serialize(book);
                if (message != null)
                {
                    ViewData["Message"] = message;
                }
                return View("CreateBook");
            }
        }

        [HttpPost]
        public IActionResult EditBook(int bookId)
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
                string title = HttpContext.Request.Form["title"];
                string description = HttpContext.Request.Form["description"];
                Book book = this._bookService.GetBook(bookId);
                if (book.authorId == user.id)
                {
                    if (title.Length > 0)
                    {
                        if (description.Length > 0)
                        {
                            book.title = title;
                            book.description = description;
                            book.updateDate = DateTime.Now;
                            this._bookService.UpdateBook(book);
                            return Redirect($"/write/MyBookDeatils?bookId={book.id}");
                        }
                        else
                        {
                            ViewData["Message"] = "Opis nie może być pusty";
                            return View();
                        }
                    }
                    else
                    {
                        ViewData["Message"] = "Tytuł nie może być pusty";
                        return View();
                    }
                }
                else 
                {
                    ViewData["Message"] = "Musisz być autorem książki, którą chcesz edytować";
                    return View();
                }
            }
        }

        [HttpGet]
        public IActionResult CreateChapter(int bookId,string? message = null)
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
                ViewData["BookId"] = bookId;
                if (message != null)
                {
                    ViewData["Message"] = message;
                }
                return View();
            }
        }

        [HttpPost]
        public IActionResult CreateChapter(int bookId)
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
                string orderInBookString = HttpContext.Request.Form["orderInBook"];
                string title = HttpContext.Request.Form["title"];
                string content = HttpContext.Request.Form["content"];
                Book book = this._bookService.GetBook(bookId);
                if(book.authorId == user.id) 
                {
                    if(int.TryParse(orderInBookString,out int orderInBook))
                    {
                        if(title.Length > 0) 
                        {
                            if (content.Length > 0)
                            {
                                Chapter chapter = new Chapter(book.id,orderInBook,title,content,0,0,DateTime.Now, DateTime.Now);
                                int result = this._bookService.CreateChapter(chapter);
                                if(result > 0) 
                                {
                                    return Redirect($"/Write/MyBookDetails?bookId={book.id}");
                                }
                                else 
                                {
                                    ViewData["Message"] = "Nie udało sie dodać rozdziału";
                                    return View();
                                }
                            }
                            else
                            {
                                ViewData["Message"] = "Rozdział nie może być pusty";
                                return View();
                            }
                        }
                        else 
                        {
                            ViewData["Message"] = "Tytuł nie może być pusty";
                            return View();
                        }
                    }
                    else 
                    {
                        ViewData["Message"] = "Numer rozdziału w książce musi być liczbą";
                        return View();
                    }
                }
                else 
                {
                    ViewData["Message"] = "Możesz dodać rodział jedynie do swojej książki";
                    return View();
                }
            }
        }

        [HttpGet]
        public IActionResult EditChapter(int chapterId, string? message = null)
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
                Chapter chapter = this._bookService.GetChapter(chapterId);
                ViewData["Chapter"] = JsonSerializer.Serialize(chapter);
                if (message != null)
                {
                    ViewData["Message"] = message;
                }
                return View("CreateChapter");
            }
        }

        [HttpPost]
        public IActionResult EditChapter(int chapterId)
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
                string orderInBookString = HttpContext.Request.Form["orderInBook"];
                string title = HttpContext.Request.Form["title"];
                string content = HttpContext.Request.Form["content"];
                Chapter chapter = this._bookService.GetChapter(chapterId);
                if (chapter.book.authorId == user.id)
                {
                    if (int.TryParse(orderInBookString, out int orderInBook))
                    {
                        if (title.Length > 0)
                        {
                            if (content.Length > 0)
                            {
                                chapter.orderInBook = orderInBook;
                                chapter.title = title;
                                chapter.content = content;
                                this._bookService.UpdateChapter(chapter);
                                return Redirect($"/write/MyBookDeatails?bookId={chapter.bookId}");
                            }
                            else
                            {
                                ViewData["Message"] = "Rozdział nie może być pusty";
                                return View();
                            }
                        }
                        else
                        {
                            ViewData["Message"] = "Tytuł nie może być pusty";
                            return View();
                        }
                    }
                    else
                    {
                        ViewData["Message"] = "Numer rozdziału w książce musi być liczbą";
                        return View();
                    }
                }
                else
                {
                    ViewData["Message"] = "Możesz dodać rodział jedynie do swojej książki";
                    return View();
                }
            }
        }

        [HttpGet]
        public IActionResult MyBookDetails(int bookId) 
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
                Book book = this._bookService.GetBook(bookId);
                if (book.author.id == user.id) 
                {
                    ViewData["Book"] = JsonSerializer.Serialize(book);
                    return View();
                }
                else 
                {
                    return Redirect("/write/index");
                }
            }
        }
    }
}

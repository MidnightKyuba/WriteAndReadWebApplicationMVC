using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using System.Data;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using WriteAndReadWebApplicationMVC.Models;
using WriteAndReadWebApplicationMVC.Services.Interfaces;

namespace WriteAndReadWebApplicationMVC.Controllers
{
    public class ReadController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IUserService _userService;

        public ReadController(IBookService bookService, IUserService userService) 
        {
            _bookService = bookService;
            _userService = userService;
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
                List<Book> booksToView = this._bookService.GetAllOtherBooks(currentUser.id, bookStartId);
                foreach (Book book in booksToView)
                {
                    book.author = this._userService.GetUser(book.authorId);
                }
                ViewData["BooksList"] = JsonSerializer.Serialize(booksToView);
                ViewData["bookStartId"] = bookStartId;
                return View("Index");
            }
        }

        public IActionResult BookRead(int bookId) 
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
                User currentUser = JsonSerializer.Deserialize<User>(HttpContext.Session.GetString("_CurrentUser"));
                if (book.authorId == currentUser.id) 
                {
                    Redirect($"/write/MyBookDetails?bookId={book.id}");
                }
                book.author = this._userService.GetUser(book.authorId);
                book.chapters = this._bookService.GetAllChaptersForBook(book.id);
                ViewData["Chapters"] = JsonSerializer.Serialize(book.chapters);
                ViewData["Book"] = JsonSerializer.Serialize(book);
                return View();
            }
        }

        public IActionResult ChapterRead(int chapterId)
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
                Book book = this._bookService.GetBook(chapter.bookId);
                book.author = this._userService.GetUser(book.authorId);
                chapter.book = book;
                User currentUser = JsonSerializer.Deserialize<User>(HttpContext.Session.GetString("_CurrentUser"));
                if (currentUser.id == chapter.book.authorId) 
                {
                    Redirect($"/write/MyBookDetails?bookId={chapter.book.id}");
                }
                chapter.readCounter++;
                if(!this._bookService.IfReadedExist(currentUser.id,chapter.id)) 
                {
                    chapter.uniqueReadCounter++;
                    Readed readed = new Readed(currentUser.id,chapter.id);
                    this._bookService.CreateReaded(readed);
                }
                this._bookService.UpdateChapter(chapter);
                chapter = this._bookService.GetChapter(chapter.id);
                chapter.book = book;
                List<Comment> comments = this._bookService.GetAllCommentsForChapter(chapter.id);
                foreach (Comment comment in comments) 
                {
                    comment.user = this._userService.GetUser(comment.userId);
                }
                chapter.comments = comments;
                ViewData["Chapter"] = JsonSerializer.Serialize(chapter);
                return View();
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateComment(int chapterId, string content) 
        {
            User currentUser = JsonSerializer.Deserialize<User>(HttpContext.Session.GetString("_CurrentUser"));
            Comment comment = new Comment(chapterId,currentUser.id,content, DateTime.Now);
            int result = this._bookService.CreateComment(comment);
            if(result > 0)
            {
                comment = this._bookService.GetComment(result);
                (string content, string login, DateTime writeDate) resultData = (comment.content,comment.user.login,comment.writeDate);
                return Json(new { success = true, result = resultData});
            }
            else 
            {
                return Json(new { success = false, result = "0"});
            }
        }
    }
}

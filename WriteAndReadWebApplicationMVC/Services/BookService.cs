using WriteAndReadWebApplicationMVC.Models;
using WriteAndReadWebApplicationMVC.Services.Interfaces;

namespace WriteAndReadWebApplicationMVC.Services
{
    public class BookService: IBookService
    {
        private readonly IBookService _bookService;
        public BookService(IBookService bookService) 
        {
            this._bookService = bookService;
        }

        public int CreateBook(Book book)
        {
            throw new NotImplementedException();
        }

        public int CreateChapter(Chapter chapter)
        {
            throw new NotImplementedException();
        }

        public int CreateComment(Comment comment)
        {
            throw new NotImplementedException();
        }

        public int CreateReaded(Readed readed)
        {
            throw new NotImplementedException();
        }

        public List<Book> GetAllBooks()
        {
            throw new NotImplementedException();
        }

        public List<Chapter> GetAllChaptersForBook(int id)
        {
            throw new NotImplementedException();
        }

        public List<Comment> GetAllCommentsForChapter(int id)
        {
            throw new NotImplementedException();
        }

        public List<Book> GetAllMyBooks()
        {
            throw new NotImplementedException();
        }

        public Book GetBook(int id)
        {
            throw new NotImplementedException();
        }

        public Chapter GetChapter(int id)
        {
            throw new NotImplementedException();
        }

        public bool IfReadedExist(int userId, int chapterId)
        {
            throw new NotImplementedException();
        }

        public int UpdateBook(Book book)
        {
            throw new NotImplementedException();
        }

        public int UpdateChapter(Chapter chapter)
        {
            throw new NotImplementedException();
        }

        public int UpdateComment(Comment comment)
        {
            throw new NotImplementedException();
        }
    }
}

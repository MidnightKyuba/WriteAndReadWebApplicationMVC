using WriteAndReadWebApplicationMVC.Models;

namespace WriteAndReadWebApplicationMVC.Services.Interfaces
{
    public interface IBookService
    {
        public List<Book> GetAllBooks();
        public List<Book> GetAllMyBooks();
        public Book GetBook(int id);
        public int CreateBook(Book book);
        public int UpdateBook(Book book);
        public List<Chapter> GetAllChaptersForBook(int id);
        public Chapter GetChapter(int id);
        public int CreateChapter(Chapter chapter);
        public int UpdateChapter(Chapter chapter);
        public List<Comment> GetAllCommentsForChapter(int id);
        public int CreateComment(Comment comment);
        public int UpdateComment(Comment comment);
        public bool IfReadedExist(int userId, int chapterId);
        public int CreateReaded(Readed readed);
    }
}

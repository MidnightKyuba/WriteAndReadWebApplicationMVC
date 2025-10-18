using WriteAndReadWebApplicationMVC.Models;

namespace WriteAndReadWebApplicationMVC.Services.Interfaces
{
    public interface IBookService
    {
        public List<Book> GetAllOtherBooks(int userId, int bookStartId);
        public List<Book> GetAllMyBooks(int userId, int bookStartId);
        public Book GetBook(int bookId);
        public int CreateBook(Book book);
        public void UpdateBook(Book book);
        public List<Chapter> GetAllChaptersForBook(int bookId);
        public Chapter GetChapter(int chapterId);
        public int CreateChapter(Chapter chapter);
        public void UpdateChapter(Chapter chapter);
        public List<Comment> GetAllCommentsForChapter(int chapterId);
        public int CreateComment(Comment comment);
        public void UpdateComment(Comment comment);
        public bool IfReadedExist(int userId, int chapterId);
        public int CreateReaded(Readed readed);
        public Comment GetComment(int commentId);
    }
}

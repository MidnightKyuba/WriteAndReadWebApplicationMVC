using WriteAndReadWebApplicationMVC.Models;
using WriteAndReadWebApplicationMVC.Services.Interfaces;

namespace WriteAndReadWebApplicationMVC.Services
{
    public class BookService: IBookService
    {
        private readonly DbWriteAndReadContext _context;
        public BookService(DbWriteAndReadContext context) 
        {
            this._context = context;
        }

        public int CreateBook(Book book)
        {
            try
            {
                _context.Books.Add(book);
                if (_context.SaveChanges() > 0)
                {
                    return book.id;
                }
                throw new Exception();
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public int CreateChapter(Chapter chapter)
        {
            try
            {
                _context.Chapters.Add(chapter);
                if (_context.SaveChanges() > 0)
                {
                    return chapter.id;
                }
                throw new Exception();
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public int CreateComment(Comment comment)
        {
            try
            {
                _context.Comments.Add(comment);
                if (_context.SaveChanges() > 0)
                {
                    return comment.id;
                }
                throw new Exception();
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public int CreateReaded(Readed readed)
        {
            try
            {
                _context.Readeds.Add(readed);
                if (_context.SaveChanges() > 0)
                {
                    return readed.id;
                }
                throw new Exception();
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public List<Book> GetAllBooks()
        {
            return _context.Books.ToList();
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
            return _context.Books.Single<Book>(b=>b.id == id);
        }

        public Chapter GetChapter(int id)
        {
            return _context.Chapters.Single<Chapter>(ch => ch.id == id);
        }

        public bool IfReadedExist(int userId, int chapterId)
        {
            return _context.Readeds.Any(r => r.userId == userId && r.chapterId == chapterId);
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

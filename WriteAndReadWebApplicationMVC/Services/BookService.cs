using System.Net;
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

        public List<Chapter> GetAllChaptersForBook(int bookId)
        {
            try
            {
                return _context.Chapters.Where(ch => ch.bookId == bookId).OrderBy(ch => ch.orderInBook).ToList();
            }
            catch (Exception e) {
                return null;
            }
        }

        public List<Comment> GetAllCommentsForChapter(int chapterId)
        {
            try
            {
                return _context.Comments.Where(c => c.chapterId == chapterId).OrderBy(c => c.writeDate).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<Book> GetAllMyBooks(int userId, int bookStartId)
        {
            try
            {
                return _context.Books.OrderBy(b => b.id).Where(b => b.authorId == userId).Skip(bookStartId-1).ToList();
            }
            catch (Exception e) 
            {
                return null;
            }
        }

        public List<Book> GetAllOtherBooks(int userId, int bookStartId)
        {
            try
            {
                return _context.Books.OrderBy(b => b.id).Where(b => b.authorId != userId).Skip(bookStartId - 1).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Book GetBook(int bookId)
        {
            return _context.Books.Single<Book>(b=>b.id == bookId);
        }

        public Chapter GetChapter(int chapterId)
        {
            return _context.Chapters.Single<Chapter>(ch => ch.id == chapterId);
        }

        public Comment GetComment(int commentId)
        {
            return _context.Comments.Single<Comment>(c => c.id == commentId);
        }

        public bool IfReadedExist(int userId, int chapterId)
        {
            return _context.Readeds.Any(r => r.userId == userId && r.chapterId == chapterId);
        }

        public void UpdateBook(Book book)
        {
            this._context.Books.Update(book);
            this._context.SaveChanges();
        }

        public void UpdateChapter(Chapter chapter)
        {
            this._context.Chapters.Update(chapter);
            this._context.SaveChanges();
        }

        public void UpdateComment(Comment comment)
        {
            this._context.Comments.Update(comment);
            this._context.SaveChanges();
        }
    }
}

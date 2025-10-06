using System.Linq.Expressions;
using WriteAndReadWebApplicationMVC.Models;
using WriteAndReadWebApplicationMVC.Services.Interfaces;

namespace WriteAndReadWebApplicationMVC.Services
{
    public class UserService : IUserService
    {
        private readonly DbWriteAndReadContext _context;

        public UserService(DbWriteAndReadContext context) 
        {
            this._context = context;
        }

        public void ChangeUserData(User user)
        {
            this._context.Users.Update(user);
            this._context.SaveChanges();
        }

        public bool CheckIfEmailExist(string email)
        {
            return _context.Users.Any(x => x.email == email);
        }

        public bool CheckIfLoginExist(string login)
        {
            return _context.Users.Any(x => x.login == login);
        }

        public User GetUser(string login)
        {
            return _context.Users.Single(x => x.login == login);
        }

        public User GetUser(int id)
        {
            return _context.Users.Single(x => x.id == id);
        }

        public int Register(User user)
        {
            try
            {
                _context.Users.Add(user);
                if(_context.SaveChanges() > 0) 
                {
                    return user.id;
                }
                throw new Exception();
            }
            catch (Exception e) 
            {
                return 0;
            }
        }
    }
}

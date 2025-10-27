using WriteAndReadWebApplicationMVC.Models;
using WriteAndReadWebApplicationMVC.Services.Interfaces;

namespace WriteAndReadWebApplicationMVC.Services
{
    public class AdminService : IAdminService
    {
        private readonly DbWriteAndReadContext _context;
        public AdminService(DbWriteAndReadContext context) 
        {
            this._context = context;
        }
        public void ChangeUserLevel(int userId)
        {
            User user = this._context.Users.Single(x => x.id == userId);
            if (user != null)
            {
                if (user.admin == true)
                {
                    user.admin = false;
                }
                else
                {
                    user.admin = true;
                }
                this._context.Users.Update(user);
                this._context.SaveChanges();
            }
            else
            {
                throw new Exception("Użytkownik nie istnieje");
            }
        }

        public void CreateBlock(Block block)
        {
            this._context.Blocks.Add(block);
            this._context.SaveChanges();
        }

        public void DeleteUser(int userId)
        {
            User user = this._context.Users.Single(x => x.id == userId);
            if (user != null)
            {
                this._context.Users.Remove(user);
                this._context.SaveChanges();
            }
            else
            {
                throw new Exception("Użytkownik nie istnieje");
            }
        }

        public List<User> GetAllUsers()
        {
            return this._context.Users.ToList();
        }
    }
}

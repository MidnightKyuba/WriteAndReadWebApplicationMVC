using WriteAndReadWebApplicationMVC.Models;

namespace WriteAndReadWebApplicationMVC.Services.Interfaces
{
    public interface IUserService
    {
        public int Register(User user);
        public bool CheckIfEmailExist(string email);
        public bool CheckIfLoginExist(string login);
        public User GetUser(string login);
        public User GetUser(int id);
        public void ChangeUserData(User user);
    }
}

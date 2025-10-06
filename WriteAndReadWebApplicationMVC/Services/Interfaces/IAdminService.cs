using WriteAndReadWebApplicationMVC.Models;

namespace WriteAndReadWebApplicationMVC.Services.Interfaces
{
    public interface IAdminService
    {
        public List<User> GetAllUsers();
        public void DeleteUser(int userId);
        public void CreateBlock(Block block);
        public void ChangeUserLevel(int userId);
    }
}

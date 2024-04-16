using DataStore.Abstraction.Models;

namespace DataStore.Abstraction.Repositories
{
    public interface IUserRepository
    {
        bool IsUsernameTaken(string username);
        bool IsEmailTaken(string email);
        void AddUser(IUser user);
        ISingleUser GetUserByEmail(string email);
        Task<IUser> GetUser(string username, string email, string password);
        IUser GetUserById(int id);
        ISingleUser GetloginUserById(int id);
        Task<ISingleUser> GetUserByName(string username);
        Task<IEnumerable<IUser>> GetAllUsers();
        Task UpdateUserAdminStatus(int id, bool isAdmin);
        Task DeleteUser(int id);
    }
}

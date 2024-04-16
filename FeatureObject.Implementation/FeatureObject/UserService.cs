using DataStore.Abstraction.Models;
using DataStore.Abstraction.Repositories;
using DataStore.Implementation.Repositories;
using FeatureObject.Abstraction.FeatureObject;

namespace FeatureObject.Implementation.FeatureObject
{
    public class UserService:IUserService
    {
        private readonly IUserRepository _userrepository;
        public UserService(IUserRepository userrepository)
        {
            _userrepository = userrepository;
        }
        public bool IsUsernameTaken(string username)
        {
            return _userrepository.IsUsernameTaken(username);
        }

        public bool IsEmailTaken(string email)
        {
            return _userrepository.IsEmailTaken(email);
        }

        public void AddUser(IUser user)
        {
            _userrepository.AddUser(user);
        }

        public IUser GetUserById(int id)
        {
            return _userrepository.GetUserById(id);
        }

        public Task<IEnumerable<IUser>> GetAllUsers()
        {
            return _userrepository.GetAllUsers();
        }

        public Task UpdateUserAdminStatus(int id, bool isAdmin)
        {
            return _userrepository.UpdateUserAdminStatus(id, isAdmin);
        }

        public Task DeleteUser(int id)
        {
            return _userrepository.DeleteUser(id);
        }
    }
}

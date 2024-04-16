using DataStore.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeatureObject.Abstraction.FeatureObject
{
    public interface IUserService
    {
        public bool IsUsernameTaken(string username);
        public bool IsEmailTaken(string email);
        public void AddUser(IUser user);
        public IUser GetUserById(int id);
        Task<IEnumerable<IUser>> GetAllUsers();
        Task UpdateUserAdminStatus(int id, bool isAdmin);
        Task DeleteUser(int id);
    }
}

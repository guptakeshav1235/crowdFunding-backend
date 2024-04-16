using DataStore.Abstraction.Models;
using DataStore.Abstraction.Repositories;
using DataStore.Implementation.Repositories;
using FeatureObject.Abstraction.FeatureObject;

namespace FeatureObject.Implementation.FeatureObject
{
    public class LoginService:ILoginService
    {
        private readonly IUserRepository _userrepository;
        public LoginService(IUserRepository userrepository)
        {
            _userrepository = userrepository;
        }

        public ISingleUser Login(string username, string email, string password)
        {
            ISingleUser user = _userrepository.GetUserByEmail(email);

            if (user != null && BCrypt.Net.BCrypt.Verify(password,user.Password))
            {
                return user;
            }
            return null;
        }

        public async Task<ISingleUser> GetUserByName(string username)
        {
            return await _userrepository.GetUserByName(username);
        }

        public ISingleUser GetloginUserById(int id)
        {
            return _userrepository.GetloginUserById(id);
        }
    }
}

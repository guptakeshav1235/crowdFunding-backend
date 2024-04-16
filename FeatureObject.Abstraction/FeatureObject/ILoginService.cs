using DataStore.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeatureObject.Abstraction.FeatureObject
{
    public interface ILoginService
    {
        ISingleUser Login(string username, string email, string password);
        Task<ISingleUser> GetUserByName(string username);
        ISingleUser GetloginUserById(int id);
    }
}

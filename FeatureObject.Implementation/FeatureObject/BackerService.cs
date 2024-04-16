using DataStore.Abstraction.Models;
using DataStore.Abstraction.Repositories;
using FeatureObject.Abstraction.FeatureObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeatureObject.Implementation.FeatureObject
{
    public class BackerService:IBackerService
    {
        private readonly IBackerRepository _backerrepository;
        public BackerService(IBackerRepository backerrepository)
        {
            _backerrepository = backerrepository;
        }

        public Task<IEnumerable<IBackers>> GetReports(string username,string email, string password)
        {
            return _backerrepository.GetReports(username, email, password);
        }
        public Task<IEnumerable<IBackers>> GetReportsForAllUsers()
        {
            return _backerrepository.GetReportsForAllUsers();
        }
    }
}

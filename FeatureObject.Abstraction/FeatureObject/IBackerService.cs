using DataStore.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeatureObject.Abstraction.FeatureObject
{
    public interface IBackerService
    {
        Task<IEnumerable<IBackers>> GetReports(string username, string email, string password);
        Task<IEnumerable<IBackers>> GetReportsForAllUsers();
    }
}

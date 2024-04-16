using DataStore.Abstraction.Models;

namespace DataStore.Abstraction.Repositories
{
    public interface IBackerRepository
    {
        Task<IEnumerable<IBackers>> GetReports(string username, string email,string password);
        Task<IEnumerable<IBackers>> GetReportsForAllUsers();
    }
}

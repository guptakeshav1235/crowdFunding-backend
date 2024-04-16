using DataStore.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeatureObject.Abstraction.FeatureObject
{
    public interface IContributionService
    {
        Task<IEnumerable<IContributionDetails>> GetContributionsDetails();
        Task<IEnumerable<IContributionDetails>> GetContributionsByUserId(int userId);
        Task AddContribution(IContributionDetails contribution);
    }
}

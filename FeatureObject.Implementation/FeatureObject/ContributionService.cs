using DataStore.Abstraction.Models;
using DataStore.Abstraction.Repositories;
using FeatureObject.Abstraction.FeatureObject;
using System.Threading.Tasks;

namespace FeatureObject.Implementation.FeatureObject
{
    public class ContributionService:IContributionService
    {
        private readonly IContributionRepositories _contributionRepository;
        public ContributionService(IContributionRepositories contributionRepository)
        {
            _contributionRepository = contributionRepository;
        }

        public Task<IEnumerable<IContributionDetails>> GetContributionsDetails()
        {
            return _contributionRepository.GetContributionsDetails();
        }

        public Task<IEnumerable<IContributionDetails>> GetContributionsByUserId(int userId)
        {
            return _contributionRepository.GetContributionsByUserId(userId);
        }

        public Task AddContribution(IContributionDetails contribution)
        {
            return _contributionRepository.AddContribution(contribution);
        }
    }
}

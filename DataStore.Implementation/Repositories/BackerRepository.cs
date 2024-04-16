using DataStore.Abstraction.Models;
using DataStore.Abstraction.Repositories;
using DataStore.Implementation.Models;

namespace DataStore.Implementation.Repositories
{
    public class BackerRepository:IBackerRepository
    {
        private readonly ICampaignRepository _campaignRepository;
        private readonly IContributionRepositories _contributionRepositories;
        private readonly IUserRepository _userRepository;

        public BackerRepository(ICampaignRepository campaignRepository, IUserRepository userRepository, IContributionRepositories contributionRepositories)
        {
            _campaignRepository = campaignRepository;
            _userRepository = userRepository;
            _contributionRepositories = contributionRepositories;
        }
        public async Task<IEnumerable<IBackers>> GetReportsForAllUsers()
        {
            var allUsers = await _userRepository.GetAllUsers();
            if (allUsers == null || !allUsers.Any())
            {
                return Enumerable.Empty<IBackers>();
            }

            var allBackers = new List<IBackers>();

            foreach (var user in allUsers)
            {
                var contributions = await _contributionRepositories.GetContributionsByUserId(user.Id);
                if (contributions == null || !contributions.Any())
                {
                    continue; // Move to the next user if no contributions found
                }

                foreach (var contribution in contributions)
                {
                    var campaign = _campaignRepository.GetCampaignById(contribution.CampaignId);
                    if (campaign == null)
                    {
                        continue; // Move to the next contribution if campaign not found
                    }

                    decimal equityOffered = CalculateEquityOffered(campaign, contribution.Amount);
                    allBackers.Add(new Backers
                    {
                        CampaignTitle = campaign.Title,
                        Backer = user.Username,
                        Amount = contribution.Amount,
                        EquityOffered = equityOffered,
                        Date = contribution.Date,
                    });
                }
            }

            return allBackers;
        }

        public async Task<IEnumerable<IBackers>> GetReports(string username,string email, string password)
        {
            var user = await _userRepository.GetUser(username,email, password);
            if (user == null)
            {
                return Enumerable.Empty<IBackers>();
            }

            var contributions = await _contributionRepositories.GetContributionsByUserId(user.Id);
            if (contributions == null || !contributions.Any())
            {
                return Enumerable.Empty<IBackers>();
            }

            var backers = new List<IBackers>();

            foreach (var contribution in contributions)
            {
                var campaign = _campaignRepository.GetCampaignById(contribution.CampaignId);
                if (campaign == null)
                {
                    continue;
                }

                decimal equityOffered = CalculateEquityOffered(campaign, contribution.Amount);
                backers.Add(new Backers
                {
                    CampaignTitle = campaign.Title,
                    Backer = user.Username,
                    Amount = contribution.Amount,
                    EquityOffered = equityOffered,
                    Date=contribution.Date,
                });
            }

            return backers;
        }

        private decimal CalculateEquityOffered(ICampaign campaign, decimal contributionAmount)
        {
            // Ensure that both EquityShares and Goal are greater than 0 to avoid division by zero
            if (campaign.EquityShares <= 0 || campaign.Goal <= 0)
            {
                return 0;
            }

            // Calculate equity offered using decimal division
            decimal equityOffered = (decimal)campaign.EquityShares / campaign.Goal * contributionAmount;

            // Log the calculation result for debugging
            //Console.WriteLine($"Equity Offered: {equityOffered}");

            return equityOffered;
        }
    }
}   

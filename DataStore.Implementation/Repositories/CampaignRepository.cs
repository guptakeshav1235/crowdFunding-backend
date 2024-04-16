using DataStore.Abstraction.Repositories;
using DataStore.Abstraction.Utilities;
using System.Data.SqlClient;
using System.Data;
using DataStore.Abstraction.Models;
using System.Data.Common;
using Dapper;
using DataStore.Implementation.Models;

namespace DataStore.Implementation.Repositories
{
    public class CampaignRepository:ICampaignRepository
    {
        private readonly IDapperContext _dappercontext;
        public CampaignRepository(IDapperContext dappercontext)
        {
            _dappercontext = dappercontext;
        }

        public async Task<IEnumerable<ICampaign>> GetAllCampaigns()
        {
            string sqlQuery = "SELECT * FROM Campaigns";
            using (var connection = _dappercontext.CreateConnection())
            {
                return await connection.QueryAsync<Campaign>(sqlQuery);
            }
        }

        public ICampaign GetCampaignById(int id)
        {
            string sqlQuery = "SELECT * FROM Campaigns WHERE Id = @Id";
            using (var connection = _dappercontext.CreateConnection())
            {
                return connection.QueryFirstOrDefault<Campaign>(sqlQuery, new { Id = id });
            }
            
        }

        public void AddCampaign(ICampaign campaign)
        {
            string sqlQuery = "INSERT INTO Campaigns (Title, Creator, Goal, EquityShares, Description) VALUES(@Title, @Creator, @Goal, @EquityShares, @Description)";
             using (var connection = _dappercontext.CreateConnection())
            {
                connection.Execute(sqlQuery, new
                {
                    Title=campaign.Title,
                    Creator=campaign.Creator,
                    Goal=campaign.Goal,
                    CurrentAmount=campaign.CurrentAmount,
                    Backers=campaign.Backers,
                    EquityShares=campaign.EquityShares,
                    Description=campaign.Description,
                });
            }
        }

        public async Task UpdateCampaign(ICampaign campaign)
        {
            string sqlQuery = @"UPDATE Campaigns 
                        SET Title = @Title, 
                            Creator = @Creator, 
                            Goal = @Goal, 
                            CurrentAmount = @CurrentAmount, 
                            Backers = @Backers, 
                            EquityShares = @EquityShares, 
                            Description = @Description 
                        WHERE Id = @Id";

            using (var connection = _dappercontext.CreateConnection())
            {
                await connection.ExecuteAsync(sqlQuery, new
                {
                    Id = campaign.Id,
                    Title = campaign.Title,
                    Creator = campaign.Creator,
                    Goal = campaign.Goal,
                    CurrentAmount = campaign.CurrentAmount,
                    Backers = campaign.Backers,
                    EquityShares = campaign.EquityShares,
                    Description = campaign.Description,
                });
            }
        }

        public async Task DeleteCampaign(int id)
        {
            using (var connection = _dappercontext.CreateConnection())
            {
                // Delete related contribution details
                string deleteContributionsQuery = "DELETE FROM ContributionDetails WHERE CampaignId = @Id";
                await connection.ExecuteAsync(deleteContributionsQuery, new { Id = id });

                // Delete the campaign
                string deleteCampaignQuery = "DELETE FROM Campaigns WHERE Id = @Id";
                await connection.ExecuteAsync(deleteCampaignQuery, new { Id = id });
            }
        }
    }
}

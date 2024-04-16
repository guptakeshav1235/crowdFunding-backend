using Dapper;
using DataStore.Abstraction.Models;
using DataStore.Abstraction.Repositories;
using DataStore.Abstraction.Utilities;
using DataStore.Implementation.Models;
using DataStore.Implementation.Utilities;
using System.Data.Common;

namespace DataStore.Implementation.Repositories
{
    public class ContributionRepository : IContributionRepositories
    {
        private readonly IDapperContext _dappercontext;
        public ContributionRepository(IDapperContext dappercontext)
        {
            _dappercontext = dappercontext;
        }
        public async Task<IEnumerable<IContributionDetails>> GetContributionsDetails()
        {
            string sqlQuery = @"
                SELECT
                    C.Id,
                    C.CampaignId,
                    C.UserId,
                    C.Amount,
                    C.Date,
                    U.Username
                FROM
                    ContributionDetails C
                INNER JOIN
                    Users U ON C.UserId = U.Id
                ORDER BY
                    C.Date DESC"; 


            using (var connection = _dappercontext.CreateConnection())
            {
                var contributions = await connection.QueryAsync<ContributionDetails, User, ContributionDetails>(
                sqlQuery,
                (contribution, user) =>
                {
                    contribution.User = user;
                    return contribution;
                },
                splitOn: "UserId");


               
                foreach (var contribution in contributions)
                {
                    if (contribution.Amount == 0)
                    {
                        var amount = await connection.ExecuteScalarAsync<int>("SELECT Amount FROM ContributionDetails WHERE Id = @Id", new { contribution.Id });
                        contribution.Amount = amount;
                    }
                }

                return contributions;
            }

        }

        public async Task<IEnumerable<IContributionDetails>> GetContributionsByUserId(int userId)
        {
            string sqlQuery = @"
                SELECT
                    C.Id,
                    C.CampaignId,
                    C.UserId,
                    C.Amount,
                    C.Date,
                    U.Username
                FROM
                    ContributionDetails C
                INNER JOIN
                    Users U ON C.UserId = U.Id
                WHERE
                    C.UserId = @UserId
                ORDER BY
                    C.Date DESC";

            using (var connection = _dappercontext.CreateConnection())
            {
                var contributions = await connection.QueryAsync<ContributionDetails, User, ContributionDetails>(
                    sqlQuery,
                    (contribution, user) =>
                    {
                        contribution.User = user;
                        return contribution;
                    },
                    new { UserId = userId },
                    splitOn: "UserId");

                // Ensure that the Amount property is correctly populated
                foreach (var contribution in contributions)
                {
                    // Check if the Amount property is not null
                    if (contribution.Amount == 0)
                    {
                        // Fetch the contribution amount from the database again if it's 0
                        var amount = await connection.ExecuteScalarAsync<int>("SELECT Amount FROM ContributionDetails WHERE Id = @Id", new { contribution.Id });
                        contribution.Amount = amount;
                    }
                }

                return contributions;
            }
        }
        public async Task AddContribution(IContributionDetails contribution)
        {
            string sqlQuery = @"
                INSERT INTO ContributionDetails (CampaignId, UserId, Amount, Date)
                VALUES (@CampaignId, @UserId, @Amount, @Date)"
            ;

            using (var connection = _dappercontext.CreateConnection())
            {
                await connection.ExecuteAsync(sqlQuery, new
                {
                    contribution.CampaignId,
                    contribution.UserId,
                    contribution.Amount,
                    Date = DateTime.UtcNow
                });
            }
        }
    }
}

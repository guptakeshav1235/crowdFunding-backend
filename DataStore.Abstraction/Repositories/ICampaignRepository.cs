using DataStore.Abstraction.Models;
namespace DataStore.Abstraction.Repositories
{
    public interface ICampaignRepository
    {
        Task<IEnumerable<ICampaign>> GetAllCampaigns();
        ICampaign GetCampaignById(int id);
        void AddCampaign(ICampaign campaign);
        Task UpdateCampaign(ICampaign campaign);
        Task DeleteCampaign(int id);
    }
}

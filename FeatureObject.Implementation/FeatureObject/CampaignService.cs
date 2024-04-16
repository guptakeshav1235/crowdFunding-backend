using DataStore.Abstraction.Models;
using DataStore.Abstraction.Repositories;
using DataStore.Implementation.Models;
using DataStore.Implementation.Repositories;
using FeatureObject.Abstraction.FeatureObject;

namespace FeatureObject.Implementation.FeatureObject
{
    public class CampaignService:ICampaignService
    {
        private readonly ICampaignRepository _campaignrepository;
        public CampaignService(ICampaignRepository campaignrepository)
        {
            _campaignrepository = campaignrepository;
        }

        public Task<IEnumerable<ICampaign>> GetAllCampaigns()
        {
            return _campaignrepository.GetAllCampaigns();
        }

        public ICampaign GetCampaignById(int id)
        {
            return _campaignrepository.GetCampaignById(id);
        }

        public void AddCampaign(ICampaign campaign)
        {
            _campaignrepository.AddCampaign(campaign);
        }

        public async Task UpdateCampaign(ICampaign campaign)
        {
            await _campaignrepository.UpdateCampaign(campaign);
        }

        public async Task DeleteCampaign(int id)
        {
            await _campaignrepository.DeleteCampaign(id);
        }
    }
}

using DataStore.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeatureObject.Abstraction.FeatureObject
{
    public interface ICampaignService
    {
        Task<IEnumerable<ICampaign>> GetAllCampaigns();
        ICampaign GetCampaignById(int id);
        public void AddCampaign(ICampaign campaign);
        Task UpdateCampaign(ICampaign campaign);
        Task DeleteCampaign(int id);
    }
}

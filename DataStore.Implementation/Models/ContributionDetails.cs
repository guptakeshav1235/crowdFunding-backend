using DataStore.Abstraction.Models;

namespace DataStore.Implementation.Models
{
    public class ContributionDetails:IContributionDetails
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public IUser User { get; set; }
    }
}

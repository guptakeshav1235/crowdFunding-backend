namespace DataStore.Abstraction.Models
{
    public interface IContributionDetails
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public IUser User { get; set; }
    }
}

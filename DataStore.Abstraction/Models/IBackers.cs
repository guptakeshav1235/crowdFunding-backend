using System.ComponentModel.DataAnnotations;

namespace DataStore.Abstraction.Models
{
    public interface IBackers
    {
        public string CampaignTitle { get; set; }
        public string Backer { get; set; }
        public decimal Amount { get; set; }
        public decimal EquityOffered { get; set; }
        public DateTime Date { get; set; }
    }
}

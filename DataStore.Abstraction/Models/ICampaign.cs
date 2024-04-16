﻿namespace DataStore.Abstraction.Models
{
    public interface ICampaign
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Creator { get; set; }
        public int Goal { get; set; }
        public int CurrentAmount {  get; set; }
        public int Backers {  get; set; }
        public int EquityShares { get; set; }
        public string Description { get; set; }
    }
}

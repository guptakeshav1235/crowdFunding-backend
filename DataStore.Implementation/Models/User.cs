using DataStore.Abstraction.Models;

namespace DataStore.Implementation.Models
{
    public class User:IUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool IsAdmin { get; set; }
    }
}

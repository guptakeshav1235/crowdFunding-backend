using BCrypt.Net;
using Dapper;
using DataStore.Abstraction.Models;
using DataStore.Abstraction.Repositories;
using DataStore.Abstraction.Utilities;
using DataStore.Implementation.Models;
using DataStore.Implementation.Utilities;
using System.Text.RegularExpressions;

namespace DataStore.Implementation.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDapperContext _dappercontext;
        public UserRepository(IDapperContext dappercontext)
        {
            _dappercontext = dappercontext;
        }


        // Email Validation Regex Pattern
        private readonly string EmailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

        // Password Validation Regex Pattern
        private readonly string PasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,15}$";

        // Username Validation Regex Pattern
        private readonly string UsernamePattern = @"^[a-zA-Z][a-zA-Z0-9_.-]{2,19}$";

        public bool IsUsernameTaken(string username)
        {
            // Validate username format
            if (!IsValidUsername(username))
            {
                throw new ArgumentException("Invalid username format");
            }

            string sqlQuery = "SELECT * FROM Users WHERE Username = @Username";
            using (var connection = _dappercontext.CreateConnection())
            {
                return connection.Query<User>(sqlQuery, new { Username = username }).Any();
            }
        }

        public bool IsEmailTaken(string email)
        {
            // Validate email format
            if (!IsValidEmail(email))
            {
                throw new ArgumentException("Invalid email format");
            }

            string sqlQuery = "SELECT * FROM Users WHERE Email = @Email";
            using (var connection = _dappercontext.CreateConnection())
            {
                return connection.Query<User>(sqlQuery, new { Email = email }).Any();
            }
        }
        public void AddUser(IUser user)
        {
            // Validate email format
            if (!IsValidEmail(user.Email))
            {
                throw new ArgumentException("Invalid email format");
            }

            // Validate password format
            if (!IsValidPassword(user.Password))
            {
                throw new ArgumentException("Invalid password format");
            }

            // Validate username format
            if (!IsValidUsername(user.Username))
            {
                throw new ArgumentException("Invalid username format");
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

            string sqlQuery = "INSERT INTO Users (Username, Email, Password, ConfirmPassword, IsAdmin) VALUES (@Username, @Email, @Password, @ConfirmPassword, @IsAdmin)";
            using (var connection = _dappercontext.CreateConnection())
            {
                connection.Execute(sqlQuery, new
                {
                    Username = user.Username,
                    Email = user.Email,
                    Password = hashedPassword,
                    ConfirmPassword = hashedPassword,
                    IsAdmin = user.IsAdmin,
                });
            }
        }
        // Method to validate email format
        private bool IsValidEmail(string email)
        {
            return !string.IsNullOrEmpty(email) && Regex.IsMatch(email, EmailPattern);
        }

        // Method to validate password format
        private bool IsValidPassword(string password)
        {
            return !string.IsNullOrEmpty(password) && Regex.IsMatch(password, PasswordPattern);
        }

        // Method to validate username format
        private bool IsValidUsername(string username)
        {
            return !string.IsNullOrEmpty(username) && Regex.IsMatch(username, UsernamePattern);
        }

        /*public bool IsUsernameTaken(string username)
        {
            string sqlQuery = "SELECT * FROM Users WHERE Username = @Username";
            using(var connection=_dappercontext.CreateConnection())
            {
                return connection.Query<User>(sqlQuery, new { Username = username } ).Any();
            }
        }

        public bool IsEmailTaken(string email)
        {
            string sqlQuery = "SELECT * FROM Users WHERE Email = @Email";
            using (var connection = _dappercontext.CreateConnection())
            {
                return connection.Query<User>(sqlQuery, new { Email = email }).Any();
            }
        }

        public void AddUser(IUser user)
        {
            string hashedPassword=BCrypt.Net.BCrypt.HashPassword(user.Password);

            string sqlQuery = "INSERT INTO Users (Username, Email, Password, ConfirmPassword,IsAdmin) VALUES (@Username, @Email, @Password, @ConfirmPassword,@IsAdmin)";
            using (var connection = _dappercontext.CreateConnection())
            {
                connection.Execute(sqlQuery, new
                {
                    Username = user.Username,
                    Email = user.Email,
                    Password = hashedPassword,
                    ConfirmPassword = hashedPassword,
                    IsAdmin=user.IsAdmin,
                });
            }
        }*/

        public ISingleUser GetUserByEmail(string email)
        {
            string sqlQuery = "SELECT * FROM Users WHERE Email = @Email";
            using (var connection = _dappercontext.CreateConnection())
            {
                return connection.QueryFirstOrDefault<SingleUser>(sqlQuery, new { Email = email });
            }
        }

        public async Task<IUser> GetUser(string username,string email, string password)
        {
            string sqlQuery = "SELECT * FROM Users WHERE Username=@Username AND Email = @Email";
            using (var connection = _dappercontext.CreateConnection())
            {
                var user = await connection.QueryFirstOrDefaultAsync<User>(sqlQuery, new { Username = username, Email = email });

                if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    return user;
                }
                return null;
            }
        }

        public IUser GetUserById(int id)
        {
            string sqlQuery = "SELECT * FROM Users WHERE Id = @Id";
            using (var connection = _dappercontext.CreateConnection())
            {
                return connection.QueryFirstOrDefault<User>(sqlQuery, new { Id = id });
            }
        }

        public ISingleUser GetloginUserById(int id)
        {
            string sqlQuery = "SELECT * FROM Users WHERE Id = @Id";
            using (var connection = _dappercontext.CreateConnection())
            {
                return connection.QueryFirstOrDefault<SingleUser>(sqlQuery, new { Id = id });
            }
        }

        public async Task<ISingleUser> GetUserByName(string username)
        {
            string sqlQuery = "SELECT * FROM Users WHERE Username = @Username";
            using (var connection = _dappercontext.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<SingleUser>(sqlQuery, new { Username = username });
            }
        }

        public async Task<IEnumerable<IUser>> GetAllUsers()
        {
            string sqlQuery = "SELECT * FROM Users";
            using (var connection = _dappercontext.CreateConnection())
            {
                return await connection.QueryAsync<User>(sqlQuery);
            }
        }

        public async Task UpdateUserAdminStatus(int id, bool isAdmin)
        {
            string sqlQuery = "UPDATE Users SET IsAdmin = @IsAdmin WHERE Id = @Id";
            using (var connection = _dappercontext.CreateConnection())
            {
                await connection.ExecuteAsync(sqlQuery, new { Id = id, IsAdmin = isAdmin });
            }
        }

        public async Task DeleteUser(int id)
        {
            using (var connection = _dappercontext.CreateConnection())
            {
                // Delete related contribution details
                string deleteContributionsQuery = "DELETE FROM ContributionDetails WHERE UserId = @Id";
                await connection.ExecuteAsync(deleteContributionsQuery, new { Id = id });

                // Delete the campaign
                string deleteUserQuery = "DELETE FROM Users WHERE Id = @Id";
                await connection.ExecuteAsync(deleteUserQuery, new { Id = id });
            }
        }
    }
}

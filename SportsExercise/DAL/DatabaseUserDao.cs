using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using SportsExercise.Models;

namespace SportsExercise.DAL
{
    internal class DatabaseUserDao : IUserDao
    {
        private const string CreateUserTableCommand = @"CREATE TABLE IF NOT EXISTS users (username varchar PRIMARY KEY, password varchar);";
        private const string SelectAllUsersCommand = @"SELECT username, password FROM users";
        private const string SelectUserByCredentialsCommand = "SELECT username, password FROM users WHERE username=@username AND password=@password";
        private const string InsertUserCommand = @"INSERT INTO users(username, password) VALUES (@username, @password)";
        private const string FetchProfileCommand = "SELECT name, bio, image FROM users WHERE username = @username";
        private const string UpdateProfileCommand = "UPDATE users SET name = @name, bio = @bio, image = @image WHERE username = @username";
        
        private readonly string _connectionString;

        public DatabaseUserDao(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
        }

        public User? GetUserByAuthToken(string authToken)
        {
            return GetAllUsers().SingleOrDefault(u => u.Token == authToken);
        }

        public User? GetUserByCredentials(string username, string password)
        {
            // TODO: handle exceptions
            User? user = null;

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(SelectUserByCredentialsCommand, connection);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password);

            // take the first row, if any
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                user = ReadUser(reader);
            }

            return user;
        }

        public bool InsertUser(User user)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();
                Console.WriteLine("InsertUser: " + user.Username + " " + user.Password);
                using var cmd = new NpgsqlCommand(InsertUserCommand, connection);
                cmd.Parameters.AddWithValue("username", user.Username);
                cmd.Parameters.AddWithValue("password", user.Password);
                var affectedRows = cmd.ExecuteNonQuery();

                return affectedRows > 0;
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine("Exception in InsertUser: " + e.Message);
                return false;
            }
        }

        private void EnsureTables()
        {
            // TODO: handle exceptions
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(CreateUserTableCommand, connection);
            cmd.ExecuteNonQuery();
        }

        private IEnumerable<User> GetAllUsers() 
        {
            // TODO: handle exceptions
            var users = new List<User>();

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(SelectAllUsersCommand, connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var user = ReadUser(reader);
                users.Add(user);
            }

            return users;
        }

        private User ReadUser(IDataRecord record)
        {
            var username = Convert.ToString(record["username"])!;
            var password = Convert.ToString(record["password"])!;

            return new User(username, password);
        }
        
        public Profile? FetchProfile(string username)
        {
            Profile? profile = null;
            
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            try
            {
                using var cmd = new NpgsqlCommand(FetchProfileCommand, connection);
                cmd.Parameters.AddWithValue("username", username);
                
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    profile =new Profile(
                        Convert.ToString(reader["name"]),
                        Convert.ToString(reader["bio"]),
                        Convert.ToString(reader["image"])
                    );
                }
                return profile;
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine("Exception in FetchProfile: " + e.Message);
                return null;
            }
            
        }

        public bool UpdateProfile(string username, Profile profile)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            try
            {
                using var cmd = new NpgsqlCommand(UpdateProfileCommand, connection);
                cmd.Parameters.AddWithValue("name", profile.Name);
                cmd.Parameters.AddWithValue("bio", profile.Bio);
                cmd.Parameters.AddWithValue("image", profile.Image);
                cmd.Parameters.AddWithValue("username", username);
                var affectedRows = cmd.ExecuteNonQuery();

                return affectedRows > 0;
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine("Exception in UpdateProfile: " + e.Message);
                return false;
            }

        }
    }
}

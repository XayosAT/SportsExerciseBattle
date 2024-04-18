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
        private const string FetchProfileCommand = @"SELECT name, bio, image FROM users WHERE username = @username";
        private const string UpdateProfileCommand = @"UPDATE users SET name = @name, bio = @bio, image = @image WHERE username = @username";
        // Assuming FetchStatsCommand is defined somewhere else and correctly includes a placeholder for @username
        private const string FetchStatsCommand = @"
    SELECT
        u.username,
        u.elo,
        COALESCE(SUM(p.count), 0) AS total_pushups,
        CASE 
            WHEN COALESCE(SUM(p.duration), 0) > 0 THEN 
                CAST(COALESCE(SUM(p.count), 0) AS FLOAT) / COALESCE(SUM(p.duration), 0)
            ELSE 0
        END AS average_pushups
    FROM
        users u
    LEFT JOIN
        push_up_records p ON u.username = p.fk_user_id
    WHERE
        u.username = @username
    GROUP BY
        u.username, u.elo;
";
        
        
        private const string FetchRecordsCommand = @"
        SELECT
            count,
            duration,
            date_time
        FROM
            push_up_records
        WHERE
            fk_user_id = @username
        ORDER BY
            date_time DESC";

        private const string InsertEntryCommand = @"INSERT INTO push_up_records(fk_user_id, count, duration) VALUES (@username, @count, @duration)";
        //create Update elo commmand, it should add the value
        private const string UpdateEloCommand = @"UPDATE users SET elo = elo + @elo WHERE username = @username";
        private readonly string _connectionString;

        public DatabaseUserDao(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
        }

        public User? GetUserByAuthToken(string authToken)
        {
            Console.WriteLine("In GetUserByAuthToken");
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

        public Stats? FetchStats(string username)
        {
            Stats? stats = null;
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            Console.WriteLine("Username in Database: " + username);
            
            try
            {
                using var cmd = new NpgsqlCommand(FetchStatsCommand, connection);
                cmd.Parameters.AddWithValue("username", username);
                
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Console.WriteLine("Username in Database: " + Convert.ToString(reader["username"]));
                    Console.WriteLine("Elo in Database: " + Convert.ToInt32(reader["elo"]));
                    Console.WriteLine("Total Pushups in Database: " + Convert.ToInt32(reader["total_pushups"]));
                    
                    stats = new Stats(
                        Convert.ToString(reader["username"]),
                        Convert.ToInt32(reader["elo"]),
                        Convert.ToInt32(reader["total_pushups"]),
                        Convert.ToDouble(reader["average_pushups"])
                        
                        
                    );
                }
                return stats;
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine("Exception in FetchStats: " + e.Message);
                return null;
            }
            
        }

        public Record[]? FetchRecords(string username)
        {
            List<Record> history = new List<Record>();
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            try
            {
                using var cmd = new NpgsqlCommand(FetchRecordsCommand, connection);
                cmd.Parameters.AddWithValue("username", username);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Record record = new Record(
                        Convert.ToInt32(reader["count"]),
                        Convert.ToInt32(reader["duration"]),
                        Convert.ToDateTime(reader["date_time"])
                    );
                    history.Add(record);
                }

                return history.ToArray();
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine("Exception in FetchRecords: " + e.Message);
                return null;
            }
        }
        
        public void InsertEntry(string username, Entry entry)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            try
            {
                using var cmd = new NpgsqlCommand(InsertEntryCommand, connection);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("count", entry.Count);
                cmd.Parameters.AddWithValue("duration", entry.DurationInSeconds);
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine("Exception in InsertEntry: " + e.Message);
            }
        }
        public void UpdateElo(string username, int elo)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            try
            {
                using var cmd = new NpgsqlCommand(UpdateEloCommand, connection);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("elo", elo);
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine("Exception in UpdateElo: " + e.Message);
            }
        }
    }
}

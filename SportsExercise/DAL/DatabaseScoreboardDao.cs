using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using SportsExercise.Models;

namespace SportsExercise.DAL;

internal class DatabaseScoreboardDao : IScoreboardDao
{
    private const string FetchStatsCommand = @"
    SELECT
        u.username,
        COALESCE(SUM(p.count), 0) AS total_pushups,
        u.elo
    FROM
        users u
    LEFT JOIN
        push_up_records p ON u.username = p.fk_user_id
    GROUP BY
        u.username, u.elo
    ORDER BY
        u.elo DESC";
    
    
    
    private readonly string _connectionString;
    public DatabaseScoreboardDao(string connectionString)
    {
        _connectionString = connectionString;
        
    }
    
    public Stats[] FetchScoreboard()
    {
        List<Stats> scoreboard = new List<Stats>();
        
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        try
        {
            using var cmd = new NpgsqlCommand(FetchStatsCommand, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Stats stats = new Stats(
                    Convert.ToString(reader["username"]),
                    Convert.ToInt32(reader["elo"]),
                    Convert.ToInt32(reader["total_pushups"]),
                    0
                );
                scoreboard.Add(stats);
            }
            
        }
        catch (NpgsqlException e)
        {
            Console.WriteLine("Error in FetchScoreboard: " + e.Message);
        }

        return scoreboard.ToArray();
    }
    
}
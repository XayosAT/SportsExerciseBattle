namespace SportsExercise.Models
{
    public class User
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Token => $"{Username}-sebToken";

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
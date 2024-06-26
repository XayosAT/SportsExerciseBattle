using SportsExercise.DAL;
using System;
using SportsExercise.Models;
using System.Timers;
using Timer = System.Timers.Timer;

namespace SportsExercise.BLL;

public class TournamentManager
{
    private static TournamentManager? _instance;
    private static readonly object _lock = new object();
    private Dictionary<string, int> _userPushUps;
    private Timer _timer;
    public bool IsTournamentActive { get; private set; }
    private DateTime _startTime;
    private readonly IUserManager _userManager;
    private string _log = "";
    
    
    private TournamentManager(IUserManager userManager)
    {
        _userManager = userManager;
        _userPushUps = new Dictionary<string, int>();
        _timer = new Timer(20000); // 20 seconds can be changed to 120000 for 2 minutes
        _timer.Elapsed += EndTournament;
        IsTournamentActive = false;
        _timer.AutoReset = false;
    }
    
    public static TournamentManager? GetInstance(IUserManager userManager)
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new TournamentManager(userManager);
                }
            }
        }
        return _instance;
    }
    
    public string AddEntry(string username, Entry entry)
    {
        
        if (entry.Count == null || entry.Name == null || entry.DurationInSeconds == null)
        {
            throw new Exception("Invalid entry");
        }
        
        lock (_lock) 
        {
            if (!IsTournamentActive)
            {
                IsTournamentActive = true;
                _userPushUps.Clear();  // Optionally clear previous data
                _timer.Start();  // Start the 2-minute countdown
                _startTime = DateTime.Now;
                PrintLogs("Tournament started");
            }

            if (!_userPushUps.ContainsKey(username))
            {
                _userPushUps[username] = 0;
            }
            _userPushUps[username] += entry.Count.Value;
            
            PrintLogs($"Adding entry for {username}: {entry.Name} {entry.Count} {entry.DurationInSeconds}");
            PrintLogs($"Total push-ups for {username}: {_userPushUps[username]}");
        }
        
        return "Entry added successfully";
    }
    
    public string GetTournamentInfo()
    {
        lock (_lock) 
        {
            if (!IsTournamentActive)
            {
                if(_log != "")
                {
                    return "TOURNAMENT HAS ENDED. " + _log;
                }
                return "No active tournament";
            }
        
            _log = "Tournament info:\n";
            int numberOfParticipants = _userPushUps.Count;
            var leadingUsers = _userPushUps.Where(x => x.Value == _userPushUps.Max(p => p.Value)).Select(x => x.Key).ToList();
            string leaders = string.Join(", ", leadingUsers);

            _log += $"Start Time: {_startTime.ToString("yyyy-MM-dd HH:mm:ss")}\n";
            _log += $"Number of Participants: {numberOfParticipants}\n";
            _log += $"Leading Users: {leaders}\n";

            return _log;
        }
        
    }
    
    private void EndTournament(object sender, ElapsedEventArgs e)
    {
        lock (_lock)
        {
            PrintLogs("Tournament ended");
            var leadingUsers = _userPushUps.Where(x => x.Value == _userPushUps.Max(p => p.Value)).Select(x => x.Key).ToList();
            string leaders = string.Join(", ", leadingUsers);
            PrintLogs($"Winner/s: {leaders}");
            _log = GetTournamentInfo();

            if (leadingUsers.Count == 1)
            {
                UpdateElo(leadingUsers[0], 2);
                //update elo for all losers
                foreach (var user in _userPushUps.Keys)
                {
                    if (user != leadingUsers[0])
                    {
                        UpdateElo(user, -1);
                    }
                }
            }
            else
            {
                foreach (var user in leadingUsers)
                {
                    UpdateElo(user, 1);
                }
                //update elo for all losers
                foreach (var user in _userPushUps.Keys)
                {
                    if (!leadingUsers.Contains(user))
                    {
                        UpdateElo(user, -1);
                    }
                }
            }
            
            
            IsTournamentActive = false;
            _timer.Stop();
        }
    }

    private void PrintLogs(string message)
    {
        Console.WriteLine("\n__________________________\n");
        Console.WriteLine("TOURNAMENT LOGS");
        Console.WriteLine(message);
        Console.WriteLine("__________________________\n");
    }
    
    private void UpdateElo(string username, int elo)
    {
        _userManager.UpdateElo(username, elo);
    }
    
    public int GetPushUps(string username)
    {
        lock (_lock)
        {
            if (_userPushUps.ContainsKey(username))
            {
                return _userPushUps[username];
            }
            return -1;
        }
    }
}
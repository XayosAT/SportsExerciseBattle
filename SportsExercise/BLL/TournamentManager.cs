using SportsExercise.DAL;
using System;
using SportsExercise.Models;

namespace SportsExercise.BLL;

public class TournamentManager
{
    private static TournamentManager _instance;
    private static readonly object _lock = new object();
    private int _entryCount;
    
    private TournamentManager()
    {
        _entryCount = 0;
    }
    
    public static TournamentManager GetInstance()
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new TournamentManager();
                }
            }
        }
        return _instance;
    }
    
    public string AddEntry(string username, Entry entry)
    {
        _entryCount++;
        Console.WriteLine($"Adding entry for {username}: {entry.Name} {entry.Count} {entry.Duration}");
        
        //return count
        return _entryCount.ToString();
    }
    
    public string GetTournamentInfo()
    {
        return "Tournament info, entry count: " + _entryCount;
    }
    
    
    
    
}
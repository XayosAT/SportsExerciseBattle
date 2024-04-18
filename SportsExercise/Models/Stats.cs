namespace SportsExercise.Models;

public class Stats
{
    public int? Elo { get; set; }
    public int? Pushups { get; set; }
    
    public string? Name { get; set; }
    
    public Stats( string? name, int? elo, int? pushups)
    {
        Name = name;
        Elo = elo;
        Pushups = pushups;
        
    }
}
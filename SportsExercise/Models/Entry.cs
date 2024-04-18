namespace SportsExercise.Models;

public class Entry
{
    public string? Name { get; set; }
    public int? Count { get; set; }
    public int? Duration { get; set; }
    
    public Entry(string? name, int? count, int? duration)
    {
        Name = name;
        Count = count;
        Duration = duration;
    }
}
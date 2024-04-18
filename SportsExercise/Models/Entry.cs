namespace SportsExercise.Models;

public class Entry
{
    public string? Name { get; set; }
    public int? Count { get; set; }
    public int? DurationInSeconds { get; set; }
    
    public Entry(string? name, int? count, int? duration)
    {
        Name = name;
        Count = count;
        DurationInSeconds = duration;
    }
}
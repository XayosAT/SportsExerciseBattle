namespace SportsExercise.Models;

public class Record
{
    public int Count { get; set; }
    public int Duration { get; set; }
    
    public DateTime Date { get; set; }
    
    public Record(int count, int duration, DateTime date)
    {
        this.Count = count;
        this.Duration = duration;
        this.Date = date;
    }
}
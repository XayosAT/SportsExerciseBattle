namespace SportsExercise.Models;

public class Record
{
    public int count { get; set; }
    public int duration { get; set; }
    
    public DateTime date { get; set; }
    
    public Record(int count, int duration, DateTime date)
    {
        this.count = count;
        this.duration = duration;
        this.date = date;
    }
}
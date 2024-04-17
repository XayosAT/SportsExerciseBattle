namespace SportsExercise.Models;

public class Profile
{
    public string? Name { get; set; }
    public string? Bio { get; set; }
    public string? Image { get; set; }
    
    public Profile(string? name, string? bio, string? image)
    {
        Name = name;
        Bio = bio;
        Image = image;
    }
}
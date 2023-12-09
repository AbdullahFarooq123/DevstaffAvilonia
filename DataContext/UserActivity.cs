using System.ComponentModel.DataAnnotations;

namespace DataContext;

public class UserActivity
{
    [Key] public int Id { get; init; }
    public required TimeSpan TimeSpent { get; set; }
    public required TimeSpan IdolTime { get; set; }
    public required int Clicks { get; set; }
    public required int KeyPresses { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace DataContext;

public class UserActivity
{
	[Key] public int Id { get; set; }

	public TimeSpan TimeSpent { get; set; }
	public TimeSpan IdolTime { get; set; }
	public int Clicks { get; set; }
	public int KeyPresses { get; set; }
}
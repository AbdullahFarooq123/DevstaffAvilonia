using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataContext;

public class Project
{
	[Key] public int Id { get; set; }
	[ForeignKey(nameof(UserActivityId))] public UserActivity? UserActivity { get; set; }
	[ForeignKey(nameof(UserId))] public User User { get; set; }
	public string Name { get; set; }
	public int? UserActivityId { get; set; }
	public int UserId { get; set; }
}
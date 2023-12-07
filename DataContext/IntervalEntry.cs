using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataContext;

public class IntervalEntry
{
	[Key] public int Id { get; set; }
	[ForeignKey(nameof(ProjectId))] public Project Project { get; set; }
	[ForeignKey(nameof(UserActivityId))] public UserActivity UserActivity { get; set; }

	public int ProjectId { get; set; }
	public DateTime CreatedAt { get; set; }
	public int UserActivityId { get; set; }
	public IEnumerable<Screenshot> Screenshots { get; set; }
	public bool IsInSync { get; set; }
}
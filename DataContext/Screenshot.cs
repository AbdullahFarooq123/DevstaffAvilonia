using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataContext;

public class Screenshot
{
	[Key] public int Id { get; set; }
	[ForeignKey(nameof(ProjectId))] public Project Project { get; set; }
	[ForeignKey(nameof(ActivityEntryId))] public IntervalEntry ActivityEntry { get; set; }

	public string Name { get; set; }
	public DateTime CreatedAt { get; set; }
	public string Content { get; set; }
	public int ProjectId { get; set; }
	public int ActivityEntryId { get; set; }
	public bool IsInSync { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataContext;

public class Screenshot
{
    [Key] public int Id { get; init; }
    public required int ProjectId { get; init; }
    [ForeignKey(nameof(ProjectId))] public Project Project { get; init; } = null!;
    public int ActivityEntryId { get; set; }
    [ForeignKey(nameof(ActivityEntryId))] public IntervalEntry ActivityEntry { get; init; } = null!;
    [MaxLength(100)] public required string Name { get; init; }
    [MaxLength(int.MaxValue)] public required string Content { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required bool IsInSync { get; init; }
}
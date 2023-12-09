using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataContext;

public class IntervalEntry
{
    [Key] public int Id { get; init; }
    [ForeignKey(nameof(ProjectId))] public Project Project { get; init; } = null!;
    [ForeignKey(nameof(UserActivityId))] public UserActivity UserActivity { get; init; } = null!;
    public required int ProjectId { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required int UserActivityId { get; init; }
    public IEnumerable<Screenshot> Screenshots { get; init; } = null!;
    public bool IsInSync { get; init; }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataContext;

public class Project
{
    [Key] public int Id { get; init; }
    public int? UserActivityId { get; init; }
    [ForeignKey(nameof(UserActivityId))] public UserActivity? UserActivity { get; init; }
    public required int UserId { get; init; }
    [ForeignKey(nameof(UserId))] public User User { get; init; } = null!;
    [MaxLength(100)] public required string Name { get; init; }
}
using System.ComponentModel.DataAnnotations;

namespace DataContext;

public class User
{
    [Key] public int Id { get; init; }
    [MaxLength(100)] public required string OrganizationName { get; init; }
    public IEnumerable<Project> Projects { get; init; } = null!;
}
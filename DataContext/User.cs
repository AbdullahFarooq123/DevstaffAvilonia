using System.ComponentModel.DataAnnotations;

namespace DataContext;

public class User
{
	[Key] public int Id { get; set; }

	public string OrganizationName { get; set; }
	public IEnumerable<Project> Projects { get; set; }
}
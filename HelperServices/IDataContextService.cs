using DataContext;

namespace HelperServices;

public interface IDataContextService
{
	public Task<User?> LoadUser(int? UserId);
	public Task<List<Project>?> LoadProjects(int? UserId);
	public Task<DateTime> LoadLastUpdatedEntry();
	public Task UpdateUserActivity(UserActivity UserActivity);
	public Task<IntervalEntry?> InsertIntervalActivity(int? ActivityId, int? ProjectId);
	public Task InsertScreenshots(int? IntervalEntryId, IEnumerable<Screenshot> Screenshots);
	public Task<UserActivity?> InsertActivity(UserActivity UserActivity);
	public Task<User?> InsertUser(User user);
	public Task<Project?> InsertProject(Project project);
}

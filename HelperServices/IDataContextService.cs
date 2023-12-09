using DataContext;

namespace HelperServices;

public interface IDataContextService
{
    public Task<User?> LoadUser(int? userId);
    public Task<List<Project>?> LoadProjects(int? userId);
    public Task<DateTime> LoadLastUpdatedEntry();
    public Task UpdateUserActivity(UserActivity userActivity);
    public Task<IntervalEntry?> InsertIntervalActivity(int? activityId, int? projectId);
    public Task InsertScreenshots(int? intervalEntryId, IEnumerable<Screenshot> screenshots);
    public Task<UserActivity?> InsertActivity(UserActivity userActivity);
    public Task<User?> InsertUser(User user);
    public Task<Project?> InsertProject(Project project);
}
using DataContext;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using GlobalExtensionMethods;

namespace HelperServices;

public class DataContextServices : IDataContextService
{
    #region ServicesObj

    private readonly IUserService _userService;
    private readonly IProjectService _projectService;
    private readonly IScreenshotService _screenshotService;
    private readonly IUserActivityService _userActivityService;
    private readonly IIntervalEntryService _intervalEntryService;

    #endregion ServicesObj

    #region Ctor

    public DataContextServices(
        IUserService userService,
        IProjectService projectService,
        IScreenshotService screenshotService,
        IUserActivityService userActivityService,
        IIntervalEntryService intervalEntryService)
    {
        _userService = userService;
        _projectService = projectService;
        _screenshotService = screenshotService;
        _userActivityService = userActivityService;
        _intervalEntryService = intervalEntryService;
    }

    #endregion Ctor

    #region LoadData

    public async Task<User?> LoadUser(int? userId)
    {
        if (!userId.HasValue) return null;
        var user = await _userService.GetByIdAsync(id: userId.Value);
        return user.HasNoValue() ? null : user;
    }

    public async Task<List<Project>?> LoadProjects(int? userId)
    {
        if (userId.HasValue)
            return await _projectService.GetByUserId(userId: userId.Value).Include(project => project.UserActivity)
                .ToListAsync();
        return null;
    }

    public async Task<DateTime> LoadLastUpdatedEntry()
    {
        var lastUpdatedEntry =
            await _intervalEntryService.GetAll().OrderByDescending(entry => entry.Id).FirstOrDefaultAsync();
        return lastUpdatedEntry?.CreatedAt.ToLocalTime() ?? DateTime.MinValue;
    }

    #endregion LoadData

    #region UpdateData

    public async Task UpdateUserActivity(UserActivity userActivity)
    {
        if (userActivity.HasValue())
        {
            var userActivityEntity = await _userActivityService.GetByIdAsync(id: userActivity.Id);
            if (userActivityEntity.HasValue())
            {
                userActivityEntity.Value().TimeSpent = userActivity.TimeSpent;
                userActivityEntity.Value().IdolTime = userActivity.IdolTime;
                userActivityEntity.Value().Clicks = userActivity.Clicks;
                userActivityEntity.Value().KeyPresses = userActivity.KeyPresses;
                await _userActivityService.UpdateAsync(entity: userActivityEntity.Value());
            }
        }
    }

    #endregion UpdateData

    #region InsertData

    public async Task<IntervalEntry?> InsertIntervalActivity(int? activityId, int? projectId)
    {
        if (!activityId.HasValue || !projectId.HasValue) return null;
        var userIntervalEntity = new IntervalEntry
        {
            ProjectId = projectId.Value,
            UserActivityId = activityId.Value,
            CreatedAt = DateTime.UtcNow
        };
        await _intervalEntryService.InsertAsync(entity: userIntervalEntity);
        return userIntervalEntity;
    }

    public async Task InsertScreenshots(int? intervalEntryId, IEnumerable<Screenshot>? screenshots)
    {
        var screenshotList = (screenshots ?? Array.Empty<Screenshot>()).ToList();
        if (intervalEntryId.HasValue && screenshotList.HasValue() && screenshotList.Any())
        {
            screenshotList.ToList().ForEach(obj => obj.ActivityEntryId = intervalEntryId.Value);
            await _screenshotService.InsertManyAsync(entities: screenshotList);
        }
    }

    public async Task<UserActivity?> InsertActivity(UserActivity userActivity)
    {
        if (!userActivity.HasValue()) return null;
        await _userActivityService.InsertAsync(entity: userActivity);
        return userActivity;
    }

    public async Task<User?> InsertUser(User user)
    {
        if (!user.HasValue()) return null;
        await _userService.InsertAsync(user);
        return user;
    }

    public async Task<Project?> InsertProject(Project project)
    {
        if (!project.HasValue()) return null;
        await _projectService.InsertAsync(entity: project);
        return project;
    }

    #endregion InsertData
}
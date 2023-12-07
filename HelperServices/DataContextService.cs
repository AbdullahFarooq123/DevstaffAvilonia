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
	public async Task<User?> LoadUser(int? UserId)
	{
		if (!UserId.HasValue) return null;
		var user = await _userService.GetByIdAsync(UserId.Value);
		if (user.HasNoValue()) return null;
		return user;
	}
	public async Task<List<Project>?> LoadProjects(int? UserId)
	{
		if (UserId.HasValue)
			return await _projectService.GetByUserId(UserId.Value).Include(project => project.UserActivity).ToListAsync();
		return null;
	}
	public async Task<DateTime> LoadLastUpdatedEntry()
	{
		var lastUpdatedEntry = await _intervalEntryService.GetAll().OrderByDescending(entry => entry.Id).FirstOrDefaultAsync();
		return lastUpdatedEntry?.CreatedAt.ToLocalTime() ?? DateTime.MinValue;
	}
	#endregion LoadData

	#region UpdateData
	public async Task UpdateUserActivity(UserActivity UserActivity)
	{
		if (UserActivity.HasValue())
		{
			var userActivityEntity = await _userActivityService.GetByIdAsync(UserActivity.Id);
			if (userActivityEntity.HasValue())
			{
				userActivityEntity.Value().TimeSpent = UserActivity.TimeSpent;
				userActivityEntity.Value().IdolTime = UserActivity.IdolTime;
				userActivityEntity.Value().Clicks = UserActivity.Clicks;
				userActivityEntity.Value().KeyPresses = UserActivity.KeyPresses;
				await _userActivityService.UpdateAsync(userActivityEntity.Value());
			}
		}
	}
	#endregion UpdateData

	#region InsertData
	public async Task<IntervalEntry?> InsertIntervalActivity(int? ActivityId, int? ProjectId)
	{
		if (ActivityId.HasValue && ProjectId.HasValue)
		{
			var userIntervalEntity = new IntervalEntry
			{
				ProjectId = ProjectId.Value,
				UserActivityId = ActivityId.Value,
				CreatedAt = DateTime.UtcNow
			};
			await _intervalEntryService.InsertAsync(userIntervalEntity);
			return userIntervalEntity;
		}
		return null;
	}
	public async Task InsertScreenshots(int? IntervalEntryId, IEnumerable<Screenshot> Screenshots)
	{
		if (IntervalEntryId.HasValue && Screenshots.HasValue() && Screenshots.Count() > 0)
		{
			Screenshots.ToList().ForEach(obj => obj.ActivityEntryId = IntervalEntryId.Value);
			await _screenshotService.InsertManyAsync(Screenshots);
		}
	}
	public async Task<UserActivity?> InsertActivity(UserActivity UserActivity)
	{
		if (UserActivity.HasValue())
		{
			await _userActivityService.InsertAsync(UserActivity);
			return UserActivity;
		}
		return null;
	}
	public async Task<User?> InsertUser(User user)
	{
		if (user.HasValue())
		{
			await _userService.InsertAsync(user);
			return user;
		}
		return null;
	}
	public async Task<Project?> InsertProject(Project project)
	{
		if (project.HasValue())
		{
			await _projectService.InsertAsync(project);
			return project;
		}
		return null;
	}
	#endregion InsertData
}
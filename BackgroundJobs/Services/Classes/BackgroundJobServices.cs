using BackgroundJobs.ActivityListeners.Interfaces;
using WinApi.Utilities;
using DataModel;
using BackgroundJobs.Services.Interfaces;

namespace BackgroundJobs.Services.Classes;

public class BackgroundJobService : IBackgroundJobService
{
	#region JobObj
	private readonly ITimerUtilities _projectActivityTimeJob;
	private readonly ITimerUtilities _projectIdleTimeJob;
	private readonly ITimerUtilities _localDataSyncJob;
	private readonly IScreenshotListener _screenShotListener;
	private readonly IMouseListener _mouseListener;
	private readonly IKeyboardListener _keyboardListener;
	private readonly AppSettings _appSettings;
	#endregion JobObj

	#region CallbackObj
	private TimerCallback? _activityTimeCallback;
	private TimerCallback? _idleTimeCallback;
	private TimerCallback? _dataSyncCallback;
	private Delegates.ParameterizedHookCallback<string>? _screenshotsActivityCallback;
	private Delegates.HookCallback? _mouseActivityCallback;
	private Delegates.HookCallback? _keyboardActivityCallback;
	#endregion CallbackObj
	private bool _servicesRegistered = false;

	#region Ctor
	public BackgroundJobService(
		ITimerUtilities projectActivityTimeJob,
		ITimerUtilities projectIdleTimeJob,
		ITimerUtilities localDataSyncJob,
		IScreenshotListener screenShotListener,
		IMouseListener mouseListener,
		IKeyboardListener keyboardListener,
		AppSettings appSettings)
	{
		_projectActivityTimeJob = projectActivityTimeJob;
		_projectIdleTimeJob = projectIdleTimeJob;
		_localDataSyncJob = localDataSyncJob;
		_screenShotListener = screenShotListener;
		_mouseListener = mouseListener;
		_keyboardListener = keyboardListener;
		_appSettings = appSettings;
	}
	#endregion Ctor

	#region JobResources
	public void RegisterCallbacks(
		TimerCallback activityTimeCallback,
		TimerCallback idleTimeCallback,
		TimerCallback dataSyncCallback,
		Delegates.ParameterizedHookCallback<string> screenshotsActivityCallback,
		Delegates.HookCallback mouseActivityCallback,
		Delegates.HookCallback keyboardActivityCallback)
	{
		_activityTimeCallback = activityTimeCallback;
		_idleTimeCallback = idleTimeCallback;
		_dataSyncCallback = dataSyncCallback;
		_screenshotsActivityCallback = screenshotsActivityCallback;
		_mouseActivityCallback = mouseActivityCallback;
		_keyboardActivityCallback = keyboardActivityCallback;
		_servicesRegistered = true;
	}
	public void HookJobs()
	{
		if (!_servicesRegistered) throw new InvalidOperationException("The callbacks have not been registered.\nUse RegisterCallbacks to register callbacks before hooking to jobs.");
		_projectActivityTimeJob.HookJob(TimerUtilities.SecondsToMillis(1), TimerUtilities.SecondsToMillis(1), _activityTimeCallback);
		_projectIdleTimeJob.HookJob(TimerUtilities.MinutesToMillis(_appSettings.AllowedIdleTime_Mins), TimerUtilities.MinutesToMillis(_appSettings.AllowedIdleTime_Mins), _idleTimeCallback);
		_localDataSyncJob.HookJob(TimerUtilities.MinutesToMillis(11), TimerUtilities.MinutesToMillis(11), _dataSyncCallback);
		_screenShotListener.HookJob(_screenshotsActivityCallback);
		_mouseListener.HookJob(_mouseActivityCallback);
		_keyboardListener.HookJob(_keyboardActivityCallback);
	}
	public void UnHookJobs()
	{
		_projectActivityTimeJob.UnHookJob();
		_projectIdleTimeJob.UnHookJob();
		_localDataSyncJob.UnHookJob();
		_screenShotListener.UnHookJob();
		_mouseListener.UnHookJob();
		_keyboardListener.UnHookJob();
	}
	#endregion JobResources

	#region Helpers
	public void ResetIdleTimeJobInterval() => ChangeTimerIntervalSec(_projectIdleTimeJob, 1);
	public int GetIdleTimeInterval() => _projectIdleTimeJob.GetCurrentTimerInterval();
	private void ChangeTimerIntervalSec(ITimerUtilities _TimerUtilities, int Seconds) => _TimerUtilities.ChangeTimerInterval(TimerUtilities.SecondsToMillis(Seconds));
	#endregion Helpers
}

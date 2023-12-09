using BackgroundJobs.ActivityListeners.Interfaces;
using WinApi.Utilities;
using BackgroundJobs.Services.Interfaces;
using DataModels;

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

    private bool _servicesRegistered;

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
        _activityTimeCallback = activityTimeCallback ?? throw new ArgumentNullException(nameof(activityTimeCallback));
        _idleTimeCallback = idleTimeCallback ?? throw new ArgumentNullException(nameof(idleTimeCallback));
        _dataSyncCallback = dataSyncCallback ?? throw new ArgumentNullException(nameof(dataSyncCallback));
        _screenshotsActivityCallback = screenshotsActivityCallback ??
                                       throw new ArgumentNullException(nameof(screenshotsActivityCallback));
        _mouseActivityCallback =
            mouseActivityCallback ?? throw new ArgumentNullException(nameof(mouseActivityCallback));
        _keyboardActivityCallback = keyboardActivityCallback ??
                                    throw new ArgumentNullException(nameof(keyboardActivityCallback));
        _servicesRegistered = true;
    }

    public void HookJobs()
    {
        if (!_servicesRegistered)
            throw new InvalidOperationException(
                "The callbacks have not been registered.\nUse RegisterCallbacks to register callbacks before hooking to jobs.");
        _projectActivityTimeJob.HookJob(TimerUtilities.SecondsToMillis(1), TimerUtilities.SecondsToMillis(1),
            _activityTimeCallback);
        _projectIdleTimeJob.HookJob(TimerUtilities.MinutesToMillis(_appSettings.AllowedIdleTimeMin),
            TimerUtilities.MinutesToMillis(_appSettings.AllowedIdleTimeMin), _idleTimeCallback);
        _localDataSyncJob.HookJob(TimerUtilities.MinutesToMillis(11), TimerUtilities.MinutesToMillis(11),
            _dataSyncCallback);
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

    private static void ChangeTimerIntervalSec(ITimerUtilities timerUtilities, int seconds) =>
        timerUtilities.ChangeTimerInterval(TimerUtilities.SecondsToMillis(seconds));

    #endregion Helpers
}
using BackgroundJobs.ActivityListeners.Interfaces;
using BackgroundJobs.Services.Interfaces;
using DataModels;
using static BackgroundJobs.Services.Classes.TimerUtilities;

namespace BackgroundJobs.Services.Classes;

public class BackgroundJobService : IBackgroundJobService
{
    #region JobObj

    private readonly ITimerUtilities _activityTimeJob;
    private readonly ITimerUtilities _idleTimeJob;
    private readonly ITimerUtilities _localDataSyncJob;
    private readonly IScreenshotListener _screenShotListener;
    private readonly IMouseListener _mouseListener;
    private readonly IKeyboardListener _keyboardListener;
    private readonly AppSettings _appSettings;

    #endregion JobObj

    #region CallbackObj

    private TimerCallback _activityTimeCallback = null!;
    private TimerCallback _idleTimeCallback = null!;
    private TimerCallback _dataSyncCallback = null!;
    private EventHandler<string> _screenshotsCallback = null!;
    private EventHandler _mouseActCallback = null!;
    private EventHandler _keyboardActCallback = null!;

    #endregion CallbackObj

    #region Private Members

    private bool _servicesRegistered;

    private const string ServiceNotRegisteredError = "The callbacks have not been registered.\n" +
                                                     "Use RegisterCallbacks to register callbacks before hooking to jobs.";

    #endregion Private Members

    #region Ctor

    public BackgroundJobService(
        ITimerUtilities activityTimeJob,
        ITimerUtilities idleTimeJob,
        ITimerUtilities localDataSyncJob,
        IScreenshotListener screenShotListener,
        IMouseListener mouseListener,
        IKeyboardListener keyboardListener,
        AppSettings appSettings)
    {
        _activityTimeJob = activityTimeJob;
        _idleTimeJob = idleTimeJob;
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
        EventHandler<string> screenshotsCallback,
        EventHandler mouseActCallback,
        EventHandler keyboardActCallback)
    {
        _activityTimeCallback = activityTimeCallback ?? throw new ArgumentNullException(nameof(activityTimeCallback));
        _idleTimeCallback = idleTimeCallback ?? throw new ArgumentNullException(nameof(idleTimeCallback));
        _dataSyncCallback = dataSyncCallback ?? throw new ArgumentNullException(nameof(dataSyncCallback));
        _screenshotsCallback = screenshotsCallback ?? throw new ArgumentNullException(nameof(screenshotsCallback));
        _mouseActCallback = mouseActCallback ?? throw new ArgumentNullException(nameof(mouseActCallback));
        _keyboardActCallback = keyboardActCallback ?? throw new ArgumentNullException(nameof(keyboardActCallback));
        _servicesRegistered = true;
    }

    public void HookJobs()
    {
        if (!_servicesRegistered) throw new InvalidOperationException(message: ServiceNotRegisteredError);
        _activityTimeJob.HookJob(intervalMs: SecToMs(sec: 1), callback: _activityTimeCallback);
        _idleTimeJob.HookJob(intervalMs: MinToMs(min: _appSettings.AllowedIdleTimeMin), callback: _idleTimeCallback);
        _localDataSyncJob.HookJob(MinToMs(min: _appSettings.SyncIntervalMin), callback: _dataSyncCallback);
        _screenShotListener.HookJob(callback: _screenshotsCallback);
        _mouseListener.HookJob(callback: _mouseActCallback);
        _keyboardListener.HookJob(callback: _keyboardActCallback);
    }

    public void UnHookJobs()
    {
        _activityTimeJob.UnHookJob();
        _idleTimeJob.UnHookJob();
        _localDataSyncJob.UnHookJob();
        _screenShotListener.UnHookJob();
        _mouseListener.UnHookJob();
        _keyboardListener.UnHookJob();
    }

    #endregion JobResources

    #region Helpers

    public void ResetIdleTimeJobInterval() => ChangeTimerIntervalSec(_idleTimeJob, 1);
    public int GetIdleTimeInterval() => _idleTimeJob.GetCurrentTimerInterval();

    private static void ChangeTimerIntervalSec(ITimerUtilities timerUtilities, int seconds) =>
        timerUtilities.ChangeTimerInterval(intervalMs: SecToMs(sec: seconds));

    #endregion Helpers
}
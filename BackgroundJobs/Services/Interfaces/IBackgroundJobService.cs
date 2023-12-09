using WinApi.Utilities;

namespace BackgroundJobs.Services.Interfaces;

public interface IBackgroundJobService
{
    public void RegisterCallbacks(
        TimerCallback activityTimeCallback,
        TimerCallback idleTimeCallback,
        TimerCallback dataSyncCallback,
        Delegates.ParameterizedHookCallback<string> screenshotsActivityCallback,
        Delegates.HookCallback mouseActivityCallback,
        Delegates.HookCallback keyboardActivityCallback
    );

    public void HookJobs();
    public void UnHookJobs();
    public int GetIdleTimeInterval();
    public void ResetIdleTimeJobInterval();
}
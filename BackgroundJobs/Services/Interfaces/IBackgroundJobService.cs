using WinApi.Utilities;

namespace BackgroundJobs.Services.Interfaces;

public interface IBackgroundJobService
{
    public void RegisterCallbacks(
        TimerCallback activityTimeCallback,
        TimerCallback idleTimeCallback,
        TimerCallback dataSyncCallback,
        EventHandler<string> screenshotsCallback,
        EventHandler mouseActCallback,
        EventHandler keyboardActCallback
    );

    public void HookJobs();
    public void UnHookJobs();
    public int GetIdleTimeInterval();
    public void ResetIdleTimeJobInterval();
}
using WinApi.Utilities;

namespace BackgroundJobs.Services.Interfaces;

public interface IBackgroundJobService
{
    public void RegisterCallbacks(
        TimerCallback activityTimeCallback,
        TimerCallback idleTimeCallback,
        TimerCallback dataSyncCallback,
        EventHandler<string> screenshotsActivityCallback,
        EventHandler mouseActivityCallback,
        EventHandler keyboardActivityCallback
    );

    public void HookJobs();
    public void UnHookJobs();
    public int GetIdleTimeInterval();
    public void ResetIdleTimeJobInterval();
}
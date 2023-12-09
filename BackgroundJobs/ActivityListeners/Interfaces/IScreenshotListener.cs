using WinApi.Utilities;

namespace BackgroundJobs.ActivityListeners.Interfaces;

public interface IScreenshotListener
{
    public void HookJob(Delegates.ParameterizedHookCallback<string>? callback);
    public void UnHookJob();
}
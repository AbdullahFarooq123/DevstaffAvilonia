using WinApi.Utilities;

namespace BackgroundJobs.ActivityListeners.Interfaces;

public interface IHookListener
{
    public void HookJob(Delegates.HookCallback? callback);
    public void UnHookJob();
}
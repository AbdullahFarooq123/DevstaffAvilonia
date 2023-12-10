using WinApi.Utilities;

namespace BackgroundJobs.ActivityListeners.Interfaces;

public interface IHookListener
{
    public void HookJob(EventHandler? callback);
    public void UnHookJob();
}
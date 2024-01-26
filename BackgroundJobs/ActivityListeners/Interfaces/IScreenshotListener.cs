namespace BackgroundJobs.ActivityListeners.Interfaces;

public interface IScreenshotListener
{
    public void HookJob(EventHandler<string> callback);
    public void UnHookJob();
}
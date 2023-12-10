namespace BackgroundJobs.Services.Interfaces;

public interface ITimerUtilities
{
    public void HookJob(int intervalMs, TimerCallback callback, int delayMs = 0);
    public void UnHookJob();
    public void ChangeTimerInterval(int intervalMs);
    public int GetCurrentTimerInterval();
}
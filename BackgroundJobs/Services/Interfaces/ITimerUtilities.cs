namespace BackgroundJobs.Services.Interfaces;

public interface ITimerUtilities
{
    public void HookJob(int DelayInMillis, int DueTimeInMillis, TimerCallback? timerCallback);
    public void UnHookJob();
    public void ChangeTimerInterval(int DelayInMillis);
    public int GetCurrentTimerInterval();
}
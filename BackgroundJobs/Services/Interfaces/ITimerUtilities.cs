namespace BackgroundJobs.Services.Interfaces;

public interface ITimerUtilities
{
    public void HookJob(int delayInMillis, int dueTimeInMillis, TimerCallback? timerCallback);
    public void UnHookJob();
    public void ChangeTimerInterval(int delayInMillis);
    public int GetCurrentTimerInterval();
}
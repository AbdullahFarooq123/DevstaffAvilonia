using BackgroundJobs.Services.Interfaces;
using GlobalExtensionMethods;

namespace BackgroundJobs.Services.Classes;

public class TimerUtilities : ITimerUtilities
{
    private Timer? _timer;
    private int _delayInMillis;

    public void HookJob(int delayInMillis, int dueTimeInMillis, TimerCallback? timerCallback) =>
        _timer = new Timer(
            timerCallback ?? throw new InvalidOperationException("Callback is null"),
            null,
            dueTimeInMillis,
            delayInMillis);

    public async void UnHookJob()
    {
        if (_timer.HasValue())
            await _timer.Value().DisposeAsync();
        _timer = null;
    }

    public void ChangeTimerInterval(int delayInMillis) =>
        _timer?.Change(Timeout.Infinite, _delayInMillis = delayInMillis);

    public int GetCurrentTimerInterval() =>
        _delayInMillis;

    public static int MinutesToMillis(int minutes) =>
        minutes * 60000;

    public static int SecondsToMillis(int seconds) =>
        seconds * 1000;

    public static int MinutesToSeconds(int minutes) =>
        minutes * 60;
}
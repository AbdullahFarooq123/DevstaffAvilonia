using BackgroundJobs.Services.Interfaces;
using GlobalExtensionMethods;

namespace BackgroundJobs.Services.Classes;

public class TimerUtilities : ITimerUtilities
{
    private Timer? _timer;
    private int _intervalMs;

    public void HookJob(int intervalMs, TimerCallback callback, int delayMs) =>
        _timer = new Timer(callback: callback, period: intervalMs, state: null, dueTime: delayMs);

    public async void UnHookJob()
    {
        if (_timer.HasValue()) await _timer.Value().DisposeAsync();
        _timer = null;
    }

    public void ChangeTimerInterval(int intervalMs) =>
        _timer?.Change(dueTime: Timeout.Infinite, period: _intervalMs = intervalMs);

    public int GetCurrentTimerInterval() => _intervalMs;
    public static int MinToMs(int min) => min * 60000;
    public static int MinToSec(int min) => min * 60;
    public static int SecToMs(int sec) => sec * 1000;
}
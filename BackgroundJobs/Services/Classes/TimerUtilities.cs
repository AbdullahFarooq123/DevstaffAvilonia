using BackgroundJobs.Services.Interfaces;
using GlobalExtensionMethods;

namespace BackgroundJobs.Services.Classes;

public class TimerUtilities : ITimerUtilities
{
	private Timer? _timer;
	private int DelayInMillis;

	public void HookJob(int DelayInMillis, int DueTimeInMillis, TimerCallback? TimerCallback) =>
		_timer = new Timer(
			TimerCallback ?? throw new InvalidOperationException("Callback is null"),
			null,
			DueTimeInMillis,
			DelayInMillis);
	public async void UnHookJob()
	{
		if (_timer.HasValue())
			await _timer.Value().DisposeAsync();
		_timer = null;
	}
	public void ChangeTimerInterval(int DelayInMillis) =>
		_timer?.Change(Timeout.Infinite, this.DelayInMillis = DelayInMillis);
	public int GetCurrentTimerInterval() =>
		DelayInMillis;
	public static int MinutesToMillis(int Minutes) =>
		Minutes * 60000;
	public static int SecondsToMillis(int Seconds) =>
		Seconds * 1000;
	public static int MinutesToSeconds(int Minutes) =>
		Minutes * 60;
}
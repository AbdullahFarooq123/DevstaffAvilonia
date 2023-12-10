using BackgroundJobs.ActivityListeners.Interfaces;
using BackgroundJobs.Services.Classes;
using BackgroundJobs.Services.Interfaces;
using XApi.Interfaces;
using XApi.Utilities;

namespace BackgroundJobs.ActivityListeners.Classes;

public class XMouseListener : IMouseListener
{
    private readonly IInputDeviceApi _inputDeviceApi;
    private readonly ITimerUtilities _timerUtilities;
    private readonly List<string> _validEventArgs = new() { "motion", "button press" };

    public XMouseListener(IInputDeviceApi inputDeviceApi, ITimerUtilities timerUtilities)
    {
        _inputDeviceApi = inputDeviceApi;
        _timerUtilities = timerUtilities;
    }

    public void HookJob(EventHandler? callback)
    {
        _inputDeviceApi.SetParams(_validEventArgs, callback, InputType.Mouse);
        _timerUtilities.HookJob(
            TimerUtilities.SecondsToMillis(1),
            0,
            _ => _inputDeviceApi.ListenInputDevice()
        );
    }

    public void UnHookJob()
    {
        _timerUtilities.UnHookJob();
        _inputDeviceApi.KillProcesses();
    }
}
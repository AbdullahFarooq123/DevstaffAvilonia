using DataModels;
using XApi.Interfaces;
using BackgroundJobs.Services.Interfaces;
using BackgroundJobs.ActivityListeners.Interfaces;
using static XApi.Utilities.InputType;
using static BackgroundJobs.Services.Classes.TimerUtilities;

namespace BackgroundJobs.ActivityListeners.Classes;

public class XMouseListener : IMouseListener
{
    private readonly AppSettings _appSettings;
    private readonly IInputDeviceApi _inputDeviceApi;
    private readonly ITimerUtilities _timerUtilities;

    private readonly List<string> _validEventArgs = new() { "motion", "button press" };

    public XMouseListener(IInputDeviceApi inputDeviceApi, ITimerUtilities timerUtilities, AppSettings appSettings)
    {
        _inputDeviceApi = inputDeviceApi ?? throw new ArgumentNullException(nameof(inputDeviceApi));
        _timerUtilities = timerUtilities ?? throw new ArgumentNullException(nameof(timerUtilities));
        _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
    }

    public void HookJob(EventHandler callback)
    {
        _inputDeviceApi.SetParams(validEventArgs: _validEventArgs, callback: callback, inputType: Mouse);
        _timerUtilities.HookJob(
            intervalMs: SecToMs(sec: _appSettings.LinuxInputRefreshSec),
            callback: _ => _inputDeviceApi.ListenInputDevice()
        );
    }

    public void UnHookJob()
    {
        _timerUtilities.UnHookJob();
        _inputDeviceApi.KillProcesses();
    }
}
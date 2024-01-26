using BackgroundJobs.ActivityListeners.Interfaces;
using BackgroundJobs.Services.Interfaces;
using DataModels;
using XApi.Interfaces;
using static BackgroundJobs.Services.Classes.TimerUtilities;
using static XApi.Utilities.InputType;

namespace BackgroundJobs.ActivityListeners.Classes;

public class XKeyboardListener : IKeyboardListener
{
    private readonly IInputDeviceApi _inputDeviceApi;
    private readonly ITimerUtilities _timerUtilities;
    private readonly AppSettings _appSettings;

    private readonly List<string> _validEventArgs = new() { "key press" };

    public XKeyboardListener(IInputDeviceApi inputDeviceApi, ITimerUtilities timerUtilities, AppSettings appSettings)
    {
        _inputDeviceApi = inputDeviceApi ?? throw new ArgumentNullException(nameof(inputDeviceApi));
        _timerUtilities = timerUtilities ?? throw new ArgumentNullException(nameof(timerUtilities));
        _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
    }

    public void HookJob(EventHandler callback)
    {
        _inputDeviceApi.SetParams(validEventArgs: _validEventArgs, callback: callback, inputType: Keyboard);
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
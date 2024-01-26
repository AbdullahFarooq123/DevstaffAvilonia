using BackgroundJobs.ActivityListeners.Interfaces;
using BackgroundJobs.Services.Interfaces;
using DataModels;
using XApi.Interfaces;
using static BackgroundJobs.Services.Classes.TimerUtilities;

namespace BackgroundJobs.ActivityListeners.Classes;

public class XScreenshotListener : IScreenshotListener
{
    private readonly IXScreenshotApi _ixScreenshotApi;
    private readonly ITimerUtilities _timerUtilities;
    private readonly AppSettings _appSettings;

    public XScreenshotListener(IXScreenshotApi ixScreenshotApi, ITimerUtilities timerUtilities, AppSettings appSettings)
    {
        _ixScreenshotApi = ixScreenshotApi ?? throw new ArgumentNullException(nameof(ixScreenshotApi));
        _timerUtilities = timerUtilities ?? throw new ArgumentNullException(nameof(timerUtilities));
        _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
    }

    public void HookJob(EventHandler<string> callback) =>
        _timerUtilities.HookJob(
            intervalMs: MinToMs(min: _appSettings.ScreenshotIntervalMin),
            callback: _ => ScreenshotCallback(callback: callback, saveFile: true)
        );

    public void UnHookJob() =>
        _timerUtilities.UnHookJob();

    private void ScreenshotCallback(EventHandler<string> callback, bool saveFile = false)
    {
        var base64Image = _ixScreenshotApi.CaptureWindow(saveFile: saveFile);
        callback.Invoke(sender: null, e: base64Image);
    }
}
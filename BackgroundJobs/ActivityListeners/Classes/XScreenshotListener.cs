using BackgroundJobs.ActivityListeners.Interfaces;
using BackgroundJobs.Services.Classes;
using BackgroundJobs.Services.Interfaces;
using GlobalExtensionMethods;
using XApi.Interfaces;

namespace BackgroundJobs.ActivityListeners.Classes;

public class XScreenshotListener : IScreenshotListener
{
    private readonly IXScreenshotApi _ixScreenshotApi;
    private readonly ITimerUtilities _timerUtilities;
    private const int IntervalTimeMin = 1;

    public XScreenshotListener(IXScreenshotApi ixScreenshotApi, ITimerUtilities timerUtilities)
    {
        _ixScreenshotApi = ixScreenshotApi;
        _timerUtilities = timerUtilities;
    }

    public void HookJob(EventHandler<string>? callback)
    {
        _timerUtilities.HookJob(TimerUtilities.MinutesToMillis(IntervalTimeMin), 0, _ =>
            CreateAndCallbackScreenshot(callback, true));
    }

    public void UnHookJob()
    {
        _timerUtilities.UnHookJob();
    }

    private void CreateAndCallbackScreenshot(EventHandler<string>? callback, bool saveImage = false)
    {
        if (callback.HasNoValue()) throw new ArgumentNullException(nameof(callback));
        var base64Image = _ixScreenshotApi.CaptureWindow(saveImage);
        callback?.Invoke(null, base64Image);
    }
}
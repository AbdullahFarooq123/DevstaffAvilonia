using BackgroundJobs.ActivityListeners.Interfaces;
using BackgroundJobs.Services.Classes;
using BackgroundJobs.Services.Interfaces;
using GlobalExtensionMethods;
using System.Drawing;
using System.Drawing.Imaging;
using WinApi.Interfaces;
using WinApi.Utilities;

namespace BackgroundJobs.ActivityListeners.Classes;

public class WinScreenshotListener : IScreenshotListener
{
    private readonly ITimerUtilities _timerUtilities;
    private readonly IScreenshotApi _screenshotApi;
    private const int IntervalTimeMin = 1;

    public WinScreenshotListener(ITimerUtilities timerUtilities, IScreenshotApi screenshotApi)
    {
        _timerUtilities = timerUtilities ?? throw new ArgumentNullException(nameof(timerUtilities));
        _screenshotApi = screenshotApi ?? throw new ArgumentNullException(nameof(screenshotApi));
    }

    public void HookJob(EventHandler<string>? callback) =>
        _timerUtilities.HookJob(TimerUtilities.MinutesToMillis(IntervalTimeMin), 0, _ =>
            CreateAndCallbackScreenshot(callback, true));

    public void UnHookJob() =>
        _timerUtilities.UnHookJob();

    private void CreateAndCallbackScreenshot(EventHandler<string>? callback, bool saveImage = false)
    {
        if (callback.HasNoValue()) throw new ArgumentNullException(nameof(callback));
        var imgSource = _screenshotApi.CaptureWindow();
        var base64Image = ConvertImageToBase64(imgSource);
        callback?.Invoke(null, base64Image);
        if (saveImage)
#pragma warning disable CA1416
            imgSource.Save(
                $@"D:\Screenshots\{DateTime.UtcNow.ToString("dd:MM:yyyy hh:mm:ss tt").Replace(":", "_")}.png",
                ImageFormat.Png);
#pragma warning restore CA1416
    }

    private string ConvertImageToBase64(Image image)
    {
        using var memoryStream = new MemoryStream();
#pragma warning disable CA1416
        image.Save(memoryStream, ImageFormat.Png);
#pragma warning restore CA1416
        var imageBytes = memoryStream.ToArray();
        var base64String = Convert.ToBase64String(imageBytes);
        return base64String;
    }
}
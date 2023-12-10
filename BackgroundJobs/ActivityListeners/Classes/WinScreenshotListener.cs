using System.Diagnostics.CodeAnalysis;
using BackgroundJobs.ActivityListeners.Interfaces;
using BackgroundJobs.Services.Interfaces;
using GlobalExtensionMethods;
using System.Drawing;
using System.Drawing.Imaging;
using DataModels;
using WinApi.Interfaces;
using static BackgroundJobs.Services.Classes.TimerUtilities;

namespace BackgroundJobs.ActivityListeners.Classes;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public class WinScreenshotListener : IScreenshotListener
{
    private readonly ITimerUtilities _timerUtilities;
    private readonly IScreenshotApi _screenshotApi;
    private readonly AppSettings _settings;

    public WinScreenshotListener(ITimerUtilities timerUtilities, IScreenshotApi screenshotApi, AppSettings settings)
    {
        _timerUtilities = timerUtilities ?? throw new ArgumentNullException(nameof(timerUtilities));
        _screenshotApi = screenshotApi ?? throw new ArgumentNullException(nameof(screenshotApi));
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    public void HookJob(EventHandler<string> callback) =>
        _timerUtilities.HookJob(
            intervalMs: MinToMs(min: _settings.ScreenshotIntervalMin),
            callback: _ => Callback(callback: callback, saveImage: true)
        );

    public void UnHookJob() =>
        _timerUtilities.UnHookJob();

    private void Callback(EventHandler<string> callback, bool saveImage = false)
    {
        var imgSource = _screenshotApi.CaptureWindow();
        var base64Image = ConvertImageToBase64(image: imgSource);
        callback.Invoke(sender: null, e: base64Image);
        if (saveImage)
            imgSource.Save(filename: $"{DateTime.Now.ToFileName()}.png", format: ImageFormat.Png);
    }

    private string ConvertImageToBase64(Image image)
    {
        using var memoryStream = new MemoryStream();
        image.Save(memoryStream, ImageFormat.Png);
        var imageBytes = memoryStream.ToArray();
        var base64String = Convert.ToBase64String(imageBytes);
        return base64String;
    }
}
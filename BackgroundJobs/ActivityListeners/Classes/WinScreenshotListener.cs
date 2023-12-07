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
	private readonly int _intervalTimeMin = 1;

	public WinScreenshotListener(ITimerUtilities timerUtilities, IScreenshotApi screenshotApi)
	{
		_timerUtilities = timerUtilities ?? throw new ArgumentNullException(nameof(timerUtilities));
		_screenshotApi = screenshotApi ?? throw new ArgumentNullException(nameof(screenshotApi));
	}
	public void HookJob(Delegates.ParameterizedHookCallback<string>? screenshotCallback) =>
		_timerUtilities.HookJob(TimerUtilities.MinutesToMillis(_intervalTimeMin), 0, (object? state) =>
				CreateAndCallbackScreenshot(screenshotCallback, true));
	public void UnHookJob() =>
		_timerUtilities.UnHookJob();

	private void CreateAndCallbackScreenshot(Delegates.ParameterizedHookCallback<string>? screenshotCallback, bool saveImage = false)
	{
		if (screenshotCallback.HasNoValue()) throw new ArgumentNullException(nameof(screenshotCallback));
		var ImgSource = _screenshotApi.CaptureWindow();
		var base64Image = ConvertImageToBase64(ImgSource);
		screenshotCallback.Value()(base64Image);
		if (saveImage)
			ImgSource.Save($"D:\\Screenshots\\{DateTime.UtcNow.ToString("dd:MM:yyyy hh:mm:ss tt").Replace(":", "_")}.png", ImageFormat.Png);
	}
	private string ConvertImageToBase64(Image image)
	{
		using (var memoryStream = new MemoryStream())
		{
			image.Save(memoryStream, ImageFormat.Png);
			var imageBytes = memoryStream.ToArray();
			var base64String = Convert.ToBase64String(imageBytes);
			return base64String;
		}
	}
}
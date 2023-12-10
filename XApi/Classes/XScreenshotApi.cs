using XApi.Interfaces;
using XApi.Utilities;

namespace XApi.Classes;

public class XScreenshotApi : IXScreenshotApi
{
    private const string ScreenshotCommand = "scrot {filename}.png -z";

    public string CaptureWindow(bool saveFile)
    {
        var path = TakeScreenshot();
        var bytes = File.ReadAllBytes(path);
        var base64 = Convert.ToBase64String(bytes);
        if (!saveFile) File.Delete(path);
        return base64;
    }

    private string TakeScreenshot()
    {
        var screenshotName = DateTime.Now.ToString("o").Replace(":", "_").Replace(" ", "_");
        var screenshotCommandWithPath = ScreenshotCommand.Replace("{filename}", screenshotName);
        LinuxCmdUtil.AwaitedCommandExec(screenshotCommandWithPath);
        return $"{screenshotName}.png";
    }
}
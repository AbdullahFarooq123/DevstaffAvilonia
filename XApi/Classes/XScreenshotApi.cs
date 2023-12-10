using GlobalExtensionMethods;
using XApi.Interfaces;
using XApi.Utilities;

namespace XApi.Classes;

public class XScreenshotApi : IXScreenshotApi
{
    private const string ScreenshotCommand = "scrot {filename}.png -z";

    public string CaptureWindow(bool saveFile)
    {
        var path = TakeScreenshot();
        var bytes = File.ReadAllBytes(path: path);
        var base64 = Convert.ToBase64String(inArray: bytes);
        if (!saveFile) File.Delete(path: path);
        return base64;
    }

    private string TakeScreenshot()
    {
        var screenshotName = DateTime.Now.ToFileName();
        var screenshotCommandWithPath = ScreenshotCommand.Replace(oldValue: "{filename}", newValue: screenshotName);
        LinuxCmdUtil.AwaitedCommandExec(command: screenshotCommandWithPath);
        return $"{screenshotName}.png";
    }
}
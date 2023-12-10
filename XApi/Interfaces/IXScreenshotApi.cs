namespace XApi.Interfaces;

public interface IXScreenshotApi
{
    public string CaptureWindow(bool saveFile = false);
}
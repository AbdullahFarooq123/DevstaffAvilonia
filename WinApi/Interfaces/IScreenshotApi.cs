using System.Drawing;

namespace WinApi.Interfaces;

public interface IScreenshotApi
{
    public Image CaptureWindow();
}

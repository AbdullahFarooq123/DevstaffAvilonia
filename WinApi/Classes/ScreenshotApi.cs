using System.Diagnostics.CodeAnalysis;
using WinApi.Interfaces;
using WinApi.Utilities;
using System.Drawing;

namespace WinApi.Classes;

[SuppressMessage(category: "Interoperability", checkId: "CA1416:Validate platform compatibility")]
public class ScreenshotApi : IScreenshotApi
{
    public Image CaptureWindow()
    {
        var (width, height) = GetWindowBounds();
        var hdcSrc = User32Util.GetWindowDC(hWnd: IntPtr.Zero);
        var hdcDest = Gdi32Util.CreateCompatibleDC(hDc: hdcSrc);
        var hBitmap = Gdi32Util.CreateCompatibleBitmap(hDc: hdcSrc, nWidth: width, nHeight: height);
        var hOld = Gdi32Util.SelectObject(hDc: hdcDest, hObject: hBitmap);
        Gdi32Util.BitBlt(
            hObject: hdcDest,
            nXDest: 0,
            nYDest: 0,
            nWidth: width,
            nHeight: height,
            hObjectSource: hdcSrc,
            nXSrc: 0, nYSrc: 0,
            dwRop: (int)ScreenshotEventId.SRCCOPY
        );
        var image = Image.FromHbitmap(hbitmap: hBitmap);
        Gdi32Util.SelectObject(hDc: hdcDest, hObject: hOld);
        Gdi32Util.DeleteObject(hObject: hBitmap);
        Gdi32Util.DeleteDC(hDc: hdcDest);
        User32Util.ReleaseDC(hWnd: IntPtr.Zero, hDc: hdcSrc);
        return image;
    }

    private (int, int) GetWindowBounds()
    {
        var windowRect = GetWindowsRectBounds();
        var width = windowRect.right - windowRect.left;
        var height = windowRect.bottom - windowRect.top;
        return (width, height);
    }

    private HookUtilities.Rect GetWindowsRectBounds() => new()
    {
        left = User32Util.GetSystemMetrics((int)ScreenshotEventId.SM_XVIRTUALSCREEN),
        top = User32Util.GetSystemMetrics((int)ScreenshotEventId.SM_YVIRTUALSCREEN),
        right = User32Util.GetSystemMetrics((int)ScreenshotEventId.SM_CXVIRTUALSCREEN),
        bottom = User32Util.GetSystemMetrics((int)ScreenshotEventId.SM_CYVIRTUALSCREEN)
    };
}
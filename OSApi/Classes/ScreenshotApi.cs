using WinApi.Interfaces;
using WinApi.Utilities;
using System.Drawing;

namespace WinApi.Classes;

public class ScreenshotApi : IScreenshotApi
{
	public Image CaptureWindow()
	{
		var (width, height) = GetWindowBounds();
		var hdcSrc = User32Util.GetWindowDC(IntPtr.Zero);
		var hdcDest = Gdi32Util.CreateCompatibleDC(hdcSrc);

		var hBitmap = Gdi32Util.CreateCompatibleBitmap(hdcSrc, width, height);
		var hOld = Gdi32Util.SelectObject(hdcDest, hBitmap);

		Gdi32Util.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, (int)ScreenshotEventId.SRCCOPY);

		var image = Image.FromHbitmap(hBitmap);

		Gdi32Util.SelectObject(hdcDest, hOld);
		Gdi32Util.DeleteObject(hBitmap);
		Gdi32Util.DeleteDC(hdcDest);
		User32Util.ReleaseDC(IntPtr.Zero, hdcSrc);

		return image;
	}

	private (int, int) GetWindowBounds()
	{
		var windowRect = GetWindowsRectBounds();
		var width = windowRect.right - windowRect.left;
		var height = windowRect.bottom - windowRect.top;
		return (width, height);
	}
	private HookUtilities.Rect GetWindowsRectBounds()
	{
		var windowRect = new HookUtilities.Rect();
		windowRect.left = User32Util.GetSystemMetrics((int)ScreenshotEventId.SM_XVIRTUALSCREEN);
		windowRect.top = User32Util.GetSystemMetrics((int)ScreenshotEventId.SM_YVIRTUALSCREEN);
		windowRect.right = User32Util.GetSystemMetrics((int)ScreenshotEventId.SM_CXVIRTUALSCREEN);
		windowRect.bottom = User32Util.GetSystemMetrics((int)ScreenshotEventId.SM_CYVIRTUALSCREEN);
		return windowRect;
	}
}

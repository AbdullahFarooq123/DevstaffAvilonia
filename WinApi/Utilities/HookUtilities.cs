using System.Runtime.InteropServices;

namespace WinApi.Utilities;

public static class User32Util
{
	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern IntPtr SetWindowsHookEx(int id, Delegates.LowLevelProc callback, IntPtr hMod, uint dwThreadId);
	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern IntPtr CallNextHookEx(IntPtr hook, int nCode, int wp, IntPtr lp);
	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern bool UnhookWindowsHookEx(IntPtr hook);
	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern IntPtr GetDesktopWindow();
	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern IntPtr GetWindowDC(IntPtr hWnd);
	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern IntPtr GetWindowRect(IntPtr hWnd, ref HookUtilities.RECT rect);
	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern int GetSystemMetrics(int nIndex);
}

public static class Gdi32Util
{
	[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hObjectSource, int nXSrc, int nYSrc, int dwRop);
	[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);
	[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
	[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern bool DeleteDC(IntPtr hDC);
	[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern bool DeleteObject(IntPtr hObject);
	[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
}

public static class Delegates
{
	public delegate IntPtr LowLevelProc(int nCode, int wParam, IntPtr lParam);
	public delegate void HookCallback();
	public delegate void ParameterizedHookCallback<Type>(Type param);
}

public static class HookUtilities
{
	[StructLayout(LayoutKind.Sequential)]
	public record RECT
	{
		public int left;
		public int top;
		public int right;
		public int bottom;
	}
}

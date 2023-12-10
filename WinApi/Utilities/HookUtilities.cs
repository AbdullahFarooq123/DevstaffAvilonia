using System.Runtime.InteropServices;

namespace WinApi.Utilities;

public static class User32Util
{
    [DllImport(dllName: "user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr
        SetWindowsHookEx(int id, Delegates.LowLevelProc? callback, IntPtr hMod, uint dwThreadId);

    [DllImport(dllName: "user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr CallNextHookEx(IntPtr hook, int nCode, int wp, IntPtr lp);

    [DllImport(dllName: "user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern bool UnhookWindowsHookEx(IntPtr hook);

    [DllImport(dllName: "user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr GetDesktopWindow();

    [DllImport(dllName: "user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr GetWindowDC(IntPtr hWnd);

    [DllImport(dllName: "user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

    [DllImport(dllName: "user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr GetWindowRect(IntPtr hWnd, ref HookUtilities.Rect rect);

    [DllImport(dllName: "user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern int GetSystemMetrics(int nIndex);
}

public static class Gdi32Util
{
    [DllImport(dllName: "gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth, int nHeight,
        IntPtr hObjectSource, int nXSrc, int nYSrc, int dwRop);

    [DllImport(dllName: "gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr CreateCompatibleBitmap(IntPtr hDc, int nWidth, int nHeight);

    [DllImport(dllName: "gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr CreateCompatibleDC(IntPtr hDc);

    [DllImport(dllName: "gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern bool DeleteDC(IntPtr hDc);

    [DllImport(dllName: "gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern bool DeleteObject(IntPtr hObject);

    [DllImport(dllName: "gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr SelectObject(IntPtr hDc, IntPtr hObject);
}

public static class Delegates
{
    public delegate IntPtr LowLevelProc(int nCode, int wParam, IntPtr lParam);
}

public static class HookUtilities
{
    [StructLayout(layoutKind: LayoutKind.Sequential)]
    public record Rect
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }
}
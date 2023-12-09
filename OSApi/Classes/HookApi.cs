using System.Diagnostics;
using System.Runtime.InteropServices;
using WinApi.Interfaces;
using WinApi.Utilities;

namespace WinApi.Classes;

public class HookApi : IHookApi
{
    private IntPtr _ptrHook = IntPtr.Zero;
    private Delegates.LowLevelProc? _lowLevelProc;

    public void Hook(IEnumerable<int> validEventIds, HookId hookId, Delegates.HookCallback? callback)
    {
        _lowLevelProc = (nCode, wp, lp) =>
        {
            if (IsValidEvent(nCode, wp, validEventIds))
            {
                var vkCode = Marshal.ReadInt32(lp);
                Debug.WriteLine($"Event Occurred : {vkCode}");
                callback?.Invoke();
            }
            else if (nCode < 0)
            {
                Unhook();
                _ptrHook = User32Util.SetWindowsHookEx((int)hookId, _lowLevelProc, IntPtr.Zero, 0);
                Debug.WriteLine($"Event Reset: {nCode}, WP : {wp}");
            }

            return User32Util.CallNextHookEx(_ptrHook, nCode, wp, lp);
        };
        _ptrHook = User32Util.SetWindowsHookEx((int)hookId, _lowLevelProc, IntPtr.Zero, 0);
    }

    public void Unhook()
    {
        if (_ptrHook != IntPtr.Zero)
            User32Util.UnhookWindowsHookEx(_ptrHook);
    }

    private bool IsValidEvent(int nCode, int wp, IEnumerable<int> validEventIds) =>
        nCode >= 0 && validEventIds.Contains(wp);
}
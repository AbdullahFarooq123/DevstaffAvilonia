using System.Diagnostics;
using System.Runtime.InteropServices;
using WinApi.Interfaces;
using WinApi.Utilities;

namespace WinApi.Classes;

public class HookApi : IHookApi
{
    private IntPtr _ptrHook = IntPtr.Zero;
    private Delegates.LowLevelProc? _lowLevelProc;

    public void Hook(IEnumerable<int> validEventIds, HookId hookId, EventHandler? callback)
    {
        _lowLevelProc = (nCode, wp, lp) =>
        {
            if (IsValidEvent(nCode: nCode, wp: wp, validEventIds: validEventIds))
            {
                var vkCode = Marshal.ReadInt32(ptr: lp);
                Debug.WriteLine(message: $"Event Occurred : {vkCode}");
                callback?.Invoke(sender: null, e: EventArgs.Empty);
            }
            else if (nCode < 0)
            {
                Unhook();
                SetWindowsHook(hookId: hookId);
                Debug.WriteLine(message: $"Event Reset: {nCode}, WP : {wp}");
            }

            return User32Util.CallNextHookEx(hook: _ptrHook, nCode: nCode, wp: wp, lp: lp);
        };
        SetWindowsHook(hookId: hookId);
    }

    public void Unhook()
    {
        if (_ptrHook != IntPtr.Zero)
            User32Util.UnhookWindowsHookEx(hook: _ptrHook);
    }

    private bool IsValidEvent(int nCode, int wp, IEnumerable<int> validEventIds) =>
        nCode >= 0 && validEventIds.Contains(value: wp);

    private void SetWindowsHook(HookId hookId) => _ptrHook = User32Util.SetWindowsHookEx(
        id: (int)hookId,
        callback: _lowLevelProc,
        hMod: IntPtr.Zero,
        dwThreadId: 0
    );
}
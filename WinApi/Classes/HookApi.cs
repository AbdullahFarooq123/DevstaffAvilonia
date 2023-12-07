using System.Diagnostics;
using System.Runtime.InteropServices;
using WinApi.Interfaces;
using WinApi.Utilities;

namespace WinApi.Classes;

public class HookApi : IHookApi
{
	private IntPtr ptrHook = IntPtr.Zero;
	private Delegates.LowLevelProc lowLevelProc;
	private DateTime now;

	public void Hook(IEnumerable<int> _validEventIds, HookId hookId, Delegates.HookCallback? callback)
	{
		now = DateTime.Now;

		lowLevelProc = new Delegates.LowLevelProc((int nCode, int wp, IntPtr lp) =>
		{
			if (IsValidEvent(nCode, wp, _validEventIds))
			{
				int vkCode = Marshal.ReadInt32(lp);
				Debug.WriteLine($"Event Occurred : {wp}");
				callback?.Invoke();
			}
			else if (nCode < 0)
			{
				Unhook();
				ptrHook = User32Util.SetWindowsHookEx((int)hookId, lowLevelProc, IntPtr.Zero, 0);
				Debug.WriteLine($"Event Reset: {nCode}, WP : {wp}");
			}
			return User32Util.CallNextHookEx(ptrHook, nCode, wp, lp);
		});
		ptrHook = User32Util.SetWindowsHookEx((int)hookId, lowLevelProc, IntPtr.Zero, 0);
	}

	public void Unhook()
	{
		if (ptrHook != IntPtr.Zero)
			User32Util.UnhookWindowsHookEx(ptrHook);
	}
	private bool IsValidEvent(int nCode, int wp, IEnumerable<int> _validEventIds) => nCode >= 0 && _validEventIds.Contains(wp);
}
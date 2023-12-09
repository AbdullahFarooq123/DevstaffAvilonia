using BackgroundJobs.ActivityListeners.Interfaces;
using WinApi.Interfaces;
using WinApi.Utilities;

namespace BackgroundJobs.ActivityListeners.Classes;

public class WinMouseListener : IMouseListener
{
    private readonly IHookApi _hookApi;
    private readonly IEnumerable<int> _validEventIds;

    public WinMouseListener(IHookApi hookApi)
    {
        _hookApi = hookApi ?? throw new ArgumentNullException(nameof(hookApi));
        _validEventIds = new List<int>
        {
            (int)MouseEventId.WM_MOUSEMOVE,
            (int)MouseEventId.WM_MOUSEHWHEEL,
            (int)MouseEventId.WM_XBUTTONDOWN,
            (int)MouseEventId.WM_LBUTTONDOWN,
            (int)MouseEventId.WM_RBUTTONDOWN,
        };
    }

    public void HookJob(Delegates.HookCallback? callback) =>
        _hookApi.Hook(_validEventIds, HookId.WH_MOUSE_LL, callback);

    public void UnHookJob() =>
        _hookApi.Unhook();
}
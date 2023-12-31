﻿using BackgroundJobs.ActivityListeners.Interfaces;
using WinApi.Interfaces;
using WinApi.Utilities;

namespace BackgroundJobs.ActivityListeners.Classes;

public class WinKeyboardListener : IKeyboardListener
{
    private readonly IHookApi _hookApi;
    private readonly IEnumerable<int> _validEventIds;

    public WinKeyboardListener(IHookApi hookApi)
    {
        _hookApi = hookApi ?? throw new ArgumentNullException(nameof(hookApi));
        _validEventIds = new List<int>
        {
            (int)KeyboardEventId.WM_SYSKEYDOWN,
            (int)KeyboardEventId.WM_KEYDOWN
        };
    }

    public void HookJob(EventHandler? callback) =>
        _hookApi.Hook(validEventIds: _validEventIds, hookId: HookId.WH_KEYBOARD_LL, callback: callback);

    public void UnHookJob() =>
        _hookApi.Unhook();
}
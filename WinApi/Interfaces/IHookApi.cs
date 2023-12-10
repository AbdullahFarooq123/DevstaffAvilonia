using WinApi.Utilities;

namespace WinApi.Interfaces;

public interface IHookApi
{
    public void Hook(IEnumerable<int> validEventIds, HookId hookId, EventHandler? callback);
    public void Unhook();
}
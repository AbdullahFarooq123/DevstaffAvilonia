using XApi.Utilities;

namespace XApi.Interfaces;

public interface IInputDeviceApi
{
    public void SetParams(IEnumerable<string> validEventArgs, EventHandler? callback, InputType inputType);
    public void ListenInputDevice();
    public void KillProcesses();
}
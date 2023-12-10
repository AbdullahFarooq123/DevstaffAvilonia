using System.Diagnostics;
using GlobalExtensionMethods;
using XApi.Interfaces;
using XApi.Utilities;

namespace XApi.Classes;

public class InputDeviceApi : IInputDeviceApi
{
    private const string InputList = "xinput list";
    private const string InputHook = "xinput test {id}";
    private readonly List<Process> _processes = new();
    private readonly List<int> _inputDeviceList = new();
    private readonly object _lockValidEventArgs = new();
    private readonly List<string> _validEventArgs = new();
    private EventHandler? _callback;
    private bool _paramsSet;
    private InputType _inputType;

    public void SetParams(IEnumerable<string> validEventArgs, EventHandler? callback, InputType inputType)
    {
        _validEventArgs.AddRange(validEventArgs);
        // ReSharper disable once InconsistentlySynchronizedField
        _callback += callback;
        _paramsSet = true;
        _inputType = inputType;
    }

    public void ListenInputDevice()
    {
        if (!_paramsSet) throw new InvalidOperationException("The 'SetParams' method is not called.");
        var newDeviceIds = GetSymmetricInputDevices();
        foreach (var deviceId in newDeviceIds)
        {
            _inputDeviceList.Add(deviceId);
            var command = InputHook.Replace("{id}", $"{deviceId}");
            var process = LinuxCmdUtil.HookOutputCmd(command, EventCallback);
            _processes.Add(process);
            process.Start();
            process.BeginOutputReadLine();
        }
    }

    public void KillProcesses()
    {
        _processes.ForEach(process => process.Kill());
        _processes.Clear();
    }

    private List<int> GetSymmetricInputDevices()
    {
        var inputDevicesList = LinuxCmdUtil.GetOutputCmd(InputList);
        var inputDevicesIds = ParseInputDevices.ParseDeviceInformation(inputDevicesList, _inputType);
        return _inputDeviceList.SymmetricDifference(inputDevicesIds);
    }

    private void EventCallback(object sender, DataReceivedEventArgs e)
    {
        if (e.Data.IsNullOrEmpty() || !_validEventArgs.Any(e.Data.ToLower().Contains)) return;
        lock (_lockValidEventArgs)
        {
            Debug.WriteLine(e.Data);
            _callback?.Invoke(null, EventArgs.Empty);
        }
    }
}
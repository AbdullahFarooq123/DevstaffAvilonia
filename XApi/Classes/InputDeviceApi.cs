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
        _validEventArgs.AddRange(collection: validEventArgs);
        // ReSharper disable once InconsistentlySynchronizedField
        _callback = callback;
        _paramsSet = true;
        _inputType = inputType;
    }

    public void ListenInputDevice()
    {
        if (!_paramsSet) throw new InvalidOperationException(message: "The 'SetParams' method is not called.");
        var newDeviceIds = GetSymmetricInputDevices();
        foreach (var deviceId in newDeviceIds)
        {
            var command = InputHook.Replace(oldValue: "{id}", newValue: $"{deviceId}");
            var process = LinuxCmdUtil.HookOutputCmd(command: command, eventCallback: EventCallback);
            _inputDeviceList.Add(item: deviceId);
            _processes.Add(item: process);
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
        var inputDevicesList = LinuxCmdUtil.GetOutputCmd(command: InputList);
        var inputDevicesIds = ParseInputDevices.ParseDeviceInformation(
            inputDevices: inputDevicesList,
            type: _inputType);
        return _inputDeviceList.SymmetricDifference(list2: inputDevicesIds);
    }

    private void EventCallback(object sender, DataReceivedEventArgs e)
    {
        if (e.Data!.IsNullOrEmpty() || !_validEventArgs.Any(e.Data!.ToLower().Contains)) return;
        lock (_lockValidEventArgs)
        {
            Debug.WriteLine(message: e.Data);
            _callback?.Invoke(sender: null, e: EventArgs.Empty);
        }
    }
}
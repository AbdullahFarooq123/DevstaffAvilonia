using System.Diagnostics;

namespace XApi.Utilities;

internal static class LinuxCmdUtil
{
    public static string GetOutputCmd(string command)
    {
        try
        {
            var startInfo = GetProcessStartInfo(command: command);
            using var process = Process.Start(startInfo: startInfo);
            using var reader = process?.StandardOutput ??
                               throw new InvalidOperationException(message: "Can't start linux process");
            return reader.ReadToEnd();
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    public static Process HookOutputCmd(string command, DataReceivedEventHandler eventCallback)
    {
        var process = GetProcessWithCommand(command: command);
        process.OutputDataReceived += eventCallback;
        return process;
    }

    public static void AwaitedCommandExec(string command)
    {
        var process = GetProcessWithCommand(command: command);
        process.Start();
        process.WaitForExit();
        process.Kill();
    }

    private static ProcessStartInfo GetProcessStartInfo(string command) =>
        new()
        {
            FileName = "/bin/bash",
            Arguments = $"-c \"{command}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = false
        };

    private static Process GetProcessWithCommand(string command) =>
        new()
        {
            StartInfo = GetProcessStartInfo(command: command)
        };
}
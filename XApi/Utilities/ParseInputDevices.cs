using System.Text.RegularExpressions;

namespace XApi.Utilities;

internal static partial class ParseInputDevices
{
    [GeneratedRegex(@"id=(\d+)")]
    private static partial Regex IdRegex();

    [GeneratedRegex(@"\[master")]
    private static partial Regex MasterRegex();

    [GeneratedRegex(@"\[slave")]
    private static partial Regex SlaveRegex();

    public static IEnumerable<int> ParseDeviceInformation(string inputDevices, InputType type)
    {
        var inputDevicesObj = new List<int>();
        var lines = inputDevices.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var extractingIds = false;
        foreach (var line in lines)
        {
            var masterMatched = MatchMaster(line, type);
            if (line.Contains("master", StringComparison.CurrentCultureIgnoreCase) && extractingIds) break;
            if (masterMatched) extractingIds = true;
            if (!extractingIds) continue;
            var slaveDeviceId = GetSlaveDeviceId(line);
            if (slaveDeviceId != -1) inputDevicesObj.Add(slaveDeviceId);
        }

        return inputDevicesObj;
    }

    private static int ExtractIdValue(string line)
    {
        var match = IdRegex().Match(line);
        if (!match.Success) return -1;
        if (int.TryParse(match.Groups[1].Value, out var id))
            return id;
        return -1;
    }

    private static bool MatchMaster(string line, InputType type)
    {
        var masterMatch = MasterRegex().Match(line.Trim());
        if (!masterMatch.Success) return false;
        return (line.Contains("pointer", StringComparison.CurrentCultureIgnoreCase) && type is InputType.Mouse) ||
               (line.Contains("keyboard", StringComparison.CurrentCultureIgnoreCase) && type is InputType.Keyboard);
    }

    private static int GetSlaveDeviceId(string line)
    {
        if (!SlaveRegex().Match(line.Trim()).Success) return -1;
        return ExtractIdValue(line);
    }
}
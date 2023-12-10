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
        var lines = inputDevices.Split(separator: '\n', options: StringSplitOptions.RemoveEmptyEntries);
        var extractingIds = false;
        foreach (var line in lines)
        {
            var masterMatched = MatchMaster(line: line, type: type);
            if (line.Contains(value: "master", comparisonType: StringComparison.CurrentCultureIgnoreCase) &&
                extractingIds) break;
            if (masterMatched) extractingIds = true;
            if (!extractingIds) continue;
            var slaveDeviceId = GetSlaveDeviceId(line: line);
            if (slaveDeviceId != -1) inputDevicesObj.Add(item: slaveDeviceId);
        }

        return inputDevicesObj;
    }

    private static int ExtractIdValue(string line)
    {
        var match = IdRegex().Match(input: line);
        if (!match.Success) return -1;
        if (int.TryParse(s: match.Groups[1].Value, result: out var id)) return id;
        return -1;
    }

    private static bool MatchMaster(string line, InputType type)
    {
        var masterMatch = MasterRegex().Match(input: line.Trim());
        if (!masterMatch.Success) return false;
        return (line.Contains(value: "pointer", comparisonType: StringComparison.CurrentCultureIgnoreCase) &&
                type is InputType.Mouse) ||
               (line.Contains(value: "keyboard", comparisonType: StringComparison.CurrentCultureIgnoreCase) &&
                type is InputType.Keyboard);
    }

    private static int GetSlaveDeviceId(string line)
    {
        if (!SlaveRegex().Match(input: line.Trim()).Success) return -1;
        return ExtractIdValue(line);
    }
}
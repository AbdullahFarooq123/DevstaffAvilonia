namespace GlobalExtensionMethods;

public static class DateExtensions
{
    public static string ToFileName(this DateTime dateTime) =>
        dateTime.ToString(format: "O").Replace(oldValue: ":", newValue: "-").Replace(oldValue: ".", newValue: "-");
}
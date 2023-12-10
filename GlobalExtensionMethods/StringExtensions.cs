namespace GlobalExtensionMethods;

public static class StringExtensions
{
    public static bool IsNullOrEmpty(this string str) =>
        string.IsNullOrWhiteSpace(str);

    public static bool IsNotNullOrEmpty(this string str) =>
        !string.IsNullOrWhiteSpace(str);
}
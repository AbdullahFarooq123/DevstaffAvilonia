namespace GlobalExtensionMethods;

public static class ObjectExtensions
{
    public static bool HasValue<T>(this T? obj) where T : class =>
        obj != null;

    public static bool HasNoValue<T>(this T? obj) where T : class =>
        obj == null;

    public static T Value<T>(this T? obj) where T
        : class
    {
        if (obj == null)
            throw new InvalidOperationException("Object is null");
        return obj;
    }
}
namespace GlobalExtensionMethods;

public static class TypeExtensions
{
    public static bool IsInstantiable(this Type type) => type is { IsAbstract: false, IsInterface: false };
    public static bool IsNotInstantiable(this Type type) => type.IsAbstract || type.IsInterface;
}
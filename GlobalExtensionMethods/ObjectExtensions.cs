namespace GlobalExtensionMethods;

public static class ObjectExtensions
{
	#region GenericExtensions
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
	#endregion GenericExtensions

	#region StringExtensions
	public static bool IsNullOrEmpty(this string str) =>
		string.IsNullOrWhiteSpace(str);
	public static bool IsNotNullOrEmpty(this string str) =>
		!string.IsNullOrWhiteSpace(str);
	#endregion StringExtensions

	public static bool IsInstantiable(this Type type) => type is { IsAbstract: false, IsInterface: false };
	public static bool IsNotInstantiable(this Type type) => type.IsAbstract || type.IsInterface;
}

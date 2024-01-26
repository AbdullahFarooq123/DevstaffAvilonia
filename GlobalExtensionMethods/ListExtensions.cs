namespace GlobalExtensionMethods;

public static class ListExtensions
{
    public static List<T> SymmetricDifference<T>(this IEnumerable<T> list1, IEnumerable<T> list2) =>
        list2.Except(second: list1).ToList();
}
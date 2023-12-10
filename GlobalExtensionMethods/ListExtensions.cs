namespace GlobalExtensionMethods;

public static class ListExtensions
{
    public static List<T> SymmetricDifference<T>(this IEnumerable<T> list1, IEnumerable<T> list2) =>
        list2.Except(list1).ToList();
}
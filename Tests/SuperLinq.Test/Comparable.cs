namespace Test;

static class Comparable<T> where T : IComparable<T>
{
	public static readonly IComparer<T> DescendingOrderComparer =
		Comparer<T>.Create((x, y) => -Math.Sign(x.CompareTo(y)));
}

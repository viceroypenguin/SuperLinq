namespace Test;

internal class BreakingCollection<T> : BreakingSequence<T?>, ICollection<T?>
{
	protected readonly IList<T?> List;

	public BreakingCollection(params T[] values) : this((IList<T?>)values) { }
	public BreakingCollection(IList<T?> list) => List = list;
	public BreakingCollection(int count) :
		this(Enumerable.Repeat(default(T), count).ToList())
	{ }

	public int Count => List.Count;

	public void Add(T? item) => Assert.Fail("LINQ Operators should not be calling this method.");
	public void Clear() => Assert.Fail("LINQ Operators should not be calling this method.");
	public bool Contains(T? item) => List.Contains(item);
	public void CopyTo(T?[] array, int arrayIndex) => List.CopyTo(array, arrayIndex);
	public bool Remove(T? item) { Assert.Fail("LINQ Operators should not be calling this method."); return false; }
	public bool IsReadOnly => true;
}

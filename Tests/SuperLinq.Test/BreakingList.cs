namespace Test;

internal static class BreakingList
{
	public static BreakingList<T> AsBreakingList<T>(this IEnumerable<T> source) => new(source);
}

internal class BreakingList<T> : BreakingSequence<T>, IList<T>, IDisposableEnumerable<T>
{
	protected readonly IList<T> List;

	public BreakingList(params T[] values) : this((IList<T>)values) { }
	public BreakingList(IEnumerable<T> source) => List = source.ToList();
	public BreakingList(IList<T> list) => List = list;

	public int Count => List.Count;

	public T this[int index]
	{
		get
		{
			if (index < 0 || index >= List.Count)
				Assert.Fail("LINQ Operators should prevent this from happening.");
			return List[index];
		}

		set => Assert.Fail("LINQ Operators should not be calling this method.");
	}

	public int IndexOf(T item)
	{
		Assert.Fail("LINQ Operators should not be calling this method.");
		return -1;
	}

	public void Add(T item) => Assert.Fail("LINQ Operators should not be calling this method.");
	public void Insert(int index, T item) => Assert.Fail("LINQ Operators should not be calling this method.");
	public void Clear() => Assert.Fail("LINQ Operators should not be calling this method.");
	public bool Contains(T item) => List.Contains(item);
	public bool Remove(T item) { Assert.Fail("LINQ Operators should not be calling this method."); return false; }
	public void RemoveAt(int index) => Assert.Fail("LINQ Operators should not be calling this method.");
	public bool IsReadOnly => true;

	public virtual void CopyTo(T[] array, int arrayIndex) => List.CopyTo(array, arrayIndex);

	public void Dispose() { }
}

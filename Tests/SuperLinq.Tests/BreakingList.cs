namespace SuperLinq.Tests;

internal static class BreakingList
{
	public static BreakingList<T> AsBreakingList<T>(this IEnumerable<T> source) => new(source);
}

internal sealed class BreakingList<T> : BreakingSequence<T>, IList<T>, IDisposableEnumerable<T>
{
	private readonly IList<T> _list;

	public BreakingList(params T[] values) : this((IList<T>)values) { }
	public BreakingList(IEnumerable<T> source) => _list = source.ToList();
	public BreakingList(IList<T> list) => _list = list;

	public int Count => _list.Count;

	public T this[int index]
	{
		get
		{
			if (index < 0 || index >= _list.Count)
				Assert.Fail("LINQ Operators should prevent this from happening.");

			return _list[index];
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
	public bool Contains(T item) => _list.Contains(item);
	public bool Remove(T item) { Assert.Fail("LINQ Operators should not be calling this method."); return false; }
	public void RemoveAt(int index) => Assert.Fail("LINQ Operators should not be calling this method.");
	public bool IsReadOnly => true;

	public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

	public void Dispose() { }
}

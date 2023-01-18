using System.Collections;

namespace Test;

/// <summary>
/// This class implement <see cref="IList{T}"/> but specifically prohibits enumeration using GetEnumerator().
/// It is provided to assist in testing extension methods that MUST NOT call the GetEnumerator()
/// method of <see cref="IEnumerable"/> - either because they should be using the indexer or because they are
/// expected to be lazily evaluated.
/// </summary>
internal sealed class BreakingList<T> : BreakingCollection<T>, IList<T>
{
	public BreakingList(IEnumerable<T> source) : base(source) { }

	public int IndexOf(T item) => List.IndexOf(item);
	public void Insert(int index, T item) => Assert.Fail("LINQ Operators should not be calling this method.");
	public void RemoveAt(int index) => Assert.Fail("LINQ Operators should not be calling this method.");

	public T this[int index]
	{
		get => List[index];
		set => Assert.Fail("LINQ Operators should not be calling this method.");
	}
}

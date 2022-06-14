namespace Test;

class BreakingCollection<T> : BreakingSequence<T>, ICollection<T>
{
	protected readonly IList<T> List;

	public BreakingCollection(params T[] values) : this((IList<T>)values) { }
	public BreakingCollection(IList<T> list) => List = list;
	public BreakingCollection(int count) :
		this(Enumerable.Repeat(default(T), count).ToList())
	{ }

	public int Count => List.Count;

	public void Add(T item) => throw new NotImplementedException();
	public void Clear() => throw new NotImplementedException();
	public bool Contains(T item) => List.Contains(item);
	public void CopyTo(T[] array, int arrayIndex) => List.CopyTo(array, arrayIndex);
	public bool Remove(T item) => throw new NotImplementedException();
	public bool IsReadOnly => true;
}

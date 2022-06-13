namespace Test;

class BreakingReadOnlyCollection<T> : BreakingSequence<T>, IReadOnlyCollection<T>
{
	readonly IReadOnlyCollection<T> _collection;

	public BreakingReadOnlyCollection(params T[] values) : this((IReadOnlyCollection<T>)values) { }
	public BreakingReadOnlyCollection(IReadOnlyCollection<T> collection) => _collection = collection;
	public int Count => _collection.Count;
}

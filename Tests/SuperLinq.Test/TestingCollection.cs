using static Test.TestingSequence;

namespace Test;

internal static class TestingCollection
{
	internal static TestingCollection<T> AsTestingCollection<T>(
		this IEnumerable<T> source, Options options = Options.None, int maxEnumerations = 1) =>
			new(source as IList<T> ?? source.ToList(), options, maxEnumerations);
}

internal class TestingCollection<T> : TestingSequence<T>, ICollection<T>
{
	private readonly ICollection<T> _collection;

	internal TestingCollection(
		ICollection<T> sequence, Options options, int maxEnumerations)
		: base(sequence, options, maxEnumerations)
	{
		_collection = sequence;
	}

	public int Count => _collection.Count;

	public void Add(T item) => Assert.Fail("LINQ Operators should not be calling this method.");
	public void Clear() => Assert.Fail("LINQ Operators should not be calling this method.");
	public bool Contains(T item) => _collection.Contains(item);
	public bool Remove(T item) { Assert.Fail("LINQ Operators should not be calling this method."); return false; }
	public bool IsReadOnly => true;

	public int CopyCount { get; private set; }
	public virtual void CopyTo(T[] array, int arrayIndex)
	{
		_collection.CopyTo(array, arrayIndex);
		CopyCount++;
	}
}

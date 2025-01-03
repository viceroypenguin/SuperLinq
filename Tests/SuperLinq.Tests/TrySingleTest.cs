using System.Collections;

namespace SuperLinq.Tests;

public sealed class TrySingleTest
{
	public static IEnumerable<IDisposableEnumerable<int?>> GetAllSequences(int numElements) =>
		Enumerable.Range(1, numElements).Cast<int?>().GetAllSequences();

	[Test]
	[MethodDataSource(nameof(GetAllSequences), Arguments = [0])]
	public void TrySingleWithEmptySource(IDisposableEnumerable<int?> seq)
	{
		using (seq)
		{
			var (cardinality, value) = seq.TrySingle("zero", "one", "many");

			Assert.Equal("zero", cardinality);
			Assert.Null(value);
		}
	}

	[Test]
	[MethodDataSource(nameof(GetAllSequences), Arguments = [1])]
	public void TrySingleWithSingleton(IDisposableEnumerable<int?> seq)
	{
		using (seq)
		{
			var (cardinality, value) = seq.TrySingle("zero", "one", "many");

			Assert.Equal("one", cardinality);
			Assert.Equal(1, value);
		}
	}

	[Test]
	public void TrySingleWithSingletonCollection()
	{
		var source = new BreakingSingleElementCollection<int>(10);
		var (cardinality, value) = source.TrySingle("zero", "one", "many");

		Assert.Equal("one", cardinality);
		Assert.Equal(10, value);
	}

	private sealed class BreakingSingleElementCollection<T>(
		T element
	) : ICollection<T>
	{
		public int Count { get; } = 1;

		public IEnumerator<T> GetEnumerator()
		{
			yield return element;
			throw new InvalidOperationException($"{nameof(SuperEnumerable.TrySingle)} should not have attempted to consume a second element.");
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public void Add(T item) => throw new NotSupportedException();
		public void Clear() => throw new NotSupportedException();
		public bool Contains(T item) => throw new NotSupportedException();
		public void CopyTo(T[] array, int arrayIndex) => throw new NotSupportedException();
		public bool Remove(T item) => throw new NotSupportedException();
		public bool IsReadOnly => true;
	}

	[Test]
	[MethodDataSource(nameof(GetAllSequences), Arguments = [2])]
	public void TrySingleWithMoreThanOne(IDisposableEnumerable<int?> seq)
	{
		using (seq)
		{
			var (cardinality, value) = seq.TrySingle("zero", "one", "many");

			Assert.Equal("many", cardinality);
			Assert.Null(value);
		}
	}
}

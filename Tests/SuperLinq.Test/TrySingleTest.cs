using System.Collections;

namespace Test;

public class TrySingleTest
{
	[Theory]
	[InlineData(SourceKind.Sequence)]
	[InlineData(SourceKind.BreakingList)]
	[InlineData(SourceKind.BreakingCollection)]
	public void TrySingleWithEmptySource(SourceKind kind)
	{
		using var source = Array.Empty<int?>().ToSourceKind(kind);

		var (cardinality, value) = source.TrySingle("zero", "one", "many");

		Assert.Equal("zero", cardinality);
		Assert.Null(value);
	}

	[Theory]
	[InlineData(SourceKind.Sequence)]
	[InlineData(SourceKind.BreakingList)]
	public void TrySingleWithSingleton(SourceKind kind)
	{
		using var source = new int?[] { 10 }.ToSourceKind(kind);

		var (cardinality, value) = source.TrySingle("zero", "one", "many");

		Assert.Equal("one", cardinality);
		Assert.Equal(10, value);
	}

	[Fact]
	public void TrySingleWithSingletonCollection()
	{
		var source = new BreakingSingleElementCollection<int>(10);
		var (cardinality, value) = source.TrySingle("zero", "one", "many");

		Assert.Equal("one", cardinality);
		Assert.Equal(10, value);
	}

	private sealed class BreakingSingleElementCollection<T> : ICollection<T>
	{
		private readonly T _element;

		public BreakingSingleElementCollection(T element) =>
			_element = element;

		public int Count { get; } = 1;

		public IEnumerator<T> GetEnumerator()
		{
			yield return _element;
			throw new InvalidOperationException($"{nameof(SuperEnumerable.TrySingle)} should not have attempted to consume a second element.");
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public void Add(T item) => throw new NotImplementedException();
		public void Clear() => throw new NotImplementedException();
		public bool Contains(T item) => throw new NotImplementedException();
		public void CopyTo(T[] array, int arrayIndex) => throw new NotImplementedException();
		public bool Remove(T item) => throw new NotImplementedException();
		public bool IsReadOnly => true;
	}

	[Theory]
	[InlineData(SourceKind.Sequence)]
	[InlineData(SourceKind.BreakingList)]
	[InlineData(SourceKind.BreakingCollection)]
	public void TrySingleWithMoreThanOne(SourceKind kind)
	{
		using var source = new int?[] { 10, 20 }.ToSourceKind(kind);

		var (cardinality, value) = source.TrySingle("zero", "one", "many");

		Assert.Equal("many", cardinality);
		Assert.Null(value);
	}

	[Fact]
	public void TrySingleDoesNotConsumeMoreThanTwoElementsFromTheSequence()
	{
		using var xs = SuperEnumerable
			.From(
				() => 1,
				() => 2,
				BreakingFunc.Of<int>())
			.AsTestingSequence();
		var (cardinality, value) = xs.TrySingle("zero", "one", "many");

		Assert.Equal("many", cardinality);
		Assert.Equal(0, value);
	}

	[Theory]
	[InlineData(0, "zero")]
	[InlineData(1, "one")]
	[InlineData(2, "many")]
	public void TrySingleEnumeratesOnceOnlyAndDisposes(int numberOfElements, string expectedCardinality)
	{
		using var seq = Enumerable.Range(1, numberOfElements).AsTestingSequence();
		var (cardinality, _) = seq.TrySingle("zero", "one", "many");
		Assert.Equal(expectedCardinality, cardinality);
	}
}

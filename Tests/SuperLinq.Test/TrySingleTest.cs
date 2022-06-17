using System.Collections;

namespace Test;

public class TrySingleTest
{
	[Theory]
	[InlineData(SourceKind.Sequence)]
	[InlineData(SourceKind.BreakingList)]
	[InlineData(SourceKind.BreakingReadOnlyList)]
	[InlineData(SourceKind.BreakingCollection)]
	[InlineData(SourceKind.BreakingReadOnlyCollection)]
	public void TrySingleWithEmptySource(SourceKind kind)
	{
		var source = Array.Empty<int?>().ToSourceKind(kind);

		var (cardinality, value) = source.TrySingle("zero", "one", "many");

		Assert.Equal("zero", cardinality);
		Assert.Null(value);
	}

	[Theory]
	[InlineData(SourceKind.Sequence)]
	[InlineData(SourceKind.BreakingList)]
	[InlineData(SourceKind.BreakingReadOnlyList)]
	public void TrySingleWithSingleton(SourceKind kind)
	{
		var source = new int?[] { 10 }.ToSourceKind(kind);

		var (cardinality, value) = source.TrySingle("zero", "one", "many");

		Assert.Equal("one", cardinality);
		Assert.Equal(10, value);
	}

	[Theory, MemberData(nameof(SingletonCollectionInlineDatas))]
	public void TrySingleWithSingletonCollection<T>(IEnumerable<T> source, T result)
	{
		var (cardinality, value) = source.TrySingle("zero", "one", "many");

		Assert.Equal("one", cardinality);
		Assert.Equal(result, value);
	}

	public static IEnumerable<object[]> SingletonCollectionInlineDatas { get; } =
		new[]
		{
			new object[] { new BreakingSingleElementCollection<int>(10), 10, },
			new object[] { new BreakingSingleElementReadOnlyCollection<int>(20), 20, },
		};

	class BreakingSingleElementCollectionBase<T> : IEnumerable<T>
	{
		readonly T _element;

		protected BreakingSingleElementCollectionBase(T element) => _element = element;

		public int Count { get; } = 1;

		public IEnumerator<T> GetEnumerator()
		{
			yield return _element;
			throw new Exception($"{nameof(SuperEnumerable.TrySingle)} should not have attempted to consume a second element.");
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}

	sealed class BreakingSingleElementCollection<T> :
		BreakingSingleElementCollectionBase<T>, ICollection<T>
	{
		public BreakingSingleElementCollection(T element) : base(element) { }

		public void Add(T item) => throw new NotImplementedException();
		public void Clear() => throw new NotImplementedException();
		public bool Contains(T item) => throw new NotImplementedException();
		public void CopyTo(T[] array, int arrayIndex) => throw new NotImplementedException();
		public bool Remove(T item) => throw new NotImplementedException();
		public bool IsReadOnly => true;
	}

	sealed class BreakingSingleElementReadOnlyCollection<T> :
		BreakingSingleElementCollectionBase<T>, IReadOnlyCollection<T>
	{
		public BreakingSingleElementReadOnlyCollection(T element) : base(element) { }
	}

	[Theory]
	[InlineData(SourceKind.Sequence)]
	[InlineData(SourceKind.BreakingList)]
	[InlineData(SourceKind.BreakingReadOnlyList)]
	[InlineData(SourceKind.BreakingCollection)]
	[InlineData(SourceKind.BreakingReadOnlyCollection)]
	public void TrySingleWithMoreThanOne(SourceKind kind)
	{
		var source = new int?[] { 10, 20 }.ToSourceKind(kind);

		var (cardinality, value) = source.TrySingle("zero", "one", "many");

		Assert.Equal("many", cardinality);
		Assert.Null(value);
	}

	[Fact]
	public void TrySingleDoesNotConsumeMoreThanTwoElementsFromTheSequence()
	{
		static IEnumerable<int> TestSequence()
		{
			yield return 1;
			yield return 2;
			throw new Exception(nameof(SuperEnumerable.TrySingle) + " should not have attempted to consume a third element.");
		}

		var (cardinality, value) = TestSequence().TrySingle("zero", "one", "many");

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

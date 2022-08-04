namespace Test.Async;

public class BindByIndexTest
{
	[Fact]
	public void BindByIndexIsLazy()
	{
		new AsyncBreakingSequence<int>().BindByIndex(new AsyncBreakingSequence<int>());
		new AsyncBreakingSequence<int>().BindByIndex(new AsyncBreakingSequence<int>(), BreakingFunc.Of<int, int, int>(), BreakingFunc.Of<int, int>());
	}

	[Fact]
	public async Task BindByIndexDisposesEnumerators()
	{
		await using var seq1 = TestingSequence.Of<int>();
		await using var seq2 = TestingSequence.Of<int>();
		await seq1.BindByIndex(seq2).AssertEmpty();
	}

	[Fact]
	public Task BindByIndexInOrder()
	{
		var seq1 = AsyncEnumerable.Range(1, 10);
		var seq2 = AsyncSeq(1, 3, 5, 7, 9);

		return seq1.BindByIndex(seq2).AssertSequenceEqual(seq2.Select(x => x + 1));
	}

	[Fact]
	public Task BindByIndexOutOfOrder()
	{
		var seq1 = AsyncEnumerable.Range(1, 10);
		var seq2 = AsyncSeq(9, 7, 5, 3, 1);

		return seq1.BindByIndex(seq2).AssertSequenceEqual(seq2.Select(x => x + 1));
	}

	[Fact]
	public Task BindByIndexComplex()
	{
		var seq1 = AsyncEnumerable.Range(1, 10);
		var seq2 = AsyncSeq(0, 1, 8, 9, 3, 4, 2);

		return seq1.BindByIndex(seq2).AssertSequenceEqual(seq2.Select(x => x + 1));
	}

	[Theory]
	[InlineData(-1)]
	[InlineData(10)]
	[InlineData(100)]
	public Task BindByIndexThrowExceptionInvalidIndex(int index)
	{
		var seq1 = AsyncEnumerable.Range(1, 10);
		var seq2 = AsyncSeq(index);

		return Assert.ThrowsAsync<ArgumentOutOfRangeException>("indices", 
			async () => await seq1.BindByIndex(seq2).Consume());
	}

	[Fact]
	public Task BindByIndexTransformInOrder()
	{
		var seq1 = AsyncEnumerable.Range(1, 10);
		var seq2 = AsyncSeq(1, 3, 5, 7, 9);

		return seq1.BindByIndex(seq2, (e, i) => e, i => default(int?)).AssertSequenceEqual(seq2.Select(x => (int?)(x + 1)));
	}

	[Fact]
	public Task BindByIndexTransformOutOfOrder()
	{
		var seq1 = AsyncEnumerable.Range(1, 10);
		var seq2 = AsyncSeq(9, 7, 5, 3, 1);

		return seq1.BindByIndex(seq2, (e, i) => e, i => default(int?)).AssertSequenceEqual(seq2.Select(x => (int?)(x + 1)));
	}

	[Fact]
	public Task BindByIndexTransformComplex()
	{
		var seq1 = AsyncEnumerable.Range(1, 10);
		var seq2 = AsyncSeq(0, 1, 8, 9, 3, 4, 2);

		return seq1.BindByIndex(seq2, (e, i) => e, i => default(int?)).AssertSequenceEqual(seq2.Select(x => (int?)(x + 1)));
	}

	[Fact]
	public Task BindByIndexTransformInvalidIndex()
	{
		var seq1 = AsyncEnumerable.Range(1, 10);
		var seq2 = AsyncSeq(1, 10, 3, 30);

		return seq1.BindByIndex(seq2, (e, i) => e, i => default(int?)).AssertSequenceEqual(2, null, 4, null);
	}

	[Fact]
	public Task BindByIndexTransformThrowExceptionNegativeIndex()
	{
		var seq1 = AsyncEnumerable.Range(1, 10);
		var seq2 = AsyncSeq(-1);

		return Assert.ThrowsAsync<ArgumentOutOfRangeException>("indices",
			async () => await seq1.BindByIndex(seq2, (e, i) => e, i => default(int?)).Consume());
	}
}

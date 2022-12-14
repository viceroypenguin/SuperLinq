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
	public async Task BindByIndexInOrder()
	{
		var seq1 = AsyncEnumerable.Range(1, 10).AsTestingSequence();
		await using var seq2 = AsyncSeq(1, 3, 5, 7, 9).AsTestingSequence();

		await seq1.BindByIndex(seq2).AssertSequenceEqual(seq2.Select(x => x + 1));
	}

	[Fact]
	public async Task BindByIndexOutOfOrder()
	{
		await using var seq1 = AsyncEnumerable.Range(1, 10).AsTestingSequence();
		await using var seq2 = AsyncSeq(9, 7, 5, 3, 1).AsTestingSequence();

		await seq1.BindByIndex(seq2).AssertSequenceEqual(seq2.Select(x => x + 1));
	}

	[Fact]
	public async Task BindByIndexComplex()
	{
		await using var seq1 = AsyncEnumerable.Range(1, 10).AsTestingSequence();
		await using var seq2 = AsyncSeq(0, 1, 8, 9, 3, 4, 2).AsTestingSequence();

		await seq1.BindByIndex(seq2).AssertSequenceEqual(seq2.Select(x => x + 1));
	}

	[Theory]
	[InlineData(-1)]
	[InlineData(10)]
	[InlineData(100)]
	public async Task BindByIndexThrowExceptionInvalidIndex(int index)
	{
		await using var seq1 = AsyncEnumerable.Range(1, 10).AsTestingSequence();
		await using var seq2 = AsyncSeq(index).AsTestingSequence();

		await Assert.ThrowsAsync<ArgumentOutOfRangeException>("indices",
			async () => await seq1.BindByIndex(seq2).Consume());
	}

	[Fact]
	public async Task BindByIndexTransformInOrder()
	{
		await using var seq1 = AsyncEnumerable.Range(1, 10).AsTestingSequence();
		await using var seq2 = AsyncSeq(1, 3, 5, 7, 9).AsTestingSequence();

		await seq1.BindByIndex(seq2, (e, i) => e, i => default(int?)).AssertSequenceEqual(seq2.Select(x => (int?)(x + 1)));
	}

	[Fact]
	public async Task BindByIndexTransformOutOfOrder()
	{
		await using var seq1 = AsyncEnumerable.Range(1, 10).AsTestingSequence();
		await using var seq2 = AsyncSeq(9, 7, 5, 3, 1).AsTestingSequence();

		await seq1.BindByIndex(seq2, (e, i) => e, i => default(int?)).AssertSequenceEqual(seq2.Select(x => (int?)(x + 1)));
	}

	[Fact]
	public async Task BindByIndexTransformComplex()
	{
		await using var seq1 = AsyncEnumerable.Range(1, 10).AsTestingSequence();
		await using var seq2 = AsyncSeq(0, 1, 8, 9, 3, 4, 2).AsTestingSequence();

		await seq1.BindByIndex(seq2, (e, i) => e, i => default(int?)).AssertSequenceEqual(seq2.Select(x => (int?)(x + 1)));
	}

	[Fact]
	public async Task BindByIndexTransformInvalidIndex()
	{
		await using var seq1 = AsyncEnumerable.Range(1, 10).AsTestingSequence();
		await using var seq2 = AsyncSeq(1, 10, 3, 30).AsTestingSequence();

		await seq1.BindByIndex(seq2, (e, i) => e, i => default(int?)).AssertSequenceEqual(2, null, 4, null);
	}

	[Fact]
	public async Task BindByIndexTransformThrowExceptionNegativeIndex()
	{
		await using var seq1 = AsyncEnumerable.Range(1, 10).AsTestingSequence();
		await using var seq2 = AsyncSeq(-1).AsTestingSequence();

		await Assert.ThrowsAsync<ArgumentOutOfRangeException>("indices",
			async () => await seq1.BindByIndex(seq2, (e, i) => e, i => default(int?)).Consume());
	}
}

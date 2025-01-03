namespace SuperLinq.Async.Tests;

public sealed class BindByIndexTest
{
	[Test]
	public void BindByIndexIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().BindByIndex(new AsyncBreakingSequence<int>());
		_ = new AsyncBreakingSequence<int>().BindByIndex(new AsyncBreakingSequence<int>(), BreakingFunc.Of<int, int, int>(), BreakingFunc.Of<int, int>());
	}

	[Test]
	public async Task BindByIndexDisposesEnumerators()
	{
		await using var seq1 = TestingSequence.Of<int>();
		await using var seq2 = TestingSequence.Of<int>();
		await seq1.BindByIndex(seq2).AssertEmpty();
	}

	[Test]
	public async Task BindByIndexInOrder()
	{
		var indexes = AsyncSeq(1, 3, 5, 7, 9);
		await using var seq1 = AsyncEnumerable.Range(1, 10).AsTestingSequence();
		await using var seq2 = indexes.AsTestingSequence();

		await seq1.BindByIndex(seq2).AssertSequenceEqual(indexes.Select(x => x + 1));
	}

	[Test]
	public async Task BindByIndexOutOfOrder()
	{
		var indexes = AsyncSeq(9, 7, 5, 3, 1);
		await using var seq1 = AsyncEnumerable.Range(1, 10).AsTestingSequence();
		await using var seq2 = indexes.AsTestingSequence();

		await seq1.BindByIndex(seq2).AssertSequenceEqual(indexes.Select(x => x + 1));
	}

	[Test]
	public async Task BindByIndexComplex()
	{
		var indexes = AsyncSeq(0, 1, 8, 9, 3, 4, 2);
		await using var seq1 = AsyncEnumerable.Range(1, 10).AsTestingSequence();
		await using var seq2 = indexes.AsTestingSequence();

		await seq1.BindByIndex(seq2).AssertSequenceEqual(indexes.Select(x => x + 1));
	}

	[Test]
	[Arguments(-1)]
	[Arguments(10)]
	[Arguments(100)]
	public async Task BindByIndexThrowExceptionInvalidIndex(int index)
	{
		await using var seq1 = AsyncEnumerable.Range(1, 10).AsTestingSequence();
		await using var seq2 = TestingSequence.Of(index);

		_ = await Assert.ThrowsAsync<ArgumentOutOfRangeException>("indices",
			async () => await seq1.BindByIndex(seq2).Consume());
	}

	[Test]
	public async Task BindByIndexTransformInOrder()
	{
		var indexes = AsyncSeq(1, 3, 5, 7, 9);
		await using var seq1 = AsyncEnumerable.Range(1, 10).AsTestingSequence();
		await using var seq2 = indexes.AsTestingSequence();

		await seq1.BindByIndex(seq2, (e, i) => e, i => default(int?))
			.AssertSequenceEqual(indexes.Select(x => (int?)(x + 1)));
	}

	[Test]
	public async Task BindByIndexTransformOutOfOrder()
	{
		var indexes = AsyncSeq(9, 7, 5, 3, 1);
		await using var seq1 = AsyncEnumerable.Range(1, 10).AsTestingSequence();
		await using var seq2 = indexes.AsTestingSequence();

		await seq1.BindByIndex(seq2, (e, i) => e, i => default(int?))
			.AssertSequenceEqual(indexes.Select(x => (int?)(x + 1)));
	}

	[Test]
	public async Task BindByIndexTransformComplex()
	{
		var indexes = AsyncSeq(0, 1, 8, 9, 3, 4, 2);
		await using var seq1 = AsyncEnumerable.Range(1, 10).AsTestingSequence();
		await using var seq2 = indexes.AsTestingSequence();

		await seq1.BindByIndex(seq2, (e, i) => e, i => default(int?))
			.AssertSequenceEqual(indexes.Select(x => (int?)(x + 1)));
	}

	[Test]
	public async Task BindByIndexTransformInvalidIndex()
	{
		await using var seq1 = AsyncEnumerable.Range(1, 10).AsTestingSequence();
		await using var seq2 = TestingSequence.Of(1, 10, 3, 30);

		await seq1.BindByIndex(seq2, (e, i) => e, i => default(int?))
			.AssertSequenceEqual(2, null, 4, null);
	}

	[Test]
	public async Task BindByIndexTransformThrowExceptionNegativeIndex()
	{
		await using var seq1 = AsyncEnumerable.Range(1, 10).AsTestingSequence();
		await using var seq2 = TestingSequence.Of(-1);

		_ = await Assert.ThrowsAsync<ArgumentOutOfRangeException>("indices",
			async () => await seq1.BindByIndex(seq2, (e, i) => e, i => default(int?)).Consume());
	}
}

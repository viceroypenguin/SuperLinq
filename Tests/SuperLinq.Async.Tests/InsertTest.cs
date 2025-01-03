#if !NO_INDEX

namespace SuperLinq.Async.Tests;

public sealed class InsertTest
{
	[Test]
	public void InsertWithNegativeIndex()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new AsyncBreakingSequence<int>()
				.Insert(AsyncSeq(97, 98, 99), -1));
	}

	[Test]
	[Arguments(7)]
	[Arguments(8)]
	[Arguments(9)]
	public async Task InsertWithIndexGreaterThanSourceLengthMaterialized(int count)
	{
		var seq1 = AsyncEnumerable.Range(0, count);
		var seq2 = AsyncSeq(97, 98, 99);

		using var test1 = seq1.AsTestingSequence();
		using var test2 = seq2.AsTestingSequence();

		var result = test1.Insert(test2, count + 1);

		_ = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await result.ForEachAsync((e, index) =>
				Assert.Equal(index, e)));
	}

	[Test]
	[Arguments(7)]
	[Arguments(8)]
	[Arguments(9)]
	public async Task InsertWithIndexGreaterThanSourceLengthLazy(int count)
	{
		var seq1 = AsyncEnumerable.Range(0, count);
		var seq2 = AsyncSeq(97, 98, 99);

		using var test1 = seq1.AsTestingSequence();
		using var test2 = seq2.AsTestingSequence();

		var result = test1.Insert(test2, count + 1).Take(count);

		Assert.Equal(await seq1.ToListAsync(), await result.ToListAsync());
	}

	[Test]
	[Arguments(3, 0)]
	[Arguments(3, 1)]
	[Arguments(3, 2)]
	[Arguments(3, 3)]
	public async Task Insert(int count, int index)
	{
		var seq1 = AsyncEnumerable.Range(1, count);
		var seq2 = AsyncSeq(97, 98, 99);

		using var test1 = seq1.AsTestingSequence();
		using var test2 = seq2.AsTestingSequence();

		var result = test1.Insert(test2, index);

		var expectations = seq1.Take(index).Concat(seq2)
			.Concat(seq1.Skip(index));

		Assert.Equal(await expectations.ToListAsync(), await result.ToListAsync());
	}

	[Test]
	public void InsertIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().Insert(new AsyncBreakingSequence<int>(), 0);
	}

	[Test]
	public void BacksertIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().Insert(new AsyncBreakingSequence<int>(), ^0);
	}

	[Test]
	[Arguments(new[] { 1, 2, 3 }, 4, new[] { 9 })]
	public async Task BacksertWithIndexGreaterThanSourceLength(int[] seq1, int index, int[] seq2)
	{
		await using var test1 = seq1.AsTestingSequence();
		await using var test2 = seq2.AsTestingSequence();

		var result = test1.Insert(test2, ^index);

		_ = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await result.ElementAtAsync(0));
	}

	[Test]
	[Arguments(new[] { 1, 2, 3 }, 0, new[] { 8, 9 }, new[] { 1, 2, 3, 8, 9 })]
	[Arguments(new[] { 1, 2, 3 }, 1, new[] { 8, 9 }, new[] { 1, 2, 8, 9, 3 })]
	[Arguments(new[] { 1, 2, 3 }, 2, new[] { 8, 9 }, new[] { 1, 8, 9, 2, 3 })]
	[Arguments(new[] { 1, 2, 3 }, 3, new[] { 8, 9 }, new[] { 8, 9, 1, 2, 3 })]
	public async Task Backsert(int[] seq1, int index, int[] seq2, int[] expected)
	{
		await using var test1 = seq1.AsTestingSequence();
		await using var test2 = seq2.AsTestingSequence();

		await test1.Insert(test2, ^index).AssertSequenceEqual(expected);
	}
}

#endif

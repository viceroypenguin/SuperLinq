#if !NO_INDEX

namespace SuperLinq.Async.Tests;

public sealed class ReplaceTest
{
	[Test]
	public void ReplaceIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().Replace(0, 10);
		_ = new AsyncBreakingSequence<int>().Replace(new Index(0), 10);
		_ = new AsyncBreakingSequence<int>().Replace(^0, 10);
	}

	[Test]
	public async Task ReplaceEmptySequence()
	{
		await using var seq = Enumerable.Empty<int>().AsTestingSequence(maxEnumerations: 6);
		await seq.Replace(0, 10).AssertSequenceEqual();
		await seq.Replace(10, 10).AssertSequenceEqual();
		await seq.Replace(new Index(0), 10).AssertSequenceEqual();
		await seq.Replace(new Index(10), 10).AssertSequenceEqual();
		await seq.Replace(^0, 10).AssertSequenceEqual();
		await seq.Replace(^10, 10).AssertSequenceEqual();
	}

	public static IEnumerable<int> Indices() =>
		Enumerable.Range(0, 10);

	[Test]
	[MethodDataSource(nameof(Indices))]
	public async Task ReplaceIntIndex(int index)
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.Replace(index, 30);
		await result.AssertSequenceEqual(
			Enumerable.Range(1, index)
				.Concat([30])
				.Concat(Enumerable.Range(index + 2, 9 - index)));
	}

	[Test]
	[MethodDataSource(nameof(Indices))]
	public async Task ReplaceStartIndex(int index)
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.Replace(new Index(index), 30);
		await result.AssertSequenceEqual(
			Enumerable.Range(1, index)
				.Concat([30])
				.Concat(Enumerable.Range(index + 2, 9 - index)));
	}

	[Test]
	[MethodDataSource(nameof(Indices))]
	public async Task ReplaceEndIndex(int index)
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.Replace(^index, 30);
		await result.AssertSequenceEqual(
			Enumerable.Range(1, 9 - index)
				.Concat([30])
				.Concat(Enumerable.Range(11 - index, index)));
	}

	[Test]
	public async Task ReplaceIntIndexPastSequenceLength()
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.Replace(10, 30);
		await result.AssertSequenceEqual(Enumerable.Range(1, 10));
	}

	[Test]
	public async Task ReplaceStartIndexPastSequenceLength()
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.Replace(new Index(10), 30);
		await result.AssertSequenceEqual(Enumerable.Range(1, 10));
	}

	[Test]
	public async Task ReplaceEndIndexPastSequenceLength()
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.Replace(^10, 30);
		await result.AssertSequenceEqual(Enumerable.Range(1, 10));
	}
}

#endif

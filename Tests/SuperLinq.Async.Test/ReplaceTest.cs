namespace Test.Async;

public class ReplaceTest
{
	[Fact]
	public void ReplaceIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().Replace(0, 10);
		_ = new AsyncBreakingSequence<int>().Replace(new Index(0), 10);
		_ = new AsyncBreakingSequence<int>().Replace(^0, 10);
	}

	[Fact]
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

	public static IEnumerable<object[]> Indices() =>
		Enumerable.Range(0, 10).Select(i => new object[] { i, });

	[Theory, MemberData(nameof(Indices))]
	public async Task ReplaceIntIndex(int index)
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.Replace(index, 30);
		await result.AssertSequenceEqual(
			Enumerable.Range(1, index)
				.Append(30)
				.Concat(Enumerable.Range(index + 2, 9 - index)));
	}

	[Theory, MemberData(nameof(Indices))]
	public async Task ReplaceStartIndex(int index)
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.Replace(new Index(index), 30);
		await result.AssertSequenceEqual(
			Enumerable.Range(1, index)
				.Append(30)
				.Concat(Enumerable.Range(index + 2, 9 - index)));
	}

	[Theory, MemberData(nameof(Indices))]
	public async Task ReplaceEndIndex(int index)
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.Replace(^index, 30);
		await result.AssertSequenceEqual(
			Enumerable.Range(1, 9 - index)
				.Append(30)
				.Concat(Enumerable.Range(11 - index, index)));
	}

	[Fact]
	public async Task ReplaceIntIndexPastSequenceLength()
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.Replace(10, 30);
		await result.AssertSequenceEqual(Enumerable.Range(1, 10));
	}

	[Fact]
	public async Task ReplaceStartIndexPastSequenceLength()
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.Replace(new Index(10), 30);
		await result.AssertSequenceEqual(Enumerable.Range(1, 10));
	}

	[Fact]
	public async Task ReplaceEndIndexPastSequenceLength()
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.Replace(^10, 30);
		await result.AssertSequenceEqual(Enumerable.Range(1, 10));
	}
}

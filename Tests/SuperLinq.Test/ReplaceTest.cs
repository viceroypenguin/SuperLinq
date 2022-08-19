namespace Test;

public class ReplaceTest
{
	[Fact]
	public void ReplaceIsLazy()
	{
		new BreakingSequence<int>().Replace(0, 10);
		new BreakingSequence<int>().Replace(^0, 10);
	}

	[Fact]
	public void ReplaceEmptySequence()
	{
		Assert.Empty(Enumerable.Empty<int>().Replace(0, 10));
		Assert.Empty(Enumerable.Empty<int>().Replace(10, 10));
		Assert.Empty(Enumerable.Empty<int>().Replace(^0, 10));
		Assert.Empty(Enumerable.Empty<int>().Replace(^10, 10));
	}

	[Fact]
	public void ReplaceStartIndex()
	{
		for (var i = 0; i < 10; i++)
			Enumerable.Range(1, 10).Replace(i, 30)
				.AssertSequenceEqual(
					Enumerable.Range(1, i)
						.Append(30)
						.Concat(Enumerable.Range(i + 2, 9 - i)));
	}

	[Fact]
	public void ReplaceEndIndex()
	{
		for (var i = 0; i < 10; i++)
			Enumerable.Range(1, 10).Replace(^i, 30)
				.AssertSequenceEqual(
					Enumerable.Range(1, 9 - i)
						.Append(30)
						.Concat(Enumerable.Range(11 - i, i)));
	}

	[Fact]
	public void ReplaceStartIndexPastSequenceLength()
	{
		Enumerable.Range(1, 10).Replace(10, 30)
			.AssertSequenceEqual(Enumerable.Range(1, 10));
	}

	[Fact]
	public void ReplaceEndIndexPastSequenceLength()
	{
		Enumerable.Range(1, 10).Replace(^10, 30)
			.AssertSequenceEqual(Enumerable.Range(1, 10));
	}
}

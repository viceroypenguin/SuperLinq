namespace Test;

public class AssertCountTest
{
	[Fact]
	public void AssertCountIsLazy()
	{
		_ = new BreakingSequence<object>().AssertCount(0);
	}

	[Fact]
	public void AssertCountNegativeCount()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>("count",
			() => new BreakingSequence<int>().AssertCount(-1));
	}

	public static IEnumerable<object[]> GetSequences() =>
		Enumerable.Range(1, 10)
			.GetAllSequences()
			.Select(x => new object[] { x });

	[Theory]
	[MemberData(nameof(GetSequences))]
	public void AssertCountShortSequence(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.AssertCount(11);
			_ = Assert.Throws<ArgumentException>("source.Count()",
				() => result.Consume());
		}
	}

	[Theory]
	[MemberData(nameof(GetSequences))]
	public void AssertCountEqualSequence(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.AssertCount(10);
			result.AssertSequenceEqual(
				Enumerable.Range(1, 10),
				testCollectionEnumerable: true);
		}
	}

	[Theory]
	[MemberData(nameof(GetSequences))]
	public void AssertCountLongSequence(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.AssertCount(9);
			_ = Assert.Throws<ArgumentException>("source.Count()",
				() => result.Consume());
		}
	}

	[Fact]
	public void AssertCountCollectionBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingCollection();

		var result = seq.AssertCount(10_000);
		Assert.Equal(10_000, result.Count());
		result.AssertSequenceEqual(Enumerable.Range(0, 10_000));
	}

	[Fact]
	public void AssertCountListBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq.AssertCount(10_000);
		Assert.Equal(10_000, result.Count());
		Assert.Equal(200, result.ElementAt(200));
		Assert.Equal(1_200, result.ElementAt(1_200));
		Assert.Equal(8_800, result.ElementAt(^1_200));

		_ = Assert.Throws<ArgumentOutOfRangeException>(
			"index",
			() => result.ElementAt(40_001));
	}

	[Fact]
	public void AssertCountUsesCollectionCountAtIterationTime()
	{
		var stack = new Stack<int>(Enumerable.Range(1, 3));
		var result = stack.AssertCount(4);
		_ = Assert.Throws<ArgumentException>("source.Count()",
			() => result.Consume());
		stack.Push(4);
		result.Consume();
	}
}

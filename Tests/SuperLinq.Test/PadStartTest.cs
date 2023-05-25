namespace Test;

public class PadStartTest
{
	[Fact]
	public void PadStartNegativeWidth()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<object>().PadStart(-1));
	}

	[Fact]
	public void PadStartIsLazy()
	{
		_ = new BreakingSequence<object>().PadStart(0);
		_ = new BreakingSequence<object>().PadStart(0, new object());
		_ = new BreakingSequence<object>().PadStart(0, BreakingFunc.Of<int, object>());
	}

	public static IEnumerable<object[]> GetIntSequences() =>
		Seq(123, 456, 789)
			.GetAllSequences()
			.Select(x => new object[] { x, });

	[Theory]
	[MemberData(nameof(GetIntSequences))]
	public void PadStartWideSourceSequence(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.PadStart(2);
			result.AssertSequenceEqual(
				Seq(123, 456, 789),
				testCollectionEnumerable: true);
		}
	}

	[Fact]
	public void PadStartWideCollectionBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingCollection();

		var result = seq.PadStart(5_000, x => x % 1_000);
		Assert.Equal(10_000, result.Count());
	}

	[Fact]
	public void PadStartWideListBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq.PadStart(5_000, x => x % 1_000);
		Assert.Equal(10_000, result.Count());
		Assert.Equal(1_200, result.ElementAt(1_200));
		Assert.Equal(8_800, result.ElementAt(^1_200));

		_ = Assert.Throws<ArgumentOutOfRangeException>(
			"index",
			() => result.ElementAt(10_001));
	}

	[Theory]
	[MemberData(nameof(GetIntSequences))]
	public void PadStartEqualSourceSequence(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.PadStart(3);
			result.AssertSequenceEqual(
				Seq(123, 456, 789),
				testCollectionEnumerable: true);
		}
	}

	[Theory]
	[MemberData(nameof(GetIntSequences))]
	public void PadStartNarrowSourceSequenceWithDefaultPadding(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.PadStart(5);
			result.AssertSequenceEqual(
				Seq(0, 0, 123, 456, 789),
				testCollectionEnumerable: true);
		}
	}

	[Theory]
	[MemberData(nameof(GetIntSequences))]
	public void PadStartNarrowSourceSequenceWithNonDefaultPadding(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.PadStart(5, -1);
			result.AssertSequenceEqual(
				Seq(-1, -1, 123, 456, 789),
				testCollectionEnumerable: true);
		}
	}

	[Fact]
	public void PadStartNarrowCollectionBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingCollection();

		var result = seq.PadStart(40_000, x => x % 1_000);
		Assert.Equal(40_000, result.Count());
	}

	[Fact]
	public void PadStartNarrowListBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq.PadStart(40_000, x => x % 1_000);
		Assert.Equal(40_000, result.Count());
		Assert.Equal(200, result.ElementAt(1_200));
		Assert.Equal(1_200, result.ElementAt(31_200));
		Assert.Equal(8_800, result.ElementAt(^1_200));

		_ = Assert.Throws<ArgumentOutOfRangeException>(
			"index",
			() => result.ElementAt(40_001));
	}

	public static IEnumerable<object[]> GetCharSequences() =>
		"hello".AsEnumerable()
			.GetAllSequences()
			.Select(x => new object[] { x, });

	[Theory]
	[MemberData(nameof(GetCharSequences))]
	public void PadStartNarrowSourceSequenceWithDynamicPadding(IDisposableEnumerable<char> seq)
	{
		using (seq)
		{
			var result = seq.PadStart(15, i => i % 2 == 0 ? '+' : '-');
			result.AssertSequenceEqual(
				"+-+-+-+-+-hello",
				testCollectionEnumerable: true);
		}
	}

	[Fact]
	public void PadStartUsesCollectionCountAtIterationTime()
	{
		var queue = new Queue<int>(Enumerable.Range(1, 3));
		var result = queue.PadStart(5, -1);
		result.AssertSequenceEqual(-1, -1, 1, 2, 3);
		queue.Enqueue(4);
		result.AssertSequenceEqual(-1, 1, 2, 3, 4);
	}
}

namespace SuperLinq.Tests;

public sealed class PadStartTest
{
	[Test]
	public void PadStartNegativeWidth()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<object>().PadStart(-1));
	}

	[Test]
	public void PadStartIsLazy()
	{
		_ = new BreakingSequence<object>().PadStart(0);
		_ = new BreakingSequence<object>().PadStart(0, new object());
		_ = new BreakingSequence<object>().PadStart(0, BreakingFunc.Of<int, object>());
	}

	public static IEnumerable<IDisposableEnumerable<int>> GetIntSequences() =>
		Seq(123, 456, 789)
			.GetAllSequences();

	[Test]
	[MethodDataSource(nameof(GetIntSequences))]
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

	[Test]
	public void PadStartWideCollectionBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingCollection();

		var result = seq.PadStart(5_000, x => x % 1_000);
		result.AssertCollectionErrorChecking(10_000);
	}

	[Test]
	public void PadStartWideListBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq.PadStart(5_000, x => x % 1_000);
		result.AssertCollectionErrorChecking(10_000);
		result.AssertListElementChecking(10_000);

		Assert.Equal(1_200, result.ElementAt(1_200));
#if !NO_INDEX
		Assert.Equal(8_800, result.ElementAt(^1_200));
#endif
	}

	[Test]
	[MethodDataSource(nameof(GetIntSequences))]
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

	[Test]
	[MethodDataSource(nameof(GetIntSequences))]
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

	[Test]
	[MethodDataSource(nameof(GetIntSequences))]
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

	[Test]
	public void PadStartNarrowCollectionBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingCollection();

		var result = seq.PadStart(40_000, x => x % 1_000);
		result.AssertCollectionErrorChecking(40_000);
	}

	[Test]
	public void PadStartNarrowListBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq.PadStart(40_000, x => x % 1_000);
		result.AssertCollectionErrorChecking(40_000);
		result.AssertListElementChecking(40_000);

		Assert.Equal(200, result.ElementAt(1_200));
		Assert.Equal(1_200, result.ElementAt(31_200));
#if !NO_INDEX
		Assert.Equal(8_800, result.ElementAt(^1_200));
#endif
	}

	public static IEnumerable<IDisposableEnumerable<char>> GetCharSequences() =>
		"hello".AsEnumerable()
			.GetAllSequences();

	[Test]
	[MethodDataSource(nameof(GetCharSequences))]
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

	[Test]
	public void PadStartUsesCollectionCountAtIterationTime()
	{
		var queue = new Queue<int>(Enumerable.Range(1, 3));
		var result = queue.PadStart(5, -1);
		result.AssertSequenceEqual(-1, -1, 1, 2, 3);
		queue.Enqueue(4);
		result.AssertSequenceEqual(-1, 1, 2, 3, 4);
	}
}

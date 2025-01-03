#if !NO_INDEX

namespace SuperLinq.Tests;

/// <summary>
/// Verify the behavior of the Exclude operator
/// </summary>
public sealed class ExcludeTests
{
	[Test]
	public void TestExcludeIsLazy()
	{
		_ = new BreakingSequence<int>().Exclude(0, 10);
	}

	[Test]
	public void TestExcludeNegativeStartIndexException()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().Exclude(-10, 10));
	}

	[Test]
	public void TestExcludeNegativeCountException()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().Exclude(0, -5));
	}

	[Test]
	public void TestExcludeWithCountEqualsZero()
	{
		using var sequence = Enumerable.Range(1, 10).AsTestingSequence();

		var result = sequence.Exclude(5, 0);
		result.AssertSequenceEqual(Enumerable.Range(1, 10));
	}

	public static IEnumerable<IDisposableEnumerable<int>> GetIntSequences() =>
		Enumerable.Range(1, 100)
			.GetAllSequences();

	[Test]
	[MethodDataSource(nameof(GetIntSequences))]
	public void TestExcludeSequenceHead(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Exclude(0, 5);
			result.AssertSequenceEqual(Enumerable.Range(6, 95));
		}
	}

	[Test]
	[MethodDataSource(nameof(GetIntSequences))]
	public void TestExcludeSequenceTail(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Exclude(95, 10);
			result.AssertSequenceEqual(Enumerable.Range(1, 95));
		}
	}

	[Test]
	[MethodDataSource(nameof(GetIntSequences))]
	public void TestExcludeSequenceMiddle(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Exclude(30, 50);
			result.AssertSequenceEqual(
				Enumerable.Range(1, 30)
					.Concat(Enumerable.Range(81, 20)));
		}
	}

	[Test]
	[MethodDataSource(nameof(GetIntSequences))]
	public void TestExcludeEntireSequence(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Exclude(0, 101);
			result.AssertSequenceEqual();
		}
	}

	[Test]
	[MethodDataSource(nameof(GetIntSequences))]
	public void TestExcludeCountGreaterThanSequenceLength(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Exclude(1, 10 * 10);
			result.AssertSequenceEqual(1);
		}
	}

	[Test]
	[MethodDataSource(nameof(GetIntSequences))]
	public void TestExcludeStartIndexGreaterThanSequenceLength(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Exclude(101, 10);
			result.AssertSequenceEqual(
				Enumerable.Range(1, 100));
		}
	}

	[Test]
	public void ExcludeListBehaviorMid()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq.Exclude(1_001, 1_000);
		result.AssertCollectionErrorChecking(9_000);
		result.AssertListElementChecking(9_000);

		Assert.Equal(10, result.ElementAt(10));
		Assert.Equal(1_000, result.ElementAt(1_000));
		Assert.Equal(2_001, result.ElementAt(1_001));
		Assert.Equal(9_950, result.ElementAt(^50));
		Assert.Equal(9_950, result.ElementAt(8_950));
	}

	[Test]
	public void ExcludeListBehaviorEnd()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq.Exclude(9_500, 1_000);
		result.AssertCollectionErrorChecking(9_500);
		result.AssertListElementChecking(9_500);

		Assert.Equal(10, result.ElementAt(10));
		Assert.Equal(9_450, result.ElementAt(^50));
	}

	[Test]
	public void ExcludeListBehaviorAfter()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq.Exclude(15_000, 1_000);
		result.AssertCollectionErrorChecking(10_000);
		result.AssertListElementChecking(10_000);

		Assert.Equal(10, result.ElementAt(10));
		Assert.Equal(9_950, result.ElementAt(^50));
	}

	[Test]
	public void ExcludeListBehaviorEntire()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq.Exclude(0, 10_000);
		result.AssertCollectionErrorChecking(0);

		_ = Assert.Throws<ArgumentOutOfRangeException>(
			"index",
			() => result.ElementAt(0));
	}

	[Test]
	public void ExcludeCollectionBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingCollection();
		var result = seq.Exclude(0, 1_000);

		result.AssertCollectionErrorChecking(9_000);
	}

	public static IEnumerable<(Range range, bool shouldThrow, bool __, int[] expected)> GetExcludeRangeCases() =>
		[
			(3..7, false, false, [0, 1, 2, 7, 8, 9]),
			(3..3, false, false, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]),
			(3..2, true, true, []),
			(3..15, false, false, [0, 1, 2]),

			(3..^3, false, false, [0, 1, 2, 7, 8, 9]),
			(6..^3, false, false, [0, 1, 2, 3, 4, 5, 7, 8, 9]),
			(7..^3, false, false, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]),
			(8..^3, false, true, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]),
			(15..^3, false, true, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]),
			(3..^15, false, true, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]),

			(^7..2, false, true, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]),
			(^7..3, false, false, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]),
			(^7..4, false, false, [0, 1, 2, 4, 5, 6, 7, 8, 9]),
			(^7..7, false, false, [0, 1, 2, 7, 8, 9]),
			(^7..15, false, true, [0, 1, 2]),
			(^15..7, false, true, [7, 8, 9]),

			(^2..^3, true, true, []),
			(^3..^3, false, false, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]),
			(^7..^3, false, false, [0, 1, 2, 7, 8, 9]),
			(^15..^3, false, true, [7, 8, 9]),
		];

	[Test]
	[MethodDataSource(nameof(GetExcludeRangeCases))]
	public void ExcludeRangeBehavior(Range range, bool shouldThrow, bool __, int[] expected)
	{
		using var ts = Enumerable.Range(0, 10)
			.AsTestingSequence();

		if (shouldThrow)
		{
			_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
				ts.Exclude(range));
			return;
		}

		var result = ts.Exclude(range);

		result.AssertSequenceEqual(expected);
	}

	[Test]
	[MethodDataSource(nameof(GetExcludeRangeCases))]
	public void ExcludeRangeCollectionBehavior(Range range, bool __, bool shouldThrow, int[] expected)
	{
		using var ts = Enumerable.Range(0, 10)
			.AsTestingCollection();

		if (shouldThrow)
		{
			_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
				ts.Exclude(range));
			return;
		}

		var result = ts.Exclude(range);

		result.AssertSequenceEqual(expected);
	}
}

#endif

namespace Test;

public class PadTest
{
	[Fact]
	public void PadNegativeWidth()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<object>().Pad(-1));
	}

	[Fact]
	public void PadIsLazy()
	{
		_ = new BreakingSequence<object>().Pad(0);
		_ = new BreakingSequence<object>().Pad(0, new object());
		_ = new BreakingSequence<object>().Pad(0, BreakingFunc.Of<int, object>());
	}

	public static IEnumerable<object[]> GetIntSequences() =>
		Seq(123, 456, 789)
			.GetAllSequences()
			.Select(x => new object[] { x, });

	[Theory]
	[MemberData(nameof(GetIntSequences))]
	public void PadWideSourceSequence(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Pad(2);
			result.AssertSequenceEqual(
				Seq(123, 456, 789),
				testCollectionEnumerable: true);
		}
	}

	[Fact]
	public void PadWideCollectionBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingCollection();

		var result = seq.Pad(5_000, x => x % 1_000);
		result.AssertCollectionErrorChecking(10_000);
	}

	[Fact]
	public void PadWideListBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq.Pad(5_000, x => x % 1_000);
		result.AssertCollectionErrorChecking(10_000);
		result.AssertListElementChecking(10_000);

		Assert.Equal(1_200, result.ElementAt(1_200));
		Assert.Equal(8_800, result.ElementAt(^1_200));
	}

	[Theory]
	[MemberData(nameof(GetIntSequences))]
	public void PadEqualSourceSequence(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Pad(3);
			result.AssertSequenceEqual(
				Seq(123, 456, 789),
				testCollectionEnumerable: true);
		}
	}

	[Theory]
	[MemberData(nameof(GetIntSequences))]
	public void PadNarrowSourceSequenceWithDefaultPadding(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Pad(5);
			result.AssertSequenceEqual(
				Seq(123, 456, 789, 0, 0),
				testCollectionEnumerable: true);
		}
	}

	[Theory]
	[MemberData(nameof(GetIntSequences))]
	public void PadNarrowSourceSequenceWithNonDefaultPadding(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Pad(5, -1);
			result.AssertSequenceEqual(
				Seq(123, 456, 789, -1, -1),
				testCollectionEnumerable: true);
		}
	}

	[Fact]
	public void PadNarrowCollectionBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingCollection();

		var result = seq.Pad(40_000, x => x % 1_000);
		result.AssertCollectionErrorChecking(40_000);
	}

	[Fact]
	public void PadNarrowListBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq.Pad(40_000, x => x % 1_000);
		result.AssertCollectionErrorChecking(40_000);
		result.AssertListElementChecking(40_000);

		Assert.Equal(1_200, result.ElementAt(1_200));
		Assert.Equal(200, result.ElementAt(11_200));
		Assert.Equal(800, result.ElementAt(^1_200));
	}

	public static IEnumerable<object[]> GetCharSequences() =>
		"hello".AsEnumerable()
			.GetAllSequences()
			.Select(x => new object[] { x, });

	[Theory]
	[MemberData(nameof(GetCharSequences))]
	public void PadNarrowSourceSequenceWithDynamicPadding(IDisposableEnumerable<char> seq)
	{
		using (seq)
		{
			var result = seq.Pad(15, i => i % 2 == 0 ? '+' : '-');
			result.AssertSequenceEqual(
				"hello-+-+-+-+-+",
				testCollectionEnumerable: true);
		}
	}
}

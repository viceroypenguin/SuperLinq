namespace Test;

public sealed class CountDownTest
{
	[Fact]
	public void IsLazy()
	{
		_ = new BreakingSequence<object>()
			.CountDown(42, BreakingFunc.Of<object, int?, object>());
	}

	[Theory]
	[InlineData(0), InlineData(-1)]
	public void ExceptionOnNegativeCount(int param)
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>("count", () =>
			new BreakingSequence<int>().CountDown(param));
	}

	public static IEnumerable<object[]> GetTheoryData()
	{
		var xs = Enumerable.Range(0, 5).ToList();

		for (var i = 1; i <= 7; i++)
		{
			var countdown = i < 5
				? Enumerable.Repeat<int?>(null, 5 - i).Concat(Enumerable.Range(0, i).Select(x => (int?)x).Reverse())
				: Enumerable.Range(0, 5).Select(x => (int?)x).Reverse();

			foreach (var seq in xs.GetAllSequences())
				yield return new object[] { seq, i, xs.EquiZip(countdown) };
		}
	}

	[Theory, MemberData(nameof(GetTheoryData))]
	public void WithSequence(IDisposableEnumerable<int> seq, int count, IEnumerable<(int, int?)> expected)
	{
		using (seq)
		{
			var result = seq.CountDown(count);
			result.AssertSequenceEqual(expected);
		}
	}

	[Fact]
	public void UsesCollectionCountAtIterationTime()
	{
		var stack = new Stack<int>(Enumerable.Range(1, 3));
		var result = stack.CountDown(2, (_, cd) => cd);
		result.AssertSequenceEqual(default(int?), 1, 0);
		stack.Push(4);
		result.AssertSequenceEqual(default(int?), null, 1, 0);
	}

	[Fact]
	public void CountDownCollectionBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingCollection();

		var result = seq.CountDown(20);
		result.AssertCollectionErrorChecking(10_000);
	}

	[Fact]
	public void CountDownListBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq.CountDown(20);
		result.AssertCollectionErrorChecking(10_000);
		result.AssertListElementChecking(10_000);

		Assert.Equal((10, default(int?)), result.ElementAt(10));
		Assert.Equal((50, default(int?)), result.ElementAt(50));
#if !NO_INDEX
		Assert.Equal((9_995, 4), result.ElementAt(^5));
#endif
	}
}

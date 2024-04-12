namespace Test.Async;

public sealed class CountDownTest
{
	[Fact]
	public void IsLazy()
	{
		_ = new AsyncBreakingSequence<object>()
			.CountDown(42, BreakingFunc.Of<object, int?, object>());
	}

	[Theory]
	[InlineData(0), InlineData(-1)]
	public void ExceptionOnNegativeCount(int param)
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>("count", () =>
			new AsyncBreakingSequence<int>().CountDown(param));
	}

	private static IEnumerable<T> GetData<T>(Func<int[], int, int?[], T> selector)
	{
		var xs = Enumerable.Range(0, 5).ToArray();
		yield return selector(xs, 1, [null, null, null, null, 0]);
		yield return selector(xs, 2, [null, null, null, 1, 0]);
		yield return selector(xs, 3, [null, null, 2, 1, 0]);
		yield return selector(xs, 4, [null, 3, 2, 1, 0]);
		yield return selector(xs, 5, [4, 3, 2, 1, 0]);
		yield return selector(xs, 6, [4, 3, 2, 1, 0]);
		yield return selector(xs, 7, [4, 3, 2, 1, 0]);
	}

	public static IEnumerable<object[]> SequenceData { get; } =
		from e in GetData((xs, count, countdown) => new
		{
			Source = xs,
			Count = count,
			Countdown = countdown,
		})
		select new object[] { e.Source, e.Count, e.Source.Zip(e.Countdown, ValueTuple.Create) };

	[Theory, MemberData(nameof(SequenceData))]
	public async Task WithSequence(int[] xs, int count, IEnumerable<(int, int?)> expected)
	{
		await using var ts = xs.Select(SuperEnumerable.Identity).AsTestingSequence();
		await ts.CountDown(count, ValueTuple.Create).AssertSequenceEqual(expected);
	}
}

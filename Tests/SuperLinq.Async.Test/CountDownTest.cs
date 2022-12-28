using SuperLinq;

namespace Test.Async;

public class CountDownTest
{
	[Fact]
	public void IsLazy()
	{
		new AsyncBreakingSequence<object>()
			.CountDown(42, BreakingFunc.Of<object, int?, object>());
	}

	[Fact]
	public void ExceptionOnNegativeCount()
	{
		Assert.Throws<ArgumentOutOfRangeException>("count", () =>
			AsyncSeq(1).CountDown(-1));
	}

	[Fact]
	public Task WithNegativeCount()
	{
		const int count = 10;
		return AsyncEnumerable.Range(1, count)
			.CountDown(-1000, (_, cd) => cd)
			.AssertSequenceEqual(Enumerable.Repeat((int?)null, count));
	}

	private static IEnumerable<T> GetData<T>(Func<int[], int, int?[], T> selector)
	{
		var xs = Enumerable.Range(0, 5).ToArray();
		yield return selector(xs, -1, new int?[] { null, null, null, null, null });
		yield return selector(xs, 0, new int?[] { null, null, null, null, null });
		yield return selector(xs, 1, new int?[] { null, null, null, null, 0 });
		yield return selector(xs, 2, new int?[] { null, null, null, 1, 0 });
		yield return selector(xs, 3, new int?[] { null, null, 2, 1, 0 });
		yield return selector(xs, 4, new int?[] { null, 3, 2, 1, 0 });
		yield return selector(xs, 5, new int?[] { 4, 3, 2, 1, 0 });
		yield return selector(xs, 6, new int?[] { 4, 3, 2, 1, 0 });
		yield return selector(xs, 7, new int?[] { 4, 3, 2, 1, 0 });
	}

	public static IEnumerable<object[]> SequenceData { get; } =
		from e in GetData((xs, count, countdown) => new
		{
			Source = xs,
			Count = count,
			Countdown = countdown,
		})
		select new object[] { e.Source, e.Count, e.Source.Zip(e.Countdown, ValueTuple.Create), };

	[Theory, MemberData(nameof(SequenceData))]
	public async Task WithSequence(int[] xs, int count, IEnumerable<(int, int?)> expected)
	{
		await using var ts = xs.Select(x => x).AsTestingSequence();
		await ts.CountDown(count, ValueTuple.Create).AssertSequenceEqual(expected);
	}
}

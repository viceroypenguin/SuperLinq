namespace SuperLinq.Async.Tests;

public sealed class CountDownTest
{
	[Test]
	public void IsLazy()
	{
		_ = new AsyncBreakingSequence<object>()
			.CountDown(42, BreakingFunc.Of<object, int?, object>());
	}

	[Test]
	[Arguments(0), Arguments(-1)]
	public void ExceptionOnNegativeCount(int param)
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>("count", () =>
			new AsyncBreakingSequence<int>().CountDown(param));
	}

	public static IEnumerable<(IEnumerable<int> seq, int count, IEnumerable<(int, int?)> expected)> GetTestCases()
	{
		var xs = Enumerable.Range(0, 5).ToList();

		for (var i = 1; i <= 7; i++)
		{
			var countdown = i < 5
				? Enumerable.Repeat<int?>(null, 5 - i).Concat(Enumerable.Range(0, i).Cast<int?>().Reverse())
				: Enumerable.Range(0, 5).Cast<int?>().Reverse();

			yield return (xs, i, xs.EquiZip(countdown));
		}
	}

	[Test]
	[MethodDataSource(nameof(GetTestCases))]
	public async Task WithSequence(IEnumerable<int> xs, int count, IEnumerable<(int, int?)> expected)
	{
		await using var ts = xs.Select(SuperEnumerable.Identity).AsTestingSequence();
		await ts.CountDown(count, ValueTuple.Create).AssertSequenceEqual(expected);
	}
}

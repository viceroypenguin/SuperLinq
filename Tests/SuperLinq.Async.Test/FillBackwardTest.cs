namespace Test.Async;

public sealed class FillBackwardTest
{
	[Fact]
	public void FillBackwardIsLazy()
	{
		_ = new AsyncBreakingSequence<object>().FillBackward();
	}

	[Fact]
	public async Task FillBackward()
	{
		await using var input = TestingSequence.Of<int?>(null, null, 1, 2, null, null, null, 3, 4, null, null);

		await input
			.FillBackward()
			.AssertSequenceEqual(1, 1, 1, 2, 3, 3, 3, 3, 4, null, null);
	}

	[Fact]
	public async Task FillBackwardWithFillSelector()
	{
		await using var xs = AsyncSeq(0, 0, 1, 2, 0, 0, 0, 3, 4, 0, 0)
			.Select(x => new { X = x, Y = x })
			.AsTestingSequence();

		await xs
			.FillBackward(e => e.X == 0, (m, nm) => new { m.X, nm.Y })
			.AssertSequenceEqual(
				new { X = 0, Y = 1 },
				new { X = 0, Y = 1 },
				new { X = 1, Y = 1 },
				new { X = 2, Y = 2 },
				new { X = 0, Y = 3 },
				new { X = 0, Y = 3 },
				new { X = 0, Y = 3 },
				new { X = 3, Y = 3 },
				new { X = 4, Y = 4 },
				new { X = 0, Y = 0 },
				new { X = 0, Y = 0 });
	}
}

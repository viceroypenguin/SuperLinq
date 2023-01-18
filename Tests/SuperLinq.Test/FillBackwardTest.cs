namespace Test;

public class FillBackwardTest
{
	[Fact]
	public void FillBackwardIsLazy()
	{
		new BreakingSequence<object>().FillBackward();
	}

	[Fact]
	public void FillBackward()
	{
		using var input = Seq<int?>(null, null, 1, 2, null, null, null, 3, 4, null, null)
			.AsTestingSequence();

		input
			.FillBackward()
			.AssertSequenceEqual(1, 1, 1, 2, 3, 3, 3, 3, 4, null, null);
	}

	[Fact]
	public void FillBackwardWithFillSelector()
	{
		using var xs = Seq(0, 0, 1, 2, 0, 0, 0, 3, 4, 0, 0)
			.Select(x => new { X = x, Y = x })
			.AsTestingSequence();

		xs
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

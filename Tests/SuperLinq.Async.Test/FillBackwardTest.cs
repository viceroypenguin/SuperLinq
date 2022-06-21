namespace Test.Async;

public class FillBackwardTest
{
	[Fact]
	public void FillBackwardIsLazy()
	{
		new AsyncBreakingSequence<object>().FillBackward();
	}

	[Fact]
	public Task FillBackward()
	{
		int? na = null;
		var input = AsyncSeq(na, na, 1, 2, na, na, na, 3, 4, na, na);
		var result = input.FillBackward();
		return result.AssertSequenceEqual(1, 1, 1, 2, 3, 3, 3, 3, 4, na, na);
	}

	[Fact]
	public Task FillBackwardWithFillSelector()
	{
		var xs = AsyncSeq(0, 0, 1, 2, 0, 0, 0, 3, 4, 0, 0);

		var result =
			xs.Select(x => new { X = x, Y = x })
			  .FillBackward(e => e.X == 0, (m, nm) => new { m.X, nm.Y });

		var expected = new[]
		{
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
			new { X = 0, Y = 0 },
		};
		return result.AssertSequenceEqual(expected);
	}
}

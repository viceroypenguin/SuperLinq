namespace Test;

public class SequenceTest
{
	[Theory]
	[InlineData(-10, -4)]
	[InlineData(-1, 5)]
	[InlineData(1, 10)]
	[InlineData(30, 55)]
	[InlineData(27, 172)]
	public void SequenceWithAscendingRange(int start, int stop)
	{
		var result = SuperEnumerable.Sequence(start, stop);
		var expectations = Enumerable.Range(start, stop - start + 1);

		Assert.Equal(expectations, result);
	}

	[Theory]
	[InlineData(-4, -10)]
	[InlineData(5, -1)]
	[InlineData(10, 1)]
	[InlineData(55, 30)]
	[InlineData(172, 27)]
	public void SequenceWithDescendingRange(int start, int stop)
	{
		var result = SuperEnumerable.Sequence(start, stop);
		var expectations = Enumerable.Range(stop, start - stop + 1).Reverse();

		Assert.Equal(expectations, result);
	}

	[Theory]
	[InlineData(-10, -4, 2)]
	[InlineData(-1, 5, 3)]
	[InlineData(1, 10, 1)]
	[InlineData(30, 55, 4)]
	[InlineData(27, 172, 9)]
	public void SequenceWithAscendingRangeAscendingStep(int start, int stop, int step)
	{
		var result = SuperEnumerable.Sequence(start, stop, step);
		var expectations = Enumerable.Range(start, stop - start + 1).TakeEvery(step);

		Assert.Equal(expectations, result);
	}

	[Theory]
	[InlineData(-10, -4, -2)]
	[InlineData(-1, 5, -3)]
	[InlineData(1, 10, -1)]
	[InlineData(30, 55, -4)]
	[InlineData(27, 172, -9)]
	public void SequenceWithAscendingRangeDescendigStep(int start, int stop, int step)
	{
		var result = SuperEnumerable.Sequence(start, stop, step);

		Assert.Empty(result);
	}

	[Theory]
	[InlineData(-4, -10, 2)]
	[InlineData(5, -1, 3)]
	[InlineData(10, 1, 1)]
	[InlineData(55, 30, 4)]
	[InlineData(172, 27, 9)]
	public void SequenceWithDescendingRangeAscendingStep(int start, int stop, int step)
	{
		var result = SuperEnumerable.Sequence(start, stop, step);

		Assert.Empty(result);
	}

	[Theory]
	[InlineData(-4, -10, -2)]
	[InlineData(5, -1, -3)]
	[InlineData(10, 1, -1)]
	[InlineData(55, 30, -4)]
	[InlineData(172, 27, -9)]
	public void SequenceWithDescendingRangeDescendigStep(int start, int stop, int step)
	{
		var result = SuperEnumerable.Sequence(start, stop, step);
		var expectations = Enumerable.Range(stop, start - stop + 1).Reverse().TakeEvery(Math.Abs(step));

		Assert.Equal(expectations, result);
	}

	[Theory]
	[InlineData(int.MaxValue, int.MaxValue, -1)]
	[InlineData(int.MaxValue, int.MaxValue, 1)]
	[InlineData(int.MaxValue, int.MaxValue, null)]
	[InlineData(0, 0, -1)]
	[InlineData(0, 0, 1)]
	[InlineData(0, 0, null)]
	[InlineData(int.MinValue, int.MinValue, -1)]
	[InlineData(int.MinValue, int.MinValue, 1)]
	[InlineData(int.MinValue, int.MinValue, null)]
	public void SequenceWithStartEqualsStop(int start, int stop, int? step)
	{
		var result = step.HasValue ? SuperEnumerable.Sequence(start, stop, step.Value)
								   : SuperEnumerable.Sequence(start, stop);

		Assert.Equal(result.Single(), start);
	}

	[Theory]
	[InlineData(int.MaxValue - 1, int.MaxValue, 1, 2)]
	[InlineData(int.MinValue + 1, int.MinValue, -1, 2)]
	[InlineData(0, int.MaxValue, 10000000, (int.MaxValue / 10000000) + 1)]
	[InlineData(int.MinValue, int.MaxValue, int.MaxValue, 3)]
	public void SequenceEdgeCases(int start, int stop, int step, int count)
	{
		var result = SuperEnumerable.Sequence(start, stop, step);

		Assert.Equal(result.Count(), count);
	}

	[Theory]
	[InlineData(5, 10)]
	[InlineData(int.MaxValue, int.MaxValue)]
	[InlineData(int.MinValue, int.MaxValue)]
	public void SequenceWithStepZero(int start, int stop)
	{
		var result = SuperEnumerable.Sequence(start, stop, 0);

		Assert.True(result.Take(100).All(x => x == start));
	}
}

﻿namespace Test.Async;

public class SequenceTest
{
	[Theory]
	[InlineData(65600, 65536, 65536)]
	[InlineData(0, -1, 2)]
	[InlineData(-65600, 65536, -65536)]
	[InlineData(int.MaxValue - 1, 2, 2)]
	public void RangeThrowsOutOfRange(int start, int count, int step)
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			AsyncSuperEnumerable.Range(start, count, step));
	}

	[Theory]
	[InlineData(1, 2, 3)]
	[InlineData(1, 2, -3)]
	[InlineData(-32, 10, 30)]
	[InlineData(32, 10, -30)]
	[InlineData(-32, 10, 0)]
	[InlineData(32, 10, -0)]
	[InlineData(10, 0, 12)]
	[InlineData(-10, 0, 12)]
	public Task Range(int start, int count, int step)
	{
		var result = AsyncSuperEnumerable.Range(start, count, step);
		var expectations = Enumerable.Range(0, count)
			.Select(i => start + (step * i));

		return result.AssertSequenceEqual(expectations);
	}


	[Theory]
	[InlineData(-10, -4)]
	[InlineData(-1, 5)]
	[InlineData(1, 10)]
	[InlineData(30, 55)]
	[InlineData(27, 172)]
	public Task SequenceWithAscendingRange(int start, int stop)
	{
		var result = AsyncSuperEnumerable.Sequence(start, stop);
		var expectations = Enumerable.Range(start, stop - start + 1);

		return result.AssertSequenceEqual(expectations);
	}

	[Theory]
	[InlineData(-4, -10)]
	[InlineData(5, -1)]
	[InlineData(10, 1)]
	[InlineData(55, 30)]
	[InlineData(172, 27)]
	public Task SequenceWithDescendingRange(int start, int stop)
	{
		var result = AsyncSuperEnumerable.Sequence(start, stop);
		var expectations = Enumerable.Range(stop, start - stop + 1).Reverse();

		return result.AssertSequenceEqual(expectations);
	}

	[Theory]
	[InlineData(-10, -4, 2)]
	[InlineData(-1, 5, 3)]
	[InlineData(1, 10, 1)]
	[InlineData(30, 55, 4)]
	[InlineData(27, 172, 9)]
	public Task SequenceWithAscendingRangeAscendingStep(int start, int stop, int step)
	{
		var result = AsyncSuperEnumerable.Sequence(start, stop, step);
		var expectations = Enumerable.Range(start, stop - start + 1).TakeEvery(step);

		return result.AssertSequenceEqual(expectations);
	}

	[Theory]
	[InlineData(-10, -4, -2)]
	[InlineData(-1, 5, -3)]
	[InlineData(1, 10, -1)]
	[InlineData(30, 55, -4)]
	[InlineData(27, 172, -9)]
	public Task SequenceWithAscendingRangeDescendigStep(int start, int stop, int step)
	{
		var result = AsyncSuperEnumerable.Sequence(start, stop, step);

		return result.AssertEmpty();
	}

	[Theory]
	[InlineData(-4, -10, 2)]
	[InlineData(5, -1, 3)]
	[InlineData(10, 1, 1)]
	[InlineData(55, 30, 4)]
	[InlineData(172, 27, 9)]
	public Task SequenceWithDescendingRangeAscendingStep(int start, int stop, int step)
	{
		var result = AsyncSuperEnumerable.Sequence(start, stop, step);

		return result.AssertEmpty();
	}

	[Theory]
	[InlineData(-4, -10, -2)]
	[InlineData(5, -1, -3)]
	[InlineData(10, 1, -1)]
	[InlineData(55, 30, -4)]
	[InlineData(172, 27, -9)]
	public Task SequenceWithDescendingRangeDescendigStep(int start, int stop, int step)
	{
		var result = AsyncSuperEnumerable.Sequence(start, stop, step);
		var expectations = Enumerable.Range(stop, start - stop + 1).Reverse().TakeEvery(Math.Abs(step));

		return result.AssertSequenceEqual(expectations);
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
	public async Task SequenceWithStartEqualsStop(int start, int stop, int? step)
	{
		var result = step.HasValue ? AsyncSuperEnumerable.Sequence(start, stop, step.Value)
								   : AsyncSuperEnumerable.Sequence(start, stop);

		Assert.Equal(await result.SingleAsync(), start);
	}

	[Theory]
	[InlineData(int.MaxValue - 1, int.MaxValue, 1, 2)]
	[InlineData(int.MinValue + 1, int.MinValue, -1, 2)]
	[InlineData(0, int.MaxValue, 10000000, (int.MaxValue / 10000000) + 1)]
	[InlineData(int.MinValue, int.MaxValue, int.MaxValue, 3)]
	public async Task SequenceEdgeCases(int start, int stop, int step, int count)
	{
		var result = AsyncSuperEnumerable.Sequence(start, stop, step);

		Assert.Equal(await result.CountAsync(), count);
	}

	[Theory]
	[InlineData(5, 10)]
	[InlineData(int.MaxValue, int.MaxValue)]
	[InlineData(int.MinValue, int.MaxValue)]
	public async Task SequenceWithStepZero(int start, int stop)
	{
		var result = AsyncSuperEnumerable.Sequence(start, stop, 0);

		Assert.True(await result.Take(100).AllAsync(x => x == start));
	}
}

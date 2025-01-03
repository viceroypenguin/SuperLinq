namespace SuperLinq.Async.Tests;

public sealed class SequenceTest
{
	[Test]
	[Arguments(65600, 65536, 65536)]
	[Arguments(0, -1, 2)]
	[Arguments(-65600, 65536, -65536)]
	[Arguments(int.MaxValue - 1, 2, 2)]
	public void RangeThrowsOutOfRange(int start, int count, int step)
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			AsyncSuperEnumerable.Range(start, count, step));
	}

	[Test]
	[Arguments(1, 2, 3)]
	[Arguments(1, 2, -3)]
	[Arguments(-32, 10, 30)]
	[Arguments(32, 10, -30)]
	[Arguments(-32, 10, 0)]
	[Arguments(32, 10, -0)]
	[Arguments(10, 0, 12)]
	[Arguments(-10, 0, 12)]
	public Task Range(int start, int count, int step)
	{
		var result = AsyncSuperEnumerable.Range(start, count, step);
		var expectations = Enumerable.Range(0, count)
			.Select(i => start + (step * i));

		return result.AssertSequenceEqual(expectations);
	}

	[Test]
	[Arguments(-10, -4)]
	[Arguments(-1, 5)]
	[Arguments(1, 10)]
	[Arguments(30, 55)]
	[Arguments(27, 172)]
	public Task SequenceWithAscendingRange(int start, int stop)
	{
		var result = AsyncSuperEnumerable.Sequence(start, stop);
		var expectations = Enumerable.Range(start, stop - start + 1);

		return result.AssertSequenceEqual(expectations);
	}

	[Test]
	[Arguments(-4, -10)]
	[Arguments(5, -1)]
	[Arguments(10, 1)]
	[Arguments(55, 30)]
	[Arguments(172, 27)]
	public Task SequenceWithDescendingRange(int start, int stop)
	{
		var result = AsyncSuperEnumerable.Sequence(start, stop);
		var expectations = Enumerable.Range(stop, start - stop + 1).Reverse();

		return result.AssertSequenceEqual(expectations);
	}

	[Test]
	[Arguments(-10, -4, 2)]
	[Arguments(-1, 5, 3)]
	[Arguments(1, 10, 1)]
	[Arguments(30, 55, 4)]
	[Arguments(27, 172, 9)]
	public Task SequenceWithAscendingRangeAscendingStep(int start, int stop, int step)
	{
		var result = AsyncSuperEnumerable.Sequence(start, stop, step);
		var expectations = Enumerable.Range(start, stop - start + 1).TakeEvery(step);

		return result.AssertSequenceEqual(expectations);
	}

	[Test]
	[Arguments(-10, -4, -2)]
	[Arguments(-1, 5, -3)]
	[Arguments(1, 10, -1)]
	[Arguments(30, 55, -4)]
	[Arguments(27, 172, -9)]
	public Task SequenceWithAscendingRangeDescendigStep(int start, int stop, int step)
	{
		var result = AsyncSuperEnumerable.Sequence(start, stop, step);

		return result.AssertEmpty();
	}

	[Test]
	[Arguments(-4, -10, 2)]
	[Arguments(5, -1, 3)]
	[Arguments(10, 1, 1)]
	[Arguments(55, 30, 4)]
	[Arguments(172, 27, 9)]
	public Task SequenceWithDescendingRangeAscendingStep(int start, int stop, int step)
	{
		var result = AsyncSuperEnumerable.Sequence(start, stop, step);

		return result.AssertEmpty();
	}

	[Test]
	[Arguments(-4, -10, -2)]
	[Arguments(5, -1, -3)]
	[Arguments(10, 1, -1)]
	[Arguments(55, 30, -4)]
	[Arguments(172, 27, -9)]
	public Task SequenceWithDescendingRangeDescendigStep(int start, int stop, int step)
	{
		var result = AsyncSuperEnumerable.Sequence(start, stop, step);
		var expectations = Enumerable.Range(stop, start - stop + 1).Reverse().TakeEvery(Math.Abs(step));

		return result.AssertSequenceEqual(expectations);
	}

	[Test]
	[Arguments(int.MaxValue, int.MaxValue, -1)]
	[Arguments(int.MaxValue, int.MaxValue, 1)]
	[Arguments(int.MaxValue, int.MaxValue, null)]
	[Arguments(0, 0, -1)]
	[Arguments(0, 0, 1)]
	[Arguments(0, 0, null)]
	[Arguments(int.MinValue, int.MinValue, -1)]
	[Arguments(int.MinValue, int.MinValue, 1)]
	[Arguments(int.MinValue, int.MinValue, null)]
	public async Task SequenceWithStartEqualsStop(int start, int stop, int? step)
	{
		var result = step.HasValue ? AsyncSuperEnumerable.Sequence(start, stop, step.Value)
								   : AsyncSuperEnumerable.Sequence(start, stop);

		Assert.Equal(await result.SingleAsync(), start);
	}

	[Test]
	[Arguments(int.MaxValue - 1, int.MaxValue, 1, 2)]
	[Arguments(int.MinValue + 1, int.MinValue, -1, 2)]
	[Arguments(0, int.MaxValue, 10000000, (int.MaxValue / 10000000) + 1)]
	[Arguments(int.MinValue, int.MaxValue, int.MaxValue, 3)]
	public async Task SequenceEdgeCases(int start, int stop, int step, int count)
	{
		var result = AsyncSuperEnumerable.Sequence(start, stop, step);

		Assert.Equal(await result.CountAsync(), count);
	}

	[Test]
	[Arguments(5, 10)]
	[Arguments(int.MaxValue, int.MaxValue)]
	[Arguments(int.MinValue, int.MaxValue)]
	public async Task SequenceWithStepZero(int start, int stop)
	{
		var result = AsyncSuperEnumerable.Sequence(start, stop, 0);

		Assert.True(await result.Take(100).AllAsync(x => x == start));
	}
}

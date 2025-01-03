namespace SuperLinq.Tests;

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
			SuperEnumerable.Range(start, count, step));
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
	public void Range(int start, int count, int step)
	{
		var result = SuperEnumerable.Range(start, count, step);

		if (count != 0)
			Assert.Equal(start, result.ElementAt(0));

		Assert.Equal(count, result.Count());

		result.AssertSequenceEqual(Enumerable.Range(0, count)
			.Select(i => start + (step * i)));
	}

	[Test]
	[Arguments(-10, -4)]
	[Arguments(-1, 5)]
	[Arguments(1, 10)]
	[Arguments(30, 55)]
	[Arguments(27, 172)]
	public void SequenceWithAscendingRange(int start, int stop)
	{
		var result = SuperEnumerable.Sequence(start, stop);

		Assert.Equal(start, result.First());
		Assert.Equal(stop, result.Last());
		Assert.Equal(stop - start + 1, result.Count());

		result.AssertSequenceEqual(Enumerable.Range(start, stop - start + 1));
	}

	[Test]
	[Arguments(-4, -10)]
	[Arguments(5, -1)]
	[Arguments(10, 1)]
	[Arguments(55, 30)]
	[Arguments(172, 27)]
	public void SequenceWithDescendingRange(int start, int stop)
	{
		var result = SuperEnumerable.Sequence(start, stop);

		Assert.Equal(start, result.First());
		Assert.Equal(stop, result.Last());
		Assert.Equal(start - stop + 1, result.Count());

		result.AssertSequenceEqual(Enumerable.Range(stop, start - stop + 1).Reverse());
	}

	[Test]
	[Arguments(-10, -4, 2)]
	[Arguments(-1, 5, 3)]
	[Arguments(1, 10, 1)]
	[Arguments(30, 55, 4)]
	[Arguments(27, 172, 9)]
	public void SequenceWithAscendingRangeAscendingStep(int start, int stop, int step)
	{
		var result = SuperEnumerable.Sequence(start, stop, step);
		var expectations = Enumerable.Range(start, stop - start + 1).TakeEvery(step);

		result.AssertSequenceEqual(expectations);
	}

	[Test]
	[Arguments(-10, -4, -2)]
	[Arguments(-1, 5, -3)]
	[Arguments(1, 10, -1)]
	[Arguments(30, 55, -4)]
	[Arguments(27, 172, -9)]
	public void SequenceWithAscendingRangeDescendingStep(int start, int stop, int step)
	{
		var result = SuperEnumerable.Sequence(start, stop, step);

		Assert.Empty(result);
	}

	[Test]
	[Arguments(-4, -10, 2)]
	[Arguments(5, -1, 3)]
	[Arguments(10, 1, 1)]
	[Arguments(55, 30, 4)]
	[Arguments(172, 27, 9)]
	public void SequenceWithDescendingRangeAscendingStep(int start, int stop, int step)
	{
		var result = SuperEnumerable.Sequence(start, stop, step);

		Assert.Empty(result);
	}

	[Test]
	[Arguments(-4, -10, -2)]
	[Arguments(5, -1, -3)]
	[Arguments(10, 1, -1)]
	[Arguments(55, 30, -4)]
	[Arguments(172, 27, -9)]
	public void SequenceWithDescendingRangeDescendigStep(int start, int stop, int step)
	{
		var result = SuperEnumerable.Sequence(start, stop, step);
		var expectations = Enumerable.Range(stop, start - stop + 1).Reverse().TakeEvery(Math.Abs(step));

		result.AssertSequenceEqual(expectations);
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
	public void SequenceWithStartEqualsStop(int start, int stop, int? step)
	{
		var result = step.HasValue
			? SuperEnumerable.Sequence(start, stop, step.Value)
			: SuperEnumerable.Sequence(start, stop);

		Assert.Equal(result.Single(), start);
	}

	[Test]
	[Arguments(int.MaxValue - 1, int.MaxValue, 1, 2)]
	[Arguments(int.MinValue + 1, int.MinValue, -1, 2)]
	[Arguments(0, int.MaxValue, 10000000, (int.MaxValue / 10000000) + 1)]
	[Arguments(int.MinValue, int.MaxValue, int.MaxValue, 3)]
	public void SequenceEdgeCases(int start, int stop, int step, int count)
	{
		var result = SuperEnumerable.Sequence(start, stop, step);

		Assert.Equal(result.Count(), count);
	}

	[Test]
	[Arguments(5, 10)]
	[Arguments(int.MaxValue, int.MaxValue)]
	[Arguments(int.MinValue, int.MaxValue)]
	public void SequenceWithStepZero(int start, int stop)
	{
		var result = SuperEnumerable.Sequence(start, stop, 0);

		Assert.True(result.Take(100).All(x => x == start));
	}

	[Test]
	public void SequenceListBehavior()
	{
		var result = SuperEnumerable.Sequence(0, 9_999);
		result.AssertCollectionErrorChecking(10_000);
		result.AssertListElementChecking(10_000);

		Assert.Equal(10, result.ElementAt(10));
		Assert.Equal(20, result.ElementAt(20));
#if !NO_INDEX
		Assert.Equal(9_950, result.ElementAt(^50));
#endif
	}
}

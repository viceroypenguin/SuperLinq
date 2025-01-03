namespace SuperLinq.Tests;

public sealed class CountBetweenTest
{
	[Test]
	public void CountBetweenWithNegativeMin()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().CountBetween(-1, 0));
	}

	[Test]
	public void CountBetweenWithNegativeMax()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
		   new BreakingSequence<int>().CountBetween(0, -1));
	}

	[Test]
	public void CountBetweenWithMaxLesserThanMin()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().CountBetween(1, 0));
	}

	[Test]
	[MethodDataSource(typeof(TestExtensions), nameof(GetSingleElementSequences))]
	public void CountBetweenWithMaxEqualsMin(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.True(seq.CountBetween(1, 1));
	}

	public static IEnumerable<(IDisposableEnumerable<int> seq, bool expected)> GetTestData() =>
		Seq(
			(1, false),
			(2, true),
			(3, true),
			(4, true),
			(5, false)
		)
			.SelectMany(x => Enumerable.Range(1, x.Item1)
				.GetBreakingCollectionSequences()
				.Select(y => (y, x.Item2))
			);

	[Test]
	[MethodDataSource(nameof(GetTestData))]
	public void CountBetweenRangeTests(IDisposableEnumerable<int> seq, bool expected)
	{
		using (seq)
			Assert.Equal(expected, seq.CountBetween(2, 4));
	}

	[Test]
	public void CountBetweenDoesNotIterateUnnecessaryElements()
	{
		using var source = SeqExceptionAt(5).AsTestingSequence();
		Assert.False(source.CountBetween(2, 3));
	}
}

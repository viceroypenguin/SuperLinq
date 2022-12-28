namespace Test;

public class CountBetweenTest
{
	[Fact]
	public void CountBetweenWithNegativeMin()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			new[] { 1 }.CountBetween(-1, 0));
	}

	[Fact]
	public void CountBetweenWithNegativeMax()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
		   new[] { 1 }.CountBetween(0, -1));
	}

	[Fact]
	public void CountBetweenWithMaxLesserThanMin()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			new[] { 1 }.CountBetween(1, 0));
	}

	[Fact]
	public void CountBetweenWithMaxEqualsMin()
	{
		foreach (var xs in Seq(1).ArrangeCollectionInlineDatas())
			using (xs)
				Assert.True(xs.CountBetween(1, 1));
	}

	[Theory]
	[InlineData(1, 2, 4, false)]
	[InlineData(2, 2, 4, true)]
	[InlineData(3, 2, 4, true)]
	[InlineData(4, 2, 4, true)]
	[InlineData(5, 2, 4, false)]
	public void CountBetweenRangeTests(int count, int min, int max, bool expecting)
	{
		foreach (var xs in Enumerable.Range(1, count).ArrangeCollectionInlineDatas())
			using (xs)
				Assert.Equal(expecting, xs.CountBetween(min, max));
	}

	[Fact]
	public void CountBetweenDoesNotIterateUnnecessaryElements()
	{
		using var source = SeqExceptionAt(5).AsTestingSequence();
		Assert.False(source.CountBetween(2, 3));
	}
}

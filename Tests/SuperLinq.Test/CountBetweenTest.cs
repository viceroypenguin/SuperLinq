namespace Test;

public sealed class CountBetweenTest
{
	[Fact]
	public void CountBetweenWithNegativeMin()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().CountBetween(-1, 0));
	}

	[Fact]
	public void CountBetweenWithNegativeMax()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
		   new BreakingSequence<int>().CountBetween(0, -1));
	}

	[Fact]
	public void CountBetweenWithMaxLesserThanMin()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().CountBetween(1, 0));
	}

	public static IEnumerable<object[]> GetSequences(IEnumerable<int> seq) =>
		seq
			.GetBreakingCollectionSequences()
			.Select(x => new object[] { x });

	[Theory]
	[MemberData(nameof(GetSequences), new int[] { 1 })]
	public void CountBetweenWithMaxEqualsMin(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.True(seq.CountBetween(1, 1));
	}

	public static IEnumerable<object[]> GetTestData(int count, int min, int max, bool expecting) =>
		Enumerable.Range(1, count)
			.GetBreakingCollectionSequences()
			.Select(x => new object[] { x, min, max, expecting });

	[Theory]
	[MemberData(nameof(GetTestData), 1, 2, 4, false)]
	[MemberData(nameof(GetTestData), 2, 2, 4, true)]
	[MemberData(nameof(GetTestData), 3, 2, 4, true)]
	[MemberData(nameof(GetTestData), 4, 2, 4, true)]
	[MemberData(nameof(GetTestData), 5, 2, 4, false)]
	public void CountBetweenRangeTests(IDisposableEnumerable<int> seq, int min, int max, bool expecting)
	{
		using (seq)
			Assert.Equal(expecting, seq.CountBetween(min, max));
	}

	[Fact]
	public void CountBetweenDoesNotIterateUnnecessaryElements()
	{
		using var source = SeqExceptionAt(5).AsTestingSequence();
		Assert.False(source.CountBetween(2, 3));
	}
}

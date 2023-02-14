namespace Test;

public class ExactlyTest
{
	[Fact]
	public void ExactlyWithNegativeCount()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().Exactly(-1));
	}

	[Fact]
	public void ExactlyWithEmptySequenceHasExactlyZeroElements()
	{
		foreach (var xs in Enumerable.Empty<int>().ArrangeCollectionInlineDatas())
		{
			using (xs)
				Assert.True(xs.Exactly(0));
		}
	}

	[Fact]
	public void ExactlyWithEmptySequenceHasExactlyOneElement()
	{
		foreach (var xs in Enumerable.Empty<int>().ArrangeCollectionInlineDatas())
		{
			using (xs)
				Assert.False(xs.Exactly(1));
		}
	}

	[Fact]
	public void ExactlyWithSingleElementHasExactlyOneElements()
	{
		foreach (var xs in new[] { 1 }.ArrangeCollectionInlineDatas())
		{
			using (xs)
				Assert.True(xs.Exactly(1));
		}
	}

	[Fact]
	public void ExactlyWithManyElementHasExactlyOneElement()
	{
		foreach (var xs in new[] { 1, 2, 3 }.ArrangeCollectionInlineDatas())
		{
			using (xs)
				Assert.False(xs.Exactly(1));
		}
	}

	[Fact]
	public void ExactlyDoesNotIterateUnnecessaryElements()
	{
		using var source = SeqExceptionAt(4).AsTestingSequence();
		Assert.False(source.Exactly(2));
	}
}

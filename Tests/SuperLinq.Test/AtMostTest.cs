namespace Test;

public class AtMostTest
{
	[Fact]
	public void AtMostWithNegativeCount()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().AtMost(-1));
	}

	[Fact]
	public void AtMostWithEmptySequenceHasAtMostZeroElements()
	{
		foreach (var xs in Enumerable.Empty<int>().ArrangeCollectionInlineDatas())
			using (xs)
				Assert.True(xs.AtMost(0));
	}

	[Fact]
	public void AtMostWithEmptySequenceHasAtMostOneElement()
	{
		foreach (var xs in Enumerable.Empty<int>().ArrangeCollectionInlineDatas())
			using (xs)
				Assert.True(xs.AtMost(1));
	}

	[Fact]
	public void AtMostWithSingleElementHasAtMostZeroElements()
	{
		foreach (var xs in Seq(1).ArrangeCollectionInlineDatas())
			using (xs)
				Assert.False(xs.AtMost(0));
	}

	[Fact]
	public void AtMostWithSingleElementHasAtMostOneElement()
	{
		foreach (var xs in Seq(1).ArrangeCollectionInlineDatas())
			using (xs)
				Assert.True(xs.AtMost(1));
	}

	[Fact]
	public void AtMostWithSingleElementHasAtMostManyElements()
	{
		foreach (var xs in new[] { 1 }.ArrangeCollectionInlineDatas())
			using (xs)
				Assert.True(xs.AtMost(2));
	}

	[Fact]
	public void AtMostWithManyElementsHasAtMostOneElements()
	{
		foreach (var xs in Seq(1, 2, 3).ArrangeCollectionInlineDatas())
			using (xs)
				Assert.False(xs.AtMost(1));
	}

	[Fact]
	public void AtMostDoesNotIterateUnnecessaryElements()
	{
		using var source = SeqExceptionAt(4).AsTestingSequence();
		Assert.False(source.AtMost(2));
	}
}

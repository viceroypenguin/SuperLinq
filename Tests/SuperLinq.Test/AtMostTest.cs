namespace Test;

public class AtMostTest
{
	[Fact]
	public void AtMostWithNegativeCount()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			new[] { 1 }.AtMost(-1));
	}

	[Fact]
	public void AtMostWithEmptySequenceHasAtMostZeroElements()
	{
		foreach (var xs in Enumerable.Empty<int>().ArrangeCollectionInlineDatas())
			Assert.True(xs.AtMost(0));
	}

	[Fact]
	public void AtMostWithEmptySequenceHasAtMostOneElement()
	{
		foreach (var xs in Enumerable.Empty<int>().ArrangeCollectionInlineDatas())
			Assert.True(xs.AtMost(1));
	}

	[Fact]
	public void AtMostWithSingleElementHasAtMostZeroElements()
	{
		foreach (var xs in new[] { 1 }.ArrangeCollectionInlineDatas())
			Assert.False(xs.AtMost(0));
	}

	[Fact]
	public void AtMostWithSingleElementHasAtMostOneElement()
	{
		foreach (var xs in new[] { 1 }.ArrangeCollectionInlineDatas())
			Assert.True(xs.AtMost(1));
	}

	[Fact]
	public void AtMostWithSingleElementHasAtMostManyElements()
	{
		foreach (var xs in new[] { 1 }.ArrangeCollectionInlineDatas())
			Assert.True(xs.AtMost(2));
	}

	[Fact]
	public void AtMostWithManyElementsHasAtMostOneElements()
	{
		foreach (var xs in new[] { 1, 2, 3 }.ArrangeCollectionInlineDatas())
			Assert.False(xs.AtMost(1));
	}

	[Fact]
	public void AtMostDoesNotIterateUnnecessaryElements()
	{
		var source = SuperEnumerable.From(() => 1,
										 () => 2,
										 () => 3,
										 () => throw new TestException());
		Assert.False(source.AtMost(2));
	}
}

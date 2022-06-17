namespace Test;

public class ExactlyTest
{
	[Fact]
	public void ExactlyWithNegativeCount()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			new[] { 1 }.Exactly(-1));
	}

	[Fact]
	public void ExactlyWithEmptySequenceHasExactlyZeroElements()
	{
		foreach (var xs in Enumerable.Empty<int>().ArrangeCollectionInlineDatas())
			Assert.True(xs.Exactly(0));
	}

	[Fact]
	public void ExactlyWithEmptySequenceHasExactlyOneElement()
	{
		foreach (var xs in Enumerable.Empty<int>().ArrangeCollectionInlineDatas())
			Assert.False(xs.Exactly(1));
	}

	[Fact]
	public void ExactlyWithSingleElementHasExactlyOneElements()
	{
		foreach (var xs in new[] { 1 }.ArrangeCollectionInlineDatas())
			Assert.True(xs.Exactly(1));
	}

	[Fact]
	public void ExactlyWithManyElementHasExactlyOneElement()
	{
		foreach (var xs in new[] { 1, 2, 3 }.ArrangeCollectionInlineDatas())
			Assert.False(xs.Exactly(1));
	}

	[Fact]
	public void ExactlyDoesNotIterateUnnecessaryElements()
	{
		var source = SuperEnumerable.From(() => 1,
										 () => 2,
										 () => 3,
										 () => throw new TestException());
		Assert.False(source.Exactly(2));
	}
}

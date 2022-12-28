namespace Test;

public class AtLeastTest
{
	[Fact]
	public void AtLeastWithNegativeCount()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			new[] { 1 }.AtLeast(-1));
	}

	[Fact]
	public void AtLeastWithEmptySequenceHasAtLeastZeroElements()
	{
		foreach (var xs in Enumerable.Empty<int>().ArrangeCollectionInlineDatas())
			using (xs)
				Assert.True(xs.AtLeast(0));
	}

	[Fact]
	public void AtLeastWithEmptySequenceHasAtLeastOneElement()
	{
		foreach (var xs in Enumerable.Empty<int>().ArrangeCollectionInlineDatas())
			using (xs)
				Assert.False(xs.AtLeast(1));
	}

	[Fact]
	public void AtLeastWithEmptySequenceHasAtLeastManyElements()
	{
		foreach (var xs in Enumerable.Empty<int>().ArrangeCollectionInlineDatas())
			using (xs)
				Assert.False(xs.AtLeast(2));
	}

	[Fact]
	public void AtLeastWithSingleElementHasAtLeastZeroElements()
	{
		foreach (var xs in Seq(1).ArrangeCollectionInlineDatas())
			using (xs)
				Assert.True(xs.AtLeast(0));
	}

	[Fact]
	public void AtLeastWithSingleElementHasAtLeastOneElement()
	{
		foreach (var xs in Seq(1).ArrangeCollectionInlineDatas())
			using (xs)
				Assert.True(xs.AtLeast(1));
	}

	[Fact]
	public void AtLeastWithSingleElementHasAtLeastManyElements()
	{
		foreach (var xs in Seq(1).ArrangeCollectionInlineDatas())
			using (xs)
				Assert.False(xs.AtLeast(2));
	}

	[Fact]
	public void AtLeastWithManyElementsHasAtLeastZeroElements()
	{
		foreach (var xs in Seq(1, 2, 3).ArrangeCollectionInlineDatas())
			using (xs)
				Assert.True(xs.AtLeast(0));
	}

	[Fact]
	public void AtLeastWithManyElementsHasAtLeastOneElement()
	{
		foreach (var xs in Seq(1, 2, 3).ArrangeCollectionInlineDatas())
			using (xs)
				Assert.True(xs.AtLeast(1));
	}

	[Fact]
	public void AtLeastWithManyElementsHasAtLeastManyElements()
	{
		foreach (var xs in Seq(1, 2, 3).ArrangeCollectionInlineDatas())
			using (xs)
				Assert.True(xs.AtLeast(2));
	}

	[Fact]
	public void AtLeastDoesNotIterateUnnecessaryElements()
	{
		using var source = SeqExceptionAt(3).AsTestingSequence();
		Assert.True(source.AtLeast(2));
	}
}

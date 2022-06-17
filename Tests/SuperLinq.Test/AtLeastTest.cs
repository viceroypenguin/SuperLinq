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
			Assert.True(xs.AtLeast(0));
	}

	[Fact]
	public void AtLeastWithEmptySequenceHasAtLeastOneElement()
	{
		foreach (var xs in Enumerable.Empty<int>().ArrangeCollectionInlineDatas())
			Assert.False(xs.AtLeast(1));
	}

	[Fact]
	public void AtLeastWithEmptySequenceHasAtLeastManyElements()
	{
		foreach (var xs in Enumerable.Empty<int>().ArrangeCollectionInlineDatas())
			Assert.False(xs.AtLeast(2));
	}

	[Fact]
	public void AtLeastWithSingleElementHasAtLeastZeroElements()
	{
		foreach (var xs in new[] { 1 }.ArrangeCollectionInlineDatas())
			Assert.True(xs.AtLeast(0));
	}

	[Fact]
	public void AtLeastWithSingleElementHasAtLeastOneElement()
	{
		foreach (var xs in new[] { 1 }.ArrangeCollectionInlineDatas())
			Assert.True(xs.AtLeast(1));
	}

	[Fact]
	public void AtLeastWithSingleElementHasAtLeastManyElements()
	{
		foreach (var xs in new[] { 1 }.ArrangeCollectionInlineDatas())
			Assert.False(xs.AtLeast(2));
	}

	[Fact]
	public void AtLeastWithManyElementsHasAtLeastZeroElements()
	{
		foreach (var xs in new[] { 1, 2, 3 }.ArrangeCollectionInlineDatas())
			Assert.True(xs.AtLeast(0));
	}

	[Fact]
	public void AtLeastWithManyElementsHasAtLeastOneElement()
	{
		foreach (var xs in new[] { 1, 2, 3 }.ArrangeCollectionInlineDatas())
			Assert.True(xs.AtLeast(1));
	}

	[Fact]
	public void AtLeastWithManyElementsHasAtLeastManyElements()
	{
		foreach (var xs in new[] { 1, 2, 3 }.ArrangeCollectionInlineDatas())
			Assert.True(xs.AtLeast(2));
	}

	[Fact]
	public void AtLeastDoesNotIterateUnnecessaryElements()
	{
		var source = SuperEnumerable.From(() => 1,
										 () => 2,
										 () => throw new TestException());
		Assert.True(source.AtLeast(2));
	}
}

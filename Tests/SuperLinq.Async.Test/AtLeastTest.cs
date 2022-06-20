namespace Test.Async;

public class AtLeastTest
{
	[Fact]
	public async ValueTask AtLeastWithNegativeCount()
	{
		await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await AsyncSeq(1).AtLeast(-1));
	}

	[Fact]
	public async ValueTask AtLeastWithEmptySequenceHasAtLeastZeroElements()
	{
		Assert.True(await AsyncEnumerable.Empty<int>().AtLeast(0));
	}

	[Fact]
	public async ValueTask AtLeastWithEmptySequenceHasAtLeastOneElement()
	{
		Assert.False(await AsyncEnumerable.Empty<int>().AtLeast(1));
	}

	[Fact]
	public async ValueTask AtLeastWithEmptySequenceHasAtLeastManyElements()
	{
		Assert.False(await AsyncEnumerable.Empty<int>().AtLeast(2));
	}

	[Fact]
	public async ValueTask AtLeastWithSingleElementHasAtLeastZeroElements()
	{
		Assert.True(await AsyncSeq(1).AtLeast(0));
	}

	[Fact]
	public async ValueTask AtLeastWithSingleElementHasAtLeastOneElement()
	{
		Assert.True(await AsyncSeq(1).AtLeast(1));
	}

	[Fact]
	public async ValueTask AtLeastWithSingleElementHasAtLeastManyElements()
	{
		Assert.False(await AsyncSeq(1).AtLeast(2));
	}

	[Fact]
	public async ValueTask AtLeastWithManyElementsHasAtLeastZeroElements()
	{
		Assert.True(await AsyncSeq(1, 2, 3).AtLeast(0));
	}

	[Fact]
	public async ValueTask AtLeastWithManyElementsHasAtLeastOneElement()
	{
		Assert.True(await AsyncSeq(1, 2, 3).AtLeast(1));
	}

	[Fact]
	public async ValueTask AtLeastWithManyElementsHasAtLeastManyElements()
	{
		Assert.True(await AsyncSeq(1, 2, 3).AtLeast(2));
	}

	[Fact]
	public async ValueTask AtLeastDoesNotIterateUnnecessaryElements()
	{
		Assert.True(await AsyncSeqExceptionAt(3).AtLeast(2));
	}
}

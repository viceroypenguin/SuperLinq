namespace Test.Async;

public class AtLeastTest
{
	[Fact]
	public async Task AtLeastWithNegativeCount()
	{
		await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await AsyncSeq(1).AtLeast(-1));
	}

	[Fact]
	public async Task AtLeastWithEmptySequenceHasAtLeastZeroElements()
	{
		Assert.True(await AsyncEnumerable.Empty<int>().AtLeast(0));
	}

	[Fact]
	public async Task AtLeastWithEmptySequenceHasAtLeastOneElement()
	{
		Assert.False(await AsyncEnumerable.Empty<int>().AtLeast(1));
	}

	[Fact]
	public async Task AtLeastWithEmptySequenceHasAtLeastManyElements()
	{
		Assert.False(await AsyncEnumerable.Empty<int>().AtLeast(2));
	}

	[Fact]
	public async Task AtLeastWithSingleElementHasAtLeastZeroElements()
	{
		Assert.True(await AsyncSeq(1).AtLeast(0));
	}

	[Fact]
	public async Task AtLeastWithSingleElementHasAtLeastOneElement()
	{
		Assert.True(await AsyncSeq(1).AtLeast(1));
	}

	[Fact]
	public async Task AtLeastWithSingleElementHasAtLeastManyElements()
	{
		Assert.False(await AsyncSeq(1).AtLeast(2));
	}

	[Fact]
	public async Task AtLeastWithManyElementsHasAtLeastZeroElements()
	{
		Assert.True(await AsyncSeq(1, 2, 3).AtLeast(0));
	}

	[Fact]
	public async Task AtLeastWithManyElementsHasAtLeastOneElement()
	{
		Assert.True(await AsyncSeq(1, 2, 3).AtLeast(1));
	}

	[Fact]
	public async Task AtLeastWithManyElementsHasAtLeastManyElements()
	{
		Assert.True(await AsyncSeq(1, 2, 3).AtLeast(2));
	}

	[Fact]
	public async Task AtLeastDoesNotIterateUnnecessaryElements()
	{
		Assert.True(await AsyncSeqExceptionAt(3).AtLeast(2));
	}
}

namespace Test.Async;

public class AssertCountTest
{
	[Fact]
	public void AssertCountIsLazy()
	{
		_ = new AsyncBreakingSequence<object>().AssertCount(0);
	}

	[Fact]
	public void AssertCountNegativeCount()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>("count",
			() => new AsyncBreakingSequence<int>().AssertCount(-1));
	}

	[Fact]
	public async Task AssertCountSequenceWithMatchingLength()
	{
		await using var data = TestingSequence.Of("foo", "bar", "baz");
		await data.AssertCount(3).Consume();
	}

	[Fact]
	public async Task AssertCountShortSequence()
	{
		await using var data = TestingSequence.Of("foo", "bar", "baz");
		_ = await Assert.ThrowsAsync<ArgumentOutOfRangeException>("source.Count()", async () =>
			await data.AssertCount(4).Consume());
	}

	[Fact]
	public async Task AssertCountLongSequence()
	{
		await using var data = TestingSequence.Of("foo", "bar", "baz");
		_ = await Assert.ThrowsAsync<ArgumentOutOfRangeException>("source.Count()", async () =>
			await data.AssertCount(2).Consume());
	}
}

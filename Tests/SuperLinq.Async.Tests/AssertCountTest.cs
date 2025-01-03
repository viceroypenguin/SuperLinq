namespace SuperLinq.Async.Tests;

public sealed class AssertCountTest
{
	[Test]
	public void AssertCountIsLazy()
	{
		_ = new AsyncBreakingSequence<object>().AssertCount(0);
	}

	[Test]
	public void AssertCountNegativeCount()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>("count",
			() => new AsyncBreakingSequence<int>().AssertCount(-1));
	}

	[Test]
	public async Task AssertCountSequenceWithMatchingLength()
	{
		await using var data = TestingSequence.Of("foo", "bar", "baz");
		await data.AssertCount(3).Consume();
	}

	[Test]
	public async Task AssertCountShortSequence()
	{
		await using var data = TestingSequence.Of("foo", "bar", "baz");
		_ = await Assert.ThrowsAsync<ArgumentOutOfRangeException>("source.Count()", async () =>
			await data.AssertCount(4).Consume());
	}

	[Test]
	public async Task AssertCountLongSequence()
	{
		await using var data = TestingSequence.Of("foo", "bar", "baz");
		_ = await Assert.ThrowsAsync<ArgumentOutOfRangeException>("source.Count()", async () =>
			await data.AssertCount(2).Consume());
	}
}

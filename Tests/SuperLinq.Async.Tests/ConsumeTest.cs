namespace SuperLinq.Async.Tests;

public sealed class ConsumeTest
{
	[Fact]
	public async Task ConsumeReallyConsumes()
	{
		var counter = 0;
		await using var xs = AsyncEnumerable.Range(0, 10)
			.Do(x => counter++)
			.AsTestingSequence();
		await xs.Consume(default);
		Assert.Equal(10, counter);
	}
}

namespace Test.Async;

public class ConsumeTest
{
	[Fact]
	public async Task ConsumeReallyConsumes()
	{
		var counter = 0;
		var sequence = AsyncEnumerable.Range(0, 10).Do(x => counter++);
		await sequence.Consume(default);
		Assert.Equal(10, counter);
	}
}

namespace SuperLinq.Async.Tests;

public sealed class ReturnTest
{
	[Fact]
	public async Task TestResultingSequenceContainsSingle()
	{
		var item = new object();
		await AsyncSuperEnumerable.Return(item)
				.AssertSequenceEqual(item);
	}
}

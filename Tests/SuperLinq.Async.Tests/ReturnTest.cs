namespace SuperLinq.Async.Tests;

public sealed class ReturnTest
{
	[Test]
	public async Task TestResultingSequenceContainsSingle()
	{
		var item = new object();
		await AsyncSuperEnumerable.Return(item)
				.AssertSequenceEqual(item);
	}
}

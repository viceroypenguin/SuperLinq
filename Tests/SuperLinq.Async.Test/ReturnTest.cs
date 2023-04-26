namespace Test.Async;

public class ReturnTest
{
	[Fact]
	public async Task TestResultingSequenceContainsSingle()
	{
		var item = new object();
		await AsyncSuperEnumerable.Return(item)
				.AssertSequenceEqual(item);
	}
}

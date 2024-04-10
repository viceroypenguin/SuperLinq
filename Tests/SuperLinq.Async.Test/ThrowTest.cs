namespace Test.Async;

public sealed class ThrowTest
{
	[Fact]
	public void ThrowIsLazy()
	{
		_ = AsyncSuperEnumerable.Throw<int>(new TestException());
	}

	[Fact]
	public async Task ThrowBehavior()
	{
		var src = new TestException();

		var seq = AsyncSuperEnumerable.Throw<int>(src);

		var tgt = await Assert.ThrowsAsync<TestException>(
			async () => await seq.Consume());
		Assert.Same(src, tgt);
	}
}

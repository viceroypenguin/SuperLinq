namespace SuperLinq.Tests;

public sealed class ThrowTest
{
	[Test]
	public void ThrowIsLazy()
	{
		_ = SuperEnumerable.Throw<int>(new TestException());
	}

	[Test]
	public void ThrowBehavior()
	{
		var seq = SuperEnumerable.Throw<int>(new TestException());
		_ = Assert.Throws<TestException>(seq.Consume);
	}
}

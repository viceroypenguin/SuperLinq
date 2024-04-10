namespace Test;

public sealed class ThrowTest
{
	[Fact]
	public void ThrowIsLazy()
	{
		_ = SuperEnumerable.Throw<int>(new TestException());
	}

	[Fact]
	public void ThrowBehavior()
	{
		var seq = SuperEnumerable.Throw<int>(new TestException());
		_ = Assert.Throws<TestException>(seq.Consume);
	}
}

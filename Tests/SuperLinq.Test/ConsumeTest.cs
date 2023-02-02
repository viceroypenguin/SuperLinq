namespace Test;

public class ConsumeTest
{
	[Fact]
	public void ConsumeReallyConsumes()
	{
		var counter = 0;
		using var xs = Enumerable.Range(0, 10)
			.Pipe(x => counter++)
			.AsTestingSequence();
		xs.Consume();
		Assert.Equal(10, counter);
	}
}

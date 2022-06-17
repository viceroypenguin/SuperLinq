namespace Test;

public class ConsumeTest
{
	[Fact]
	public void ConsumeReallyConsumes()
	{
		var counter = 0;
		var sequence = Enumerable.Range(0, 10).Pipe(x => counter++);
		sequence.Consume();
		Assert.Equal(10, counter);
	}
}

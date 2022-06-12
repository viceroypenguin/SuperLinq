using NUnit.Framework;

namespace SuperLinq.Test;

[TestFixture]
public class ConsumeTest
{
	[Test]
	public void ConsumeReallyConsumes()
	{
		var counter = 0;
		var sequence = Enumerable.Range(0, 10).Pipe(x => counter++);
		sequence.Consume();
		Assert.AreEqual(10, counter);
	}
}

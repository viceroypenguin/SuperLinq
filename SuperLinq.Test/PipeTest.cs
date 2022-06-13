using System.Text;
using NUnit.Framework;

namespace Test;

[TestFixture]
public class PipeTest
{
	[Test]
	public void PipeWithSequence()
	{
		var results = new List<int>();
		var returned = new[] { 1, 2, 3 }.Pipe(results.Add);
		// Lazy - nothing has executed yet
		Assert.That(results, Is.Empty);
		returned.AssertSequenceEqual(1, 2, 3);
		// Now it has...
		results.AssertSequenceEqual(1, 2, 3);
	}

	[Test]
	public void PipeIsLazy()
	{
		new BreakingSequence<int>().Pipe(BreakingAction.Of<int>());
	}

	[Test]
	public void PipeActionOccursBeforeYield()
	{
		var source = new[] { new StringBuilder(), new StringBuilder() };
		// The action will occur "in" the pipe, so by the time Where gets it, the
		// sequence will be empty.
		Assert.That(source.Pipe(sb => sb.Append('x'))
						  .Where(x => x.Length == 0),
					Is.Empty);
	}
}

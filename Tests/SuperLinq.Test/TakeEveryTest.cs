using NUnit.Framework;

namespace Test;

[TestFixture]
public class TakeEveryTest
{
	[Test]
	public void TakeEveryNegativeSkip()
	{
		AssertThrowsArgument.OutOfRangeException("step", () =>
			Array.Empty<object>().TakeEvery(-1));
	}

	[Test]
	public void TakeEveryOutOfRangeZeroStep()
	{
		AssertThrowsArgument.OutOfRangeException("step", () =>
			Array.Empty<object>().TakeEvery(0));
	}

	[Test]
	public void TakeEveryEmptySequence()
	{
		Assert.That(Array.Empty<object>().TakeEvery(1), Is.Empty);
	}

	[Test]
	public void TakeEveryNonEmptySequence()
	{
		var result = new[] { 1, 2, 3, 4, 5 }.TakeEvery(1);
		result.AssertSequenceEqual(1, 2, 3, 4, 5);
	}

	[Test]
	public void TakeEveryOtherOnNonEmptySequence()
	{
		var result = new[] { 1, 2, 3, 4, 5 }.TakeEvery(2);
		result.AssertSequenceEqual(1, 3, 5);
	}

	[Test]
	public void TakeEveryThirdOnNonEmptySequence()
	{
		var result = new[] { 1, 2, 3, 4, 5 }.TakeEvery(3);
		result.AssertSequenceEqual(1, 4);
	}

	[Test]
	public void TakeEveryIsLazy()
	{
		new BreakingSequence<object>().TakeEvery(1);
	}
}

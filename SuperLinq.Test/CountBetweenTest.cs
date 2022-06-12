using NUnit.Framework;

namespace SuperLinq.Test;

[TestFixture]
public class CountBetweenTest
{
	[Test]
	public void CountBetweenWithNegativeMin()
	{
		AssertThrowsArgument.OutOfRangeException("min", () =>
			new[] { 1 }.CountBetween(-1, 0));
	}

	[Test]
	public void CountBetweenWithNegativeMax()
	{
		AssertThrowsArgument.OutOfRangeException("max", () =>
		   new[] { 1 }.CountBetween(0, -1));
	}

	[Test]
	public void CountBetweenWithMaxLesserThanMin()
	{
		AssertThrowsArgument.OutOfRangeException("max", () =>
			new[] { 1 }.CountBetween(1, 0));
	}

	[Test]
	public void CountBetweenWithMaxEqualsMin()
	{
		foreach (var xs in new[] { 1 }.ArrangeCollectionTestCases())
			Assert.IsTrue(xs.CountBetween(1, 1));
	}

	[TestCase(1, 2, 4, false)]
	[TestCase(2, 2, 4, true)]
	[TestCase(3, 2, 4, true)]
	[TestCase(4, 2, 4, true)]
	[TestCase(5, 2, 4, false)]
	public void CountBetweenRangeTests(int count, int min, int max, bool expecting)
	{
		foreach (var xs in Enumerable.Range(1, count).ArrangeCollectionTestCases())
			Assert.That(xs.CountBetween(min, max), Is.EqualTo(expecting));
	}

	[Test]
	public void CountBetweenDoesNotIterateUnnecessaryElements()
	{
		var source = SuperEnumerable.From(() => 1,
										 () => 2,
										 () => 3,
										 () => 4,
										 () => throw new TestException());
		Assert.False(source.CountBetween(2, 3));
	}
}

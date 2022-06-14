using NUnit.Framework;

namespace Test;

[TestFixture]
public class AtMostTest
{
	[Test]
	public void AtMostWithNegativeCount()
	{
		AssertThrowsArgument.OutOfRangeException("count",
			() => new[] { 1 }.AtMost(-1));
	}

	[Test]
	public void AtMostWithEmptySequenceHasAtMostZeroElements()
	{
		foreach (var xs in Enumerable.Empty<int>().ArrangeCollectionTestCases())
			Assert.IsTrue(xs.AtMost(0));
	}

	[Test]
	public void AtMostWithEmptySequenceHasAtMostOneElement()
	{
		foreach (var xs in Enumerable.Empty<int>().ArrangeCollectionTestCases())
			Assert.IsTrue(xs.AtMost(1));
	}

	[Test]
	public void AtMostWithSingleElementHasAtMostZeroElements()
	{
		foreach (var xs in new[] { 1 }.ArrangeCollectionTestCases())
			Assert.IsFalse(xs.AtMost(0));
	}

	[Test]
	public void AtMostWithSingleElementHasAtMostOneElement()
	{
		foreach (var xs in new[] { 1 }.ArrangeCollectionTestCases())
			Assert.IsTrue(xs.AtMost(1));
	}

	[Test]
	public void AtMostWithSingleElementHasAtMostManyElements()
	{
		foreach (var xs in new[] { 1 }.ArrangeCollectionTestCases())
			Assert.IsTrue(xs.AtMost(2));
	}

	[Test]
	public void AtMostWithManyElementsHasAtMostOneElements()
	{
		foreach (var xs in new[] { 1, 2, 3 }.ArrangeCollectionTestCases())
			Assert.IsFalse(xs.AtMost(1));
	}

	[Test]
	public void AtMostDoesNotIterateUnnecessaryElements()
	{
		var source = SuperEnumerable.From(() => 1,
										 () => 2,
										 () => 3,
										 () => throw new TestException());
		Assert.IsFalse(source.AtMost(2));
	}
}

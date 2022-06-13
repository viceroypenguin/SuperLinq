using NUnit.Framework;

namespace Test;

[TestFixture]
public class ExactlyTest
{
	[Test]
	public void ExactlyWithNegativeCount()
	{
		AssertThrowsArgument.OutOfRangeException("count", () =>
			new[] { 1 }.Exactly(-1));
	}

	[Test]
	public void ExactlyWithEmptySequenceHasExactlyZeroElements()
	{
		foreach (var xs in Enumerable.Empty<int>().ArrangeCollectionTestCases())
			Assert.IsTrue(xs.Exactly(0));
	}

	[Test]
	public void ExactlyWithEmptySequenceHasExactlyOneElement()
	{
		foreach (var xs in Enumerable.Empty<int>().ArrangeCollectionTestCases())
			Assert.IsFalse(xs.Exactly(1));
	}

	[Test]
	public void ExactlyWithSingleElementHasExactlyOneElements()
	{
		foreach (var xs in new[] { 1 }.ArrangeCollectionTestCases())
			Assert.IsTrue(xs.Exactly(1));
	}

	[Test]
	public void ExactlyWithManyElementHasExactlyOneElement()
	{
		foreach (var xs in new[] { 1, 2, 3 }.ArrangeCollectionTestCases())
			Assert.IsFalse(xs.Exactly(1));
	}

	[Test]
	public void ExactlyDoesNotIterateUnnecessaryElements()
	{
		var source = SuperEnumerable.From(() => 1,
										 () => 2,
										 () => 3,
										 () => throw new TestException());
		Assert.IsFalse(source.Exactly(2));
	}
}

using NUnit.Framework;

namespace Test;

[TestFixture]
public class AtLeastTest
{
	[Test]
	public void AtLeastWithNegativeCount()
	{
		AssertThrowsArgument.OutOfRangeException("count", () =>
			new[] { 1 }.AtLeast(-1));
	}

	[Test]
	public void AtLeastWithEmptySequenceHasAtLeastZeroElements()
	{
		foreach (var xs in Enumerable.Empty<int>().ArrangeCollectionTestCases())
			Assert.IsTrue(xs.AtLeast(0));
	}

	[Test]
	public void AtLeastWithEmptySequenceHasAtLeastOneElement()
	{
		foreach (var xs in Enumerable.Empty<int>().ArrangeCollectionTestCases())
			Assert.IsFalse(xs.AtLeast(1));
	}

	[Test]
	public void AtLeastWithEmptySequenceHasAtLeastManyElements()
	{
		foreach (var xs in Enumerable.Empty<int>().ArrangeCollectionTestCases())
			Assert.IsFalse(xs.AtLeast(2));
	}

	[Test]
	public void AtLeastWithSingleElementHasAtLeastZeroElements()
	{
		foreach (var xs in new[] { 1 }.ArrangeCollectionTestCases())
			Assert.IsTrue(xs.AtLeast(0));
	}

	[Test]
	public void AtLeastWithSingleElementHasAtLeastOneElement()
	{
		foreach (var xs in new[] { 1 }.ArrangeCollectionTestCases())
			Assert.IsTrue(xs.AtLeast(1));
	}

	[Test]
	public void AtLeastWithSingleElementHasAtLeastManyElements()
	{
		foreach (var xs in new[] { 1 }.ArrangeCollectionTestCases())
			Assert.IsFalse(xs.AtLeast(2));
	}

	[Test]
	public void AtLeastWithManyElementsHasAtLeastZeroElements()
	{
		foreach (var xs in new[] { 1, 2, 3 }.ArrangeCollectionTestCases())
			Assert.IsTrue(xs.AtLeast(0));
	}

	[Test]
	public void AtLeastWithManyElementsHasAtLeastOneElement()
	{
		foreach (var xs in new[] { 1, 2, 3 }.ArrangeCollectionTestCases())
			Assert.IsTrue(xs.AtLeast(1));
	}

	[Test]
	public void AtLeastWithManyElementsHasAtLeastManyElements()
	{
		foreach (var xs in new[] { 1, 2, 3 }.ArrangeCollectionTestCases())
			Assert.IsTrue(xs.AtLeast(2));
	}

	[Test]
	public void AtLeastDoesNotIterateUnnecessaryElements()
	{
		var source = SuperEnumerable.From(() => 1,
										 () => 2,
										 () => throw new TestException());
		Assert.IsTrue(source.AtLeast(2));
	}
}

using NUnit.Framework;

namespace SuperLinq.Test;

[TestFixture]
public class TakeLastTest
{
	[Test]
	public void TakeLast()
	{
		AssertTakeLast(new[] { 12, 34, 56, 78, 910, 1112 },
					   3,
					   result => result.AssertSequenceEqual(78, 910, 1112));
	}

	[Test]
	public void TakeLastOnSequenceShortOfCount()
	{
		AssertTakeLast(new[] { 12, 34, 56 },
					   5,
					   result => result.AssertSequenceEqual(12, 34, 56));
	}

	[Test]
	public void TakeLastWithNegativeCount()
	{
		AssertTakeLast(new[] { 12, 34, 56 },
					   -2,
					   result => Assert.That(result, Is.Empty));
	}

	[Test]
	public void TakeLastIsLazy()
	{
		new BreakingSequence<object>().TakeLast(1);
	}

	[Test]
	public void TakeLastDisposesSequenceEnumerator()
	{
		using var seq = TestingSequence.Of(1, 2, 3);
		seq.TakeLast(1).Consume();
	}

	[TestCase(SourceKind.BreakingList)]
	[TestCase(SourceKind.BreakingReadOnlyList)]
	public void TakeLastOptimizedForCollections(SourceKind sourceKind)
	{
		var sequence = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }.ToSourceKind(sourceKind);

		sequence.TakeLast(3).AssertSequenceEqual(8, 9, 10);
	}

	static void AssertTakeLast<T>(ICollection<T> input, int count, Action<IEnumerable<T>> action)
	{
		// Test that the behaviour does not change whether a collection
		// or a sequence is used as the source.

		action(input.TakeLast(count));
		action(input.Select(x => x).TakeLast(count));
	}
}

using NUnit.Framework;

namespace SuperLinq.Test;

[TestFixture]
public class BatchTest
{
	[Test]
	public void BatchZeroSize()
	{
		AssertThrowsArgument.OutOfRangeException("size", () =>
			Array.Empty<object>().Batch(0));
	}

	[Test]
	public void BatchNegativeSize()
	{
		AssertThrowsArgument.OutOfRangeException("size", () =>
			Array.Empty<object>().Batch(-1));
	}

	[Test]
	public void BatchEvenlyDivisibleSequence()
	{
		var result = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Batch(3);

		using var reader = result.Read();
		reader.Read().AssertSequenceEqual(1, 2, 3);
		reader.Read().AssertSequenceEqual(4, 5, 6);
		reader.Read().AssertSequenceEqual(7, 8, 9);
		reader.ReadEnd();
	}

	[Test]
	public void BatchUnevenlyDivisibleSequence()
	{
		var result = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Batch(4);

		using var reader = result.Read();
		reader.Read().AssertSequenceEqual(1, 2, 3, 4);
		reader.Read().AssertSequenceEqual(5, 6, 7, 8);
		reader.Read().AssertSequenceEqual(9);
		reader.ReadEnd();
	}

	[Test]
	public void BatchSequenceTransformingResult()
	{
		var result = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Batch(4, batch => batch.Sum());
		result.AssertSequenceEqual(10, 26, 9);
	}

	[Test]
	public void BatchSequenceYieldsListsOfBatches()
	{
		var result = new[] { 1, 2, 3 }.Batch(2);

		using var reader = result.Read();
		Assert.That(reader.Read(), Is.InstanceOf(typeof(IList<int>)));
		Assert.That(reader.Read(), Is.InstanceOf(typeof(IList<int>)));
		reader.ReadEnd();
	}

	[Test]
	public void BatchSequencesAreIndependentInstances()
	{
		var result = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Batch(4);

		using var reader = result.Read();
		var first = reader.Read();
		var second = reader.Read();
		var third = reader.Read();
		reader.ReadEnd();

		first.AssertSequenceEqual(1, 2, 3, 4);
		second.AssertSequenceEqual(5, 6, 7, 8);
		third.AssertSequenceEqual(9);
	}

	[Test]
	public void BatchIsLazy()
	{
		new BreakingSequence<object>().Batch(1);
	}

	[TestCase(SourceKind.BreakingCollection, 0)]
	[TestCase(SourceKind.BreakingList, 0)]
	[TestCase(SourceKind.BreakingReadOnlyList, 0)]
	[TestCase(SourceKind.BreakingCollection, 1)]
	[TestCase(SourceKind.BreakingList, 1)]
	[TestCase(SourceKind.BreakingReadOnlyList, 1)]
	[TestCase(SourceKind.BreakingCollection, 2)]
	[TestCase(SourceKind.BreakingList, 2)]
	[TestCase(SourceKind.BreakingReadOnlyList, 2)]
	public void BatchCollectionSmallerThanSize(SourceKind kind, int oversize)
	{
		var xs = new[] { 1, 2, 3, 4, 5 };
		var result = xs.ToSourceKind(kind).Batch(xs.Length + oversize);
		using var reader = result.Read();
		reader.Read().AssertSequenceEqual(1, 2, 3, 4, 5);
		reader.ReadEnd();
	}

	[Test]
	public void BatchReadOnlyCollectionSmallerThanSize()
	{
		var collection = ReadOnlyCollection.From(1, 2, 3, 4, 5);
		var result = collection.Batch(collection.Count * 2);
		using var reader = result.Read();
		reader.Read().AssertSequenceEqual(1, 2, 3, 4, 5);
		reader.ReadEnd();
	}

	[TestCase(SourceKind.Sequence)]
	[TestCase(SourceKind.BreakingList)]
	[TestCase(SourceKind.BreakingReadOnlyList)]
	[TestCase(SourceKind.BreakingReadOnlyCollection)]
	[TestCase(SourceKind.BreakingCollection)]
	public void BatchEmptySource(SourceKind kind)
	{
		var batches = Enumerable.Empty<int>().ToSourceKind(kind).Batch(100);
		Assert.That(batches, Is.Empty);
	}
}

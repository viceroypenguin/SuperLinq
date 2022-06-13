using NUnit.Framework;

namespace Test;

[TestFixture]
public class InsertTest
{
	[Test]
	public void InsertWithNegativeIndex()
	{
		AssertThrowsArgument.OutOfRangeException("index", () =>
			 Enumerable.Range(1, 10).Insert(new[] { 97, 98, 99 }, -1));
	}

	[TestCase(7)]
	[TestCase(8)]
	[TestCase(9)]
	public void InsertWithIndexGreaterThanSourceLengthMaterialized(int count)
	{
		var seq1 = Enumerable.Range(0, count).ToList();
		var seq2 = new[] { 97, 98, 99 };

		using var test1 = seq1.AsTestingSequence();
		using var test2 = seq2.AsTestingSequence();

		var result = test1.Insert(test2, count + 1);

		AssertThrowsArgument.OutOfRangeException("index", () =>
			result.ForEach((e, index) =>
				Assert.That(e, Is.EqualTo(seq1[index]))));
	}

	[TestCase(7)]
	[TestCase(8)]
	[TestCase(9)]
	public void InsertWithIndexGreaterThanSourceLengthLazy(int count)
	{
		var seq1 = Enumerable.Range(0, count);
		var seq2 = new[] { 97, 98, 99 };

		using var test1 = seq1.AsTestingSequence();
		using var test2 = seq2.AsTestingSequence();

		var result = test1.Insert(test2, count + 1).Take(count);

		Assert.That(seq1, Is.EqualTo(result));
	}

	[TestCase(3, 0)]
	[TestCase(3, 1)]
	[TestCase(3, 2)]
	[TestCase(3, 3)]
	public void Insert(int count, int index)
	{
		var seq1 = Enumerable.Range(1, count);
		var seq2 = new[] { 97, 98, 99 };

		using var test1 = seq1.AsTestingSequence();
		using var test2 = seq2.AsTestingSequence();

		var result = test1.Insert(test2, index);

		var expectations = seq1.Take(index).Concat(seq2).Concat(seq1.Skip(index));
		Assert.That(result, Is.EqualTo(expectations));
	}

	[Test]
	public void InsertIsLazy()
	{
		new BreakingSequence<int>().Insert(new BreakingSequence<int>(), 0);
	}
}

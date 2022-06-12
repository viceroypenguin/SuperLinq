using NUnit.Framework;

namespace SuperLinq.Test;

[TestFixture]
public class SkipLastTest
{
	[TestCase(0)]
	[TestCase(-1)]
	public void SkipLastWithCountLesserThanOne(int skip)
	{
		var numbers = Enumerable.Range(1, 5);

		Assert.That(numbers.SkipLast(skip), Is.EqualTo(numbers));
	}

	[Test]
	public void SkipLast()
	{
		const int take = 100;
		const int skip = 20;

		var sequence = Enumerable.Range(1, take);

		var expectations = sequence.Take(take - skip);

		Assert.That(expectations, Is.EqualTo(sequence.SkipLast(skip)));
	}

	[TestCase(5)]
	[TestCase(6)]
	public void SkipLastWithSequenceShorterThanCount(int skip)
	{
		Assert.That(Enumerable.Range(1, 5).SkipLast(skip), Is.Empty);
	}

	[Test]
	public void SkipLastIsLazy()
	{
		new BreakingSequence<object>().SkipLast(1);
	}
}

using NUnit.Framework;

namespace SuperLinq.Test;

[TestFixture]
public class ScanTest
{
	[Test]
	public void ScanEmpty()
	{
		Assert.That(Array.Empty<int>().Scan(SampleData.Plus), Is.Empty);
	}

	[Test]
	public void ScanSum()
	{
		var result = Enumerable.Range(1, 10).Scan(SampleData.Plus);
		var gold = new[] { 1, 3, 6, 10, 15, 21, 28, 36, 45, 55 };
		result.AssertSequenceEqual(gold);
	}

	[Test]
	public void ScanIsLazy()
	{
		new BreakingSequence<object>().Scan(BreakingFunc.Of<object, object, object>());
	}

	[Test]
	public void ScanDoesNotIterateExtra()
	{
		var sequence = Enumerable.Range(1, 3).Concat(new BreakingSequence<int>()).Scan(SampleData.Plus);
		var gold = new[] { 1, 3, 6 };
		Assert.Throws<InvalidOperationException>(sequence.Consume);
		sequence.Take(3).AssertSequenceEqual(gold);
	}

	[Test]
	public void SeededScanEmpty()
	{
		Assert.AreEqual(-1, Array.Empty<int>().Scan(-1, SampleData.Plus).Single());
	}

	[Test]
	public void SeededScanSum()
	{
		var result = Enumerable.Range(1, 10).Scan(0, SampleData.Plus);
		var gold = new[] { 0, 1, 3, 6, 10, 15, 21, 28, 36, 45, 55 };
		result.AssertSequenceEqual(gold);
	}

	[Test]
	public void SeededScanIsLazy()
	{
		new BreakingSequence<object>().Scan(null, BreakingFunc.Of<object, object, object>());
	}

	[Test]
	public void SeededScanDoesNotIterateExtra()
	{
		var sequence = Enumerable.Range(1, 3).Concat(new BreakingSequence<int>()).Scan(0, SampleData.Plus);
		var gold = new[] { 0, 1, 3, 6 };
		Assert.Throws<InvalidOperationException>(sequence.Consume);
		sequence.Take(4).AssertSequenceEqual(gold);
	}

}

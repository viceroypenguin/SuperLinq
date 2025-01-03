namespace SuperLinq.Tests;

public sealed class ScanTest
{
	[Test]
	public void ScanEmpty()
	{
		using var seq = TestingSequence.Of<int>();

		var result = seq.Scan((a, b) => a + b);
		result.AssertSequenceEqual();
	}

	[Test]
	public void ScanSum()
	{
		using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.Scan((a, b) => a + b);
		result.AssertSequenceEqual(1, 3, 6, 10, 15, 21, 28, 36, 45, 55);
	}

	[Test]
	public void ScanIsLazy()
	{
		_ = new BreakingSequence<object>().Scan(BreakingFunc.Of<object, object, object>());
	}

	[Test]
	public void ScanDoesNotIterateExtra()
	{
		using var seq = SeqExceptionAt(4).AsTestingSequence(maxEnumerations: 2);

		var result = seq.Scan((a, b) => a + b);

		_ = Assert.Throws<TestException>(result.Consume);
		result.Take(3).AssertSequenceEqual(1, 3, 6);
	}

	[Test]
	public void SeededScanEmpty()
	{
		using var seq = TestingSequence.Of<int>();

		var result = seq.Scan(-1, (a, b) => a + b);
		Assert.Equal(-1, result.Single());
	}

	[Test]
	public void SeededScanSum()
	{
		using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.Scan(0, (a, b) => a + b);
		result.AssertSequenceEqual(0, 1, 3, 6, 10, 15, 21, 28, 36, 45, 55);
	}

	[Test]
	public void SeededScanIsLazy()
	{
		_ = new BreakingSequence<object>().Scan(seed: null,
			BreakingFunc.Of<object?, object, object>());
	}

	[Test]
	public void SeededScanDoesNotIterateExtra()
	{
		using var seq = SeqExceptionAt(4).AsTestingSequence(maxEnumerations: 2);

		var result = seq.Scan(0, (a, b) => a + b);

		_ = Assert.Throws<TestException>(result.Consume);
		result.Take(4).AssertSequenceEqual(0, 1, 3, 6);
	}
}

namespace Test;

public class ScanExTest
{
	[Fact]
	public void ScanExEmpty()
	{
		using var seq = TestingSequence.Of<int>();

		var result = seq.ScanEx((a, b) => a + b);
		result.AssertSequenceEqual();
	}

	[Fact]
	public void ScanExSum()
	{
		using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.ScanEx((a, b) => a + b);
		result.AssertSequenceEqual(1, 3, 6, 10, 15, 21, 28, 36, 45, 55);
	}

	[Fact]
	public void ScanExIsLazy()
	{
		_ = new BreakingSequence<object>().ScanEx(BreakingFunc.Of<object, object, object>());
	}

	[Fact]
	public void ScanExDoesNotIterateExtra()
	{
		using var seq = SeqExceptionAt(4).AsTestingSequence(maxEnumerations: 2);

		var result = seq.ScanEx((a, b) => a + b);

		_ = Assert.Throws<TestException>(result.Consume);
		result.Take(3).AssertSequenceEqual(1, 3, 6);
	}

	[Fact]
	public void SeededScanExEmpty()
	{
		using var seq = TestingSequence.Of<int>();

		var result = seq.ScanEx(-1, (a, b) => a + b);
		Assert.Equal(-1, result.Single());
	}

	[Fact]
	public void SeededScanExSum()
	{
		using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.ScanEx(0, (a, b) => a + b);
		result.AssertSequenceEqual(0, 1, 3, 6, 10, 15, 21, 28, 36, 45, 55);
	}

	[Fact]
	public void SeededScanExIsLazy()
	{
		_ = new BreakingSequence<object>().ScanEx(seed: null,
			BreakingFunc.Of<object?, object, object>());
	}

	[Fact]
	public void SeededScanExDoesNotIterateExtra()
	{
		using var seq = SeqExceptionAt(4).AsTestingSequence(maxEnumerations: 2);

		var result = seq.ScanEx(0, (a, b) => a + b);

		_ = Assert.Throws<TestException>(result.Consume);
		result.Take(4).AssertSequenceEqual(0, 1, 3, 6);
	}
}

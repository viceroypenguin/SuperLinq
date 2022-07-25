namespace Test;

public class ScanExTest
{
	[Fact]
	public void ScanExEmpty()
	{
		Assert.Empty(Enumerable.Empty<int>().ScanEx((a, b) => a + b));
	}

	[Fact]
	public void ScanExSum()
	{
		var result = Enumerable.Range(1, 10).ScanEx((a, b) => a + b);
		result.AssertSequenceEqual(1, 3, 6, 10, 15, 21, 28, 36, 45, 55);
	}

	[Fact]
	public void ScanExIsLazy()
	{
		new BreakingSequence<object>().ScanEx(BreakingFunc.Of<object, object, object>());
	}

	[Fact]
	public void ScanExDoesNotIterateExtra()
	{
		var sequence = Enumerable.Range(1, 3).Concat(new BreakingSequence<int>()).ScanEx((a, b) => a + b);
		Assert.Throws<InvalidOperationException>(sequence.Consume);
		sequence.Take(3).AssertSequenceEqual(1, 3, 6);
	}

	[Fact]
	public void SeededScanExEmpty()
	{
		Assert.Equal(-1, Enumerable.Empty<int>().ScanEx(-1, (a, b) => a + b).Single());
	}

	[Fact]
	public void SeededScanExSum()
	{
		var result = Enumerable.Range(1, 10).ScanEx(0, (a, b) => a + b);
		result.AssertSequenceEqual(0, 1, 3, 6, 10, 15, 21, 28, 36, 45, 55);
	}

	[Fact]
	public void SeededScanExIsLazy()
	{
		new BreakingSequence<object>().ScanEx(seed: null,
			BreakingFunc.Of<object?, object, object>());
	}

	[Fact]
	public void SeededScanExDoesNotIterateExtra()
	{
		var sequence = Enumerable.Range(1, 3).Concat(new BreakingSequence<int>()).ScanEx(0, (a, b) => a + b);
		Assert.Throws<InvalidOperationException>(sequence.Consume);
		sequence.Take(4).AssertSequenceEqual(0, 1, 3, 6);
	}
}

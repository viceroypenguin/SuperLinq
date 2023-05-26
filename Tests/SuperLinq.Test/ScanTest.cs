namespace Test;

public class ScanTest
{
	[Fact]
	public void ScanEmpty()
	{
		using var seq = TestingSequence.Of<int>();

		var result = seq.Scan((a, b) => a + b);
		result.AssertSequenceEqual();
	}

	[Fact]
	public void ScanSum()
	{
		using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.Scan((a, b) => a + b);
		result.AssertSequenceEqual(1, 3, 6, 10, 15, 21, 28, 36, 45, 55);
	}

	[Fact]
	public void ScanIsLazy()
	{
		_ = new BreakingSequence<object>().Scan(BreakingFunc.Of<object, object, object>());
	}

	[Fact]
	public void ScanDoesNotIterateExtra()
	{
		using var seq = SeqExceptionAt(4).AsTestingSequence(maxEnumerations: 2);

		var result = seq.Scan((a, b) => a + b);

		_ = Assert.Throws<TestException>(result.Consume);
		result.Take(3).AssertSequenceEqual(1, 3, 6);
	}

	[Fact]
	public void ScanCollection()
	{
		using var seq = Enumerable.Range(1, 10).AsBreakingCollection();

		var result = seq.Scan((a, b) => a + b);
		result.AssertCollectionErrorChecking(10);

		result.ToArray()
			.AssertSequenceEqual(1, 3, 6, 10, 15, 21, 28, 36, 45, 55);
		Assert.Equal(1, seq.CopyCount);

		var arr = new int[20];
		_ = result.CopyTo(arr, 5);
		arr
			.AssertSequenceEqual(0, 0, 0, 0, 0, 1, 3, 6, 10, 15, 21, 28, 36, 45, 55, 0, 0, 0, 0, 0);
		Assert.Equal(2, seq.CopyCount);
	}

	[Fact]
	public void ScanList()
	{
		using var seq = Enumerable.Range(1, 10).AsBreakingList();

		var result = seq.Scan((a, b) => a + b);
		result.AssertCollectionErrorChecking(10);

		result.ToArray()
			.AssertSequenceEqual(1, 3, 6, 10, 15, 21, 28, 36, 45, 55);

		var arr = new int[20];
		_ = result.CopyTo(arr, 5);
		arr
			.AssertSequenceEqual(0, 0, 0, 0, 0, 1, 3, 6, 10, 15, 21, 28, 36, 45, 55, 0, 0, 0, 0, 0);
	}

	[Fact]
	public void SeededScanEmpty()
	{
		using var seq = TestingSequence.Of<int>();

		var result = seq.Scan(-1, (a, b) => a + b);
		Assert.Equal(-1, result.Single());
	}

	[Fact]
	public void SeededScanSum()
	{
		using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.Scan(0, (a, b) => a + b);
		result.AssertSequenceEqual(0, 1, 3, 6, 10, 15, 21, 28, 36, 45, 55);
	}

	[Fact]
	public void SeededScanIsLazy()
	{
		_ = new BreakingSequence<object>().Scan(seed: null,
			BreakingFunc.Of<object?, object, object>());
	}

	[Fact]
	public void SeededScanDoesNotIterateExtra()
	{
		using var seq = SeqExceptionAt(4).AsTestingSequence(maxEnumerations: 2);

		var result = seq.Scan(0, (a, b) => a + b);

		_ = Assert.Throws<TestException>(result.Consume);
		result.Take(4).AssertSequenceEqual(0, 1, 3, 6);
	}

	[Fact]
	public void SeededScanCollection()
	{
		using var seq = Enumerable.Range(1, 10).AsBreakingCollection();

		var result = seq.Scan(5, (a, b) => a + b);
		result.AssertCollectionErrorChecking(11);

		result.ToArray()
			.AssertSequenceEqual(5, 6, 8, 11, 15, 20, 26, 33, 41, 50, 60);
		Assert.Equal(1, seq.CopyCount);

		var arr = new int[20];
		_ = result.CopyTo(arr, 5);
		arr
			.AssertSequenceEqual(0, 0, 0, 0, 0, 5, 6, 8, 11, 15, 20, 26, 33, 41, 50, 60, 0, 0, 0, 0);
		Assert.Equal(2, seq.CopyCount);
	}

	[Fact]
	public void SeededScanList()
	{
		using var seq = Enumerable.Range(1, 10).AsBreakingList();

		var result = seq.Scan(5, (a, b) => a + b);
		result.AssertCollectionErrorChecking(11);

		result.ToArray()
			.AssertSequenceEqual(5, 6, 8, 11, 15, 20, 26, 33, 41, 50, 60);

		var arr = new int[20];
		_ = result.CopyTo(arr, 5);
		arr
			.AssertSequenceEqual(0, 0, 0, 0, 0, 5, 6, 8, 11, 15, 20, 26, 33, 41, 50, 60, 0, 0, 0, 0);
	}
}

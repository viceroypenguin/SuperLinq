namespace Test.Async;

/// <summary>
/// Verify the behavior of the Window operator
/// </summary>
public sealed class WindowTests
{
	/// <summary>
	/// Verify that Window behaves in a lazy manner
	/// </summary>
	[Fact]
	public void TestWindowIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().Window(1);
	}

	[Fact]
	public async Task WindowModifiedBeforeMoveNextDoesNotAffectNextWindow()
	{
		await using var sequence = AsyncEnumerable.Range(0, 3).AsTestingSequence();
		await using var e = sequence.Window(2).GetAsyncEnumerator();

		_ = await e.MoveNextAsync();
		var window1 = e.Current;
		window1[1] = -1;
		_ = await e.MoveNextAsync();
		var window2 = e.Current;

		Assert.Equal(1, window2[0]);
	}

	[Fact]
	public async Task WindowModifiedAfterMoveNextDoesNotAffectNextWindow()
	{
		await using var sequence = AsyncEnumerable.Range(0, 3).AsTestingSequence();
		await using var e = sequence.Window(2).GetAsyncEnumerator();

		_ = await e.MoveNextAsync();
		var window1 = e.Current;
		_ = await e.MoveNextAsync();
		window1[1] = -1;
		var window2 = e.Current;

		Assert.Equal(1, window2[0]);
	}

	[Fact]
	public async Task WindowModifiedDoesNotAffectPreviousWindow()
	{
		await using var sequence = AsyncEnumerable.Range(0, 3).AsTestingSequence();
		await using var e = sequence.Window(2).GetAsyncEnumerator();

		_ = await e.MoveNextAsync();
		var window1 = e.Current;
		_ = await e.MoveNextAsync();
		var window2 = e.Current;
		window2[0] = -1;

		Assert.Equal(1, window1[1]);
	}

	/// <summary>
	/// Verify that a negative window size results in an exception
	/// </summary>
	[Fact]
	public void TestWindowNegativeWindowSizeException()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new AsyncBreakingSequence<int>().Window(-5));
	}

	/// <summary>
	/// Verify that a sliding window of an any size over an empty sequence
	/// is an empty sequence
	/// </summary>
	[Fact]
	public async Task TestWindowEmptySequence()
	{
		await using var sequence = TestingSequence.Of<int>();

		var result = sequence.Window(5);
		await result.AssertEmpty();
	}

	/// <summary>
	/// Verify that decomposing a sequence into windows of a single item
	/// degenerates to the original sequence.
	/// </summary>
	[Fact]
	public async Task TestWindowOfSingleElement()
	{
		await using var sequence = Enumerable.Range(0, 100).AsTestingSequence();

		var result = await sequence.Window(1).ToListAsync();

		// number of windows should be equal to the source sequence length
		Assert.Equal(100, result.Count);
		// each window should contain single item consistent of element at that offset
		var index = -1;
		foreach (var window in result)
			Assert.Equal(Enumerable.Range(0, 100).ElementAt(++index), window.Single());
	}

	/// <summary>
	/// Verify that asking for a window large than the source sequence results
	/// in a empty sequence.
	/// </summary>
	[Fact]
	public async Task TestWindowLargerThanSequence()
	{
		await using var sequence = Enumerable.Range(0, 100).AsTestingSequence();

		var result = sequence.Window(101);

		// there should only be one window whose contents is the same
		// as the source sequence
		await result.AssertEmpty();
	}

	/// <summary>
	/// Verify that asking for a window smaller than the source sequence results
	/// in N sequences, where N = (source.Count() - windowSize) + 1.
	/// </summary>
	[Fact]
	public async Task TestWindowSmallerThanSequence()
	{
		await using var sequence = Enumerable.Range(0, 100).AsTestingSequence();

		var result = await sequence.Window(33).ToListAsync();

		// ensure that the number of windows is correct
		Assert.Equal(100 - 33 + 1, result.Count);
		// ensure each window contains the correct set of items
		var index = -1;
		foreach (var window in result)
			Assert.Equal(Enumerable.Range(0, 100).Skip(++index).Take(33), window);
	}

	/// <summary>
	/// Verify that later windows do not modify any of the previous ones.
	/// </summary>

	[Fact]
	public async Task TestWindowWindowsImmutability()
	{
		await using var windows = AsyncEnumerable.Range(1, 5).Window(2).AsTestingSequence();

		await using var reader = windows.Read();
		(await reader.Read()).AssertSequenceEqual(1, 2);
		(await reader.Read()).AssertSequenceEqual(2, 3);
		(await reader.Read()).AssertSequenceEqual(3, 4);
		(await reader.Read()).AssertSequenceEqual(4, 5);
		await reader.ReadEnd();
	}
}

namespace Test.Async;

/// <summary>
/// Verify the behavior of the Window operator
/// </summary>
public class WindowTests
{
	/// <summary>
	/// Verify that Window behaves in a lazy manner
	/// </summary>
	[Fact]
	public void TestWindowIsLazy()
	{
		new AsyncBreakingSequence<int>().Window(1);
	}

	[Fact]
	public async Task WindowModifiedBeforeMoveNextDoesNotAffectNextWindow()
	{
		var sequence = AsyncEnumerable.Range(0, 3);
		await using var e = sequence.Window(2).GetAsyncEnumerator();

		await e.MoveNextAsync();
		var window1 = e.Current;
		window1[1] = -1;
		await e.MoveNextAsync();
		var window2 = e.Current;

		Assert.Equal(1, window2[0]);
	}

	[Fact]
	public async Task WindowModifiedAfterMoveNextDoesNotAffectNextWindow()
	{
		var sequence = AsyncEnumerable.Range(0, 3);
		await using var e = sequence.Window(2).GetAsyncEnumerator();

		await e.MoveNextAsync();
		var window1 = e.Current;
		await e.MoveNextAsync();
		window1[1] = -1;
		var window2 = e.Current;

		Assert.Equal(1, window2[0]);
	}

	[Fact]
	public async Task WindowModifiedDoesNotAffectPreviousWindow()
	{
		var sequence = AsyncEnumerable.Range(0, 3);
		await using var e = sequence.Window(2).GetAsyncEnumerator();

		await e.MoveNextAsync();
		var window1 = e.Current;
		await e.MoveNextAsync();
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
		var sequence = AsyncEnumerable.Repeat(1, 10);

		Assert.Throws<ArgumentOutOfRangeException>(() =>
			sequence.Window(-5));
	}

	/// <summary>
	/// Verify that a sliding window of an any size over an empty sequence
	/// is an empty sequence
	/// </summary>
	[Fact]
	public Task TestWindowEmptySequence()
	{
		var sequence = AsyncEnumerable.Empty<int>();
		var result = sequence.Window(5);

		return result.AssertEmpty();
	}

	/// <summary>
	/// Verify that decomposing a sequence into windows of a single item
	/// degenerates to the original sequence.
	/// </summary>
	[Fact]
	public async Task TestWindowOfSingleElement()
	{
		const int Count = 100;
		var sequence = Enumerable.Range(1, Count);
		var result = sequence.ToAsyncEnumerable().Window(1);

		// number of windows should be equal to the source sequence length
		Assert.Equal(Count, await result.CountAsync());
		// each window should contain single item consistent of element at that offset
		var index = -1;
		await foreach (var window in result)
			Assert.Equal(sequence.ElementAt(++index), window.Single());
	}

	/// <summary>
	/// Verify that asking for a window large than the source sequence results
	/// in a empty sequence.
	/// </summary>
	[Fact]
	public Task TestWindowLargerThanSequence()
	{
		const int Count = 100;
		var sequence = AsyncEnumerable.Range(1, Count);
		var result = sequence.Window(Count + 1);

		// there should only be one window whose contents is the same
		// as the source sequence
		return result.AssertEmpty();
	}

	/// <summary>
	/// Verify that asking for a window smaller than the source sequence results
	/// in N sequences, where N = (source.Count() - windowSize) + 1.
	/// </summary>
	[Fact]
	public async Task TestWindowSmallerThanSequence()
	{
		const int Count = 100;
		const int WindowSize = Count / 3;
		var sequence = Enumerable.Range(1, Count);
		var result = sequence.ToAsyncEnumerable().Window(WindowSize);

		// ensure that the number of windows is correct
		Assert.Equal(Count - WindowSize + 1, await result.CountAsync());
		// ensure each window contains the correct set of items
		var index = -1;
		await foreach (var window in result)
			Assert.Equal(sequence.Skip(++index).Take(WindowSize), window);
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

namespace Test;

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
		new BreakingSequence<int>().Window(1);
	}

	[Fact]
	public void WindowModifiedBeforeMoveNextDoesNotAffectNextWindow()
	{
		var sequence = Enumerable.Range(0, 3);
		using var e = sequence.Window(2).GetEnumerator();

		e.MoveNext();
		var window1 = e.Current;
		window1[1] = -1;
		e.MoveNext();
		var window2 = e.Current;

		Assert.Equal(1, window2[0]);
	}

	[Fact]
	public void WindowModifiedAfterMoveNextDoesNotAffectNextWindow()
	{
		var sequence = Enumerable.Range(0, 3);
		using var e = sequence.Window(2).GetEnumerator();

		e.MoveNext();
		var window1 = e.Current;
		e.MoveNext();
		window1[1] = -1;
		var window2 = e.Current;

		Assert.Equal(1, window2[0]);
	}

	[Fact]
	public void WindowModifiedDoesNotAffectPreviousWindow()
	{
		var sequence = Enumerable.Range(0, 3);
		using var e = sequence.Window(2).GetEnumerator();

		e.MoveNext();
		var window1 = e.Current;
		e.MoveNext();
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
		var sequence = Enumerable.Repeat(1, 10);

		Assert.Throws<ArgumentOutOfRangeException>(() =>
			sequence.Window(-5));
	}

	/// <summary>
	/// Verify that a sliding window of an any size over an empty sequence
	/// is an empty sequence
	/// </summary>
	[Fact]
	public void TestWindowEmptySequence()
	{
		var sequence = Enumerable.Empty<int>();
		var result = sequence.Window(5);

		Assert.Empty(result);
	}

	/// <summary>
	/// Verify that decomposing a sequence into windows of a single item
	/// degenerates to the original sequence.
	/// </summary>
	[Fact]
	public void TestWindowOfSingleElement()
	{
		const int count = 100;
		var sequence = Enumerable.Range(1, count);
		var result = sequence.Window(1);

		// number of windows should be equal to the source sequence length
		Assert.Equal(count, result.Count());
		// each window should contain single item consistent of element at that offset
		var index = -1;
		foreach (var window in result)
			Assert.Equal(sequence.ElementAt(++index), window.Single());
	}

	/// <summary>
	/// Verify that asking for a window large than the source sequence results
	/// in a empty sequence.
	/// </summary>
	[Fact]
	public void TestWindowLargerThanSequence()
	{
		const int count = 100;
		var sequence = Enumerable.Range(1, count);
		var result = sequence.Window(count + 1);

		// there should only be one window whose contents is the same
		// as the source sequence
		Assert.Empty(result);
	}

	/// <summary>
	/// Verify that asking for a window smaller than the source sequence results
	/// in N sequences, where N = (source.Count() - windowSize) + 1.
	/// </summary>
	[Fact]
	public void TestWindowSmallerThanSequence()
	{
		const int count = 100;
		const int windowSize = count / 3;
		var sequence = Enumerable.Range(1, count);
		var result = sequence.Window(windowSize);

		// ensure that the number of windows is correct
		Assert.Equal(count - windowSize + 1, result.Count());
		// ensure each window contains the correct set of items
		var index = -1;
		foreach (var window in result)
			Assert.Equal(sequence.Skip(++index).Take(windowSize), window);
	}

	/// <summary>
	/// Verify that later windows do not modify any of the previous ones.
	/// </summary>

	[Fact]
	public void TestWindowWindowsImmutability()
	{
		using var windows = Enumerable.Range(1, 5).Window(2).AsTestingSequence();

		using var reader = windows.ToArray().Read();
		reader.Read().AssertSequenceEqual(1, 2);
		reader.Read().AssertSequenceEqual(2, 3);
		reader.Read().AssertSequenceEqual(3, 4);
		reader.Read().AssertSequenceEqual(4, 5);
		reader.ReadEnd();
	}
}

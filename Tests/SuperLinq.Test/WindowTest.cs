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
		new BreakingSequence<int>().Window(1, BreakingFunc.Of<IReadOnlyList<int>, int>());
		new BreakingSequence<int>().Window(new int[3], BreakingFunc.Of<IReadOnlyList<int>, int>());
		new BreakingSequence<int>().Window(new int[3], 1, BreakingFunc.Of<IReadOnlyList<int>, int>());
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

		Assert.Throws<ArgumentOutOfRangeException>(() =>
			sequence.Window(Array.Empty<int>(), -5, SuperEnumerable.Identity));

		Assert.Throws<ArgumentOutOfRangeException>(() =>
			sequence.Window(-5, SuperEnumerable.Identity));
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
	/// Verify that a sliding window of an any size over an empty sequence
	/// is an empty sequence
	/// </summary>
	[Fact]
	public void TestWindowEmptySequence()
	{
		using var sequence = TestingSequence.Of<int>();

		var result = sequence.Window(5);
		Assert.Empty(result);
	}

	[Fact]
	public void TestWindowBufferEmptySequence()
	{
		using var sequence = TestingSequence.Of<int>();

		var result = sequence.Window(5, SuperEnumerable.Identity);
		Assert.Empty(result);
	}

	/// <summary>
	/// Verify that decomposing a sequence into windows of a single item
	/// degenerates to the original sequence.
	/// </summary>
	[Fact]
	public void TestWindowOfSingleElement()
	{
		var sequence = Enumerable.Range(1, 100);
		using var xs = sequence.AsTestingSequence();
		var result = xs.Window(1).ToList();

		// number of windows should be equal to the source sequence length
		Assert.Equal(100, result.Count);
		// each window should contain single item consistent of element at that offset
		foreach (var (actual, expected) in result.Zip(sequence))
			Assert.Equal(SuperEnumerable.Return(expected), actual);
	}

	[Fact]
	public void TestWindowBufferOfSingleElement()
	{
		var sequence = Enumerable.Range(1, 100);
		using var xs = sequence.AsTestingSequence();
		var result = xs.Window(1, l => l[0]).ToList();

		// number of windows should be equal to the source sequence length
		Assert.Equal(100, result.Count);
		// each window should contain single item consistent of element at that offset
		foreach (var (actual, expected) in result.Zip(sequence))
			Assert.Equal(expected, actual);
	}

	/// <summary>
	/// Verify that asking for a window large than the source sequence results
	/// in a empty sequence.
	/// </summary>
	[Fact]
	public void TestWindowLargerThanSequence()
	{
		var sequence = Enumerable.Range(1, 10);
		using var xs = sequence.AsTestingSequence();
		var result = xs.Window(11).ToList();

		// there should only be one window whose contents is the same
		// as the source sequence
		Assert.Empty(result);
	}

	[Fact]
	public void TestWindowBufferLargerThanSequence()
	{
		var sequence = Enumerable.Range(1, 10);
		using var xs = sequence.AsTestingSequence();
		var result = xs.Window(11, l => l.Sum());

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
		const int Count = 100;
		const int WindowSize = Count / 3;
		var sequence = Enumerable.Range(1, 100);
		using var xs = sequence.AsTestingSequence();
		var result = xs.Window(WindowSize).ToList();

		// ensure that the number of windows is correct
		Assert.Equal(Count - WindowSize + 1, result.Count);
		// ensure each window contains the correct set of items
		var index = 0;
		foreach (var window in result)
			Assert.Equal(sequence.Skip(index++).Take(WindowSize), window);
	}

	[Fact]
	public void TestWindowBufferSmallerThanSequence()
	{
		const int Count = 100;
		const int WindowSize = Count / 3;
		var sequence = Enumerable.Range(1, 100);
		using var xs = sequence.AsTestingSequence();
		var result = xs.Window(WindowSize, l => l.Sum()).ToList();

		// ensure that the number of windows is correct
		Assert.Equal(Count - WindowSize + 1, result.Count);
		// ensure each window contains the correct set of items
		var index = 0;
		foreach (var window in result)
			Assert.Equal(sequence.Skip(index++).Take(WindowSize).Sum(), window);
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

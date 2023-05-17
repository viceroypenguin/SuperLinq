namespace Test;

/// <summary>
/// Verify the behavior of the Window operator
/// </summary>
public partial class WindowTests
{
	/// <summary>
	/// Verify that Window behaves in a lazy manner
	/// </summary>
	[Fact]
	public void TestWindowIsLazy()
	{
		_ = new BreakingSequence<int>().Window(1);
	}

	/// <summary>
	/// Verify that a negative window size results in an exception
	/// </summary>
	[Fact]
	public void TestWindowNegativeWindowSizeException()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().Window(-5));
	}

	[Fact]
	public void WindowModifiedBeforeMoveNextDoesNotAffectNextWindow()
	{
		using var sequence = Enumerable.Range(0, 3).AsTestingSequence();
		using var e = sequence.Window(2).GetEnumerator();

		_ = e.MoveNext();
		var window1 = e.Current;
		window1[1] = -1;
		_ = e.MoveNext();
		var window2 = e.Current;

		Assert.Equal(1, window2[0]);
	}

	[Fact]
	public void WindowModifiedAfterMoveNextDoesNotAffectNextWindow()
	{
		using var sequence = Enumerable.Range(0, 3).AsTestingSequence();
		using var e = sequence.Window(2).GetEnumerator();

		_ = e.MoveNext();
		var window1 = e.Current;
		_ = e.MoveNext();
		window1[1] = -1;
		var window2 = e.Current;

		Assert.Equal(1, window2[0]);
	}

	[Fact]
	public void WindowModifiedDoesNotAffectPreviousWindow()
	{
		using var sequence = Enumerable.Range(0, 3).AsTestingSequence();
		using var e = sequence.Window(2).GetEnumerator();

		_ = e.MoveNext();
		var window1 = e.Current;
		_ = e.MoveNext();
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

	/// <summary>
	/// Verify that decomposing a sequence into windows of a single item
	/// degenerates to the original sequence.
	/// </summary>
	[Fact]
	public void TestWindowOfSingleElement()
	{
		using var sequence = Enumerable.Range(0, 100).AsTestingSequence();

		var result = sequence.Window(1).ToList();

		// number of windows should be equal to the source sequence length
		Assert.Equal(100, result.Count);
		// each window should contain single item consistent of element at that offset
		foreach (var (actual, expected) in result.Zip(Enumerable.Range(0, 100)))
			Assert.Equal(SuperEnumerable.Return(expected), actual);
	}

	/// <summary>
	/// Verify that asking for a window large than the source sequence results
	/// in a empty sequence.
	/// </summary>
	[Fact]
	public void TestWindowLargerThanSequence()
	{
		using var sequence = Enumerable.Range(0, 10).AsTestingSequence(TestingSequence.Options.AllowRepeatedMoveNexts);

		var result = sequence.Window(11).ToList();

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
		using var sequence = Enumerable.Range(0, 100).AsTestingSequence();

		var result = sequence.Window(33).ToList();

		// ensure that the number of windows is correct
		Assert.Equal(100 - 33 + 1, result.Count);
		// ensure each window contains the correct set of items
		var index = 0;
		foreach (var window in result)
			Assert.Equal(Enumerable.Range(0, 100).Skip(index++).Take(33), window);
	}
}

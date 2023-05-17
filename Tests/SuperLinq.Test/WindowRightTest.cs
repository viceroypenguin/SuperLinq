namespace Test;

public partial class WindowRightTest
{
	[Fact]
	public void WindowRightIsLazy()
	{
		_ = new BreakingSequence<int>().WindowRight(1);
	}

	[Fact]
	public void WindowRightNegativeWindowSizeException()
	{
		var sequence = Enumerable.Repeat(1, 10);

		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			sequence.WindowRight(-5));
	}

	[Fact]
	public void WindowModifiedBeforeMoveNextDoesNotAffectNextWindow()
	{
		using var sequence = Enumerable.Range(0, 3).AsTestingSequence();
		using var e = sequence.WindowRight(2).GetEnumerator();

		_ = e.MoveNext();
		var window1 = e.Current;
		window1[0] = -1;
		_ = e.MoveNext();
		var window2 = e.Current;

		Assert.Equal(0, window2[0]);
	}

	[Fact]
	public void WindowModifiedAfterMoveNextDoesNotAffectNextWindow()
	{
		using var sequence = Enumerable.Range(0, 3).AsTestingSequence();
		using var e = sequence.WindowRight(2).GetEnumerator();

		_ = e.MoveNext();
		var window1 = e.Current;
		_ = e.MoveNext();
		window1[0] = -1;
		var window2 = e.Current;

		Assert.Equal(0, window2[0]);
	}

	[Fact]
	public void WindowModifiedDoesNotAffectPreviousWindow()
	{
		using var sequence = Enumerable.Range(0, 3).AsTestingSequence();
		using var e = sequence.WindowRight(2).GetEnumerator();

		_ = e.MoveNext();
		var window1 = e.Current;
		_ = e.MoveNext();
		var window2 = e.Current;
		window2[0] = -1;

		Assert.Equal(0, window1[0]);
	}

	/// <summary>
	/// Verify that a sliding window of an any size over an empty sequence
	/// is an empty sequence
	/// </summary>
	[Fact]
	public void WindowRightEmptySequence()
	{
		using var sequence = TestingSequence.Of<int>();

		var result = sequence.WindowRight(5);
		Assert.Empty(result);
	}

	/// <summary>
	/// Verify that decomposing a sequence into windows of a single item
	/// degenerates to the original sequence.
	/// </summary>
	[Fact]
	public void WindowRightSingleElement()
	{
		using var xs = Enumerable.Range(1, 100).AsTestingSequence();
		var result = xs.WindowRight(1).ToList();

		// number of windows should be equal to the source sequence length
		Assert.Equal(100, result.Count);
		// each window should contain single item consistent of element at that offset
		foreach (var (actual, expected) in result.Zip(Enumerable.Range(1, 100)))
			Assert.Equal(SuperEnumerable.Return(expected), actual);
	}

	[Fact]
	public void WindowRightWithWindowSizeLargerThanSequence()
	{
		using var sequence = Enumerable.Range(1, 5).AsTestingSequence();

		using var reader = sequence.WindowRight(10).Read();
		reader.Read().AssertSequenceEqual(1);
		reader.Read().AssertSequenceEqual(1, 2);
		reader.Read().AssertSequenceEqual(1, 2, 3);
		reader.Read().AssertSequenceEqual(1, 2, 3, 4);
		reader.Read().AssertSequenceEqual(1, 2, 3, 4, 5);
		reader.ReadEnd();
	}

	[Fact]
	public void WindowRightWithWindowSizeSmallerThanSequence()
	{
		using var sequence = Enumerable.Range(1, 5).AsTestingSequence();

		using var reader = sequence.WindowRight(3).Read();
		reader.Read().AssertSequenceEqual(1);
		reader.Read().AssertSequenceEqual(1, 2);
		reader.Read().AssertSequenceEqual(1, 2, 3);
		reader.Read().AssertSequenceEqual(2, 3, 4);
		reader.Read().AssertSequenceEqual(3, 4, 5);
		reader.ReadEnd();
	}
}

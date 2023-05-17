namespace Test;

public partial class WindowLeftTest
{
	[Fact]
	public void WindowLeftIsLazy()
	{
		_ = new BreakingSequence<int>().WindowLeft(1);
	}

	[Fact]
	public void WindowLeftNegativeWindowSizeException()
	{
		var sequence = Enumerable.Repeat(1, 10);

		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			sequence.WindowLeft(-5));
	}

	[Fact]
	public void WindowModifiedBeforeMoveNextDoesNotAffectNextWindow()
	{
		using var sequence = Enumerable.Range(0, 3).AsTestingSequence();
		using var e = sequence.WindowLeft(2).GetEnumerator();

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
		using var e = sequence.WindowLeft(2).GetEnumerator();

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
		using var e = sequence.WindowLeft(2).GetEnumerator();

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
	public void WindowLeftEmptySequence()
	{
		using var sequence = TestingSequence.Of<int>();

		var result = sequence.WindowLeft(5).ToList();
		Assert.Empty(result);
	}

	/// <summary>
	/// Verify that decomposing a sequence into windows of a single item
	/// degenerates to the original sequence.
	/// </summary>
	[Fact]
	public void WindowLeftSingleElement()
	{
		using var xs = Enumerable.Range(1, 100).AsTestingSequence();
		var result = xs.WindowLeft(1).ToList();

		// number of windows should be equal to the source sequence length
		Assert.Equal(100, result.Count);
		// each window should contain single item consistent of element at that offset
		foreach (var (actual, expected) in result.Zip(Enumerable.Range(1, 100)))
			Assert.Equal(SuperEnumerable.Return(expected), actual);
	}

	[Fact]
	public void WindowLeftWithWindowSizeLargerThanSequence()
	{
		using var sequence = Enumerable.Range(1, 5).AsTestingSequence();

		using var reader = sequence.WindowLeft(10).Read();
		reader.Read().AssertSequenceEqual(1, 2, 3, 4, 5);
		reader.Read().AssertSequenceEqual(2, 3, 4, 5);
		reader.Read().AssertSequenceEqual(3, 4, 5);
		reader.Read().AssertSequenceEqual(4, 5);
		reader.Read().AssertSequenceEqual(5);
		reader.ReadEnd();
	}

	[Fact]
	public void WindowLeftWithWindowSizeSmallerThanSequence()
	{
		using var sequence = Enumerable.Range(1, 5).AsTestingSequence();

		using var reader = sequence.WindowLeft(3).Read();
		reader.Read().AssertSequenceEqual(1, 2, 3);
		reader.Read().AssertSequenceEqual(2, 3, 4);
		reader.Read().AssertSequenceEqual(3, 4, 5);
		reader.Read().AssertSequenceEqual(4, 5);
		reader.Read().AssertSequenceEqual(5);
		reader.ReadEnd();
	}
}

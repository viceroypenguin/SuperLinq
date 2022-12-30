namespace Test;

public class WindowLeftTest
{
	[Fact]
	public void WindowLeftIsLazy()
	{
		new BreakingSequence<int>().WindowLeft(1);
		new BreakingSequence<int>().WindowLeft(1, BreakingFunc.Of<IReadOnlyList<int>, int>());
		new BreakingSequence<int>().WindowLeft(new int[3], BreakingFunc.Of<IReadOnlyList<int>, int>());
		new BreakingSequence<int>().WindowLeft(new int[3], 1, BreakingFunc.Of<IReadOnlyList<int>, int>());
	}

	[Fact]
	public void WindowLeftNegativeWindowSizeException()
	{
		var sequence = Enumerable.Repeat(1, 10);

		Assert.Throws<ArgumentOutOfRangeException>(() =>
			sequence.WindowLeft(-5));

		Assert.Throws<ArgumentOutOfRangeException>(() =>
			sequence.WindowLeft(Array.Empty<int>(), -5, SuperEnumerable.Identity));

		Assert.Throws<ArgumentOutOfRangeException>(() =>
			sequence.WindowLeft(-5, SuperEnumerable.Identity));
	}

	[Fact]
	public void WindowModifiedBeforeMoveNextDoesNotAffectNextWindow()
	{
		using var sequence = Enumerable.Range(0, 3).AsTestingSequence();
		using var e = sequence.WindowLeft(2).GetEnumerator();

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
		using var sequence = Enumerable.Range(0, 3).AsTestingSequence();
		using var e = sequence.WindowLeft(2).GetEnumerator();

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
		using var sequence = Enumerable.Range(0, 3).AsTestingSequence();
		using var e = sequence.WindowLeft(2).GetEnumerator();

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
	public void WindowLeftEmptySequence()
	{
		using var sequence = Seq<int>().AsTestingSequence();

		var result = sequence.WindowLeft(5);
		Assert.Empty(result);
	}

	[Fact]
	public void WindowLeftBufferEmptySequence()
	{
		using var sequence = Seq<int>().AsTestingSequence();

		var result = sequence.WindowLeft(5, SuperEnumerable.Identity);
		Assert.Empty(result);
	}

	/// <summary>
	/// Verify that decomposing a sequence into windows of a single item
	/// degenerates to the original sequence.
	/// </summary>
	[Fact]
	public void WindowLeftSingleElement()
	{
		var sequence = Enumerable.Range(1, 100);
		using var xs = sequence.AsTestingSequence();
		var result = xs.WindowLeft(1).ToList();

		// number of windows should be equal to the source sequence length
		Assert.Equal(100, result.Count);
		// each window should contain single item consistent of element at that offset
		foreach (var (actual, expected) in result.Zip(sequence))
			Assert.Equal(SuperEnumerable.Return(expected), actual);
	}

	[Fact]
	public void WindowLeftBufferSingleElement()
	{
		var sequence = Enumerable.Range(1, 100);
		using var xs = sequence.AsTestingSequence();
		var result = xs.WindowLeft(1, l => l[0]).ToList();

		// number of windows should be equal to the source sequence length
		Assert.Equal(100, result.Count);
		// each window should contain single item consistent of element at that offset
		foreach (var (actual, expected) in result.Zip(sequence))
			Assert.Equal(expected, actual);
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
	public void WindowLeftBufferWithWindowSizeLargerThanSequence()
	{
		using var sequence = Enumerable.Range(1, 5).AsTestingSequence();

		using var reader = sequence.WindowLeft(10, a => string.Join("", a)).Read();
		Assert.Equal("12345", reader.Read());
		Assert.Equal("2345", reader.Read());
		Assert.Equal("345", reader.Read());
		Assert.Equal("45", reader.Read());
		Assert.Equal("5", reader.Read());
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

	[Fact]
	public void WindowLeftBufferWithWindowSizeSmallerThanSequence()
	{
		using var sequence = Enumerable.Range(1, 5).AsTestingSequence();

		using var reader = sequence.WindowLeft(3, a => string.Join("", a)).Read();
		Assert.Equal("123", reader.Read());
		Assert.Equal("234", reader.Read());
		Assert.Equal("345", reader.Read());
		Assert.Equal("45", reader.Read());
		Assert.Equal("5", reader.Read());
		reader.ReadEnd();
	}
}

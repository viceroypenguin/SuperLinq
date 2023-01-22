namespace Test;

public class WindowRightTest
{
	[Fact]
	public void WindowRightIsLazy()
	{
		new BreakingSequence<int>().WindowRight(1);
		new BreakingSequence<int>().WindowRight(1, BreakingFunc.Of<IReadOnlyList<int>, int>());
		new BreakingSequence<int>().WindowRight(new int[3], BreakingFunc.Of<IReadOnlyList<int>, int>());
		new BreakingSequence<int>().WindowRight(new int[3], 1, BreakingFunc.Of<IReadOnlyList<int>, int>());
	}

	[Fact]
	public void WindowRightNegativeWindowSizeException()
	{
		var sequence = Enumerable.Repeat(1, 10);

		Assert.Throws<ArgumentOutOfRangeException>(() =>
			sequence.WindowRight(-5));

		Assert.Throws<ArgumentOutOfRangeException>(() =>
			sequence.WindowRight(Array.Empty<int>(), -5, SuperEnumerable.Identity));

		Assert.Throws<ArgumentOutOfRangeException>(() =>
			sequence.WindowRight(-5, SuperEnumerable.Identity));
	}

	[Fact]
	public void WindowModifiedBeforeMoveNextDoesNotAffectNextWindow()
	{
		using var sequence = Enumerable.Range(0, 3).AsTestingSequence();
		using var e = sequence.WindowRight(2).GetEnumerator();

		e.MoveNext();
		var window1 = e.Current;
		window1[0] = -1;
		e.MoveNext();
		var window2 = e.Current;

		Assert.Equal(0, window2[0]);
	}

	[Fact]
	public void WindowModifiedAfterMoveNextDoesNotAffectNextWindow()
	{
		using var sequence = Enumerable.Range(0, 3).AsTestingSequence();
		using var e = sequence.WindowRight(2).GetEnumerator();

		e.MoveNext();
		var window1 = e.Current;
		e.MoveNext();
		window1[0] = -1;
		var window2 = e.Current;

		Assert.Equal(0, window2[0]);
	}

	[Fact]
	public void WindowModifiedDoesNotAffectPreviousWindow()
	{
		using var sequence = Enumerable.Range(0, 3).AsTestingSequence();
		using var e = sequence.WindowRight(2).GetEnumerator();

		e.MoveNext();
		var window1 = e.Current;
		e.MoveNext();
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

	[Fact]
	public void WindowRightBufferEmptySequence()
	{
		using var sequence = TestingSequence.Of<int>();

		var result = sequence.WindowRight(5, SuperEnumerable.Identity);
		Assert.Empty(result);
	}

	/// <summary>
	/// Verify that decomposing a sequence into windows of a single item
	/// degenerates to the original sequence.
	/// </summary>
	[Fact]
	public void WindowRightSingleElement()
	{
		var sequence = Enumerable.Range(1, 100);
		using var xs = sequence.AsTestingSequence();
		var result = xs.WindowRight(1).ToList();

		// number of windows should be equal to the source sequence length
		Assert.Equal(100, result.Count);
		// each window should contain single item consistent of element at that offset
		foreach (var (actual, expected) in result.Zip(sequence))
			Assert.Equal(SuperEnumerable.Return(expected), actual);
	}

	[Fact]
	public void WindowRightBufferSingleElement()
	{
		var sequence = Enumerable.Range(1, 100);
		using var xs = sequence.AsTestingSequence();
		var result = xs.WindowRight(1, l => l[0]).ToList();

		// number of windows should be equal to the source sequence length
		Assert.Equal(100, result.Count);
		// each window should contain single item consistent of element at that offset
		foreach (var (actual, expected) in result.Zip(sequence))
			Assert.Equal(expected, actual);
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
	public void WindowRightBufferWithWindowSizeLargerThanSequence()
	{
		using var sequence = Enumerable.Range(1, 5).AsTestingSequence();

		using var reader = sequence.WindowRight(10, a => string.Join("", a)).Read();
		Assert.Equal("1", reader.Read());
		Assert.Equal("12", reader.Read());
		Assert.Equal("123", reader.Read());
		Assert.Equal("1234", reader.Read());
		Assert.Equal("12345", reader.Read());
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

	[Fact]
	public void WindowRightBufferWithWindowSizeSmallerThanSequence()
	{
		using var sequence = Enumerable.Range(1, 5).AsTestingSequence();

		using var reader = sequence.WindowRight(3, a => string.Join("", a)).Read();
		Assert.Equal("1", reader.Read());
		Assert.Equal("12", reader.Read());
		Assert.Equal("123", reader.Read());
		Assert.Equal("234", reader.Read());
		Assert.Equal("345", reader.Read());
		reader.ReadEnd();
	}
}

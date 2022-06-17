namespace Test;

public class WindowLeftTest
{
	[Fact]
	public void WindowLeftIsLazy()
	{
		new BreakingSequence<int>().WindowLeft(1);
	}

	[Fact]
	public void WindowModifiedBeforeMoveNextDoesNotAffectNextWindow()
	{
		var sequence = Enumerable.Range(0, 3);
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
		var sequence = Enumerable.Range(0, 3);
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
		var sequence = Enumerable.Range(0, 3);
		using var e = sequence.WindowLeft(2).GetEnumerator();

		e.MoveNext();
		var window1 = e.Current;
		e.MoveNext();
		var window2 = e.Current;
		window2[0] = -1;

		Assert.Equal(1, window1[1]);
	}

	[Fact]
	public void WindowLeftWithNegativeWindowSize()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			Enumerable.Repeat(1, 10).WindowLeft(-5));
	}

	[Fact]
	public void WindowLeftWithEmptySequence()
	{
		using var xs = Enumerable.Empty<int>().AsTestingSequence();

		var result = xs.WindowLeft(5);

		Assert.Empty(result);
	}

	[Fact]
	public void WindowLeftWithSingleElement()
	{
		const int count = 100;
		var sequence = Enumerable.Range(1, count).ToArray();

		IList<int>[] result;
		using (var ts = sequence.AsTestingSequence())
			result = ts.WindowLeft(1).ToArray();

		// number of windows should be equal to the source sequence length
		Assert.Equal(count, result.Length);

		// each window should contain single item consistent of element at that offset
		foreach (var (index, item) in result.Index())
			Assert.Equal(item.Single(), sequence[index]);
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

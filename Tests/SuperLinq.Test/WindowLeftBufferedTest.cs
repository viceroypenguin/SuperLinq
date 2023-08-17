namespace Test;

public partial class WindowLeftTest
{
	[Fact]
	public void WindowLeftBufferedIsLazy()
	{
		_ = new BreakingSequence<int>().WindowLeft(1, BreakingFunc.Of<ArraySegment<int>, int>());
		_ = new BreakingSequence<int>().WindowLeft(new int[3], BreakingFunc.Of<ArraySegment<int>, int>());
		_ = new BreakingSequence<int>().WindowLeft(new int[3], 1, BreakingFunc.Of<ArraySegment<int>, int>());
	}

	[Fact]
	public void WindowLeftBufferedNegativeWindowSizeException()
	{
		var sequence = Enumerable.Repeat(1, 10);

		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			sequence.WindowLeft(Array.Empty<int>(), -5, SuperEnumerable.Identity));

		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			sequence.WindowLeft(-5, SuperEnumerable.Identity));
	}

	[Fact]
	public void WindowLeftBufferedEmptySequence()
	{
		using var sequence = Seq<int>().AsTestingSequence();

		var result = sequence.WindowLeft(5, SuperEnumerable.Identity);
		Assert.Empty(result);
	}

	[Fact]
	public void WindowLeftBufferedSingleElement()
	{
		using var xs = Enumerable.Range(1, 100).AsTestingSequence();
		var result = xs.WindowLeft(1, l => l[0]).ToList();

		// number of windows should be equal to the source sequence length
		Assert.Equal(100, result.Count);
		// each window should contain single item consistent of element at that offset
		foreach (var (actual, expected) in result.Zip(Enumerable.Range(1, 100)))
			Assert.Equal(expected, actual);
	}

	[Fact]
	public void WindowLeftBufferedWithWindowSizeLargerThanSequence()
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
	public void WindowLeftBufferedWithWindowSizeSmallerThanSequence()
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

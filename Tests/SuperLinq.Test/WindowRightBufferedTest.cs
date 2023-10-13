namespace Test;

public partial class WindowRightTest
{
	[Fact]
	public void WindowRightBufferedIsLazy()
	{
		_ = new BreakingSequence<int>().WindowRight(1, BreakingFunc.Of<IReadOnlyList<int>, int>());
		_ = new BreakingSequence<int>().WindowRight(new int[3], BreakingFunc.Of<IReadOnlyList<int>, int>());
		_ = new BreakingSequence<int>().WindowRight(new int[3], 1, BreakingFunc.Of<IReadOnlyList<int>, int>());
	}

	[Fact]
	public void WindowRightBufferedNegativeWindowSizeException()
	{
		var sequence = Enumerable.Repeat(1, 10);

		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			sequence.WindowRight([], -5, SuperEnumerable.Identity));

		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			sequence.WindowRight(-5, SuperEnumerable.Identity));
	}

	[Fact]
	public void WindowRightBufferedEmptySequence()
	{
		using var sequence = Seq<int>().AsTestingSequence();

		var result = sequence.WindowRight(5, SuperEnumerable.Identity);
		Assert.Empty(result);
	}

	[Fact]
	public void WindowRightBufferedSingleElement()
	{
		using var xs = Enumerable.Range(1, 100).AsTestingSequence();
		var result = xs.WindowRight(1, l => l[0]).ToList();

		// number of windows should be equal to the source sequence length
		Assert.Equal(100, result.Count);
		// each window should contain single item consistent of element at that offset
		foreach (var (actual, expected) in result.Zip(Enumerable.Range(1, 100)))
			Assert.Equal(expected, actual);
	}

	[Fact]
	public void WindowRightBufferedWithWindowSizeLargerThanSequence()
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
	public void WindowRightBufferedWithWindowSizeSmallerThanSequence()
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

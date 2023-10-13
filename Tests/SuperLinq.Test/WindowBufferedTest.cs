namespace Test;

public partial class WindowTests
{
	[Fact]
	public void TestWindowBufferedIsLazy()
	{
		_ = new BreakingSequence<int>().Window(1, BreakingFunc.Of<IReadOnlyList<int>, int>());
		_ = new BreakingSequence<int>().Window(new int[3], BreakingFunc.Of<IReadOnlyList<int>, int>());
		_ = new BreakingSequence<int>().Window(new int[3], 1, BreakingFunc.Of<IReadOnlyList<int>, int>());
	}

	[Fact]
	public void TestWindowBufferedNegativeWindowSizeException()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().Window(new int[3], -5, SuperEnumerable.Identity));

		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().Window(-5, SuperEnumerable.Identity));
	}

	[Fact]
	public void TestWindowBufferedEmptySequence()
	{
		using var sequence = Seq<int>().AsTestingSequence();

		var result = sequence.Window(5, SuperEnumerable.Identity);
		Assert.Empty(result);
	}

	[Fact]
	public void TestWindowBufferedOfSingleElement()
	{
		using var sequence = Enumerable.Range(0, 100).AsTestingSequence();

		var result = sequence.Window(1, l => l[0]).ToList();

		// number of windows should be equal to the source sequence length
		Assert.Equal(100, result.Count);
		// each window should contain single item consistent of element at that offset
		foreach (var (actual, expected) in result.Zip(Enumerable.Range(0, 100)))
			Assert.Equal(expected, actual);
	}

	[Fact]
	public void TestWindowBufferedLargerThanSequence()
	{
		using var sequence = Enumerable.Range(0, 10).AsTestingSequence(TestingSequence.Options.AllowRepeatedMoveNexts);

		var result = sequence.Window(11, l => l.Sum());

		// there should only be one window whose contents is the same
		// as the source sequence
		Assert.Empty(result);
	}

	[Fact]
	public void TestWindowBufferedSmallerThanSequence()
	{
		using var sequence = Enumerable.Range(0, 100).AsTestingSequence();

		var result = sequence.Window(33, l => l.Sum()).ToList();

		// ensure that the number of windows is correct
		Assert.Equal(100 - 33 + 1, result.Count);
		// ensure each window contains the correct set of items
		var index = 0;
		foreach (var window in result)
			Assert.Equal(Enumerable.Range(0, 100).Skip(index++).Take(33).Sum(), window);
	}
}

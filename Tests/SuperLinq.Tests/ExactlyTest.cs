namespace SuperLinq.Tests;

public sealed class ExactlyTest
{
	[Test]
	public void ExactlyWithNegativeCount()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().Exactly(-1));
	}

	[Test]
	[MethodDataSource(typeof(TestExtensions), nameof(GetEmptySequences))]
	public void ExactlyWithEmptySequenceHasExactlyZeroElements(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.True(seq.Exactly(0));
	}

	[Test]
	[MethodDataSource(typeof(TestExtensions), nameof(GetEmptySequences))]
	public void ExactlyWithEmptySequenceHasExactlyOneElement(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.False(seq.Exactly(1));
	}

	[Test]
	[MethodDataSource(typeof(TestExtensions), nameof(GetSingleElementSequences))]
	public void ExactlyWithSingleElementHasExactlyZeroElements(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.False(seq.Exactly(0));
	}

	[Test]
	[MethodDataSource(typeof(TestExtensions), nameof(GetSingleElementSequences))]
	public void ExactlyWithSingleElementHasExactlyOneElements(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.True(seq.Exactly(1));
	}

	[Test]
	[MethodDataSource(typeof(TestExtensions), nameof(GetSingleElementSequences))]
	public void ExactlyWithSingleElementHasExactlyTwoElements(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.False(seq.Exactly(2));
	}

	[Test]
	[MethodDataSource(typeof(TestExtensions), nameof(GetThreeElementSequences))]
	public void ExactlyWithThreeElementsHasExactlyTwoElements(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.False(seq.Exactly(2));
	}

	[Test]
	[MethodDataSource(typeof(TestExtensions), nameof(GetThreeElementSequences))]
	public void ExactlyWithThreeElementsHasExactlyThreeElements(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.True(seq.Exactly(3));
	}

	[Test]
	[MethodDataSource(typeof(TestExtensions), nameof(GetThreeElementSequences))]
	public void ExactlyWithThreeElementsHasExactlyFourElements(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.False(seq.Exactly(4));
	}

	[Test]
	public void ExactlyDoesNotIterateUnnecessaryElements()
	{
		using var source = SeqExceptionAt(4).AsTestingSequence();
		Assert.False(source.Exactly(2));
	}
}

namespace SuperLinq.Tests;

public sealed class AtMostTest
{
	[Test]
	public void AtMostWithNegativeCount()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().AtMost(-1));
	}

	[Test]
	[MethodDataSource(typeof(TestExtensions), nameof(GetEmptySequences))]
	public void AtMostWithEmptySequenceHasAtMostZeroElements(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.True(seq.AtMost(0));
	}

	[Test]
	[MethodDataSource(typeof(TestExtensions), nameof(GetEmptySequences))]
	public void AtMostWithEmptySequenceHasAtMostOneElement(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.True(seq.AtMost(1));
	}

	[Test]
	[MethodDataSource(typeof(TestExtensions), nameof(GetSingleElementSequences))]
	public void AtMostWithSingleElementHasAtMostZeroElements(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.False(seq.AtMost(0));
	}

	[Test]
	[MethodDataSource(typeof(TestExtensions), nameof(GetSingleElementSequences))]
	public void AtMostWithSingleElementHasAtMostOneElement(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.True(seq.AtMost(1));
	}

	[Test]
	[MethodDataSource(typeof(TestExtensions), nameof(GetSingleElementSequences))]
	public void AtMostWithSingleElementHasAtMostManyElements(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.True(seq.AtMost(2));
	}

	[Test]
	[MethodDataSource(typeof(TestExtensions), nameof(GetThreeElementSequences))]
	public void AtMostWithManyElementsHasAtMostZeroElements(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.False(seq.AtMost(0));
	}

	[Test]
	[MethodDataSource(typeof(TestExtensions), nameof(GetThreeElementSequences))]
	public void AtMostWithManyElementsHasAtMostOneElement(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.False(seq.AtMost(1));
	}

	[Test]
	[MethodDataSource(typeof(TestExtensions), nameof(GetThreeElementSequences))]
	public void AtMostWithManyElementsHasAtMostManyElement(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.True(seq.AtMost(4));
	}

	[Test]
	public void AtMostDoesNotIterateUnnecessaryElements()
	{
		using var source = SeqExceptionAt(4).AsTestingSequence();
		Assert.False(source.AtMost(2));
	}
}

namespace SuperLinq.Tests;

public sealed class AtLeastTest
{
	[Test]
	public void AtLeastWithNegativeCount()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().AtLeast(-1));
	}

	[Test]
	[MethodDataSource(typeof(TestExtensions), nameof(GetEmptySequences))]
	public void AtLeastWithEmptySequenceHasAtLeastZeroElements(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.True(seq.AtLeast(0));
	}

	[Test]
	[MethodDataSource(typeof(TestExtensions), nameof(GetEmptySequences))]
	public void AtLeastWithEmptySequenceHasAtLeastOneElement(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.False(seq.AtLeast(1));
	}

	[Test]
	[MethodDataSource(typeof(TestExtensions), nameof(GetSingleElementSequences))]
	public void AtLeastWithSingleElementHasAtLeastZeroElements(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.True(seq.AtLeast(0));
	}

	[Test]
	[MethodDataSource(typeof(TestExtensions), nameof(GetSingleElementSequences))]
	public void AtLeastWithSingleElementHasAtLeastOneElement(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.True(seq.AtLeast(1));
	}

	[Test]
	[MethodDataSource(typeof(TestExtensions), nameof(GetSingleElementSequences))]
	public void AtLeastWithSingleElementHasAtLeastManyElements(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.False(seq.AtLeast(2));
	}

	[Test]
	[MethodDataSource(typeof(TestExtensions), nameof(GetThreeElementSequences))]
	public void AtLeastWithManyElementsHasAtLeastZeroElements(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.True(seq.AtLeast(0));
	}

	[Test]
	[MethodDataSource(typeof(TestExtensions), nameof(GetThreeElementSequences))]
	public void AtLeastWithManyElementsHasAtLeastOneElement(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.True(seq.AtLeast(1));
	}

	[Test]
	[MethodDataSource(typeof(TestExtensions), nameof(GetThreeElementSequences))]
	public void AtLeastWithManyElementsHasAtLeastManyElement(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.False(seq.AtLeast(4));
	}

	[Test]
	public void AtLeastDoesNotIterateUnnecessaryElements()
	{
		using var source = SeqExceptionAt(3).AsTestingSequence();
		Assert.True(source.AtLeast(2));
	}
}

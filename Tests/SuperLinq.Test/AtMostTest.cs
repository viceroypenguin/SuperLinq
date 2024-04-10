namespace Test;

public sealed class AtMostTest
{
	[Fact]
	public void AtMostWithNegativeCount()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().AtMost(-1));
	}

	public static IEnumerable<object[]> GetSequences(IEnumerable<int> seq) =>
		seq
			.GetBreakingCollectionSequences()
			.Select(x => new object[] { x });

	[Theory]
	[MemberData(nameof(GetSequences), new int[] { })]
	public void AtMostWithEmptySequenceHasAtMostZeroElements(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.True(seq.AtMost(0));
	}

	[Theory]
	[MemberData(nameof(GetSequences), new int[] { })]
	public void AtMostWithEmptySequenceHasAtMostOneElement(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.True(seq.AtMost(1));
	}

	[Theory]
	[MemberData(nameof(GetSequences), new int[] { 1, })]
	public void AtMostWithSingleElementHasAtMostZeroElements(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.False(seq.AtMost(0));
	}

	[Theory]
	[MemberData(nameof(GetSequences), new int[] { 1, })]
	public void AtMostWithSingleElementHasAtMostOneElement(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.True(seq.AtMost(1));
	}

	[Theory]
	[MemberData(nameof(GetSequences), new int[] { 1, })]
	public void AtMostWithSingleElementHasAtMostManyElements(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.True(seq.AtMost(2));
	}

	[Theory]
	[MemberData(nameof(GetSequences), new int[] { 1, 2, 3, })]
	public void AtMostWithManyElementsHasAtMostZeroElements(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.False(seq.AtMost(0));
	}

	[Theory]
	[MemberData(nameof(GetSequences), new int[] { 1, 2, 3, })]
	public void AtMostWithManyElementsHasAtMostOneElement(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.False(seq.AtMost(1));
	}

	[Theory]
	[MemberData(nameof(GetSequences), new int[] { 1, 2, 3, })]
	public void AtMostWithManyElementsHasAtMostManyElement(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.True(seq.AtMost(4));
	}

	[Fact]
	public void AtMostDoesNotIterateUnnecessaryElements()
	{
		using var source = SeqExceptionAt(4).AsTestingSequence();
		Assert.False(source.AtMost(2));
	}
}

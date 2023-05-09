namespace Test;

public class AtLeastTest
{
	[Fact]
	public void AtLeastWithNegativeCount()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().AtLeast(-1));
	}

	public static IEnumerable<object[]> GetSequences(IEnumerable<int> seq) =>
		seq
			.GetCollectionSequences()
			.Select(x => new object[] { x });

	[Theory]
	[MemberData(nameof(GetSequences), new int[] { })]
	public void AtLeastWithEmptySequenceHasAtLeastZeroElements(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.True(seq.AtLeast(0));
	}

	[Theory]
	[MemberData(nameof(GetSequences), new int[] { })]
	public void AtLeastWithEmptySequenceHasAtLeastOneElement(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.False(seq.AtLeast(1));
	}

	[Theory]
	[MemberData(nameof(GetSequences), new int[] { 1, })]
	public void AtLeastWithSingleElementHasAtLeastZeroElements(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.True(seq.AtLeast(0));
	}

	[Theory]
	[MemberData(nameof(GetSequences), new int[] { 1, })]
	public void AtLeastWithSingleElementHasAtLeastOneElement(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.True(seq.AtLeast(1));
	}

	[Theory]
	[MemberData(nameof(GetSequences), new int[] { 1, })]
	public void AtLeastWithSingleElementHasAtLeastManyElements(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.False(seq.AtLeast(2));
	}

	[Theory]
	[MemberData(nameof(GetSequences), new int[] { 1, 2, 3, })]
	public void AtLeastWithManyElementsHasAtLeastZeroElements(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.True(seq.AtLeast(0));
	}

	[Theory]
	[MemberData(nameof(GetSequences), new int[] { 1, 2, 3, })]
	public void AtLeastWithManyElementsHasAtLeastOneElement(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.True(seq.AtLeast(1));
	}

	[Theory]
	[MemberData(nameof(GetSequences), new int[] { 1, 2, 3, })]
	public void AtLeastWithManyElementsHasAtLeastManyElement(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.False(seq.AtLeast(4));
	}

	[Fact]
	public void AtLeastDoesNotIterateUnnecessaryElements()
	{
		using var source = SeqExceptionAt(3).AsTestingSequence();
		Assert.True(source.AtLeast(2));
	}
}

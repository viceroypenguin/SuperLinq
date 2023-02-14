namespace Test;

public class ExactlyTest
{
	[Fact]
	public void ExactlyWithNegativeCount()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().Exactly(-1));
	}

	public static IEnumerable<object[]> GetSequences(IEnumerable<int> seq) =>
		seq
			.ArrangeCollectionInlineDatas()
			.Select(x => new object[] { x });

	[Theory]
	[MemberData(nameof(GetSequences), new int[] { })]
	public void ExactlyWithEmptySequenceHasExactlyZeroElements(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.True(seq.Exactly(0));
	}

	[Theory]
	[MemberData(nameof(GetSequences), new int[] { })]
	public void ExactlyWithEmptySequenceHasExactlyOneElement(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.False(seq.Exactly(1));
	}

	[Theory]
	[MemberData(nameof(GetSequences), new int[] { 1, })]
	public void ExactlyWithSingleElementHasExactlyZeroElements(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.False(seq.Exactly(0));
	}

	[Theory]
	[MemberData(nameof(GetSequences), new int[] { 1, })]
	public void ExactlyWithSingleElementHasExactlyOneElements(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.True(seq.Exactly(1));
	}

	[Theory]
	[MemberData(nameof(GetSequences), new int[] { 1, })]
	public void ExactlyWithSingleElementHasExactlyTwoElements(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.False(seq.Exactly(2));
	}

	[Theory]
	[MemberData(nameof(GetSequences), new int[] { 1, 2, 3, })]
	public void ExactlyWithThreeElementsHasExactlyTwoElements(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.False(seq.Exactly(2));
	}

	[Theory]
	[MemberData(nameof(GetSequences), new int[] { 1, 2, 3, })]
	public void ExactlyWithThreeElementsHasExactlyThreeElements(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.True(seq.Exactly(3));
	}

	[Theory]
	[MemberData(nameof(GetSequences), new int[] { 1, 2, 3, })]
	public void ExactlyWithThreeElementsHasExactlyFourElements(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.False(seq.Exactly(4));
	}

	[Fact]
	public void ExactlyDoesNotIterateUnnecessaryElements()
	{
		using var source = SeqExceptionAt(4).AsTestingSequence();
		Assert.False(source.Exactly(2));
	}
}

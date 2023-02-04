namespace Test;

public class ReplaceTest
{
	[Fact]
	public void ReplaceIsLazy()
	{
		new BreakingSequence<int>().Replace(0, 10);
		new BreakingSequence<int>().Replace(new Index(10), 10);
		new BreakingSequence<int>().Replace(^0, 10);
	}

	[Fact]
	public void ReplaceEmptySequence()
	{
		using var seq = Enumerable.Empty<int>().AsTestingSequence(maxEnumerations: 6);
		seq.Replace(0, 10).AssertSequenceEqual();
		seq.Replace(10, 10).AssertSequenceEqual();
		seq.Replace(new Index(0), 10).AssertSequenceEqual();
		seq.Replace(new Index(10), 10).AssertSequenceEqual();
		seq.Replace(^0, 10).AssertSequenceEqual();
		seq.Replace(^10, 10).AssertSequenceEqual();
	}

	public static IEnumerable<object[]> Indices() =>
		Enumerable.Range(0, 10).Select(i => new object[] { i, });

	[Theory, MemberData(nameof(Indices))]
	public void ReplaceIntIndex(int index)
	{
		using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.Replace(index, 30);
		result.AssertSequenceEqual(
			Enumerable.Range(1, index)
				.Append(30)
				.Concat(Enumerable.Range(index + 2, 9 - index)));
	}

	[Theory, MemberData(nameof(Indices))]
	public void ReplaceStartIndex(int index)
	{
		using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.Replace(new Index(index), 30);
		result.AssertSequenceEqual(
			Enumerable.Range(1, index)
				.Append(30)
				.Concat(Enumerable.Range(index + 2, 9 - index)));
	}

	[Theory, MemberData(nameof(Indices))]
	public void ReplaceEndIndex(int index)
	{
		using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.Replace(^index, 30);
		result.AssertSequenceEqual(
			Enumerable.Range(1, 9 - index)
				.Append(30)
				.Concat(Enumerable.Range(11 - index, index)));
	}

	[Theory, MemberData(nameof(Indices))]
	public void ReplaceEndIndexWithCollection(int index)
	{
		var seq = Enumerable.Range(1, 10).ToArray();

		// offset by one because 0..9 is less useful than 1..10
		var result = seq.Replace(^(index + 1), 30);
		result.AssertSequenceEqual(
			Enumerable.Range(1, 9 - index)
				.Append(30)
				.Concat(Enumerable.Range(11 - index, index)));
	}

	[Fact]
	public void ReplaceIntIndexPastSequenceLength()
	{
		using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.Replace(10, 30);
		result.AssertSequenceEqual(Enumerable.Range(1, 10));
	}

	[Fact]
	public void ReplaceStartIndexPastSequenceLength()
	{
		using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.Replace(new Index(10), 30);
		result.AssertSequenceEqual(Enumerable.Range(1, 10));
	}

	[Fact]
	public void ReplaceEndIndexPastSequenceLength()
	{
		using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.Replace(^10, 30);
		result.AssertSequenceEqual(Enumerable.Range(1, 10));
	}
}

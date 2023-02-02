namespace Test;

public class ToArrayByIndexTest
{
	[Theory]
	[InlineData(false, new int[0])]
	[InlineData(false, new[] { 5 })]
	[InlineData(false, new[] { 1, 5 })]
	[InlineData(false, new[] { 0, 9 })]
	[InlineData(false, new[] { 0, 5, 9 })]
	[InlineData(false, new[] { 2, 3, 5, 9 })]
	[InlineData(false, new[] { 5, 2, 9, 3 })]
	[InlineData(true, new int[0])]
	[InlineData(true, new[] { 5 })]
	[InlineData(true, new[] { 1, 5 })]
	[InlineData(true, new[] { 0, 9 })]
	[InlineData(true, new[] { 0, 5, 9 })]
	[InlineData(true, new[] { 2, 3, 5, 9 })]
	[InlineData(true, new[] { 5, 2, 9, 3 })]
	public void ToArrayByIndex(bool withLength, int[] indices)
	{
		using var input = indices
			.Select(i => new { Index = i })
			.AsTestingSequence();

		var result = withLength
			? input.ToArrayByIndex(10, e => e.Index).ToList()
			: input.ToArrayByIndex(e => e.Index).ToList();

		var maxLength = withLength ? 10 : indices.DefaultIfEmpty(-1).Max() + 1;
		var expected = Enumerable.Repeat(default(int?), maxLength);
		foreach (var i in indices)
			expected = expected.Replace(i, i);

		result.AssertSequenceEqual(
			expected.Select(x => x == null ? null : new { Index = x.Value, }));
	}

	[Fact]
	public void ToArrayByIndexWithBadIndexSelectorThrows()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			Seq(42).ToArrayByIndex(_ => -1));

		Assert.Throws<ArgumentOutOfRangeException>(() =>
			Seq(42).ToArrayByIndex(_ => -1, BreakingFunc.Of<int, object>()));
	}

	[Theory]
	[InlineData(10, -1)]
	[InlineData(10, 10)]
	public void ToArrayByIndexWithLengthWithBadIndexSelectorThrows(int length, int badIndex)
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			Seq(42).ToArrayByIndex(length, _ => badIndex));

		Assert.Throws<ArgumentOutOfRangeException>(() =>
			Seq(42).ToArrayByIndex(length, _ => badIndex, BreakingFunc.Of<int, object>()));
	}

	[Fact]
	public void ToArrayByIndexOverwritesAtSameIndex()
	{
		var a = new { Index = 2 };
		var b = new { Index = 2 };
		using var input = new[] { a, b }.AsTestingSequence(maxEnumerations: 5);

		Assert.Equal(new[] { null, null, b, }, input.ToArrayByIndex(e => e.Index));
		Assert.Equal(new[] { null, null, b, }, input.ToArrayByIndex(e => e.Index, SuperEnumerable.Identity));

		Assert.Equal(new[] { null, null, b, null, }, input.ToArrayByIndex(4, e => e.Index));
		Assert.Equal(new[] { null, null, b, null, }, input.ToArrayByIndex(4, e => e.Index, SuperEnumerable.Identity));
		Assert.Equal(new[] { null, null, b, null, }, input.ToArrayByIndex(4, e => e.Index, (e, _) => e));
	}
}

using System.Diagnostics.CodeAnalysis;

namespace SuperLinq.Tests;

public sealed class ToArrayByIndexTest
{
	[Test]
	[Arguments(false, new int[0])]
	[Arguments(false, new[] { 5 })]
	[Arguments(false, new[] { 1, 5 })]
	[Arguments(false, new[] { 0, 9 })]
	[Arguments(false, new[] { 0, 5, 9 })]
	[Arguments(false, new[] { 2, 3, 5, 9 })]
	[Arguments(false, new[] { 5, 2, 9, 3 })]
	[Arguments(true, new int[0])]
	[Arguments(true, new[] { 5 })]
	[Arguments(true, new[] { 1, 5 })]
	[Arguments(true, new[] { 0, 9 })]
	[Arguments(true, new[] { 0, 5, 9 })]
	[Arguments(true, new[] { 2, 3, 5, 9 })]
	[Arguments(true, new[] { 5, 2, 9, 3 })]
	[SuppressMessage("Style", "IDE0305:Simplify collection initialization")]
	public void ToArrayByIndex(bool withLength, int[] indices)
	{
		using var input = indices
			.Select(i => new { Index = i })
			.AsTestingSequence();

		var result = withLength
			? input.ToArrayByIndex(10, e => e.Index).ToList()
			: input.ToArrayByIndex(e => e.Index).ToList();

		var maxLength = withLength ? 10 : indices.DefaultIfEmpty(-1).Max() + 1;
		var expected = new int?[maxLength];
		foreach (var i in indices)
			expected[i] = i;

		result.AssertSequenceEqual(
			expected.Select(x => x is null ? null : new { Index = x.Value }));
	}

	[Test]
	public void ToArrayByIndexWithBadIndexSelectorThrows()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			Seq(42).ToArrayByIndex(_ => -1));

		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			Seq(42).ToArrayByIndex(_ => -1, BreakingFunc.Of<int, object>()));
	}

	[Test]
	[Arguments(10, -1)]
	[Arguments(10, 10)]
	public void ToArrayByIndexWithLengthWithBadIndexSelectorThrows(int length, int badIndex)
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			Seq(42).ToArrayByIndex(length, _ => badIndex));

		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			Seq(42).ToArrayByIndex(length, _ => badIndex, BreakingFunc.Of<int, object>()));
	}

	[Test]
	public void ToArrayByIndexOverwritesAtSameIndex()
	{
		var a = new { Index = 2 };
		var b = new { Index = 2 };
		using var input = new[] { a, b }.AsTestingSequence(maxEnumerations: 5);

		Assert.Equal([null, null, b], input.ToArrayByIndex(e => e.Index));
		Assert.Equal([null, null, b], input.ToArrayByIndex(e => e.Index, SuperEnumerable.Identity));

		Assert.Equal([null, null, b, null], input.ToArrayByIndex(4, e => e.Index));
		Assert.Equal([null, null, b, null], input.ToArrayByIndex(4, e => e.Index, SuperEnumerable.Identity));
		Assert.Equal([null, null, b, null], input.ToArrayByIndex(4, e => e.Index, (e, _) => e));
	}
}

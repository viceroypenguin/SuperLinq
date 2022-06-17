namespace Test;

public class ToArrayByIndexTest
{
	[Theory]
	[InlineData(-1, new int[0])]
	[InlineData(-1, new[] { 5 })]
	[InlineData(-1, new[] { 1, 5 })]
	[InlineData(-1, new[] { 0, 9 })]
	[InlineData(-1, new[] { 0, 5, 9 })]
	[InlineData(-1, new[] { 2, 3, 5, 9 })]
	[InlineData(-1, new[] { 5, 2, 9, 3 })]
	[InlineData(10, new int[0])]
	[InlineData(10, new[] { 5 })]
	[InlineData(10, new[] { 1, 5 })]
	[InlineData(10, new[] { 0, 9 })]
	[InlineData(10, new[] { 0, 5, 9 })]
	[InlineData(10, new[] { 2, 3, 5, 9 })]
	[InlineData(10, new[] { 5, 2, 9, 3 })]
	public void ToArrayByIndex(int length, int[] indicies)
	{
		var input = indicies.Select(i => new { Index = i }).ToArray();
		var result = length < 0 ? input.ToArrayByIndex(e => e.Index)
				   : input.ToArrayByIndex(length, e => e.Index);
		var nils = result.ToList();

		var lastIndex = length < 0
					  ? input.Select(e => e.Index).DefaultIfEmpty(-1).Max()
					  : length - 1;
		var expectedLength = lastIndex + 1;
		Assert.Equal(expectedLength, result.Length);

		foreach (var e in from e in input
						  orderby e.Index descending
						  select e)
		{
			Assert.Equal(input.Single(inp => inp.Index == e.Index), result[e.Index]);
			nils.RemoveAt(e.Index);
		}

		Assert.Equal(expectedLength - input.Length, nils.Count);
		Assert.True(nils.All(e => e == null));
	}

	[Fact]
	public void ToArrayByIndexWithBadIndexSelectorThrows()
	{
		var input = new[] { 42 };

		Assert.Throws<InvalidOperationException>(() =>
			input.ToArrayByIndex(_ => -1));

		Assert.Throws<InvalidOperationException>(() =>
			input.ToArrayByIndex(_ => -1, BreakingFunc.Of<int, object>()));
	}

	[Theory]
	[InlineData(10, -1)]
	[InlineData(10, 10)]
	public void ToArrayByIndexWithLengthWithBadIndexSelectorThrows(int length, int badIndex)
	{
		var input = new[] { 42 };
		Assert.Throws<InvalidOperationException>(() =>
			input.ToArrayByIndex(length, _ => badIndex));

		Assert.Throws<InvalidOperationException>(() =>
			input.ToArrayByIndex(10, _ => -1, BreakingFunc.Of<int, object>()));
	}

	[Fact]
	public void ToArrayByIndexOverwritesAtSameIndex()
	{
		var a = new { Index = 2 };
		var b = new { Index = 2 };
		var input = new[] { a, b };

		Assert.Equal(new[] { null, null, b, }, input.ToArrayByIndex(e => e.Index));
		Assert.Equal(new[] { null, null, b, }, input.ToArrayByIndex(e => e.Index, e => e));

		Assert.Equal(new[] { null, null, b, null, }, input.ToArrayByIndex(4, e => e.Index));
		Assert.Equal(new[] { null, null, b, null, }, input.ToArrayByIndex(4, e => e.Index, e => e));
		Assert.Equal(new[] { null, null, b, null, }, input.ToArrayByIndex(4, e => e.Index, (e, _) => e));
	}
}

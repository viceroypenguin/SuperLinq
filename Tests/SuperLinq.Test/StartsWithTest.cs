namespace Test;

public class StartsWithTest
{
	[Theory]
	[InlineData(new[] { 1, 2, 3 }, new[] { 1, 2 }, true)]
	[InlineData(new[] { 1, 2, 3 }, new[] { 1, 2, 3 }, true)]
	[InlineData(new[] { 1, 2, 3 }, new[] { 1, 2, 3, 4 }, false)]
	public void StartsWithWithIntegers(IEnumerable<int> first, IEnumerable<int> second, bool expected)
	{
		Assert.Equal(expected, first.StartsWith(second));
	}

	[Theory]
	[InlineData(new[] { '1', '2', '3' }, new[] { '1', '2' }, true)]
	[InlineData(new[] { '1', '2', '3' }, new[] { '1', '2', '3' }, true)]
	[InlineData(new[] { '1', '2', '3' }, new[] { '1', '2', '3', '4' }, false)]
	public void StartsWithWithChars(IEnumerable<char> first, IEnumerable<char> second, bool expected)
	{
		Assert.Equal(expected, first.StartsWith(second));
	}

	[Theory]
	[InlineData("123", "12", true)]
	[InlineData("123", "123", true)]
	[InlineData("123", "1234", false)]
	public void StartsWithWithStrings(string first, string second, bool expected)
	{
		// Conflict with String.StartsWith(), which has precedence in this case
		Assert.Equal(expected, SuperEnumerable.StartsWith(first, second));
	}

	[Fact]
	public void StartsWithReturnsTrueIfBothEmpty()
	{
		Assert.True(Array.Empty<int>().StartsWith(Array.Empty<int>()));
	}

	[Fact]
	public void StartsWithReturnsFalseIfOnlyFirstIsEmpty()
	{
		Assert.False(Array.Empty<int>().StartsWith(new[] { 1, 2, 3 }));
	}

	[Theory]
	[InlineData("", "", true)]
	[InlineData("1", "", true)]
	public void StartsWithReturnsTrueIfSecondIsEmpty(string first, string second, bool expected)
	{
		// Conflict with String.StartsWith(), which has precedence in this case
		Assert.Equal(expected, SuperEnumerable.StartsWith(first, second));
	}

	[Fact]
	public void StartsWithDisposesBothSequenceEnumerators()
	{
		using var first = TestingSequence.Of(1, 2, 3);
		using var second = TestingSequence.Of(1);

		first.StartsWith(second);
	}

	[Fact]
	public void StartsWithUsesSpecifiedEqualityComparerOrDefault()
	{
		var first = new[] { 1, 2, 3 };
		var second = new[] { 4, 5, 6 };

		Assert.False(first.StartsWith(second));
		Assert.False(first.StartsWith(second, null));
		Assert.False(first.StartsWith(second, EqualityComparer.Create<int>(delegate { return false; })));
		Assert.True(first.StartsWith(second, EqualityComparer.Create<int>(delegate { return true; })));
	}

	[Theory]
	[InlineData(SourceKind.BreakingCollection)]
	public void StartsWithUsesCollectionsCountToAvoidUnnecessaryIteration(SourceKind sourceKind)
	{
		var first = new[] { 1, 2 }.ToSourceKind(sourceKind);
		var second = new[] { 1, 2, 3 }.ToSourceKind(sourceKind);

		Assert.False(first.StartsWith(second));
	}
}

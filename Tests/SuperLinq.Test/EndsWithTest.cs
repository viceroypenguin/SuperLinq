using System.Diagnostics.CodeAnalysis;

namespace Test;

public class EndsWithTest
{
	[Theory]
	[InlineData(new[] { 1, 2, 3 }, new[] { 2, 3 }, true)]
	[InlineData(new[] { 1, 2, 3 }, new[] { 1, 2, 3 }, true)]
	[InlineData(new[] { 1, 2, 3 }, new[] { 0, 1, 2, 3 }, false)]
	public void EndsWithWithIntegers(IEnumerable<int> first, IEnumerable<int> second, bool expected)
	{
		Assert.Equal(expected, first.EndsWith(second));
	}

	[Theory]
	[InlineData(new[] { '1', '2', '3' }, new[] { '2', '3' }, true)]
	[InlineData(new[] { '1', '2', '3' }, new[] { '1', '2', '3' }, true)]
	[InlineData(new[] { '1', '2', '3' }, new[] { '0', '1', '2', '3' }, false)]
	public void EndsWithWithChars(IEnumerable<char> first, IEnumerable<char> second, bool expected)
	{
		Assert.Equal(expected, first.EndsWith(second));
	}

	[Theory]
	[InlineData("123", "23", true)]
	[InlineData("123", "123", true)]
	[InlineData("123", "0123", false)]
	public void EndsWithWithStrings(string first, string second, bool expected)
	{
		// Conflict with String.EndsWith(), which has precedence in this case
		Assert.Equal(expected, SuperEnumerable.EndsWith(first, second));
	}

	[Fact]
	public void EndsWithReturnsTrueIfBothEmpty()
	{
		Assert.True(Array.Empty<int>().EndsWith(Array.Empty<int>()));
	}

	[Fact]
	public void EndsWithReturnsFalseIfOnlyFirstIsEmpty()
	{
		Assert.False(Array.Empty<int>().EndsWith(new[] { 1, 2, 3 }));
	}

	[Theory]
	[InlineData("", "", true)]
	[InlineData("1", "", true)]
	public void EndsWithReturnsTrueIfSecondIsEmpty(string first, string second, bool expected)
	{
		// Conflict with String.EndsWith(), which has precedence in this case
		Assert.Equal(expected, SuperEnumerable.EndsWith(first, second));
	}

	[Fact]
	public void EndsWithDisposesBothSequenceEnumerators()
	{
		using var first = TestingSequence.Of(1, 2, 3);
		using var second = TestingSequence.Of(1);

		first.EndsWith(second);
	}

	[Fact]
	[SuppressMessage("ReSharper", "RedundantArgumentDefaultValue")]
	public void EndsWithUsesSpecifiedEqualityComparerOrDefault()
	{
		var first = new[] { 1, 2, 3 };
		var second = new[] { 4, 5, 6 };

		Assert.False(first.EndsWith(second));
		Assert.False(first.EndsWith(second, null));
		Assert.False(first.EndsWith(second, EqualityComparer.Create<int>(delegate { return false; })));
		Assert.True(first.EndsWith(second, EqualityComparer.Create<int>(delegate { return true; })));
	}

	[Theory]
	[InlineData(SourceKind.BreakingCollection)]
	[InlineData(SourceKind.BreakingReadOnlyCollection)]
	public void EndsWithUsesCollectionsCountToAvoidUnnecessaryIteration(SourceKind sourceKind)
	{
		var first = new[] { 1, 2 }.ToSourceKind(sourceKind);
		var second = new[] { 1, 2, 3 }.ToSourceKind(sourceKind);

		Assert.False(first.EndsWith(second));
	}
}

using System.Collections.Generic;
using Xunit;

namespace Test;

public class StartsWithTest
{
	[Theory]
	[InlineData(new[] { 1, 2, 3 }, new[] { 1, 2 }, true)]
	[InlineData(new[] { 1, 2, 3 }, new[] { 1, 2, 3 }, true)]
	[InlineData(new[] { 1, 2, 3 }, new[] { 1, 2, 3, 4 }, false)]
	public void StartsWithWithIntegers(IEnumerable<int> first, IEnumerable<int> second, bool expected)
	{
		using var f = first.AsTestingSequence();
		using var s = second.AsTestingSequence();

		Assert.Equal(expected, f.StartsWith(s));
	}

	[Theory]
	[InlineData(new[] { '1', '2', '3' }, new[] { '1', '2' }, true)]
	[InlineData(new[] { '1', '2', '3' }, new[] { '1', '2', '3' }, true)]
	[InlineData(new[] { '1', '2', '3' }, new[] { '1', '2', '3', '4' }, false)]
	public void StartsWithWithChars(IEnumerable<char> first, IEnumerable<char> second, bool expected)
	{
		using var f = first.AsTestingSequence();
		using var s = second.AsTestingSequence();

		Assert.Equal(expected, f.StartsWith(s));
	}

	[Theory]
	[InlineData("123", "12", true)]
	[InlineData("123", "123", true)]
	[InlineData("123", "1234", false)]
	public void StartsWithWithStrings(string first, string second, bool expected)
	{
		using var f = first.AsTestingSequence();
		using var s = second.AsTestingSequence();

		Assert.Equal(expected, f.StartsWith(s));
	}

	[Fact]
	public void StartsWithReturnsTrueIfBothEmpty()
	{
		using var f = Array.Empty<int>().AsTestingSequence();
		using var s = Array.Empty<int>().AsTestingSequence();

		Assert.True(f.StartsWith(s));
	}

	[Fact]
	public void StartsWithReturnsFalseIfOnlyFirstIsEmpty()
	{
		using var f = Array.Empty<int>().AsTestingSequence();
		using var s = Seq(1, 2, 3).AsTestingSequence();

		Assert.False(f.StartsWith(s));
	}

	[Theory]
	[InlineData("", "", true)]
	[InlineData("1", "", true)]
	public void StartsWithReturnsTrueIfSecondIsEmpty(string first, string second, bool expected)
	{
		using var f = first.AsTestingSequence();
		using var s = second.AsTestingSequence();

		Assert.Equal(expected, f.StartsWith(s));
	}

	[Fact]
	public void StartsWithUsesSpecifiedEqualityComparerOrDefault()
	{
		var first = Seq(1, 2, 3);
		var second = Seq(4, 5, 6);

		Assert.False(first.StartsWith(second, null));
		Assert.False(first.StartsWith(second, EqualityComparer.Create<int>(delegate { return false; })));
		Assert.True(first.StartsWith(second, EqualityComparer.Create<int>(delegate { return true; })));
	}

	[Fact]
	public void StartsWithUsesCollectionsCountToAvoidUnnecessaryIteration()
	{
		using var first = new BreakingCollection<int>(1, 2);
		using var second = new BreakingCollection<int>(1, 2, 3);

		Assert.False(first.StartsWith(second));
	}
}

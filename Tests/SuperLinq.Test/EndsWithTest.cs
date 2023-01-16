using System.Collections.Generic;
using Xunit;

namespace Test;

public class EndsWithTest
{
	[Theory]
	[InlineData(new[] { 1, 2, 3 }, new[] { 2, 3 }, true)]
	[InlineData(new[] { 1, 2, 3 }, new[] { 1, 2, 3 }, true)]
	[InlineData(new[] { 1, 2, 3 }, new[] { 0, 1, 2, 3 }, false)]
	public void EndsWithWithIntegers(IEnumerable<int> first, IEnumerable<int> second, bool expected)
	{
		using var f = first.AsTestingSequence();
		using var s = second.AsTestingSequence();
		Assert.Equal(expected, f.EndsWith(s));
	}

	[Theory]
	[InlineData(new[] { '1', '2', '3' }, new[] { '2', '3' }, true)]
	[InlineData(new[] { '1', '2', '3' }, new[] { '1', '2', '3' }, true)]
	[InlineData(new[] { '1', '2', '3' }, new[] { '0', '1', '2', '3' }, false)]
	public void EndsWithWithChars(IEnumerable<char> first, IEnumerable<char> second, bool expected)
	{
		using var f = first.AsTestingSequence();
		using var s = second.AsTestingSequence();
		Assert.Equal(expected, f.EndsWith(s));
	}

	[Theory]
	[InlineData("123", "23", true)]
	[InlineData("123", "123", true)]
	[InlineData("123", "0123", false)]
	public void EndsWithWithStrings(string first, string second, bool expected)
	{
		using var f = first.AsTestingSequence();
		using var s = second.AsTestingSequence();
		Assert.Equal(expected, f.EndsWith(s));
	}

	[Fact]
	public void EndsWithReturnsTrueIfBothEmpty()
	{
		using var f = Array.Empty<int>().AsTestingSequence();
		using var s = Array.Empty<int>().AsTestingSequence();
		Assert.True(f.EndsWith(s));
	}

	[Fact]
	public void EndsWithReturnsFalseIfOnlyFirstIsEmpty()
	{
		using var f = Seq<int>().AsTestingSequence();
		using var s = Seq(1, 2, 3).AsTestingSequence();
		Assert.False(f.EndsWith(s));
	}

	[Theory]
	[InlineData("", "", true)]
	[InlineData("1", "", true)]
	public void EndsWithReturnsTrueIfSecondIsEmpty(string first, string second, bool expected)
	{
		using var f = first.AsTestingSequence();
		using var s = second.AsTestingSequence();
		Assert.Equal(expected, f.EndsWith(s));
	}

	[Fact]
	public void EndsWithUsesSpecifiedEqualityComparerOrDefault()
	{
		var first = Seq(1, 2, 3);
		var second = Seq(4, 5, 6);

		Assert.False(first.EndsWith(second));
		Assert.False(first.EndsWith(second, null));
		Assert.False(first.EndsWith(second, EqualityComparer.Create<int>(delegate { return false; })));
		Assert.True(first.EndsWith(second, EqualityComparer.Create<int>(delegate { return true; })));
	}

	[Fact]
	public void EndsWithUsesCollectionsCountToAvoidUnnecessaryIteration()
	{
		using var first = new BreakingCollection<int>(1, 2);
		using var second = new BreakingCollection<int>(1, 2, 3);

		Assert.False(first.EndsWith(second));
	}
}

#if !NO_INDEX

namespace SuperLinq.Tests;

public sealed class EndsWithTest
{
	[Test]
	[Arguments(new[] { 1, 2, 3 }, new[] { 2, 3 }, true)]
	[Arguments(new[] { 1, 2, 3 }, new[] { 1, 2, 3 }, true)]
	[Arguments(new[] { 1, 2, 3 }, new[] { 0, 1, 2, 3 }, false)]
	public void EndsWithWithIntegers(IEnumerable<int> first, IEnumerable<int> second, bool expected)
	{
		using var f = first.AsTestingSequence();
		using var s = second.AsTestingSequence();
		Assert.Equal(expected, f.EndsWith(s));
	}

	[Test]
	[Arguments(new[] { '1', '2', '3' }, new[] { '2', '3' }, true)]
	[Arguments(new[] { '1', '2', '3' }, new[] { '1', '2', '3' }, true)]
	[Arguments(new[] { '1', '2', '3' }, new[] { '0', '1', '2', '3' }, false)]
	public void EndsWithWithChars(IEnumerable<char> first, IEnumerable<char> second, bool expected)
	{
		using var f = first.AsTestingSequence();
		using var s = second.AsTestingSequence();
		Assert.Equal(expected, f.EndsWith(s));
	}

	[Test]
	[Arguments("123", "23", true)]
	[Arguments("123", "123", true)]
	[Arguments("123", "0123", false)]
	public void EndsWithWithStrings(string first, string second, bool expected)
	{
		using var f = first.AsTestingSequence();
		using var s = second.AsTestingSequence();
		Assert.Equal(expected, f.EndsWith(s));
	}

	[Test]
	public void EndsWithReturnsTrueIfBothEmpty()
	{
		using var f = Array.Empty<int>().AsTestingSequence();
		using var s = Array.Empty<int>().AsTestingSequence();
		Assert.True(f.EndsWith(s));
	}

	[Test]
	public void EndsWithReturnsFalseIfOnlyFirstIsEmpty()
	{
		using var f = TestingSequence.Of<int>();
		using var s = TestingSequence.Of(1, 2, 3);
		Assert.False(f.EndsWith(s));
	}

	[Test]
	[Arguments("", "", true)]
	[Arguments("1", "", true)]
	public void EndsWithReturnsTrueIfSecondIsEmpty(string first, string second, bool expected)
	{
		using var f = first.AsTestingSequence();
		using var s = second.AsTestingSequence();
		Assert.Equal(expected, f.EndsWith(s));
	}

	[Test]
	public void EndsWithUsesSpecifiedEqualityComparerOrDefault()
	{
		var first = Seq(1, 2, 3);
		var second = Seq(4, 5, 6);

		Assert.False(first.EndsWith(second));
		Assert.False(first.EndsWith(second, comparer: null));
		Assert.False(first.EndsWith(second, EqualityComparer.Create<int>(delegate { return false; })));
		Assert.True(first.EndsWith(second, EqualityComparer.Create<int>(delegate { return true; })));
	}

	[Test]
	public void EndsWithUsesCollectionsCountToAvoidUnnecessaryIteration()
	{
		using var first = new BreakingCollection<int>(1, 2);
		using var second = new BreakingCollection<int>(1, 2, 3);

		Assert.False(first.EndsWith(second));
	}
}

#endif

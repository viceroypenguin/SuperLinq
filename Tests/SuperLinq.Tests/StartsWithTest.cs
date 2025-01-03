namespace SuperLinq.Tests;

public sealed class StartsWithTest
{
	[Test]
	[Arguments(new[] { 1, 2, 3 }, new[] { 1, 2 }, true)]
	[Arguments(new[] { 1, 2, 3 }, new[] { 1, 2, 3 }, true)]
	[Arguments(new[] { 1, 2, 3 }, new[] { 1, 2, 3, 4 }, false)]
	public void StartsWithWithIntegers(IEnumerable<int> first, IEnumerable<int> second, bool expected)
	{
		using var f = first.AsTestingSequence();
		using var s = second.AsTestingSequence();

		Assert.Equal(expected, f.StartsWith(s));
	}

	[Test]
	[Arguments(new[] { '1', '2', '3' }, new[] { '1', '2' }, true)]
	[Arguments(new[] { '1', '2', '3' }, new[] { '1', '2', '3' }, true)]
	[Arguments(new[] { '1', '2', '3' }, new[] { '1', '2', '3', '4' }, false)]
	public void StartsWithWithChars(IEnumerable<char> first, IEnumerable<char> second, bool expected)
	{
		using var f = first.AsTestingSequence();
		using var s = second.AsTestingSequence();

		Assert.Equal(expected, f.StartsWith(s));
	}

	[Test]
	[Arguments("123", "12", true)]
	[Arguments("123", "123", true)]
	[Arguments("123", "1234", false)]
	public void StartsWithWithStrings(string first, string second, bool expected)
	{
		using var f = first.AsTestingSequence();
		using var s = second.AsTestingSequence();

		Assert.Equal(expected, f.StartsWith(s));
	}

	[Test]
	public void StartsWithReturnsTrueIfBothEmpty()
	{
		using var f = Array.Empty<int>().AsTestingSequence();
		using var s = Array.Empty<int>().AsTestingSequence();

		Assert.True(f.StartsWith(s));
	}

	[Test]
	public void StartsWithReturnsFalseIfOnlyFirstIsEmpty()
	{
		using var f = Array.Empty<int>().AsTestingSequence();
		using var s = TestingSequence.Of(1, 2, 3);

		Assert.False(f.StartsWith(s));
	}

	[Test]
	[Arguments("", "", true)]
	[Arguments("1", "", true)]
	public void StartsWithReturnsTrueIfSecondIsEmpty(string first, string second, bool expected)
	{
		using var f = first.AsTestingSequence();
		using var s = second.AsTestingSequence();

		Assert.Equal(expected, f.StartsWith(s));
	}

	[Test]
	public void StartsWithUsesSpecifiedEqualityComparerOrDefault()
	{
		var first = Seq(1, 2, 3);
		var second = Seq(4, 5, 6);

		Assert.False(first.StartsWith(second, comparer: null));
		Assert.False(first.StartsWith(second, EqualityComparer.Create<int>(delegate { return false; })));
		Assert.True(first.StartsWith(second, EqualityComparer.Create<int>(delegate { return true; })));
	}

	[Test]
	public void StartsWithUsesCollectionsCountToAvoidUnnecessaryIteration()
	{
		using var first = new BreakingCollection<int>(1, 2);
		using var second = new BreakingCollection<int>(1, 2, 3);

		Assert.False(first.StartsWith(second));
	}
}

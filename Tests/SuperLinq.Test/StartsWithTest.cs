using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace Test;

[TestFixture]
public class StartsWithTest
{
	[TestCase(new[] { 1, 2, 3 }, new[] { 1, 2 }, ExpectedResult = true)]
	[TestCase(new[] { 1, 2, 3 }, new[] { 1, 2, 3 }, ExpectedResult = true)]
	[TestCase(new[] { 1, 2, 3 }, new[] { 1, 2, 3, 4 }, ExpectedResult = false)]
	public bool StartsWithWithIntegers(IEnumerable<int> first, IEnumerable<int> second)
	{
		return first.StartsWith(second);
	}

	[TestCase(new[] { '1', '2', '3' }, new[] { '1', '2' }, ExpectedResult = true)]
	[TestCase(new[] { '1', '2', '3' }, new[] { '1', '2', '3' }, ExpectedResult = true)]
	[TestCase(new[] { '1', '2', '3' }, new[] { '1', '2', '3', '4' }, ExpectedResult = false)]
	public bool StartsWithWithChars(IEnumerable<char> first, IEnumerable<char> second)
	{
		return first.StartsWith(second);
	}

	[TestCase("123", "12", ExpectedResult = true)]
	[TestCase("123", "123", ExpectedResult = true)]
	[TestCase("123", "1234", ExpectedResult = false)]
	public bool StartsWithWithStrings(string first, string second)
	{
		// Conflict with String.StartsWith(), which has precedence in this case
		return SuperEnumerable.StartsWith(first, second);
	}

	[Test]
	public void StartsWithReturnsTrueIfBothEmpty()
	{
		Assert.True(Array.Empty<int>().StartsWith(Array.Empty<int>()));
	}

	[Test]
	public void StartsWithReturnsFalseIfOnlyFirstIsEmpty()
	{
		Assert.False(Array.Empty<int>().StartsWith(new[] { 1, 2, 3 }));
	}

	[TestCase("", "", ExpectedResult = true)]
	[TestCase("1", "", ExpectedResult = true)]
	public bool StartsWithReturnsTrueIfSecondIsEmpty(string first, string second)
	{
		// Conflict with String.StartsWith(), which has precedence in this case
		return SuperEnumerable.StartsWith(first, second);
	}

	[Test]
	public void StartsWithDisposesBothSequenceEnumerators()
	{
		using var first = TestingSequence.Of(1, 2, 3);
		using var second = TestingSequence.Of(1);

		first.StartsWith(second);
	}

	[Test]
	[SuppressMessage("ReSharper", "RedundantArgumentDefaultValue")]
	public void StartsWithUsesSpecifiedEqualityComparerOrDefault()
	{
		var first = new[] { 1, 2, 3 };
		var second = new[] { 4, 5, 6 };

		Assert.False(first.StartsWith(second));
		Assert.False(first.StartsWith(second, null));
		Assert.False(first.StartsWith(second, EqualityComparer.Create<int>(delegate { return false; })));
		Assert.True(first.StartsWith(second, EqualityComparer.Create<int>(delegate { return true; })));
	}

	[TestCase(SourceKind.BreakingCollection)]
	[TestCase(SourceKind.BreakingReadOnlyCollection)]
	public void StartsWithUsesCollectionsCountToAvoidUnnecessaryIteration(SourceKind sourceKind)
	{
		var first = new[] { 1, 2 }.ToSourceKind(sourceKind);
		var second = new[] { 1, 2, 3 }.ToSourceKind(sourceKind);

		Assert.False(first.StartsWith(second));
	}
}

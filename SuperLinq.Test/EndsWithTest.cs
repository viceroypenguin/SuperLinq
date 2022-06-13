using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace Test;

[TestFixture]
public class EndsWithTest
{
	[TestCase(new[] { 1, 2, 3 }, new[] { 2, 3 }, ExpectedResult = true)]
	[TestCase(new[] { 1, 2, 3 }, new[] { 1, 2, 3 }, ExpectedResult = true)]
	[TestCase(new[] { 1, 2, 3 }, new[] { 0, 1, 2, 3 }, ExpectedResult = false)]
	public bool EndsWithWithIntegers(IEnumerable<int> first, IEnumerable<int> second)
	{
		return first.EndsWith(second);
	}

	[TestCase(new[] { '1', '2', '3' }, new[] { '2', '3' }, ExpectedResult = true)]
	[TestCase(new[] { '1', '2', '3' }, new[] { '1', '2', '3' }, ExpectedResult = true)]
	[TestCase(new[] { '1', '2', '3' }, new[] { '0', '1', '2', '3' }, ExpectedResult = false)]
	public bool EndsWithWithChars(IEnumerable<char> first, IEnumerable<char> second)
	{
		return first.EndsWith(second);
	}

	[TestCase("123", "23", ExpectedResult = true)]
	[TestCase("123", "123", ExpectedResult = true)]
	[TestCase("123", "0123", ExpectedResult = false)]
	public bool EndsWithWithStrings(string first, string second)
	{
		// Conflict with String.EndsWith(), which has precedence in this case
		return SuperEnumerable.EndsWith(first, second);
	}

	[Test]
	public void EndsWithReturnsTrueIfBothEmpty()
	{
		Assert.True(Array.Empty<int>().EndsWith(Array.Empty<int>()));
	}

	[Test]
	public void EndsWithReturnsFalseIfOnlyFirstIsEmpty()
	{
		Assert.False(Array.Empty<int>().EndsWith(new[] { 1, 2, 3 }));
	}

	[TestCase("", "", ExpectedResult = true)]
	[TestCase("1", "", ExpectedResult = true)]
	public bool EndsWithReturnsTrueIfSecondIsEmpty(string first, string second)
	{
		// Conflict with String.EndsWith(), which has precedence in this case
		return SuperEnumerable.EndsWith(first, second);
	}

	[Test]
	public void EndsWithDisposesBothSequenceEnumerators()
	{
		using var first = TestingSequence.Of(1, 2, 3);
		using var second = TestingSequence.Of(1);

		first.EndsWith(second);
	}

	[Test]
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

	[TestCase(SourceKind.BreakingCollection)]
	[TestCase(SourceKind.BreakingReadOnlyCollection)]
	public void EndsWithUsesCollectionsCountToAvoidUnnecessaryIteration(SourceKind sourceKind)
	{
		var first = new[] { 1, 2 }.ToSourceKind(sourceKind);
		var second = new[] { 1, 2, 3 }.ToSourceKind(sourceKind);

		Assert.False(first.EndsWith(second));
	}
}

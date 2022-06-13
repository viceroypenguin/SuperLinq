using NUnit.Framework;

namespace SuperLinq.Test;

[TestFixture]
public class CountByTest
{
	[Test]
	public void CountBySimpleTest()
	{
		var result = new[] { 1, 2, 3, 4, 5, 6, 1, 2, 3, 1, 1, 2 }.CountBy(c => c);

		result.AssertSequenceEqual(
			(1, 4),
			(2, 3),
			(3, 2),
			(4, 1),
			(5, 1),
			(6, 1));
	}

	[Test]
	public void CountByWithSecondOccurenceImmediatelyAfterFirst()
	{
		var result = "jaffer".CountBy(c => c);

		result.AssertSequenceEqual(
			('j', 1),
			('a', 1),
			('f', 2),
			('e', 1),
			('r', 1));
	}

	[Test]
	public void CountByEvenOddTest()
	{
		var result = Enumerable.Range(1, 100).CountBy(c => c % 2);

		result.AssertSequenceEqual(
			(1, 50),
			(0, 50));
	}

	[Test]
	public void CountByWithEqualityComparer()
	{
		var result = new[] { "a", "B", "c", "A", "b", "A" }.CountBy(c => c, StringComparer.OrdinalIgnoreCase);

		result.AssertSequenceEqual(
			("a", 3),
			("B", 2),
			("c", 1));
	}

	[Test]
	public void CountByHasKeysOrderedLikeGroupBy()
	{
		var randomSequence = SuperEnumerable.Random(0, 100).Take(100).ToArray();

		var countByKeys = randomSequence.CountBy(x => x).Select(x => x.key);
		var groupByKeys = randomSequence.GroupBy(x => x).Select(x => x.Key);

		countByKeys.AssertSequenceEqual(groupByKeys);
	}

	[Test]
	public void CountByIsLazy()
	{
		new BreakingSequence<string>().CountBy(BreakingFunc.Of<string, int>());
	}

	[Test]
	public void CountByWithSomeNullKeys()
	{
		var ss = new[]
		{
			"foo", null, "bar", "baz", null, null, "baz", "bar", null, "foo"
		};
		var result = ss.CountBy(s => s);

		result.AssertSequenceEqual(
			("foo", 2),
			(default, 4),
			("bar", 2),
			("baz", 2));
	}
}

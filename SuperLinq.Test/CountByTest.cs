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
			KeyValuePair.Create(1, 4),
			KeyValuePair.Create(2, 3),
			KeyValuePair.Create(3, 2),
			KeyValuePair.Create(4, 1),
			KeyValuePair.Create(5, 1),
			KeyValuePair.Create(6, 1));
	}

	[Test]
	public void CountByWithSecondOccurenceImmediatelyAfterFirst()
	{
		var result = "jaffer".CountBy(c => c);

		result.AssertSequenceEqual(
			KeyValuePair.Create('j', 1),
			KeyValuePair.Create('a', 1),
			KeyValuePair.Create('f', 2),
			KeyValuePair.Create('e', 1),
			KeyValuePair.Create('r', 1));
	}

	[Test]
	public void CountByEvenOddTest()
	{
		var result = Enumerable.Range(1, 100).CountBy(c => c % 2);

		result.AssertSequenceEqual(
			KeyValuePair.Create(1, 50),
			KeyValuePair.Create(0, 50));
	}

	[Test]
	public void CountByWithEqualityComparer()
	{
		var result = new[] { "a", "B", "c", "A", "b", "A" }.CountBy(c => c, StringComparer.OrdinalIgnoreCase);

		result.AssertSequenceEqual(
			KeyValuePair.Create("a", 3),
			KeyValuePair.Create("B", 2),
			KeyValuePair.Create("c", 1));
	}

	[Test]
	public void CountByHasKeysOrderedLikeGroupBy()
	{
		var randomSequence = SuperEnumerable.Random(0, 100).Take(100).ToArray();

		var countByKeys = randomSequence.CountBy(x => x).Select(x => x.Key);
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
			KeyValuePair.Create("foo", 2),
			KeyValuePair.Create((string)null, 4),
			KeyValuePair.Create("bar", 2),
			KeyValuePair.Create("baz", 2));
	}
}

using NUnit.Framework;

namespace SuperLinq.Test;

[TestFixture]
public class PartialSortByTests
{
	[Test]
	public void PartialSortBy()
	{
		var ns = SuperEnumerable.RandomDouble().Take(10).ToArray();

		const int count = 5;
		var sorted = ns.Select((n, i) => KeyValuePair.Create(i, n))
					   .Reverse()
					   .PartialSortBy(count, e => e.Key);

		sorted.Select(e => e.Value).AssertSequenceEqual(ns.Take(count));
	}

	[Test]
	public void PartialSortWithOrder()
	{
		var ns = SuperEnumerable.RandomDouble().Take(10).ToArray();

		const int count = 5;
		var sorted = ns.Select((n, i) => KeyValuePair.Create(i, n))
						.Reverse()
						.PartialSortBy(count, e => e.Key, OrderByDirection.Ascending);

		sorted.Select(e => e.Value).AssertSequenceEqual(ns.Take(count));

		sorted = ns.Select((n, i) => KeyValuePair.Create(i, n))
					.Reverse()
					.PartialSortBy(count, e => e.Key, OrderByDirection.Descending);

		sorted.Select(e => e.Value).AssertSequenceEqual(ns.Reverse().Take(count));
	}

	[Test]
	public void PartialSortWithComparer()
	{
		var alphabet = Enumerable.Range(0, 26)
								 .Select((n, i) => ((char)((i % 2 == 0 ? 'A' : 'a') + n)).ToString())
								 .ToArray();

		var ns = alphabet.Zip(SuperEnumerable.RandomDouble(), KeyValuePair.Create).ToArray();
		var sorted = ns.PartialSortBy(5, e => e.Key, StringComparer.Ordinal);

		sorted.Select(e => e.Key[0]).AssertSequenceEqual('A', 'C', 'E', 'G', 'I');
	}

	[Test]
	public void PartialSortByIsLazy()
	{
		new BreakingSequence<object>().PartialSortBy(1, BreakingFunc.Of<object, object>());
	}

	[Test, Ignore("TODO")]
	public void PartialSortByIsStable()
	{
		// Force creation of same strings to avoid reference equality at
		// start via interned literals.

		var foobar = "foobar".ToCharArray();
		var foobars = Enumerable.Repeat(foobar, 10)
								.Select(chars => new string(chars))
								.ToArray();

		var sorted = foobars.PartialSort(5);

		// Pair expected and actuals by index and then check
		// reference equality, finding the first mismatch.

		var mismatchIndex =
			foobars.Index()
				   .Zip(sorted, (expected, actual) => new
				   {
					   Index = expected.Key,
					   Pass = ReferenceEquals(expected.Value, actual)
				   })
				   .FirstOrDefault(e => !e.Pass)?.Index;

		Assert.That(mismatchIndex, Is.Null, "Mismatch index");
	}
}

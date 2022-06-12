using NUnit.Framework;

namespace SuperLinq.Test;

[TestFixture]
public class ScanByTest
{
	[Test]
	public void ScanByIsLazy()
	{
		new BreakingSequence<string>().ScanBy(
			BreakingFunc.Of<string, int>(),
			BreakingFunc.Of<int, char>(),
			BreakingFunc.Of<char, int, string, char>());
	}

	[Test]
	public void ScanBy()
	{
		var source = new[]
		{
				"ana",
				"beatriz",
				"carla",
				"bob",
				"davi",
				"adriano",
				"angelo",
				"carlos"
			};

		var result =
				source.ScanBy(
					item => item.First(),
					key => (Element: default(string), Key: key, State: key - 1),
					(state, key, item) => (item, char.ToUpperInvariant(key), state.State + 1));

		result.AssertSequenceEqual(
		   KeyValuePair.Create('a', ("ana", 'A', 97)),
		   KeyValuePair.Create('b', ("beatriz", 'B', 98)),
		   KeyValuePair.Create('c', ("carla", 'C', 99)),
		   KeyValuePair.Create('b', ("bob", 'B', 99)),
		   KeyValuePair.Create('d', ("davi", 'D', 100)),
		   KeyValuePair.Create('a', ("adriano", 'A', 98)),
		   KeyValuePair.Create('a', ("angelo", 'A', 99)),
		   KeyValuePair.Create('c', ("carlos", 'C', 100)));
	}

	[Test]
	public void ScanByWithSecondOccurenceImmediatelyAfterFirst()
	{
		var result = "jaffer".ScanBy(c => c, k => -1, (i, k, e) => i + 1);

		result.AssertSequenceEqual(
			KeyValuePair.Create('j', 0),
			KeyValuePair.Create('a', 0),
			KeyValuePair.Create('f', 0),
			KeyValuePair.Create('f', 1),
			KeyValuePair.Create('e', 0),
			KeyValuePair.Create('r', 0));
	}

	[Test]
	public void ScanByWithEqualityComparer()
	{
		var source = new[] { "a", "B", "c", "A", "b", "A" };
		var result = source.ScanBy(c => c,
								   k => -1,
								   (i, k, e) => i + 1,
								   StringComparer.OrdinalIgnoreCase);

		result.AssertSequenceEqual(
		   KeyValuePair.Create("a", 0),
		   KeyValuePair.Create("B", 0),
		   KeyValuePair.Create("c", 0),
		   KeyValuePair.Create("A", 1),
		   KeyValuePair.Create("b", 1),
		   KeyValuePair.Create("A", 2));
	}

	[Test]
	public void ScanByWithSomeNullKeys()
	{
		var source = new[] { "foo", null, "bar", "baz", null, null, "baz", "bar", null, "foo" };
		var result = source.ScanBy(c => c, k => -1, (i, k, e) => i + 1);

		result.AssertSequenceEqual(
			KeyValuePair.Create("foo", 0),
			KeyValuePair.Create((string)null, 0),
			KeyValuePair.Create("bar", 0),
			KeyValuePair.Create("baz", 0),
			KeyValuePair.Create((string)null, 1),
			KeyValuePair.Create((string)null, 2),
			KeyValuePair.Create("baz", 1),
			KeyValuePair.Create("bar", 1),
			KeyValuePair.Create((string)null, 3),
			KeyValuePair.Create("foo", 1));
	}

	[Test]
	public void ScanByWithNullSeed()
	{
		var nil = (object)null;
		var source = new[] { "foo", null, "bar", null, "baz" };
		var result = source.ScanBy(c => c, k => nil, (i, k, e) => nil);

		result.AssertSequenceEqual(
			KeyValuePair.Create("foo", nil),
			KeyValuePair.Create((string)null, nil),
			KeyValuePair.Create("bar", nil),
			KeyValuePair.Create((string)null, nil),
			KeyValuePair.Create("baz", nil));
	}

	[Test]
	public void ScanByDoesNotIterateUnnecessaryElements()
	{
		var source = SuperEnumerable.From(() => "ana",
										 () => "beatriz",
										 () => "carla",
										 () => "bob",
										 () => "davi",
										 () => throw new TestException(),
										 () => "angelo",
										 () => "carlos");

		var result = source.ScanBy(c => c.First(), k => -1, (i, k, e) => i + 1);

		result.Take(5).AssertSequenceEqual(
			KeyValuePair.Create('a', 0),
			KeyValuePair.Create('b', 0),
			KeyValuePair.Create('c', 0),
			KeyValuePair.Create('b', 1),
			KeyValuePair.Create('d', 0));

		Assert.Throws<TestException>(() =>
			result.ElementAt(5));
	}
}

namespace Test.Async;

public class ScanByTest
{
	[Fact]
	public void ScanByIsLazy()
	{
		new AsyncBreakingSequence<string>().ScanBy(
			BreakingFunc.Of<string, int>(),
			BreakingFunc.Of<int, char>(),
			BreakingFunc.Of<char, int, string, char>());
	}

	[Fact]
	public Task ScanBy()
	{
		var source = AsyncSeq(
			"ana",
			"beatriz",
			"carla",
			"bob",
			"davi",
			"adriano",
			"angelo",
			"carlos");

		var result =
			source.ScanBy(
				item => item.First(),
				key => (Element: default(string), Key: key, State: key - 1),
				(state, key, item) => (item, char.ToUpperInvariant(key), state.State + 1));

		return result.AssertSequenceEqual(
			('a', ("ana", 'A', 97)),
			('b', ("beatriz", 'B', 98)),
			('c', ("carla", 'C', 99)),
			('b', ("bob", 'B', 99)),
			('d', ("davi", 'D', 100)),
			('a', ("adriano", 'A', 98)),
			('a', ("angelo", 'A', 99)),
			('c', ("carlos", 'C', 100)));
	}

	[Fact]
	public Task ScanByWithSecondOccurenceImmediatelyAfterFirst()
	{
		var result = "jaffer".ToAsyncEnumerable().ScanBy(SuperEnumerable.Identity, k => -1, (i, k, e) => i + 1);

		return result.AssertSequenceEqual(('j', 0), ('a', 0), ('f', 0), ('f', 1), ('e', 0), ('r', 0));
	}

	[Fact]
	public Task ScanByWithEqualityComparer()
	{
		var source = AsyncSeq("a", "B", "c", "A", "b", "A");
		var result = source.ScanBy(
			SuperEnumerable.Identity,
			k => -1,
			(i, k, e) => i + 1,
			StringComparer.OrdinalIgnoreCase);

		return result.AssertSequenceEqual(("a", 0), ("B", 0), ("c", 0), ("A", 1), ("b", 1), ("A", 2));
	}

	[Fact]
	public Task ScanByWithSomeNullKeys()
	{
		var source = AsyncSeq("foo", null, "bar", "baz", null, null, "baz", "bar", null, "foo");
		var result = source.ScanBy(SuperEnumerable.Identity, k => -1, (i, k, e) => i + 1);

		return result.AssertSequenceEqual(("foo", 0), (null, 0), ("bar", 0), ("baz", 0), (null, 1), (null, 2), ("baz", 1), ("bar", 1), (null, 3), ("foo", 1));
	}

	[Fact]
	public Task ScanByWithNullSeed()
	{
		var nil = (object?)null;
		var source = AsyncSeq("foo", null, "bar", null, "baz");
		var result = source.ScanBy(SuperEnumerable.Identity, k => nil, (i, k, e) => nil);

		return result.AssertSequenceEqual(("foo", nil), (null, nil), ("bar", nil), (null, nil), ("baz", nil));
	}

	[Fact]
	public async Task ScanByDoesNotIterateUnnecessaryElements()
	{
		var source = AsyncSuperEnumerable.From(
			() => Task.FromResult("ana"),
			() => Task.FromResult("beatriz"),
			() => Task.FromResult("carla"),
			() => Task.FromResult("bob"),
			() => Task.FromResult("davi"),
			AsyncBreakingFunc.Of<string>(),
			() => Task.FromResult("angelo"),
			() => Task.FromResult("carlos"));

		var result = source.ScanBy(c => c.First(), k => -1, (i, k, e) => i + 1);

		await result.Take(5).AssertSequenceEqual(
			('a', 0),
			('b', 0),
			('c', 0),
			('b', 1),
			('d', 0));

		await Assert.ThrowsAsync<TestException>(async () =>
			await result.ElementAtAsync(5));
	}
}

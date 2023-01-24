namespace Test.Async;

public class IndexByTest
{
	[Fact]
	public async Task IndexBySimpleTest()
	{
		using var source = TestingSequence.Of("ana", "beatriz", "carla", "bob", "davi", "adriano", "angelo", "carlos");

		var result = source.IndexBy(x => x.First());
		await result.AssertSequenceEqual(
			(0, "ana"),
			(0, "beatriz"),
			(0, "carla"),
			(1, "bob"),
			(0, "davi"),
			(1, "adriano"),
			(2, "angelo"),
			(1, "carlos"));
	}

	[Fact]
	public async Task IndexByWithSecondOccurenceImmediatelyAfterFirst()
	{
		using var source = "jaffer".AsTestingSequence();

		var result = source.IndexBy(SuperEnumerable.Identity);
		await result.AssertSequenceEqual((0, 'j'), (0, 'a'), (0, 'f'), (1, 'f'), (0, 'e'), (0, 'r'));
	}

	[Fact]
	public async Task IndexByWithEqualityComparer()
	{
		using var source = TestingSequence.Of("a", "B", "c", "A", "b", "A");

		var result = source.IndexBy(SuperEnumerable.Identity, StringComparer.OrdinalIgnoreCase);
		await result.AssertSequenceEqual((0, "a"), (0, "B"), (0, "c"), (1, "A"), (1, "b"), (2, "A"));
	}

	[Fact]
	public void IndexByIsLazy()
	{
		new AsyncBreakingSequence<string>().IndexBy(BreakingFunc.Of<string, char>());
	}

	[Fact]
	public async Task IndexByWithSomeNullKeys()
	{
		using var source = TestingSequence.Of("foo", null, "bar", "baz", null, null, "baz", "bar", null, "foo");

		var result = source.IndexBy(SuperEnumerable.Identity);
		await result.AssertSequenceEqual((0, "foo"), (0, null), (0, "bar"), (0, "baz"), (1, null), (2, null), (1, "baz"), (1, "bar"), (3, null), (1, "foo"));
	}

	[Fact]
	public async Task IndexByDoesNotIterateUnnecessaryElements()
	{
		using var source = AsyncSuperEnumerable
			.From(
				() => Task.FromResult("ana"),
				() => Task.FromResult("beatriz"),
				() => Task.FromResult("carla"),
				() => Task.FromResult("bob"),
				() => Task.FromResult("davi"),
				AsyncBreakingFunc.Of<string>(),
				() => Task.FromResult("angelo"),
				() => Task.FromResult("carlos"))
			.AsTestingSequence(2);

		var result = source.IndexBy(x => x.First());

		await result.Take(5).AssertSequenceEqual(
			(0, "ana"),
			(0, "beatriz"),
			(0, "carla"),
			(1, "bob"),
			(0, "davi"));

		await Assert.ThrowsAsync<TestException>(async () =>
			await result.ElementAtAsync(5));
	}
}

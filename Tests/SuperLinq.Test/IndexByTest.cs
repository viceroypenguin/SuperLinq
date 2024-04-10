namespace Test;

public sealed class IndexByTest
{
	[Fact]
	public void IndexBySimpleTest()
	{
		using var source = TestingSequence.Of("ana", "beatriz", "carla", "bob", "davi", "adriano", "angelo", "carlos");

		var result = source.IndexBy(x => x.First());
		result.AssertSequenceEqual(
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
	public void IndexByWithSecondOccurenceImmediatelyAfterFirst()
	{
		using var source = "jaffer".AsTestingSequence();

		var result = source.IndexBy(SuperEnumerable.Identity);
		result.AssertSequenceEqual((0, 'j'), (0, 'a'), (0, 'f'), (1, 'f'), (0, 'e'), (0, 'r'));
	}

	[Fact]
	public void IndexByWithEqualityComparer()
	{
		using var source = TestingSequence.Of("a", "B", "c", "A", "b", "A");

		var result = source.IndexBy(SuperEnumerable.Identity, StringComparer.OrdinalIgnoreCase);
		result.AssertSequenceEqual((0, "a"), (0, "B"), (0, "c"), (1, "A"), (1, "b"), (2, "A"));
	}

	[Fact]
	public void IndexByIsLazy()
	{
		_ = new BreakingSequence<string>().IndexBy(BreakingFunc.Of<string, char>());
	}

	[Fact]
	public void IndexByWithSomeNullKeys()
	{
		using var source = TestingSequence.Of("foo", null, "bar", "baz", null, null, "baz", "bar", null, "foo");

		var result = source.IndexBy(SuperEnumerable.Identity);
		result.AssertSequenceEqual((0, "foo"), (0, null), (0, "bar"), (0, "baz"), (1, null), (2, null), (1, "baz"), (1, "bar"), (3, null), (1, "foo"));
	}

	[Fact]
	public void IndexBytDoesNotIterateUnnecessaryElements()
	{
		using var source = SuperEnumerable
			.From(
				() => "ana",
				() => "beatriz",
				() => "carla",
				() => "bob",
				() => "davi",
				() => throw new TestException(),
				() => "angelo",
				() => "carlos")
			.AsTestingSequence(maxEnumerations: 2);

		var result = source.IndexBy(x => x.First());

		result.Take(5)
			.AssertSequenceEqual(
				(0, "ana"),
				(0, "beatriz"),
				(0, "carla"),
				(1, "bob"),
				(0, "davi"));

		_ = Assert.Throws<TestException>(() =>
			result.ElementAt(5));
	}
}

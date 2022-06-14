using NUnit.Framework;

namespace Test;

[TestFixture]
public class IndexByTest
{
	[Test]
	public void IndexBySimpleTest()
	{
		var source = new[] { "ana", "beatriz", "carla", "bob", "davi", "adriano", "angelo", "carlos" };
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

	[Test]
	public void IndexByWithSecondOccurenceImmediatelyAfterFirst()
	{
		var result = "jaffer".IndexBy(c => c);

		result.AssertSequenceEqual((0, 'j'), (0, 'a'), (0, 'f'), (1, 'f'), (0, 'e'), (0, 'r'));
	}

	[Test]
	public void IndexByWithEqualityComparer()
	{
		var source = new[] { "a", "B", "c", "A", "b", "A" };
		var result = source.IndexBy(c => c, StringComparer.OrdinalIgnoreCase);

		result.AssertSequenceEqual((0, "a"), (0, "B"), (0, "c"), (1, "A"), (1, "b"), (2, "A"));
	}

	[Test]
	public void IndexByIsLazy()
	{
		new BreakingSequence<string>().IndexBy(BreakingFunc.Of<string, char>());
	}

	[Test]
	public void IndexByWithSomeNullKeys()
	{
		var source = new[] { "foo", null, "bar", "baz", null, null, "baz", "bar", null, "foo" };
		var result = source.IndexBy(c => c);

		const string @null = null; // type inference happiness
		result.AssertSequenceEqual((0, "foo"), (0, @null), (0, "bar"), (0, "baz"), (1, @null), (2, @null), (1, "baz"), (1, "bar"), (3, @null), (1, "foo"));
	}

	[Test]
	public void IndexBytDoesNotIterateUnnecessaryElements()
	{
		var source = SuperEnumerable.From(() => "ana",
										 () => "beatriz",
										 () => "carla",
										 () => "bob",
										 () => "davi",
										 () => throw new TestException(),
										 () => "angelo",
										 () => "carlos");

		var result = source.IndexBy(x => x.First());

		result.Take(5).AssertSequenceEqual(
			(0, "ana"),
			(0, "beatriz"),
			(0, "carla"),
			(1, "bob"),
			(0, "davi"));

		Assert.Throws<TestException>(() =>
			result.ElementAt(5));
	}
}

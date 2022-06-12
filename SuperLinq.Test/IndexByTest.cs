using NUnit.Framework;

namespace SuperLinq.Test;

[TestFixture]
public class IndexByTest
{
	[Test]
	public void IndexBySimpleTest()
	{
		var source = new[] { "ana", "beatriz", "carla", "bob", "davi", "adriano", "angelo", "carlos" };
		var result = source.IndexBy(x => x.First());

		result.AssertSequenceEqual(
			KeyValuePair.Create(0, "ana"),
			KeyValuePair.Create(0, "beatriz"),
			KeyValuePair.Create(0, "carla"),
			KeyValuePair.Create(1, "bob"),
			KeyValuePair.Create(0, "davi"),
			KeyValuePair.Create(1, "adriano"),
			KeyValuePair.Create(2, "angelo"),
			KeyValuePair.Create(1, "carlos"));
	}

	[Test]
	public void IndexByWithSecondOccurenceImmediatelyAfterFirst()
	{
		var result = "jaffer".IndexBy(c => c);

		result.AssertSequenceEqual(
			KeyValuePair.Create(0, 'j'),
			KeyValuePair.Create(0, 'a'),
			KeyValuePair.Create(0, 'f'),
			KeyValuePair.Create(1, 'f'),
			KeyValuePair.Create(0, 'e'),
			KeyValuePair.Create(0, 'r'));
	}

	[Test]
	public void IndexByWithEqualityComparer()
	{
		var source = new[] { "a", "B", "c", "A", "b", "A" };
		var result = source.IndexBy(c => c, StringComparer.OrdinalIgnoreCase);

		result.AssertSequenceEqual(
		   KeyValuePair.Create(0, "a"),
		   KeyValuePair.Create(0, "B"),
		   KeyValuePair.Create(0, "c"),
		   KeyValuePair.Create(1, "A"),
		   KeyValuePair.Create(1, "b"),
		   KeyValuePair.Create(2, "A"));
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
		result.AssertSequenceEqual(
			KeyValuePair.Create(0, "foo"),
			KeyValuePair.Create(0, @null),
			KeyValuePair.Create(0, "bar"),
			KeyValuePair.Create(0, "baz"),
			KeyValuePair.Create(1, @null),
			KeyValuePair.Create(2, @null),
			KeyValuePair.Create(1, "baz"),
			KeyValuePair.Create(1, "bar"),
			KeyValuePair.Create(3, @null),
			KeyValuePair.Create(1, "foo"));
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
			KeyValuePair.Create(0, "ana"),
			KeyValuePair.Create(0, "beatriz"),
			KeyValuePair.Create(0, "carla"),
			KeyValuePair.Create(1, "bob"),
			KeyValuePair.Create(0, "davi"));

		Assert.Throws<TestException>(() =>
			result.ElementAt(5));
	}
}

namespace SuperLinq.Tests;

public sealed class DuplicatesTest
{
	[Test]
	public void DuplicatesIsLazy()
	{
		_ = new BreakingSequence<int>().Duplicates();
	}

	[Test]
	[Arguments(new int[] { 1, 2, 3 }, new int[] { })]
	[Arguments(new int[] { 1, 2, 1, 3, 1, 2, 1 }, new int[] { 1, 2 })]
	[Arguments(new int[] { 3, 3, 2, 2, 1, 1 }, new int[] { 3, 2, 1 })]
	public void DuplicatesBehavior(IEnumerable<int> source, IEnumerable<int> expected)
	{
		using var ts = source.AsTestingSequence();

		var result = ts.Duplicates();
		result.AssertSequenceEqual(expected);
	}

	public static IEnumerable<(IEnumerable<string> source, StringComparer comparer, IEnumerable<string> expected)> GetStringParameters() =>
		[
			(["foo", "bar", "qux"], StringComparer.Ordinal, []),
			(["foo", "FOO", "bar", "qux"], StringComparer.Ordinal, []),
			(["foo", "FOO", "bar", "qux"], StringComparer.OrdinalIgnoreCase, ["FOO"]),
			(["Bar", "foo", "FOO", "bar", "qux"], StringComparer.OrdinalIgnoreCase, ["FOO", "bar"]),
		];

	[Test]
	[MethodDataSource(nameof(GetStringParameters))]
	public void DuplicatesComparerBehavior(IEnumerable<string> source, StringComparer comparer, IEnumerable<string> expected)
	{
		using var ts = source.AsTestingSequence();

		var result = ts.Duplicates(comparer);
		result.AssertSequenceEqual(expected);
	}
}

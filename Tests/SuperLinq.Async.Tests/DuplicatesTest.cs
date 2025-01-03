namespace SuperLinq.Async.Tests;

public sealed class DuplicatesTest
{
	[Test]
	public void DuplicatesIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().Duplicates();
	}

	[Test]
	[Arguments(new int[] { 1, 2, 3 }, new int[] { })]
	[Arguments(new int[] { 1, 2, 1, 3, 1, 2, 1 }, new int[] { 1, 2 })]
	[Arguments(new int[] { 3, 3, 2, 2, 1, 1 }, new int[] { 3, 2, 1 })]
	public async Task DuplicatesBehavior(IEnumerable<int> source, IEnumerable<int> expected)
	{
		await using var ts = source.AsTestingSequence();

		var result = ts.Duplicates();
		await result.AssertSequenceEqual(expected);
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
	public async Task DuplicatesComparerBehavior(IEnumerable<string> source, StringComparer comparer, IEnumerable<string> expected)
	{
		await using var ts = source.AsTestingSequence();

		var result = ts.Duplicates(comparer);
		await result.AssertSequenceEqual(expected);
	}
}

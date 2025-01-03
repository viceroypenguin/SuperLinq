namespace SuperLinq.Async.Tests;

public sealed class HasDuplicatesTest
{
	[Test]
	public async Task DuplicatesDoesNotEnumerateUnnecessarily()
	{
		using var ts = AsyncSeq(1, 2, 3).Concat(AsyncSeqExceptionAt(2)).AsTestingSequence();

		var result = await ts.HasDuplicates();
		Assert.True(result);
	}

	[Test]
	[Arguments(new int[] { 1, 2, 3 }, false)]
	[Arguments(new int[] { 1, 2, 1, 3, 1, 2, 1 }, true)]
	[Arguments(new int[] { 3, 3, 2, 2, 1, 1 }, true)]
	public async Task DuplicatesBehavior(IEnumerable<int> source, bool expected)
	{
		await using var ts = source.AsTestingSequence();

		var result = await ts.HasDuplicates();
		Assert.Equal(expected, result);
	}

	public static IEnumerable<(IEnumerable<string> source, StringComparer comparer, bool expected)> GetStringParameters() =>
		[
			(["foo", "bar", "qux"], StringComparer.Ordinal, false),
			(["foo", "FOO", "bar", "qux"], StringComparer.Ordinal, false),
			(["foo", "FOO", "bar", "qux"], StringComparer.OrdinalIgnoreCase, true),
			(["Bar", "foo", "FOO", "bar", "qux"], StringComparer.OrdinalIgnoreCase, true),
		];

	[Test]
	[MethodDataSource(nameof(GetStringParameters))]
	public async Task DuplicatesComparerBehavior(IEnumerable<string> source, StringComparer comparer, bool expected)
	{
		await using var ts = source.AsTestingSequence();

		var result = await ts.HasDuplicates(comparer);
		Assert.Equal(expected, result);
	}
}

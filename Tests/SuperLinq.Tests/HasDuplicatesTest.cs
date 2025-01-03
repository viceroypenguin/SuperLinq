namespace SuperLinq.Tests;

public sealed class HasDuplicatesTest
{
	[Test]
	public void DuplicatesDoesNotEnumerateUnnecessarily()
	{
		using var ts = Seq(1, 2, 3).Concat(SeqExceptionAt(2)).AsTestingSequence();

		var result = ts.HasDuplicates();
		Assert.True(result);
	}

	[Test]
	[Arguments(new int[] { 1, 2, 3 }, false)]
	[Arguments(new int[] { 1, 2, 1, 3, 1, 2, 1 }, true)]
	[Arguments(new int[] { 3, 3, 2, 2, 1, 1 }, true)]
	public void DuplicatesBehavior(IEnumerable<int> source, bool expected)
	{
		using var ts = source.AsTestingSequence();

		var result = ts.HasDuplicates();
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
	public void DuplicatesComparerBehavior(IEnumerable<string> source, StringComparer comparer, bool expected)
	{
		using var ts = source.AsTestingSequence();

		var result = ts.HasDuplicates(comparer);
		Assert.Equal(expected, result);
	}
}

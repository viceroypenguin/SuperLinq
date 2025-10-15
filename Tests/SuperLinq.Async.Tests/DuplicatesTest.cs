namespace SuperLinq.Async.Tests;

public sealed class DuplicatesTest
{
	[Fact]
	public void DuplicatesIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().Duplicates();
	}

	[Theory]
	[InlineData(new int[] { 1, 2, 3 }, new int[] { })]
	[InlineData(new int[] { 1, 2, 1, 3, 1, 2, 1 }, new int[] { 1, 2 })]
	[InlineData(new int[] { 3, 3, 2, 2, 1, 1 }, new int[] { 3, 2, 1 })]
	public async Task DuplicatesBehavior(IEnumerable<int> source, IEnumerable<int> expected)
	{
		await using var ts = source.AsTestingSequence();

		var result = ts.Duplicates();
		await result.AssertSequenceEqual(expected);
	}

	public static IEnumerable<object[]> GetStringParameters()
	{
		yield return new object[]
		{
			new string[] { "foo", "bar", "qux" },
			StringComparer.Ordinal,
			Array.Empty<string>(),
		};
		yield return new object[]
		{
			new string[] { "foo", "FOO", "bar", "qux" },
			StringComparer.Ordinal,
			Array.Empty<string>(),
		};
		yield return new object[]
		{
			new string[] { "foo", "FOO", "bar", "qux" },
			StringComparer.OrdinalIgnoreCase,
			new string[] { "FOO" },
		};
		yield return new object[]
		{
			new string[] { "Bar", "foo", "FOO", "bar", "qux" },
			StringComparer.OrdinalIgnoreCase,
			new string[] { "FOO", "bar" },
		};
	}

	[Theory]
	[MemberData(nameof(GetStringParameters))]
	public async Task DuplicatesComparerBehavior(IEnumerable<string> source, StringComparer comparer, IEnumerable<string> expected)
	{
		await using var ts = source.AsTestingSequence();

		var result = ts.Duplicates(comparer);
		await result.AssertSequenceEqual(expected);
	}
}

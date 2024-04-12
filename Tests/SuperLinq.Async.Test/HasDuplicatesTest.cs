namespace Test.Async;

public sealed class HasDuplicatesTest
{
	[Fact]
	public async Task DuplicatesDoesNotEnumerateUnnecessarily()
	{
		using var ts = AsyncSeq(1, 2, 3).Concat(AsyncSeqExceptionAt(2)).AsTestingSequence();

		var result = await ts.HasDuplicates();
		Assert.True(result);
	}

	[Theory]
	[InlineData(new int[] { 1, 2, 3 }, false)]
	[InlineData(new int[] { 1, 2, 1, 3, 1, 2, 1 }, true)]
	[InlineData(new int[] { 3, 3, 2, 2, 1, 1 }, true)]
	public async Task DuplicatesBehavior(IEnumerable<int> source, bool expected)
	{
		await using var ts = source.AsTestingSequence();

		var result = await ts.HasDuplicates();
		Assert.Equal(expected, result);
	}

	public static IEnumerable<object[]> GetStringParameters()
	{
		yield return new object[]
		{
			new string[] { "foo", "bar", "qux" },
			StringComparer.Ordinal,
			false,
		};
		yield return new object[]
		{
			new string[] { "foo", "FOO", "bar", "qux" },
			StringComparer.Ordinal,
			false,
		};
		yield return new object[]
		{
			new string[] { "foo", "FOO", "bar", "qux" },
			StringComparer.OrdinalIgnoreCase,
			true,
		};
		yield return new object[]
		{
			new string[] { "Bar", "foo", "FOO", "bar", "qux" },
			StringComparer.OrdinalIgnoreCase,
			true,
		};
	}

	[Theory]
	[MemberData(nameof(GetStringParameters))]
	public async Task DuplicatesComparerBehavior(IEnumerable<string> source, StringComparer comparer, bool expected)
	{
		await using var ts = source.AsTestingSequence();

		var result = await ts.HasDuplicates(comparer);
		Assert.Equal(expected, result);
	}
}

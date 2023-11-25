namespace Test;

public class DuplicatesTest
{
	[Fact]
	public void DuplicatesIsLazy()
	{
		_ = new BreakingSequence<int>().Duplicates();
	}

	[Theory]
	[InlineData(new int[] { 1, 2, 3, }, new int[] { })]
	[InlineData(new int[] { 1, 2, 1, 3, 1, 2, 1, }, new int[] { 1, 2, })]
	[InlineData(new int[] { 3, 3, 2, 2, 1, 1, }, new int[] { 3, 2, 1, })]
	public void DuplicatesBehavior(IEnumerable<int> source, IEnumerable<int> expected)
	{
		using var ts = source.AsTestingSequence();

		var result = ts.Duplicates();
		result.AssertSequenceEqual(expected);
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
			new string[] { "FOO", },
		};
		yield return new object[]
		{
			new string[] { "Bar", "foo", "FOO", "bar", "qux" },
			StringComparer.OrdinalIgnoreCase,
			new string[] { "FOO", "bar", },
		};
	}

	[Theory]
	[MemberData(nameof(GetStringParameters))]
	public void DuplicatesComparerBehavior(IEnumerable<string> source, StringComparer comparer, IEnumerable<string> expected)
	{
		using var ts = source.AsTestingSequence();

		var result = ts.Duplicates(comparer);
		result.AssertSequenceEqual(expected);
	}
}

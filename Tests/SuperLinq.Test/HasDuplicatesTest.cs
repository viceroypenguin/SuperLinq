namespace Test;

public class HasDuplicatesTest
{
	[Fact]
	public void DuplicatesDoesNotEnumerateUnnecessarily()
	{
		using var ts = Seq(1, 2, 3).Concat(SeqExceptionAt(2)).AsTestingSequence();

		var result = ts.HasDuplicates();
		Assert.True(result);
	}

	[Theory]
	[InlineData(new int[] { 1, 2, 3, }, false)]
	[InlineData(new int[] { 1, 2, 1, 3, 1, 2, 1, }, true)]
	[InlineData(new int[] { 3, 3, 2, 2, 1, 1, }, true)]
	public void DuplicatesBehavior(IEnumerable<int> source, bool expected)
	{
		using var ts = source.AsTestingSequence();

		var result = ts.HasDuplicates();
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
	public void DuplicatesComparerBehavior(IEnumerable<string> source, StringComparer comparer, bool expected)
	{
		using var ts = source.AsTestingSequence();

		var result = ts.HasDuplicates(comparer);
		Assert.Equal(expected, result);
	}
}

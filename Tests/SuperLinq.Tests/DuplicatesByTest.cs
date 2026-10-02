namespace SuperLinq.Tests;

public sealed class DuplicatesByTest
{
	[Fact]
	public void DuplicatesByIsLazy()
	{
		_ = new BreakingSequence<int>().DuplicatesBy(BreakingFunc.Of<int, int>());
	}

	[Fact]
	public void DuplicatesByReturnsSecondElementForEachDuplicateKey()
	{
		var source = new[] { "a", "bb", "c", "ddd", "ee", "ffff", "gg" };

		var result = source.DuplicatesBy(x => x.Length);

		result.AssertSequenceEqual("c", "ee");
	}

	[Fact]
	public void DuplicatesByUsesKeyComparer()
	{
		var source = new[] { "a", "B", "A", "b", "a" };

		var result = source.DuplicatesBy(x => x, StringComparer.OrdinalIgnoreCase);

		result.AssertSequenceEqual("A", "b");
	}

	[Fact]
	public void DuplicatesBySupportsNullKeys()
	{
		var source = new[] { "a", "A", "b" };

		var result = source.DuplicatesBy(x => StringComparer.Ordinal.Equals(x, "b") ? x : null, EqualityComparer<string?>.Default);

		result.AssertSequenceEqual("A");
	}
}

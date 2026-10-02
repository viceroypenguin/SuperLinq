namespace SuperLinq.Async.Tests;

public sealed class DuplicatesByTest
{
	[Fact]
	public void DuplicatesByIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().DuplicatesBy(BreakingFunc.Of<int, int>());
	}

	[Fact]
	public async Task DuplicatesByReturnsSecondElementForEachDuplicateKey()
	{
		await using var source = new[] { "a", "bb", "c", "ddd", "ee", "ffff", "gg" }.AsTestingSequence();

		var result = source.DuplicatesBy(x => x.Length);

		await result.AssertSequenceEqual("c", "ee");
	}

	[Fact]
	public async Task DuplicatesByUsesKeyComparer()
	{
		await using var source = new[] { "a", "B", "A", "b", "a" }.AsTestingSequence();

		var result = source.DuplicatesBy(x => x, StringComparer.OrdinalIgnoreCase);

		await result.AssertSequenceEqual("A", "b");
	}

	[Fact]
	public async Task DuplicatesBySupportsNullKeys()
	{
		await using var source = new[] { "a", "A", "b" }.AsTestingSequence();

		var result = source.DuplicatesBy(x => StringComparer.Ordinal.Equals(x, "b") ? x : null, EqualityComparer<string?>.Default);

		await result.AssertSequenceEqual("A");
	}
}

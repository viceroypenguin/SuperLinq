namespace Test.Async;

public class SplitTest
{
	[Fact]
	public Task SplitWithSeparatorAndResultTransformation()
	{
		var result = "the quick brown fox".ToAsyncEnumerable().Split(' ', chars => new string(chars.ToArray()));
		return result.AssertSequenceEqual("the", "quick", "brown", "fox");
	}

	[Fact]
	public Task SplitUptoMaxCount()
	{
		var result = "the quick brown fox".ToAsyncEnumerable().Split(' ', 2, chars => new string(chars.ToArray()));
		return result.AssertSequenceEqual("the", "quick", "brown fox");
	}

	[Fact]
	public async Task SplitWithSeparatorSelector()
	{
		var result = AsyncSeq<int?>(1, 2, null, 3, null, 4, 5, 6).Split(n => n == null);

		await using var reader = result.Read();
		(await reader.Read()).AssertSequenceEqual(1, 2);
		(await reader.Read()).AssertSequenceEqual(3);
		(await reader.Read()).AssertSequenceEqual(4, 5, 6);
		await reader.ReadEnd();
	}

	[Fact]
	public async Task SplitWithSeparatorSelectorUptoMaxCount()
	{
		var result = AsyncSeq<int?>(1, 2, null, 3, null, 4, 5, 6).Split(n => n == null, 1);

		await using var reader = result.Read();
		(await reader.Read()).AssertSequenceEqual(1, 2);
		(await reader.Read()).AssertSequenceEqual(3, null, 4, 5, 6);
		await reader.ReadEnd();
	}
}

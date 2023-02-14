namespace Test.Async;

public class SplitTest
{
	[Fact]
	public void SplitIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().Split(1);
		_ = new AsyncBreakingSequence<int>().Split(1, 2);
	}

	[Fact]
	public async Task SplitWithComparer()
	{
		await using var sequence = Enumerable.Range(1, 10).AsTestingSequence();
		var result = sequence.Split(2, EqualityComparer.Create<int>((x, y) => x % 2 == y % 2));
		await result.AssertSequenceEqual(Enumerable.Range(1, 5).Select(x => new[] { (x * 2) - 1, }));
	}

	[Fact]
	public async Task SplitWithComparerUptoMaxCount()
	{
		await using var sequence = Enumerable.Range(1, 10).AsTestingSequence();
		var result = sequence.Split(2, EqualityComparer.Create<int>((x, y) => x % 2 == y % 2), 2);
		await result.AssertSequenceEqual(new[] { 1 }, new[] { 3 }, Enumerable.Range(5, 6));
	}

	[Fact]
	public async Task SplitWithSeparatorAndResultTransformation()
	{
		await using var sequence = "the quick brown fox".AsTestingSequence();
		var result = sequence.Split(' ', chars => new string(chars.ToArray()));
		await result.AssertSequenceEqual("the", "quick", "brown", "fox");
	}

	[Fact]
	public async Task SplitUptoMaxCount()
	{
		await using var sequence = "the quick brown fox".AsTestingSequence();
		var result = sequence.Split(' ', 2, chars => new string(chars.ToArray()));
		await result.AssertSequenceEqual("the", "quick", "brown fox");
	}

	[Fact]
	public async Task SplitWithSeparatorSelector()
	{
		await using var sequence = TestingSequence.Of<int?>(1, 2, null, 3, null, 4, 5, 6);
		var result = sequence.Split(n => n == null);

		await using var reader = result.Read();
		(await reader.Read()).AssertSequenceEqual(1, 2);
		(await reader.Read()).AssertSequenceEqual(3);
		(await reader.Read()).AssertSequenceEqual(4, 5, 6);
		await reader.ReadEnd();
	}

	[Fact]
	public async Task SplitWithSeparatorSelectorUptoMaxCount()
	{
		await using var sequence = TestingSequence.Of<int?>(1, 2, null, 3, null, 4, 5, 6);
		var result = sequence.Split(n => n == null, 1);

		await using var reader = result.Read();
		(await reader.Read()).AssertSequenceEqual(1, 2);
		(await reader.Read()).AssertSequenceEqual(3, null, 4, 5, 6);
		await reader.ReadEnd();
	}
}

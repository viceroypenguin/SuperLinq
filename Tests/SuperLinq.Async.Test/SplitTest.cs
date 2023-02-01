namespace Test.Async;

public class SplitTest
{
	[Fact]
	public void SplitIsLazy()
	{
		new AsyncBreakingSequence<int>().Split(1);
		new AsyncBreakingSequence<int>().Split(1, 2);
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

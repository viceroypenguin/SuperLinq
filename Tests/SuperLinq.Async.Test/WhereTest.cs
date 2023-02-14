namespace Test.Async;

public class WhereTest
{
	[Fact]
	public void WhereIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().Where(new AsyncBreakingSequence<bool>());
	}

	[Theory]
	[InlineData(2, 3)]
	[InlineData(3, 2)]
	public async Task WhereRequiresEqualLengths(int sLength, int fLength)
	{
		_ = await Assert.ThrowsAsync<ArgumentException>(async () =>
			await AsyncEnumerable.Repeat(1, sLength).Where(AsyncEnumerable.Repeat(false, fLength)).Consume());
	}

	[Fact]
	public async Task WhereFiltersIntSequence()
	{
		var seq = AsyncEnumerable.Range(1, 10);
		var filter = seq.Select(x => x % 2 == 0);

		await using var ts1 = seq.AsTestingSequence();
		var result = ts1.Where(filter);
		await result.AssertSequenceEqual(
			seq.Where(x => x % 2 == 0));
	}

	[Fact]
	public async Task WhereFiltersStringSequence()
	{
		var words = Seq("foo", "hello", "world", "Bar", "QuX", "ay", "az");
		await using var ts1 = words.AsTestingSequence();

		await ts1.Where(AsyncSeq(true, false, false, true, false, true, false))
			.AssertSequenceEqual("foo", "Bar", "ay");
	}
}

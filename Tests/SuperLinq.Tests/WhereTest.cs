namespace SuperLinq.Tests;

public sealed class WhereTest
{
	[Fact]
	public void WhereIsLazy()
	{
		_ = new BreakingSequence<int>().Where(new BreakingSequence<bool>());
	}

	[Theory]
	[InlineData(2, 3)]
	[InlineData(3, 2)]
	public void WhereRequiresEqualLengths(int sLength, int fLength)
	{
		_ = Assert.Throws<ArgumentException>(() =>
			Enumerable.Repeat(1, sLength).Where(Enumerable.Repeat(false, fLength)).Consume());
	}

	[Fact]
	public void WhereFiltersIntSequence()
	{
		var seq = Enumerable.Range(1, 10);
		var filter = seq.Select(x => x % 2 == 0);

		using var ts1 = seq.AsTestingSequence();
		var result = ts1.Where(filter);
		result.AssertSequenceEqual(
			seq.Where(x => x % 2 == 0));
	}

	[Fact]
	public void WhereFiltersStringSequence()
	{
		var words = Seq("foo", "hello", "world", "Bar", "QuX", "ay", "az");
		using var ts1 = words.AsTestingSequence();

		ts1.Where(Seq(true, false, false, true, false, true, false))
			.AssertSequenceEqual("foo", "Bar", "ay");
	}
}

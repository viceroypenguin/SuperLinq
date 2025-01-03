namespace SuperLinq.Tests;

public sealed class WhereTest
{
	[Test]
	public void WhereIsLazy()
	{
		_ = new BreakingSequence<int>().Where(new BreakingSequence<bool>());
	}

	[Test]
	[Arguments(2, 3)]
	[Arguments(3, 2)]
	public void WhereRequiresEqualLengths(int sLength, int fLength)
	{
		_ = Assert.Throws<ArgumentException>(() =>
			Enumerable.Repeat(1, sLength).Where(Enumerable.Repeat(false, fLength)).Consume());
	}

	[Test]
	public void WhereFiltersIntSequence()
	{
		var seq = Enumerable.Range(1, 10);
		var filter = seq.Select(x => x % 2 == 0);

		using var ts1 = seq.AsTestingSequence();
		var result = ts1.Where(filter);
		result.AssertSequenceEqual(
			seq.Where(x => x % 2 == 0));
	}

	[Test]
	public void WhereFiltersStringSequence()
	{
		var words = Seq("foo", "hello", "world", "Bar", "QuX", "ay", "az");
		using var ts1 = words.AsTestingSequence();

		ts1.Where(Seq(true, false, false, true, false, true, false))
			.AssertSequenceEqual("foo", "Bar", "ay");
	}
}

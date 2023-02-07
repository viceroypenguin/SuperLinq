namespace Test;

public class AssertCountTest
{
	[Fact]
	public void AssertCountIsLazy()
	{
		_ = new BreakingSequence<object>().AssertCount(0);
	}

	[Fact]
	public void AssertCountNegativeCount()
	{
		Assert.Throws<ArgumentOutOfRangeException>("count",
			() => new BreakingSequence<int>().AssertCount(-1));
	}

	[Fact]
	public void AssertCountSequenceWithMatchingLength()
	{
		var data = new[] { "foo", "bar", "baz" };
		data.AssertCount(3).Consume();
		using (var xs = data.AsTestingSequence())
			xs.AssertCount(3).Consume();
		using (var xs = new BreakingCollection<string>(data))
			// expect a `TestException` as we try to enumerate the list
			// but should _not_ get an `ArgumentException` as the 
			// count is correct for the sequence
			Assert.Throws<TestException>(
				() => xs.AssertCount(3).Consume());
	}

	[Fact]
	public void AssertCountShortSequence()
	{
		var data = new[] { "foo", "bar", "baz" };
		foreach (var xs in data.ArrangeCollectionInlineDatas())
			using (xs)
				Assert.Throws<ArgumentException>("source.Count()",
					() => xs.AssertCount(4).Consume());
	}

	[Fact]
	public void AssertCountLongSequence()
	{
		var data = new[] { "foo", "bar", "baz" };
		foreach (var xs in data.ArrangeCollectionInlineDatas())
			using (xs)
				Assert.Throws<ArgumentException>("source.Count()",
					() => xs.AssertCount(2).Consume());
	}

	[Fact]
	public void AssertCountUsesCollectionCountAtIterationTime()
	{
		var stack = new Stack<int>(Enumerable.Range(1, 3));
		var result = stack.AssertCount(4);
		stack.Push(4);
		result.Consume();
	}
}

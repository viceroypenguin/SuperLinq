namespace Test;

public class ShuffleTest
{
	static Random seed = new Random(12345);

	[Fact]
	public void ShuffleIsLazy()
	{
		new BreakingSequence<int>().Shuffle();
	}

	[Fact]
	public void Shuffle()
	{
		var source = Enumerable.Range(1, 100);
		var result = source.Shuffle();

		Assert.Equal(source, result.OrderBy(x => x));
	}

	[Fact]
	public void ShuffleWithEmptySequence()
	{
		var source = Enumerable.Empty<int>();
		var result = source.Shuffle();

		Assert.Empty(result);
	}

	[Fact]
	public void ShuffleIsIdempotent()
	{
		var sequence = Enumerable.Range(1, 100).ToArray();
		var sequenceClone = sequence.ToArray();

		// force complete enumeration of random subsets
		sequence.Shuffle().Consume();

		// verify the original sequence is untouched
		Assert.Equal(sequenceClone, sequence);
	}

	[Fact]
	public void ShuffleSeedIsLazy()
	{
		new BreakingSequence<int>().Shuffle(seed);
	}

	[Fact]
	public void ShuffleSeed()
	{
		var source = Enumerable.Range(1, 100);
		var result = source.Shuffle(seed);

		Assert.NotEqual(source, result);
		Assert.Equal(source, result.OrderBy(x => x));
	}

	[Fact]
	public void ShuffleSeedWithEmptySequence()
	{
		var source = Enumerable.Empty<int>();
		var result = source.Shuffle(seed);

		Assert.Empty(result);
	}

	[Fact]
	public void ShuffleSeedIsIdempotent()
	{
		var sequence = Enumerable.Range(1, 100).ToArray();
		var sequenceClone = sequence.ToArray();

		// force complete enumeration of random subsets
		sequence.Shuffle(seed).Consume();

		// verify the original sequence is untouched
		Assert.Equal(sequenceClone, sequence);
	}
}

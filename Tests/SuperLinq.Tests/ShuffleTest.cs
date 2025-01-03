namespace SuperLinq.Tests;

public sealed class ShuffleTest
{
	private static readonly Random s_seed = new(12345);

	[Test]
	public void ShuffleIsLazy()
	{
		_ = new BreakingSequence<int>().Shuffle();
	}

	[Test]
	public void Shuffle()
	{
		using var source = Enumerable.Range(1, 100).AsTestingSequence();

		var result = source.Shuffle();
		result.AssertCollectionEqual(Enumerable.Range(1, 100));
	}

	[Test]
	public void ShuffleWithEmptySequence()
	{
		using var source = Enumerable.Empty<int>().AsTestingSequence();

		var result = source.Shuffle();
		result.AssertSequenceEqual();
	}

	[Test]
	public void ShuffleIsIdempotent()
	{
		var sequence = Enumerable.Range(1, 100).ToArray();

		// force complete enumeration of random subsets
		sequence.Shuffle().Consume();

		// verify the original sequence is untouched
		sequence.AssertSequenceEqual(Enumerable.Range(1, 100));
	}

	[Test]
	public void ShuffleSeedIsLazy()
	{
		_ = new BreakingSequence<int>().Shuffle(s_seed);
	}

	[Test]
	public void ShuffleSeed()
	{
		using var source = Enumerable.Range(1, 100).AsTestingSequence();

		var result = source.Shuffle(s_seed).ToList();
		result.AssertCollectionEqual(Enumerable.Range(1, 100));
	}

	[Test]
	public void ShuffleSeedWithEmptySequence()
	{
		using var source = Enumerable.Empty<int>().AsTestingSequence();

		var result = source.Shuffle(s_seed);
		result.AssertSequenceEqual();
	}

	[Test]
	public void ShuffleSeedIsIdempotent()
	{
		var sequence = Enumerable.Range(1, 100).ToArray();

		// force complete enumeration of random subsets
		sequence.Shuffle(s_seed).Consume();

		// verify the original sequence is untouched
		sequence.AssertSequenceEqual(Enumerable.Range(1, 100));
	}
}

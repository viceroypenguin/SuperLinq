namespace SuperLinq.Tests;

[Obsolete("References `DistinctBy` which is obsolete in net6+")]
public sealed class ShuffleTest
{
	private static readonly Random s_seed = new(12345);

	[Test]
	public void ShuffleIsLazy()
	{
		_ = SuperEnumerable.Shuffle(new BreakingSequence<int>());
	}

	[Test]
	public void Shuffle()
	{
		using var source = Enumerable.Range(1, 100).AsTestingSequence();

		var result = SuperEnumerable.Shuffle(source);
		result.AssertCollectionEqual(Enumerable.Range(1, 100));
	}

	[Test]
	public void ShuffleWithEmptySequence()
	{
		using var source = Enumerable.Empty<int>().AsTestingSequence();

		var result = SuperEnumerable.Shuffle(source);
		result.AssertSequenceEqual();
	}

	[Test]
	public void ShuffleIsIdempotent()
	{
		var sequence = Enumerable.Range(1, 100).ToArray();

		// force complete enumeration of random subsets
		SuperEnumerable.Shuffle(sequence).Consume();

		// verify the original sequence is untouched
		sequence.AssertSequenceEqual(Enumerable.Range(1, 100));
	}

	[Test]
	public void ShuffleSeedIsLazy()
	{
		_ = SuperEnumerable.Shuffle(new BreakingSequence<int>(), s_seed);
	}

	[Test]
	public void ShuffleSeed()
	{
		using var source = Enumerable.Range(1, 100).AsTestingSequence();

		var result = SuperEnumerable.Shuffle(source, s_seed).ToList();
		result.AssertCollectionEqual(Enumerable.Range(1, 100));
	}

	[Test]
	public void ShuffleSeedWithEmptySequence()
	{
		using var source = Enumerable.Empty<int>().AsTestingSequence();

		var result = SuperEnumerable.Shuffle(source, s_seed);
		result.AssertSequenceEqual();
	}

	[Test]
	public void ShuffleSeedIsIdempotent()
	{
		var sequence = Enumerable.Range(1, 100).ToArray();

		// force complete enumeration of random subsets
		SuperEnumerable.Shuffle(sequence, s_seed).Consume();

		// verify the original sequence is untouched
		sequence.AssertSequenceEqual(Enumerable.Range(1, 100));
	}
}

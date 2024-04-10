namespace Test;

public sealed class PartitionTest
{
	[Fact]
	public void Partition()
	{
		using var sequence = Enumerable.Range(0, 10).AsTestingSequence();

		var (evens, odds) = sequence.Partition(x => x % 2 == 0);

		evens.AssertSequenceEqual(0, 2, 4, 6, 8);
		odds.AssertSequenceEqual(1, 3, 5, 7, 9);
	}

	[Fact]
	public void PartitionWithEmptySequence()
	{
		using var sequence = Enumerable.Empty<int>().AsTestingSequence();

		var (evens, odds) = sequence.Partition(x => x % 2 == 0);

		evens.AssertSequenceEqual();
		odds.AssertSequenceEqual();
	}

	[Fact]
	public void PartitionWithResultSelector()
	{
		using var sequence = Enumerable.Range(0, 10).AsTestingSequence();

		var (evens, odds) = sequence.Partition(x => x % 2 == 0, ValueTuple.Create);

		evens.AssertSequenceEqual(0, 2, 4, 6, 8);
		odds.AssertSequenceEqual(1, 3, 5, 7, 9);
	}
}

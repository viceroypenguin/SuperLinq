namespace SuperLinq.Async.Tests;

public sealed class PartitionTest
{
	[Test]
	public async Task Partition()
	{
		await using var sequence = Enumerable.Range(0, 10).AsTestingSequence();

		var (evens, odds) = await sequence.Partition(x => x % 2 == 0);

		evens.AssertSequenceEqual(0, 2, 4, 6, 8);
		odds.AssertSequenceEqual(1, 3, 5, 7, 9);
	}

	[Test]
	public async Task PartitionWithEmptySequence()
	{
		await using var sequence = Enumerable.Empty<int>().AsTestingSequence();

		var (evens, odds) = await sequence.Partition(x => x % 2 == 0);

		evens.AssertSequenceEqual();
		odds.AssertSequenceEqual();
	}

	[Test]
	public async Task PartitionWithResultSelector()
	{
		await using var sequence = Enumerable.Range(0, 10).AsTestingSequence();

		var (evens, odds) = await sequence.Partition(x => x % 2 == 0, ValueTuple.Create);

		evens.AssertSequenceEqual(0, 2, 4, 6, 8);
		odds.AssertSequenceEqual(1, 3, 5, 7, 9);
	}
}

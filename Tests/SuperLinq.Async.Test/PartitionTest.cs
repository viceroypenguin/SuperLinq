namespace Test.Async;

public class PartitionTest
{
	[Fact]
	public async Task Partition()
	{
		await using var sequence = Enumerable.Range(0, 10).AsTestingSequence();

		var (evens, odds) = await sequence.Partition(x => x % 2 == 0);

		await evens.AssertSequenceEqual(0, 2, 4, 6, 8);
		await odds.AssertSequenceEqual(1, 3, 5, 7, 9);
	}

	[Fact]
	public async Task PartitionWithEmptySequence()
	{
		await using var sequence = Enumerable.Empty<int>().AsTestingSequence();

		var (evens, odds) = await sequence.Partition(x => x % 2 == 0);

		await evens.AssertSequenceEqual();
		await odds.AssertSequenceEqual();
	}

	[Fact]
	public async Task PartitionWithResultSelector()
	{
		await using var sequence = Enumerable.Range(0, 10).AsTestingSequence();

		var (evens, odds) = await sequence.Partition(x => x % 2 == 0, ValueTuple.Create);

		await evens.AssertSequenceEqual(0, 2, 4, 6, 8);
		await odds.AssertSequenceEqual(1, 3, 5, 7, 9);
	}

	[Fact]
	public async Task PartitionBooleanGrouping()
	{
		await using var sequence = Enumerable.Range(0, 10).AsTestingSequence();

		var (evens, odds) = await sequence
			.GroupBy(x => x % 2 == 0)
			.Partition(ValueTuple.Create);

		await evens.AssertSequenceEqual(0, 2, 4, 6, 8);
		await odds.AssertSequenceEqual(1, 3, 5, 7, 9);
	}

	[Fact]
	public async Task PartitionNullableBooleanGrouping()
	{
		await using var xs = TestingSequence.Of<int?>(1, 2, 3, null, 5, 6, 7, null, 9, 10);

		var (lt5, gte5, nils) = await xs
			.GroupBy(x => x != null ? x < 5 : (bool?)null)
			.Partition(ValueTuple.Create);

		await lt5.AssertSequenceEqual(1, 2, 3);
		await gte5.AssertSequenceEqual(5, 6, 7, 9, 10);
		await nils.AssertSequenceEqual(default(int?), null);
	}

	[Fact]
	public async Task PartitionBooleanGroupingWithSingleKey()
	{
		await using var sequence = Enumerable.Range(0, 10).AsTestingSequence();

		var (m3, etc) = await sequence
			.GroupBy(x => x % 3)
			.Partition(0, ValueTuple.Create);

		await m3.AssertSequenceEqual(0, 3, 6, 9);

		await using var r = etc.Read();
		var r1 = await r.Read();
		Assert.Equal(1, r1.Key);
		await r1.AssertSequenceEqual(1, 4, 7);

		var r2 = await r.Read();
		Assert.Equal(2, r2.Key);
		await r2.AssertSequenceEqual(2, 5, 8);

		await r.ReadEnd();
	}

	[Fact]
	public async Task PartitionBooleanGroupingWitTwoKeys()
	{
		await using var sequence = Enumerable.Range(0, 10).AsTestingSequence();

		var (ms, r1, etc) = await sequence
			.GroupBy(x => x % 3)
			.Partition(0, 1, ValueTuple.Create);

		await ms.AssertSequenceEqual(0, 3, 6, 9);
		await r1.AssertSequenceEqual(1, 4, 7);

		await using var r = etc.Read();
		var r2 = await r.Read();
		Assert.Equal(2, r2.Key);
		await r2.AssertSequenceEqual(2, 5, 8);
		await r.ReadEnd();
	}

	[Fact]
	public async Task PartitionBooleanGroupingWitThreeKeys()
	{
		await using var sequence = Enumerable.Range(0, 10).AsTestingSequence();

		var (ms, r1, r2, etc) = await sequence
			.GroupBy(x => x % 3)
			.Partition(0, 1, 2, ValueTuple.Create);

		await ms.AssertSequenceEqual(0, 3, 6, 9);
		await r1.AssertSequenceEqual(1, 4, 7);
		await r2.AssertSequenceEqual(2, 5, 8);
		await etc.AssertSequenceEqual();
	}

	[Fact]
	public async Task PartitionBooleanGroupingWithSingleKeyWithComparer()
	{
		await using var words = TestingSequence.Of("foo", "bar", "FOO", "Bar");

		var (foo, etc) = await words
			.GroupBy(SuperEnumerable.Identity, StringComparer.OrdinalIgnoreCase)
			.Partition("foo", StringComparer.OrdinalIgnoreCase, ValueTuple.Create);

		await foo.AssertSequenceEqual("foo", "FOO");

		await using var r = etc.Read();
		var bar = await r.Read();
		await bar.AssertSequenceEqual("bar", "Bar");
		await r.ReadEnd();
	}

	[Fact]
	public async Task PartitionBooleanGroupingWithTwoKeysWithComparer()
	{
		await using var words = TestingSequence.Of("foo", "bar", "FOO", "Bar", "baz", "QUx", "bAz", "QuX");

		var (foos, bar, etc) = await words
			.GroupBy(SuperEnumerable.Identity, StringComparer.OrdinalIgnoreCase)
			.Partition("foo", "bar", StringComparer.OrdinalIgnoreCase, ValueTuple.Create);

		await foos.AssertSequenceEqual("foo", "FOO");
		await bar.AssertSequenceEqual("bar", "Bar");

		await using var r = etc.Read();
		var baz = await r.Read();
		Assert.Equal("baz", baz.Key);
		await baz.AssertSequenceEqual("baz", "bAz");

		var qux = await r.Read();
		Assert.Equal("QUx", qux.Key);
		await qux.AssertSequenceEqual("QUx", "QuX");

		await r.ReadEnd();
	}

	[Fact]
	public async Task PartitionBooleanGroupingWithThreeKeysWithComparer()
	{
		await using var words = TestingSequence.Of("foo", "bar", "FOO", "Bar", "baz", "QUx", "bAz", "QuX");

		var (foos, bar, baz, etc) = await words
			.GroupBy(SuperEnumerable.Identity, StringComparer.OrdinalIgnoreCase)
			.Partition("foo", "bar", "baz", StringComparer.OrdinalIgnoreCase, Tuple.Create);

		await foos.AssertSequenceEqual("foo", "FOO");
		await bar.AssertSequenceEqual("bar", "Bar");
		await baz.AssertSequenceEqual("baz", "bAz");

		await using var r = etc.Read();
		var qux = await r.Read();
		Assert.Equal("QUx", qux.Key);
		await qux.AssertSequenceEqual("QUx", "QuX");
		await r.ReadEnd();
	}
}

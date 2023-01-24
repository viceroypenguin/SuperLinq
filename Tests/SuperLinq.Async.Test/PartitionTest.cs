namespace Test.Async;

public class PartitionTest
{
	[Fact]
	public async Task Partition()
	{
		var (evens, odds) =
			await AsyncEnumerable.Range(0, 10).Partition(x => x % 2 == 0);

		await evens.AssertSequenceEqual(0, 2, 4, 6, 8);
		await odds.AssertSequenceEqual(1, 3, 5, 7, 9);
	}

	[Fact]
	public async Task PartitionWithEmptySequence()
	{
		var (evens, odds) =
			await AsyncEnumerable.Empty<int>().Partition(x => x % 2 == 0);

		await evens.AssertEmpty();
		await odds.AssertEmpty();
	}

	[Fact]
	public async Task PartitionWithResultSelector()
	{
		var (evens, odds) =
			await AsyncEnumerable.Range(0, 10).Partition(x => x % 2 == 0, ValueTuple.Create);

		await evens.AssertSequenceEqual(0, 2, 4, 6, 8);
		await odds.AssertSequenceEqual(1, 3, 5, 7, 9);
	}

	[Fact]
	public async Task PartitionBooleanGrouping()
	{
		var (evens, odds) =
			await AsyncEnumerable
				.Range(0, 10)
				.GroupBy(x => x % 2 == 0)
				.Partition((t, f) => ValueTuple.Create(t, f));

		await evens.AssertSequenceEqual(0, 2, 4, 6, 8);
		await odds.AssertSequenceEqual(1, 3, 5, 7, 9);
	}

	[Fact]
	public async Task PartitionNullableBooleanGrouping()
	{
		var xs = AsyncSeq<int?>(1, 2, 3, null, 5, 6, 7, null, 9, 10);

		var (lt5, gte5, nils) = await xs
			.GroupBy(x => x != null ? x < 5 : (bool?)null)
			.Partition((t, f, n) => ValueTuple.Create(t, f, n));

		await lt5.AssertSequenceEqual(1, 2, 3);
		await gte5.AssertSequenceEqual(5, 6, 7, 9, 10);
		await nils.AssertSequenceEqual(null, null);
	}

	[Fact]
	public async Task PartitionBooleanGroupingWithSingleKey()
	{
		var (m3, etc) = await AsyncEnumerable
			.Range(0, 10)
			.GroupBy(x => x % 3)
			.Partition(0, Tuple.Create);

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
	public async Task PartitionBooleanGroupingWithTwoKeys()
	{
		var (ms, r1, etc) = await AsyncEnumerable
			.Range(0, 10)
			.GroupBy(x => x % 3)
			.Partition(0, 1, Tuple.Create);

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
		var (ms, r1, r2, etc) = await AsyncEnumerable
			.Range(0, 10)
			.GroupBy(x => x % 3)
			.Partition(0, 1, 2, Tuple.Create);

		await ms.AssertSequenceEqual(0, 3, 6, 9);
		await r1.AssertSequenceEqual(1, 4, 7);
		await r2.AssertSequenceEqual(2, 5, 8);
		await etc.AssertEmpty();
	}

	[Fact]
	public async Task PartitionBooleanGroupingWithSingleKeyWithComparer()
	{
		var words = AsyncSeq("foo", "bar", "FOO", "Bar");

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
		var words = AsyncSeq("foo", "bar", "FOO", "Bar", "baz", "QUx", "bAz", "QuX");

		var (foos, bar, etc) = await words
			.GroupBy(SuperEnumerable.Identity, StringComparer.OrdinalIgnoreCase)
			.Partition("foo", "bar", StringComparer.OrdinalIgnoreCase, Tuple.Create);

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
		var words = AsyncSeq("foo", "bar", "FOO", "Bar", "baz", "QUx", "bAz", "QuX");


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

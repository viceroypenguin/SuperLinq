namespace Test;

#pragma warning disable CS0618

public class PartitionTest
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

	[Fact]
	public void PartitionBooleanGrouping()
	{
		using var sequence = Enumerable.Range(0, 10).AsTestingSequence();

		var (evens, odds) = sequence
			.GroupBy(x => x % 2 == 0)
			.Partition(ValueTuple.Create);

		evens.AssertSequenceEqual(0, 2, 4, 6, 8);
		odds.AssertSequenceEqual(1, 3, 5, 7, 9);
	}

	[Fact]
	public void PartitionNullableBooleanGrouping()
	{
		using var xs = TestingSequence.Of<int?>(1, 2, 3, null, 5, 6, 7, null, 9, 10);

		var (lt5, gte5, nils) = xs
			.GroupBy(x => x != null ? x < 5 : (bool?)null)
			.Partition(ValueTuple.Create);

		lt5.AssertSequenceEqual(1, 2, 3);
		gte5.AssertSequenceEqual(5, 6, 7, 9, 10);
		nils.AssertSequenceEqual(default(int?), null);
	}

	[Fact]
	public void PartitionBooleanGroupingWithSingleKey()
	{
		using var sequence = Enumerable.Range(0, 10).AsTestingSequence();

		var (m3, etc) = sequence
			.GroupBy(x => x % 3)
			.Partition(0, ValueTuple.Create);

		m3.AssertSequenceEqual(0, 3, 6, 9);

		using var r = etc.Read();
		var r1 = r.Read();
		Assert.Equal(1, r1.Key);
		r1.AssertSequenceEqual(1, 4, 7);

		var r2 = r.Read();
		Assert.Equal(2, r2.Key);
		r2.AssertSequenceEqual(2, 5, 8);

		r.ReadEnd();
	}

	[Fact]
	public void PartitionBooleanGroupingWitTwoKeys()
	{
		using var sequence = Enumerable.Range(0, 10).AsTestingSequence();

		var (ms, r1, etc) = sequence
			.GroupBy(x => x % 3)
			.Partition(0, 1, ValueTuple.Create);

		ms.AssertSequenceEqual(0, 3, 6, 9);
		r1.AssertSequenceEqual(1, 4, 7);

		using var r = etc.Read();
		var r2 = r.Read();
		Assert.Equal(2, r2.Key);
		r2.AssertSequenceEqual(2, 5, 8);
		r.ReadEnd();
	}

	[Fact]
	public void PartitionBooleanGroupingWitThreeKeys()
	{
		using var sequence = Enumerable.Range(0, 10).AsTestingSequence();

		var (ms, r1, r2, etc) = sequence
			.GroupBy(x => x % 3)
			.Partition(0, 1, 2, ValueTuple.Create);

		ms.AssertSequenceEqual(0, 3, 6, 9);
		r1.AssertSequenceEqual(1, 4, 7);
		r2.AssertSequenceEqual(2, 5, 8);
		etc.AssertSequenceEqual();
	}

	[Fact]
	public void PartitionBooleanGroupingWithSingleKeyWithComparer()
	{
		using var words = TestingSequence.Of("foo", "bar", "FOO", "Bar");

		var (foo, etc) = words
			.GroupBy(SuperEnumerable.Identity, StringComparer.OrdinalIgnoreCase)
			.Partition("foo", StringComparer.OrdinalIgnoreCase, ValueTuple.Create);

		foo.AssertSequenceEqual("foo", "FOO");

		using var r = etc.Read();
		var bar = r.Read();
		bar.AssertSequenceEqual("bar", "Bar");
		r.ReadEnd();
	}

	[Fact]
	public void PartitionBooleanGroupingWithTwoKeysWithComparer()
	{
		using var words = TestingSequence.Of("foo", "bar", "FOO", "Bar", "baz", "QUx", "bAz", "QuX");

		var (foos, bar, etc) = words
			.GroupBy(SuperEnumerable.Identity, StringComparer.OrdinalIgnoreCase)
			.Partition("foo", "bar", StringComparer.OrdinalIgnoreCase, ValueTuple.Create);

		foos.AssertSequenceEqual("foo", "FOO");
		bar.AssertSequenceEqual("bar", "Bar");

		using var r = etc.Read();
		var baz = r.Read();
		Assert.Equal("baz", baz.Key);
		baz.AssertSequenceEqual("baz", "bAz");

		var qux = r.Read();
		Assert.Equal("QUx", qux.Key);
		qux.AssertSequenceEqual("QUx", "QuX");

		r.ReadEnd();
	}

	[Fact]
	public void PartitionBooleanGroupingWithThreeKeysWithComparer()
	{
		using var words = TestingSequence.Of("foo", "bar", "FOO", "Bar", "baz", "QUx", "bAz", "QuX");

		var (foos, bar, baz, etc) = words
			.GroupBy(SuperEnumerable.Identity, StringComparer.OrdinalIgnoreCase)
			.Partition("foo", "bar", "baz", StringComparer.OrdinalIgnoreCase, Tuple.Create);

		foos.AssertSequenceEqual("foo", "FOO");
		bar.AssertSequenceEqual("bar", "Bar");
		baz.AssertSequenceEqual("baz", "bAz");

		using var r = etc.Read();
		var qux = r.Read();
		Assert.Equal("QUx", qux.Key);
		qux.AssertSequenceEqual("QUx", "QuX");
		r.ReadEnd();
	}
}

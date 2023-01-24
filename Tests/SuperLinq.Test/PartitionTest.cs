namespace Test;

public class PartitionTest
{
	[Fact]
	public void Partition()
	{
		var (evens, odds) =
			Enumerable.Range(0, 10)
					  .Partition(x => x % 2 == 0);

		Assert.Equal(new[] { 0, 2, 4, 6, 8 }, evens);
		Assert.Equal(new[] { 1, 3, 5, 7, 9 }, odds);
	}

	[Fact]
	public void PartitionWithEmptySequence()
	{
		var (evens, odds) =
			Enumerable.Empty<int>()
					  .Partition(x => x % 2 == 0);

		Assert.Empty(evens);
		Assert.Empty(odds);
	}

	[Fact]
	public void PartitionWithResultSelector()
	{
		var (evens, odds) =
			Enumerable.Range(0, 10)
					  .Partition(x => x % 2 == 0, Tuple.Create);

		Assert.Equal(new[] { 0, 2, 4, 6, 8 }, evens);
		Assert.Equal(new[] { 1, 3, 5, 7, 9 }, odds);
	}

	[Fact]
	public void PartitionBooleanGrouping()
	{
		var (evens, odds) =
			Enumerable.Range(0, 10)
					  .GroupBy(x => x % 2 == 0)
					  .Partition((t, f) => Tuple.Create(t, f));

		Assert.Equal(new[] { 0, 2, 4, 6, 8 }, evens);
		Assert.Equal(new[] { 1, 3, 5, 7, 9 }, odds);
	}

	[Fact]
	public void PartitionNullableBooleanGrouping()
	{
		var xs = new int?[] { 1, 2, 3, null, 5, 6, 7, null, 9, 10 };

		var (lt5, gte5, nils) =
			xs.GroupBy(x => x != null ? x < 5 : (bool?)null)
			  .Partition((t, f, n) => Tuple.Create(t, f, n));

		Assert.Equal(new int?[] { 1, 2, 3 }, lt5);
		Assert.Equal(new int?[] { 5, 6, 7, 9, 10 }, gte5);
		Assert.Equal(new int?[] { null, null }, nils);
	}

	[Fact]
	public void PartitionBooleanGroupingWithSingleKey()
	{
		var (m3, etc) =
			Enumerable.Range(0, 10)
					  .GroupBy(x => x % 3)
					  .Partition(0, Tuple.Create);

		Assert.Equal(new[] { 0, 3, 6, 9 }, m3);

		using var r = etc.Read();
		var r1 = r.Read();
		Assert.Equal(1, r1.Key);
		Assert.Equal(new[] { 1, 4, 7 }, r1);

		var r2 = r.Read();
		Assert.Equal(2, r2.Key);
		Assert.Equal(new[] { 2, 5, 8 }, r2);

		r.ReadEnd();
	}

	[Fact]
	public void PartitionBooleanGroupingWitTwoKeys()
	{
		var (ms, r1, etc) =
			Enumerable.Range(0, 10)
					  .GroupBy(x => x % 3)
					  .Partition(0, 1, Tuple.Create);

		Assert.Equal(new[] { 0, 3, 6, 9 }, ms);
		Assert.Equal(new[] { 1, 4, 7 }, r1);

		using var r = etc.Read();
		var r2 = r.Read();
		Assert.Equal(2, r2.Key);
		Assert.Equal(new[] { 2, 5, 8 }, r2);
		r.ReadEnd();
	}

	[Fact]
	public void PartitionBooleanGroupingWitThreeKeys()
	{
		var (ms, r1, r2, etc) =
			Enumerable.Range(0, 10)
				.GroupBy(x => x % 3)
				.Partition(0, 1, 2, Tuple.Create);

		Assert.Equal(new[] { 0, 3, 6, 9 }, ms);
		Assert.Equal(new[] { 1, 4, 7 }, r1);
		Assert.Equal(new[] { 2, 5, 8 }, r2);
		Assert.Empty(etc);
	}

	[Fact]
	public void PartitionBooleanGroupingWithSingleKeyWithComparer()
	{
		var words =
			new[] { "foo", "bar", "FOO", "Bar" };

		var (foo, etc) =
			words.GroupBy(SuperEnumerable.Identity, StringComparer.OrdinalIgnoreCase)
				.Partition("foo", StringComparer.OrdinalIgnoreCase, Tuple.Create);

		Assert.Equal(new[] { "foo", "FOO" }, foo);

		using var r = etc.Read();
		var bar = r.Read();
		Assert.Equal(new[] { "bar", "Bar" }, bar);
		r.ReadEnd();
	}

	[Fact]
	public void PartitionBooleanGroupingWithTwoKeysWithComparer()
	{
		var words =
			new[] { "foo", "bar", "FOO", "Bar", "baz", "QUx", "bAz", "QuX" };

		var (foos, bar, etc) =
			words.GroupBy(SuperEnumerable.Identity, StringComparer.OrdinalIgnoreCase)
				 .Partition("foo", "bar", StringComparer.OrdinalIgnoreCase, Tuple.Create);

		Assert.Equal(new[] { "foo", "FOO" }, foos);
		Assert.Equal(new[] { "bar", "Bar" }, bar);

		using var r = etc.Read();
		var baz = r.Read();
		Assert.Equal("baz", baz.Key);
		Assert.Equal(new[] { "baz", "bAz" }, baz);

		var qux = r.Read();
		Assert.Equal("QUx", qux.Key);
		Assert.Equal(new[] { "QUx", "QuX" }, qux);

		r.ReadEnd();
	}

	[Fact]
	public void PartitionBooleanGroupingWithThreeKeysWithComparer()
	{
		var words =
			new[] { "foo", "bar", "FOO", "Bar", "baz", "QUx", "bAz", "QuX" };

		var (foos, bar, baz, etc) =
			words.GroupBy(SuperEnumerable.Identity, StringComparer.OrdinalIgnoreCase)
				.Partition("foo", "bar", "baz", StringComparer.OrdinalIgnoreCase, Tuple.Create);

		Assert.Equal(new[] { "foo", "FOO" }, foos);
		Assert.Equal(new[] { "bar", "Bar" }, bar);
		Assert.Equal(new[] { "baz", "bAz" }, baz);

		using var r = etc.Read();
		var qux = r.Read();
		Assert.Equal("QUx", qux.Key);
		Assert.Equal(new[] { "QUx", "QuX" }, qux);
		r.ReadEnd();
	}
}

namespace Test;

public class ToLookupTest
{
	[Fact]
	public void ToLookupWithKeyValuePairs()
	{
		var pairs = new[]
		{
				KeyValuePair.Create("foo", 1),
				KeyValuePair.Create("bar", 3),
				KeyValuePair.Create("baz", 4),
				KeyValuePair.Create("foo", 2),
				KeyValuePair.Create("baz", 5),
				KeyValuePair.Create("baz", 6),
			};

		var dict = pairs.ToLookup();

		Assert.Equal(3, dict.Count);
		Assert.Equal([1, 2], dict["foo"]);
		Assert.Equal([3], dict["bar"]);
		Assert.Equal([4, 5, 6], dict["baz"]);
	}

	[Fact]
	public void ToLookupWithCouples()
	{
		var pairs = new[]
		{
				("foo", 1),
				("bar", 3),
				("baz", 4),
				("foo", 2),
				("baz", 5),
				("baz", 6),
			};

		var dict = pairs.ToLookup();

		Assert.Equal(3, dict.Count);
		Assert.Equal([1, 2], dict["foo"]);
		Assert.Equal([3], dict["bar"]);
		Assert.Equal([4, 5, 6], dict["baz"]);
	}

	[Fact]
	public void ToLookupWithKeyValuePairsWithComparer()
	{
		var pairs = new[]
		{
				KeyValuePair.Create("foo", 1),
				KeyValuePair.Create("bar", 3),
				KeyValuePair.Create("baz", 4),
				KeyValuePair.Create("foo", 2),
				KeyValuePair.Create("baz", 5),
				KeyValuePair.Create("baz", 6),
			};

		var dict = pairs.ToLookup(StringComparer.OrdinalIgnoreCase);

		Assert.Equal(3, dict.Count);
		Assert.Equal([1, 2], dict["FOO"]);
		Assert.Equal([3], dict["BAR"]);
		Assert.Equal([4, 5, 6], dict["BAZ"]);
	}

	[Fact]
	public void ToLookupWithCouplesWithComparer()
	{
		var pairs = new[]
		{
				("foo", 1),
				("bar", 3),
				("baz", 4),
				("foo", 2),
				("baz", 5),
				("baz", 6),
			};

		var dict = pairs.ToLookup(StringComparer.OrdinalIgnoreCase);

		Assert.Equal(3, dict.Count);
		Assert.Equal([1, 2], dict["FOO"]);
		Assert.Equal([3], dict["BAR"]);
		Assert.Equal([4, 5, 6], dict["BAZ"]);
	}
}

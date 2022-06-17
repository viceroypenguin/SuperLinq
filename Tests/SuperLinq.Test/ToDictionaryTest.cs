namespace Test;

public class ToDictionaryTest
{
	[Fact]
	public void ToDictionaryWithKeyValuePairs()
	{
		var pairs = new[]
		{
				KeyValuePair.Create("foo", 123),
				KeyValuePair.Create("bar", 456),
				KeyValuePair.Create("baz", 789),
			};

		var dict = pairs.ToDictionary();

		Assert.Equal(123, dict["foo"]);
		Assert.Equal(456, dict["bar"]);
		Assert.Equal(789, dict["baz"]);
	}

	[Fact]
	public void ToDictionaryWithCouples()
	{
		var pairs = new[]
		{
				("foo", 123),
				("bar", 456),
				("baz", 789),
			};

		var dict = pairs.ToDictionary();

		Assert.Equal(123, dict["foo"]);
		Assert.Equal(456, dict["bar"]);
		Assert.Equal(789, dict["baz"]);
	}

	[Fact]
	public void ToDictionaryWithKeyValuePairsWithComparer()
	{
		var pairs = new[]
		{
				KeyValuePair.Create("foo", 123),
				KeyValuePair.Create("bar", 456),
				KeyValuePair.Create("baz", 789),
			};

		var dict = pairs.ToDictionary(StringComparer.OrdinalIgnoreCase);

		Assert.Equal(123, dict["FOO"]);
		Assert.Equal(456, dict["BAR"]);
		Assert.Equal(789, dict["BAZ"]);
	}

	[Fact]
	public void ToDictionaryWithCouplesWithComparer()
	{
		var pairs = new[]
		{
				("foo", 123),
				("bar", 456),
				("baz", 789),
			};

		var dict = pairs.ToDictionary(StringComparer.OrdinalIgnoreCase);

		Assert.Equal(123, dict["FOO"]);
		Assert.Equal(456, dict["BAR"]);
		Assert.Equal(789, dict["BAZ"]);
	}
}

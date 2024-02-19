namespace Test;

[Obsolete("References `ToDictionary` which is obsolete in net8+")]
public class ToDictionaryTest
{
	[Fact]
	public void ToDictionaryWithKeyValuePairs()
	{
		using var pairs = TestingSequence.Of(
			KeyValuePair.Create("foo", 123),
			KeyValuePair.Create("bar", 456),
			KeyValuePair.Create("baz", 789));

		var dict = SuperEnumerable.ToDictionary(pairs);

		Assert.Equal(123, dict["foo"]);
		Assert.Equal(456, dict["bar"]);
		Assert.Equal(789, dict["baz"]);
	}

	[Fact]
	public void ToDictionaryWithCouples()
	{
		using var pairs = TestingSequence.Of(
			("foo", 123),
			("bar", 456),
			("baz", 789));

		var dict = SuperEnumerable.ToDictionary(pairs);

		Assert.Equal(123, dict["foo"]);
		Assert.Equal(456, dict["bar"]);
		Assert.Equal(789, dict["baz"]);
	}

	[Fact]
	public void ToDictionaryWithKeyValuePairsWithComparer()
	{
		using var pairs = TestingSequence.Of(
			KeyValuePair.Create("foo", 123),
			KeyValuePair.Create("bar", 456),
			KeyValuePair.Create("baz", 789));

		var dict = SuperEnumerable.ToDictionary(pairs, StringComparer.OrdinalIgnoreCase);

		Assert.Equal(123, dict["FOO"]);
		Assert.Equal(456, dict["BAR"]);
		Assert.Equal(789, dict["BAZ"]);
	}

	[Fact]
	public void ToDictionaryWithCouplesWithComparer()
	{
		using var pairs = TestingSequence.Of(
			("foo", 123),
			("bar", 456),
			("baz", 789));

		var dict = SuperEnumerable.ToDictionary(pairs, StringComparer.OrdinalIgnoreCase);

		Assert.Equal(123, dict["FOO"]);
		Assert.Equal(456, dict["BAR"]);
		Assert.Equal(789, dict["BAZ"]);
	}
}

namespace SuperLinq.Tests;

[Obsolete("References `ToDictionary` which is obsolete in net8+")]
public sealed class ToDictionaryTest
{
	[Test]
	public void ToDictionaryWithKeyValuePairs()
	{
		using var pairs = TestingSequence.Of(
			CreatePair("foo", 123),
			CreatePair("bar", 456),
			CreatePair("baz", 789));

		var dict = SuperEnumerable.ToDictionary(pairs);

		Assert.Equal(123, dict["foo"]);
		Assert.Equal(456, dict["bar"]);
		Assert.Equal(789, dict["baz"]);
	}

	[Test]
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

	[Test]
	public void ToDictionaryWithKeyValuePairsWithComparer()
	{
		using var pairs = TestingSequence.Of(
			CreatePair("foo", 123),
			CreatePair("bar", 456),
			CreatePair("baz", 789));

		var dict = SuperEnumerable.ToDictionary(pairs, StringComparer.OrdinalIgnoreCase);

		Assert.Equal(123, dict["FOO"]);
		Assert.Equal(456, dict["BAR"]);
		Assert.Equal(789, dict["BAZ"]);
	}

	[Test]
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

	private static KeyValuePair<TKey, TValue> CreatePair<TKey, TValue>(TKey key, TValue value) => new(key, value);
}

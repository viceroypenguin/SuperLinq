using NUnit.Framework;

namespace SuperLinq.Test;

[TestFixture]
public class ToDictionaryTest
{
	[Test]
	public void ToDictionaryWithKeyValuePairs()
	{
		var pairs = new[]
		{
				KeyValuePair.Create("foo", 123),
				KeyValuePair.Create("bar", 456),
				KeyValuePair.Create("baz", 789),
			};

		var dict = pairs.ToDictionary();

		Assert.That(dict["foo"], Is.EqualTo(123));
		Assert.That(dict["bar"], Is.EqualTo(456));
		Assert.That(dict["baz"], Is.EqualTo(789));
	}

	[Test]
	public void ToDictionaryWithCouples()
	{
		var pairs = new[]
		{
				("foo", 123),
				("bar", 456),
				("baz", 789),
			};

		var dict = pairs.ToDictionary();

		Assert.That(dict["foo"], Is.EqualTo(123));
		Assert.That(dict["bar"], Is.EqualTo(456));
		Assert.That(dict["baz"], Is.EqualTo(789));
	}

	[Test]
	public void ToDictionaryWithKeyValuePairsWithComparer()
	{
		var pairs = new[]
		{
				KeyValuePair.Create("foo", 123),
				KeyValuePair.Create("bar", 456),
				KeyValuePair.Create("baz", 789),
			};

		var dict = pairs.ToDictionary(StringComparer.OrdinalIgnoreCase);

		Assert.That(dict["FOO"], Is.EqualTo(123));
		Assert.That(dict["BAR"], Is.EqualTo(456));
		Assert.That(dict["BAZ"], Is.EqualTo(789));
	}

	[Test]
	public void ToDictionaryWithCouplesWithComparer()
	{
		var pairs = new[]
		{
				("foo", 123),
				("bar", 456),
				("baz", 789),
			};

		var dict = pairs.ToDictionary(StringComparer.OrdinalIgnoreCase);

		Assert.That(dict["FOO"], Is.EqualTo(123));
		Assert.That(dict["BAR"], Is.EqualTo(456));
		Assert.That(dict["BAZ"], Is.EqualTo(789));
	}
}

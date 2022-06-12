using NUnit.Framework;

namespace SuperLinq.Test;

[TestFixture]
public class ToLookupTest
{
	[Test]
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

		Assert.That(dict.Count, Is.EqualTo(3));
		Assert.That(dict["foo"], Is.EqualTo(new[] { 1, 2 }));
		Assert.That(dict["bar"], Is.EqualTo(new[] { 3 }));
		Assert.That(dict["baz"], Is.EqualTo(new[] { 4, 5, 6 }));
	}

	[Test]
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

		Assert.That(dict.Count, Is.EqualTo(3));
		Assert.That(dict["foo"], Is.EqualTo(new[] { 1, 2 }));
		Assert.That(dict["bar"], Is.EqualTo(new[] { 3 }));
		Assert.That(dict["baz"], Is.EqualTo(new[] { 4, 5, 6 }));
	}

	[Test]
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

		Assert.That(dict.Count, Is.EqualTo(3));
		Assert.That(dict["FOO"], Is.EqualTo(new[] { 1, 2 }));
		Assert.That(dict["BAR"], Is.EqualTo(new[] { 3 }));
		Assert.That(dict["BAZ"], Is.EqualTo(new[] { 4, 5, 6 }));
	}

	[Test]
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

		Assert.That(dict.Count, Is.EqualTo(3));
		Assert.That(dict["FOO"], Is.EqualTo(new[] { 1, 2 }));
		Assert.That(dict["BAR"], Is.EqualTo(new[] { 3 }));
		Assert.That(dict["BAZ"], Is.EqualTo(new[] { 4, 5, 6 }));
	}
}

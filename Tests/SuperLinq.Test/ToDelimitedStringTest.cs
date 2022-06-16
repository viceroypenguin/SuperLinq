using NUnit.Framework;

namespace Test;

[TestFixture]
public class ToDelimitedStringTest
{
	[Test]
	public void ToDelimitedStringWithNonEmptySequenceAndDelimiter()
	{
		var result = new[] { 1, 2, 3 }.ToDelimitedString("-");
		Assert.That(result, Is.EqualTo("1-2-3"));
	}

	[Test]
	public void ToDelimitedStringWithNonEmptySequenceContainingNulls()
	{
		var result = new object[] { 1, null, "foo", true }.ToDelimitedString(",");
		Assert.That(result, Is.EqualTo("1,,foo,True"));
	}

	[Test]
	public void ToDelimitedStringWithNonEmptySequenceContainingNullsAtStart()
	{
		var result = new object[] { null, null, "foo" }.ToDelimitedString(",");
		Assert.That(result, Is.EqualTo(",,foo"));
	}
}

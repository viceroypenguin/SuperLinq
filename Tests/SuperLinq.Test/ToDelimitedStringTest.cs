namespace Test;

public class ToDelimitedStringTest
{
	[Fact]
	public void ToDelimitedStringWithNonEmptySequenceAndDelimiter()
	{
		var result = new[] { 1, 2, 3 }.ToDelimitedString("-");
		Assert.Equal("1-2-3", result);
	}

	[Fact]
	public void ToDelimitedStringWithNonEmptySequenceContainingNulls()
	{
		var result = new object[] { 1, null, "foo", true }.ToDelimitedString(",");
		Assert.Equal("1,,foo,True", result);
	}

	[Fact]
	public void ToDelimitedStringWithNonEmptySequenceContainingNullsAtStart()
	{
		var result = new object[] { null, null, "foo" }.ToDelimitedString(",");
		Assert.Equal(",,foo", result);
	}
}

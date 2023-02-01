namespace Test;

public class ToDelimitedStringTest
{
	[Fact]
	public void ToDelimitedStringWithNonEmptySequenceAndDelimiter()
	{
		using var seq = TestingSequence.Of(1, 2, 3);
		var result = seq.ToDelimitedString("-");
		Assert.Equal("1-2-3", result);
	}

	[Fact]
	public void ToDelimitedStringWithNonEmptySequenceContainingNulls()
	{
		using var seq = TestingSequence.Of<object?>(1, null, "foo", true);
		var result = seq.ToDelimitedString(",");
		Assert.Equal("1,,foo,True", result);
	}

	[Fact]
	public void ToDelimitedStringWithNonEmptySequenceContainingNullsAtStart()
	{
		using var seq = TestingSequence.Of<object?>(null, null, "foo");
		var result = seq.ToDelimitedString(",");
		Assert.Equal(",,foo", result);
	}
}

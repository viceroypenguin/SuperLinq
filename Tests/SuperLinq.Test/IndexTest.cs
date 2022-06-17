namespace Test;

public class IndexTest
{
	[Fact]
	public void IndexIsLazy()
	{
		var bs = new BreakingSequence<object>();
		bs.Index();
		bs.Index(0);
	}

	[Fact]
	public void IndexSequence()
	{
		const string one = "one";
		const string two = "two";
		const string three = "three";
		var result = new[] { one, two, three }.Index();
		result.AssertSequenceEqual(
			(0, one),
			(1, two),
			(2, three));
	}

	[Fact]
	public void IndexSequenceStartIndex()
	{
		const string one = "one";
		const string two = "two";
		const string three = "three";
		var result = new[] { one, two, three }.Index(10);
		result.AssertSequenceEqual(
			(10, one),
			(11, two),
			(12, three));
	}
}

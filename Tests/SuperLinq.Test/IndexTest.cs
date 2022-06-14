using NUnit.Framework;

namespace Test;

[TestFixture]
public class IndexTest
{
	[Test]
	public void IndexIsLazy()
	{
		var bs = new BreakingSequence<object>();
		bs.Index();
		bs.Index(0);
	}

	[Test]
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

	[Test]
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

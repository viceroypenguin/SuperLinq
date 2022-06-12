using NUnit.Framework;

namespace SuperLinq.Test;

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
			KeyValuePair.Create(0, one),
			KeyValuePair.Create(1, two),
			KeyValuePair.Create(2, three));
	}

	[Test]
	public void IndexSequenceStartIndex()
	{
		const string one = "one";
		const string two = "two";
		const string three = "three";
		var result = new[] { one, two, three }.Index(10);
		result.AssertSequenceEqual(
			KeyValuePair.Create(10, one),
			KeyValuePair.Create(11, two),
			KeyValuePair.Create(12, three));
	}
}

using NUnit.Framework;

namespace SuperLinq.Test;

[TestFixture]
public class ForEachTest
{
	[Test]
	public void ForEachWithSequence()
	{
		var results = new List<int>();
		new[] { 1, 2, 3 }.ForEach(results.Add);
		results.AssertSequenceEqual(1, 2, 3);
	}

	[Test]
	public void ForEachIndexedWithSequence()
	{
		var valueResults = new List<int>();
		var indexResults = new List<int>();
		new[] { 9, 7, 8 }.ForEach((x, index) => { valueResults.Add(x); indexResults.Add(index); });
		valueResults.AssertSequenceEqual(9, 7, 8);
		indexResults.AssertSequenceEqual(0, 1, 2);
	}
}

using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace SuperLinq.Test;

[TestFixture]
public class PrependTest
{
	[Test]
	public void PrependWithNonEmptyTailSequence()
	{
		string[] tail = { "second", "third" };
		var head = "first";
		var whole = tail.Prepend(head);
		whole.AssertSequenceEqual("first", "second", "third");
	}

	[Test]
	public void PrependWithEmptyTailSequence()
	{
		string[] tail = Array.Empty<string>();
		var head = "first";
		var whole = tail.Prepend(head);
		whole.AssertSequenceEqual("first");
	}

	[Test]
	public void PrependWithNullHead()
	{
		string[] tail = { "second", "third" };
		string head = null;
		var whole = tail.Prepend(head);
		whole.AssertSequenceEqual(null, "second", "third");
	}

	[Test]
	public void PrependIsLazyInTailSequence()
	{
		new BreakingSequence<string>().Prepend("head");
	}

	[TestCaseSource(nameof(PrependManySource))]
	public int[] PrependMany(int[] head, int[] tail)
	{
		return tail.Aggregate(head.AsEnumerable(), SuperEnumerable.Prepend).ToArray();
	}

	public static IEnumerable<ITestCaseData> PrependManySource =>
		from x in Enumerable.Range(0, 11)
		from y in Enumerable.Range(1, 11)
		select new
		{
			Head = Enumerable.Range(0, y).Select(n => 0 - n).ToArray(),
			Tail = Enumerable.Range(1, x).ToArray(),
		}
		into e
		select new TestCaseData(e.Head, e.Tail)
			.SetName("Head = [" + string.Join(", ", e.Head) + "], " +
					 "Tail = [" + string.Join(", ", e.Tail) + "]")
			.Returns(e.Tail.Reverse().Concat(e.Head).ToArray());

	[Test]
	public void PrependWithSharedSource()
	{
		var first = new[] { 1 }.Prepend(2);
		var second = first.Prepend(3).Prepend(4);
		var third = first.Prepend(4).Prepend(8);

		second.AssertSequenceEqual(4, 3, 2, 1);
		third.AssertSequenceEqual(8, 4, 2, 1);
	}
}

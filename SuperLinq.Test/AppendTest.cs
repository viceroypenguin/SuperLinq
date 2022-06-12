using NUnit.Framework;

namespace SuperLinq.Test;

[TestFixture]
public class AppendTest
{
	#region Append with single head and tail sequence
	[Test]
	public void AppendWithNonEmptyHeadSequence()
	{
		var head = new[] { "first", "second" };
		var tail = "third";
		var whole = head.Append(tail);
		whole.AssertSequenceEqual("first", "second", "third");
	}

	[Test]
	public void AppendWithEmptyHeadSequence()
	{
		string[] head = Array.Empty<string>();
		var tail = "first";
		var whole = head.Append(tail);
		whole.AssertSequenceEqual("first");
	}

	[Test]
	public void AppendWithNullTail()
	{
		var head = new[] { "first", "second" };
		string tail = null;
		var whole = head.Append(tail);
		whole.AssertSequenceEqual("first", "second", null);
	}

	[Test]
	public void AppendIsLazyInHeadSequence()
	{
		new BreakingSequence<string>().Append("tail");
	}
	#endregion

	[TestCaseSource(nameof(ContactManySource))]
	public void AppendMany(int[] head, int[] tail)
	{
		tail.Aggregate(head.AsEnumerable(), (xs, x) => xs.Append(x))
			.AssertSequenceEqual(head.Concat(tail));
	}

	public static IEnumerable<object> ContactManySource =>
		from x in Enumerable.Range(0, 11)
		from y in Enumerable.Range(1, 20 - x)
		select new
		{
			Head = Enumerable.Range(1, x).ToArray(),
			Tail = Enumerable.Range(x + 1, y).ToArray(),
		}
		into e
		select new TestCaseData(e.Head,
								e.Tail).SetName("Head = [" + string.Join(", ", e.Head) + "], " +
												"Tail = [" + string.Join(", ", e.Tail) + "]");

	[Test]
	public void AppendWithSharedSource()
	{
		var first = new[] { 1 }.Append(2);
		var second = first.Append(3).Append(4);
		var third = first.Append(4).Append(8);

		second.AssertSequenceEqual(1, 2, 3, 4);
		third.AssertSequenceEqual(1, 2, 4, 8);
	}
}

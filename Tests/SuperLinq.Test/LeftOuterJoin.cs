using static Test.JoinOperation;

namespace Test;

public sealed class LeftOuterJoinTest
{
	private static IEnumerable<((string, string) Left, (string, string) Right)> ExecuteJoin(
		IEnumerable<(string, string)> left,
		IEnumerable<(string, string)> right,
		JoinOperation op,
		bool passProjectors) =>
		(op, passProjectors) switch
		{
			(Loop, false) => left.LeftOuterLoopJoin(right, l => l.Item1, r => r.Item1, StringComparer.OrdinalIgnoreCase),
			(Loop, true) => left.LeftOuterLoopJoin(right, l => l.Item1, r => r.Item1, l => (l, default), ValueTuple.Create, StringComparer.OrdinalIgnoreCase),
			(Hash, false) => left.LeftOuterHashJoin(right, l => l.Item1, r => r.Item1, StringComparer.OrdinalIgnoreCase),
			(Hash, true) => left.LeftOuterHashJoin(right, l => l.Item1, r => r.Item1, l => (l, default), ValueTuple.Create, StringComparer.OrdinalIgnoreCase),
			(Merge, false) => left.LeftOuterMergeJoin(right, l => l.Item1, r => r.Item1, StringComparer.OrdinalIgnoreCase),
			(Merge, true) => left.LeftOuterMergeJoin(right, l => l.Item1, r => r.Item1, l => (l, default), ValueTuple.Create, StringComparer.OrdinalIgnoreCase),

			_ => throw new NotSupportedException(),
		};

	public static IEnumerable<object[]> GetLeftOuterJoins() =>
		new[] { Loop, Hash, Merge, }.Cartesian(new[] { false, true, }, (x, y) => new object[] { x, y, });

	[Theory, MemberData(nameof(GetLeftOuterJoins))]
	public void LeftOuterJoinIsLazy(JoinOperation op, bool passProjectors)
	{
		var xs = new BreakingSequence<(string, string)>();
		var ys = new BreakingSequence<(string, string)>();

		_ = ExecuteJoin(xs, ys, op, passProjectors);
	}

	[Theory, MemberData(nameof(GetLeftOuterJoins))]
	public void LeftOuterJoinResults(JoinOperation op, bool passProjectors)
	{
		var foo = ("one", "foo");
		var bar1 = ("two", "bar");
		var bar2 = ("Two", "bar");
		var bar3 = ("TWO", "bar");
		var baz = ("three", "baz");
		var qux = ("four", "qux");
		var quux = ("five", "quux");
		var quuz = ("six", "quuz");

		using var xs = new[] { foo, bar1, qux }.OrderBy(x => x.Item1, StringComparer.OrdinalIgnoreCase).AsTestingSequence();
		using var ys = new[] { bar2, bar3, baz, quuz, quux }.OrderBy(x => x.Item1, StringComparer.OrdinalIgnoreCase).AsTestingSequence();

		var result = ExecuteJoin(xs, ys, op, passProjectors);
		result.AssertCollectionEqual(
			(bar1, bar2),
			(bar1, bar3),
			(foo, default),
			(qux, default));
	}

	[Theory, MemberData(nameof(GetLeftOuterJoins))]
	public void LeftOuterJoinEmptyLeft(JoinOperation op, bool passProjectors)
	{
		var foo = ("one", "foo");
		var bar = ("two", "bar");
		var baz = ("three", "baz");

		using var xs = TestingSequence.Of<(string, string)>();
		using var ys = TestingSequence.Of(foo, bar, baz);

		var result = ExecuteJoin(xs, ys, op, passProjectors);
		result.AssertCollectionEqual();
	}

	[Theory, MemberData(nameof(GetLeftOuterJoins))]
	public void LeftOuterJoinEmptyRight(JoinOperation op, bool passProjectors)
	{
		var foo = ("one", "foo");
		var bar = ("two", "bar");
		var baz = ("three", "baz");

		using var xs = TestingSequence.Of(foo, bar, baz);
		using var ys = TestingSequence.Of<(string, string)>();

		var result = ExecuteJoin(xs, ys, op, passProjectors);
		result.AssertCollectionEqual(
			(foo, default),
			(bar, default),
			(baz, default));
	}
}

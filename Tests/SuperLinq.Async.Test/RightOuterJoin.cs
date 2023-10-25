﻿using static Test.Async.JoinOperation;

namespace Test.Async;

public class RightOuterJoinTest
{
	private static IAsyncEnumerable<((string, string) Left, (string, string) Right)> ExecuteJoin(
		IAsyncEnumerable<(string, string)> left,
		IAsyncEnumerable<(string, string)> right,
		JoinOperation op,
		bool passProjectors) =>
		(op, passProjectors) switch
		{
			(Hash, false) => left.RightOuterHashJoin(right, l => l.Item1, r => r.Item1, StringComparer.OrdinalIgnoreCase),
			(Hash, true) => left.RightOuterHashJoin(right, l => l.Item1, r => r.Item1, r => (default, r), ValueTuple.Create, StringComparer.OrdinalIgnoreCase),
			(Merge, false) => left.RightOuterMergeJoin(right, l => l.Item1, r => r.Item1, StringComparer.OrdinalIgnoreCase),
			(Merge, true) => left.RightOuterMergeJoin(right, l => l.Item1, r => r.Item1, r => (default, r), ValueTuple.Create, StringComparer.OrdinalIgnoreCase),

			_ => throw new NotSupportedException(),
		};

	public static IEnumerable<object[]> GetRightOuterJoins() =>
		new[] { Hash, Merge, }.Cartesian(new[] { false, true, }, (x, y) => new object[] { x, y, });

	[Theory, MemberData(nameof(GetRightOuterJoins))]
	public void RightOuterJoinIsLazy(JoinOperation op, bool passProjectors)
	{
		var xs = new AsyncBreakingSequence<(string, string)>();
		var ys = new AsyncBreakingSequence<(string, string)>();

		_ = ExecuteJoin(xs, ys, op, passProjectors);
	}

	[Theory, MemberData(nameof(GetRightOuterJoins))]
	public async Task RightOuterJoinResults(JoinOperation op, bool passProjectors)
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
		await result.AssertCollectionEqual(
			(bar1, bar2),
			(bar1, bar3),
			(default, baz),
			(default, quuz),
			(default, quux));
	}

	[Theory, MemberData(nameof(GetRightOuterJoins))]
	public async Task RightOuterJoinEmptyLeft(JoinOperation op, bool passProjectors)
	{
		var foo = ("one", "foo");
		var bar = ("two", "bar");
		var baz = ("three", "baz");

		using var xs = TestingSequence.Of<(string, string)>();
		using var ys = TestingSequence.Of(foo, bar, baz);

		var result = ExecuteJoin(xs, ys, op, passProjectors);
		await result.AssertCollectionEqual(
			(default, foo),
			(default, bar),
			(default, baz));
	}

	[Theory, MemberData(nameof(GetRightOuterJoins))]
	public async Task RightOuterJoinEmptyRight(JoinOperation op, bool passProjectors)
	{
		var foo = ("one", "foo");
		var bar = ("two", "bar");
		var baz = ("three", "baz");

		using var xs = TestingSequence.Of(foo, bar, baz);
		using var ys = TestingSequence.Of<(string, string)>();

		var result = ExecuteJoin(xs, ys, op, passProjectors);
		await result.AssertCollectionEqual();
	}
}

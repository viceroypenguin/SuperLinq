﻿using static Test.FullOuterJoinTest.Side;

namespace Test;

public class FullOuterJoinTest
{
	public enum Side { Left, Right, Both }

	public static IEnumerable<object[]> GetJoinTypes() =>
		new[]
		{
			new object[] { JoinType.Hash, },
			new object[] { JoinType.Merge, },
		};

	[Theory, MemberData(nameof(GetJoinTypes))]
	public void FullOuterJoinIsLazy(JoinType joinType)
	{
		var xs = new BreakingSequence<int>();
		var ys = new BreakingSequence<double>();

		xs.FullOuterJoin(
			ys, joinType,
			BreakingFunc.Of<int, string>(),
			BreakingFunc.Of<double, string>());

		xs.FullOuterJoin(
			ys, joinType,
			BreakingFunc.Of<int, string>(),
			BreakingFunc.Of<double, string>(),
			StringComparer.Ordinal);

		xs.FullOuterJoin(
			ys, joinType,
			BreakingFunc.Of<int, string>(),
			BreakingFunc.Of<double, string>(),
			BreakingFunc.Of<int, object>(),
			BreakingFunc.Of<double, object>(),
			BreakingFunc.Of<int, double, object>());

		xs.FullOuterJoin(
			ys, joinType,
			BreakingFunc.Of<int, string>(),
			BreakingFunc.Of<double, string>(),
			BreakingFunc.Of<int, object>(),
			BreakingFunc.Of<double, object>(),
			BreakingFunc.Of<int, double, object>(),
			StringComparer.Ordinal);
	}

	[Theory, MemberData(nameof(GetJoinTypes))]
	public void FullOuterJoinResults(JoinType joinType)
	{
		var foo = (1, "foo");
		var bar1 = (2, "bar");
		var bar2 = (2, "Bar");
		var bar3 = (2, "BAR");
		var baz = (3, "baz");
		var qux = (4, "qux");
		var quux = (5, "quux");
		var quuz = (6, "quuz");

		var xs = Seq(foo, bar1, qux);
		var ys = Seq(bar2, bar3, baz, quuz, quux);

		var missing = default((int, string));

		var result = xs
			.FullOuterJoin(ys,
				joinType,
				x => x.Item1,
				y => y.Item1,
				x => (Left, x, missing),
				y => (Right, missing, y),
				(x, y) => (Both, x, y));

		result.AssertCollectionEqual(
			(Left, foo, missing),
			(Both, bar1, bar2),
			(Both, bar1, bar3),
			(Right, missing, baz),
			(Left, qux, missing),
			(Right, missing, quux),
			(Right, missing, quuz));
	}

	[Theory, MemberData(nameof(GetJoinTypes))]
	public void FullOuterJoinWithComparerResults(JoinType joinType)
	{
		var foo = ("one", "foo");
		var bar1 = ("two", "bar");
		var bar2 = ("Two", "bar");
		var bar3 = ("TWO", "bar");
		var baz = ("three", "baz");
		var qux = ("four", "qux");
		var quux = ("five", "quux");
		var quuz = ("six", "quuz");

		var xs = Seq(foo, bar1, qux);
		var ys = Seq(bar2, bar3, baz, quuz, quux);

		var missing = default((string, string));

		var result = xs
			.FullOuterJoin(ys,
				joinType,
				x => x.Item1,
				y => y.Item1,
				x => (Left, x, missing),
				y => (Right, missing, y),
				(x, y) => (Both, x, y),
				StringComparer.OrdinalIgnoreCase);

		result.AssertCollectionEqual(
			(Left, foo, missing),
			(Both, bar1, bar2),
			(Both, bar1, bar3),
			(Left, qux, missing),
			(Right, missing, quux),
			(Right, missing, baz),
			(Right, missing, quuz));
	}

	[Theory, MemberData(nameof(GetJoinTypes))]
	public void FullOuterJoinEmptyLeft(JoinType joinType)
	{
		var foo = (1, "foo");
		var bar = (2, "bar");
		var baz = (3, "baz");

		var xs = Seq<(int, string)>();
		var ys = Seq(foo, bar, baz);

		var missing = default((int, string));

		var result = xs
			.FullOuterJoin(ys,
				joinType,
				x => x.Item1,
				y => y.Item1,
				x => (Left, x, missing),
				y => (Right, missing, y),
				(x, y) => (Both, x, y));

		result.AssertSequenceEqual(
			(Right, missing, foo),
			(Right, missing, bar),
			(Right, missing, baz));
	}

	[Theory, MemberData(nameof(GetJoinTypes))]
	public void FullOuterJoinEmptyRight(JoinType joinType)
	{
		var foo = (1, "foo");
		var bar = (2, "bar");
		var baz = (3, "baz");

		var xs = Seq(foo, bar, baz);
		var ys = Seq<(int, string)>();

		var missing = default((int, string));

		var result = xs
			.FullOuterJoin(ys,
				joinType,
				x => x.Item1,
				y => y.Item1,
				x => (Left, x, missing),
				y => (Right, missing, y),
				(x, y) => (Both, x, y));

		result.AssertSequenceEqual(
			(Left, foo, missing),
			(Left, bar, missing),
			(Left, baz, missing));
	}
}
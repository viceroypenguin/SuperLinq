using static Test.Async.FullOuterJoinTest.Side;

namespace Test.Async;

public class LeftOuterJoinTest
{
	public static IEnumerable<object[]> GetJoinTypes() =>
		new[]
		{
			new object[] { JoinType.Loop, },
			new object[] { JoinType.Hash, },
			new object[] { JoinType.Merge, },
		};

	[Theory, MemberData(nameof(GetJoinTypes))]
	public void LeftOuterJoinIsLazy(JoinType joinType)
	{
		var xs = new AsyncBreakingSequence<int>();
		var ys = new AsyncBreakingSequence<double>();

		xs.LeftOuterJoin(
			ys, joinType,
			BreakingFunc.Of<int, string>(),
			BreakingFunc.Of<double, string>());

		xs.LeftOuterJoin(
			ys, joinType,
			BreakingFunc.Of<int, string>(),
			BreakingFunc.Of<double, string>(),
			StringComparer.Ordinal);

		xs.LeftOuterJoin(
			ys, joinType,
			BreakingFunc.Of<int, string>(),
			BreakingFunc.Of<double, string>(),
			BreakingFunc.Of<int, object>(),
			BreakingFunc.Of<int, double, object>());

		xs.LeftOuterJoin(
			ys, joinType,
			BreakingFunc.Of<int, string>(),
			BreakingFunc.Of<double, string>(),
			BreakingFunc.Of<int, object>(),
			BreakingFunc.Of<int, double, object>(),
			StringComparer.Ordinal);
	}

	[Theory, MemberData(nameof(GetJoinTypes))]
	public async Task LeftOuterJoinResults(JoinType joinType)
	{
		var foo = (1, "foo");
		var bar1 = (2, "bar");
		var bar2 = (2, "Bar");
		var bar3 = (2, "BAR");
		var baz = (3, "baz");
		var qux = (4, "qux");
		var quux = (5, "quux");
		var quuz = (6, "quuz");

		var xs = AsyncSeq(foo, bar1, qux);
		var ys = AsyncSeq(bar2, bar3, baz, quuz, quux);

		var missing = default((int, string));

		var result = xs
			.LeftOuterJoin(ys,
				joinType,
				x => x.Item1,
				y => y.Item1,
				x => (Left, x, missing),
				(x, y) => (Both, x, y));

		await result.AssertCollectionEqual(
			(Left, foo, missing),
			(Both, bar1, bar2),
			(Both, bar1, bar3),
			(Left, qux, missing));
	}

	[Theory, MemberData(nameof(GetJoinTypes))]
	public async Task LeftOuterJoinWithComparerResults(JoinType joinType)
	{
		var foo = ("one", "foo");
		var bar1 = ("two", "bar");
		var bar2 = ("Two", "bar");
		var bar3 = ("TWO", "bar");
		var baz = ("three", "baz");
		var qux = ("four", "qux");
		var quux = ("five", "quux");
		var quuz = ("six", "quuz");

		var xs = AsyncSeq(foo, bar1, qux);
		var ys = AsyncSeq(bar2, bar3, baz, quuz, quux);

		var missing = default((string, string));

		var result = xs
			.LeftOuterJoin(ys,
				joinType,
				x => x.Item1,
				y => y.Item1,
				x => (Left, x, missing),
				(x, y) => (Both, x, y),
				StringComparer.OrdinalIgnoreCase);

		await result.AssertCollectionEqual(
			(Left, foo, missing),
			(Both, bar1, bar2),
			(Both, bar1, bar3),
			(Left, qux, missing));
	}

	[Theory, MemberData(nameof(GetJoinTypes))]
	public async Task LeftOuterJoinEmptyLeft(JoinType joinType)
	{
		var foo = (1, "foo");
		var bar = (2, "bar");
		var baz = (3, "baz");

		var xs = AsyncSeq<(int, string)>();
		var ys = AsyncSeq(foo, bar, baz);

		var missing = default((int, string));

		var result = xs
			.LeftOuterJoin(ys,
				joinType,
				x => x.Item1,
				y => y.Item1,
				x => (Left, x, missing),
				(x, y) => (Both, x, y));

		await result.AssertSequenceEqual();
	}

	[Theory, MemberData(nameof(GetJoinTypes))]
	public async Task LeftOuterJoinEmptyRight(JoinType joinType)
	{
		var foo = (1, "foo");
		var bar = (2, "bar");
		var baz = (3, "baz");

		var xs = AsyncSeq(foo, bar, baz);
		var ys = AsyncSeq<(int, string)>();

		var missing = default((int, string));

		var result = xs
			.LeftOuterJoin(ys,
				joinType,
				x => x.Item1,
				y => y.Item1,
				x => (Left, x, missing),
				(x, y) => (Both, x, y));

		await result.AssertSequenceEqual(
			(Left, foo, missing),
			(Left, bar, missing),
			(Left, baz, missing));
	}
}

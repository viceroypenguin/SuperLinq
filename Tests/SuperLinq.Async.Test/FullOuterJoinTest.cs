using SuperLinq;
using static Test.Async.FullOuterJoinTest.Side;

namespace Test.Async;

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
		var xs = new AsyncBreakingSequence<int>();
		var ys = new AsyncBreakingSequence<double>();

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
	public async Task FullOuterJoinResults(JoinType joinType)
	{
		var foo = (1, "foo");
		var bar1 = (2, "bar");
		var bar2 = (2, "Bar");
		var bar3 = (2, "BAR");
		var baz = (3, "baz");
		var qux = (4, "qux");
		var quux = (5, "quux");
		var quuz = (6, "quuz");

		using var xs = AsyncSeq(foo, bar1, qux).AsTestingSequence();
		using var ys = AsyncSeq(bar2, bar3, baz, quuz, quux).AsTestingSequence();

		var missing = default((int, string));

		var result = xs
			.FullOuterJoin(ys,
				joinType,
				x => x.Item1,
				y => y.Item1,
				x => (Left, x, missing),
				y => (Right, missing, y),
				(x, y) => (Both, x, y));

		await result.AssertCollectionEqual(
			(Left, foo, missing),
			(Both, bar1, bar2),
			(Both, bar1, bar3),
			(Right, missing, baz),
			(Left, qux, missing),
			(Right, missing, quux),
			(Right, missing, quuz));
	}

	[Theory, MemberData(nameof(GetJoinTypes))]
	public async Task FullOuterJoinWithComparerResults(JoinType joinType)
	{
		var foo = ("one", "foo");
		var bar1 = ("two", "bar");
		var bar2 = ("Two", "bar");
		var bar3 = ("TWO", "bar");
		var baz = ("three", "baz");
		var qux = ("four", "qux");
		var quux = ("five", "quux");
		var quuz = ("six", "quuz");

		using var xs = AsyncSeq(foo, bar1, qux).AsTestingSequence();
		using var ys = AsyncSeq(bar2, bar3, baz, quuz, quux).AsTestingSequence();

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

		await result.AssertCollectionEqual(
			(Left, foo, missing),
			(Both, bar1, bar2),
			(Both, bar1, bar3),
			(Left, qux, missing),
			(Right, missing, quux),
			(Right, missing, baz),
			(Right, missing, quuz));
	}

	[Theory, MemberData(nameof(GetJoinTypes))]
	public async Task FullOuterJoinEmptyLeft(JoinType joinType)
	{
		var foo = (1, "foo");
		var bar = (2, "bar");
		var baz = (3, "baz");

		using var xs = AsyncSeq<(int, string)>().AsTestingSequence();
		using var ys = AsyncSeq(foo, bar, baz).AsTestingSequence();

		var missing = default((int, string));

		var result = xs
			.FullOuterJoin(ys,
				joinType,
				x => x.Item1,
				y => y.Item1,
				x => (Left, x, missing),
				y => (Right, missing, y),
				(x, y) => (Both, x, y));

		await result.AssertSequenceEqual(
			(Right, missing, foo),
			(Right, missing, bar),
			(Right, missing, baz));
	}

	[Theory, MemberData(nameof(GetJoinTypes))]
	public async Task FullOuterJoinEmptyRight(JoinType joinType)
	{
		var foo = (1, "foo");
		var bar = (2, "bar");
		var baz = (3, "baz");

		using var xs = AsyncSeq(foo, bar, baz).AsTestingSequence();
		using var ys = AsyncSeq<(int, string)>().AsTestingSequence();

		var missing = default((int, string));

		var result = xs
			.FullOuterJoin(ys,
				joinType,
				x => x.Item1,
				y => y.Item1,
				x => (Left, x, missing),
				y => (Right, missing, y),
				(x, y) => (Both, x, y));

		await result.AssertSequenceEqual(
			(Left, foo, missing),
			(Left, bar, missing),
			(Left, baz, missing));
	}
}

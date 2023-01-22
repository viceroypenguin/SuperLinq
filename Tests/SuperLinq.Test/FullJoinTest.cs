using static Test.FullOuterJoinTest.Side;

namespace Test;

#pragma warning disable CS0618 // Type or member is obsolete

public class FullJoinTest
{
	[Fact]
	public void FullJoinWithHomogeneousSequencesIsLazy()
	{
		var xs = new BreakingSequence<int>();
		var ys = new BreakingSequence<int>();

		xs.FullJoin(ys, e => e,
			BreakingFunc.Of<int, object>(),
			BreakingFunc.Of<int, object>(),
			BreakingFunc.Of<int, int, object>());
	}

	[Fact]
	public void FullJoinWithHomogeneousSequencesWithComparerIsLazy()
	{
		var xs = new BreakingSequence<int>();
		var ys = new BreakingSequence<int>();

		xs.FullJoin(ys, e => e,
			BreakingFunc.Of<int, object>(),
			BreakingFunc.Of<int, object>(),
			BreakingFunc.Of<int, int, object>(),
			comparer: null);
	}

	[Fact]
	public void FullJoinIsLazy()
	{
		var xs = new BreakingSequence<int>();
		var ys = new BreakingSequence<object>();

		xs.FullJoin(ys, x => x, y => y.GetHashCode(),
			BreakingFunc.Of<int, object>(),
			BreakingFunc.Of<object, object>(),
			BreakingFunc.Of<int, object, object>());
	}

	[Fact]
	public void FullJoinWithComparerIsLazy()
	{
		var xs = new BreakingSequence<int>();
		var ys = new BreakingSequence<object>();

		xs.FullJoin(ys, x => x, y => y.GetHashCode(),
			BreakingFunc.Of<int, object>(),
			BreakingFunc.Of<object, object>(),
			BreakingFunc.Of<int, object, object>(),
			comparer: null);
	}

	[Fact]
	public void FullJoinResults()
	{
		var foo = (1, "foo");
		var bar1 = (2, "bar");
		var bar2 = (2, "Bar");
		var bar3 = (2, "BAR");
		var baz = (3, "baz");
		var qux = (4, "qux");
		var quux = (5, "quux");
		var quuz = (6, "quuz");

		using var xs = TestingSequence.Of(foo, bar1, qux);
		using var ys = TestingSequence.Of(quux, bar2, baz, bar3, quuz);

		var missing = default((int, string));

		var result =
			xs.FullJoin(ys,
						x => x.Item1,
						y => y.Item1,
						x => (Left, x, missing),
						y => (Right, missing, y),
						(x, y) => (Both, x, y));

		result.AssertSequenceEqual(
			(Left, foo, missing),
			(Both, bar1, bar2),
			(Both, bar1, bar3),
			(Left, qux, missing),
			(Right, missing, quux),
			(Right, missing, baz),
			(Right, missing, quuz));
	}

	[Fact]
	public void FullJoinWithComparerResults()
	{
		var foo = ("one", "foo");
		var bar1 = ("two", "bar");
		var bar2 = ("Two", "bar");
		var bar3 = ("TWO", "bar");
		var baz = ("three", "baz");
		var qux = ("four", "qux");
		var quux = ("five", "quux");
		var quuz = ("six", "quuz");

		using var xs = TestingSequence.Of(foo, bar1, qux);
		using var ys = TestingSequence.Of(quux, bar2, baz, bar3, quuz);

		var missing = default((string, string));

		var result =
			xs.FullJoin(ys,
						x => x.Item1,
						y => y.Item1,
						x => (Left, x, missing),
						y => (Right, missing, y),
						(x, y) => (Both, x, y),
						StringComparer.OrdinalIgnoreCase);

		result.AssertSequenceEqual(
			(Left, foo, missing),
			(Both, bar1, bar2),
			(Both, bar1, bar3),
			(Left, qux, missing),
			(Right, missing, quux),
			(Right, missing, baz),
			(Right, missing, quuz));
	}

	[Fact]
	public void FullJoinEmptyLeft()
	{
		var foo = (1, "foo");
		var bar = (2, "bar");
		var baz = (3, "baz");

		using var xs = Array.Empty<(int, string)>().AsTestingSequence();
		using var ys = TestingSequence.Of(foo, bar, baz);

		var missing = default((int, string));

		var result =
			xs.FullJoin(ys,
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

	[Fact]
	public void FullJoinEmptyRight()
	{
		var foo = (1, "foo");
		var bar = (2, "bar");
		var baz = (3, "baz");

		using var xs = TestingSequence.Of(foo, bar, baz);
		using var ys = Array.Empty<(int, string)>().AsTestingSequence();

		var missing = default((int, string));

		var result =
			xs.FullJoin(ys,
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

#pragma warning restore CS0618 // Type or member is obsolete

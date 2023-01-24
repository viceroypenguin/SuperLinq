using static Test.FullOuterJoinTest.Side;

namespace Test;

#pragma warning disable CS0618 // Type or member is obsolete

public class RightJoinTest
{
	[Fact]
	public void RightJoinWithHomogeneousSequencesIsLazy()
	{
		var xs = new BreakingSequence<int>();
		var ys = new BreakingSequence<int>();

		xs.RightJoin(ys, SuperEnumerable.Identity,
			BreakingFunc.Of<int, object>(),
			BreakingFunc.Of<int, int, object>());
	}

	[Fact]
	public void RightJoinWithHomogeneousSequencesWithComparerIsLazy()
	{
		var xs = new BreakingSequence<int>();
		var ys = new BreakingSequence<int>();

		xs.RightJoin(ys, SuperEnumerable.Identity,
			BreakingFunc.Of<int, object>(),
			BreakingFunc.Of<int, int, object>(),
			comparer: null);
	}

	[Fact]
	public void RightJoinIsLazy()
	{
		var xs = new BreakingSequence<int>();
		var ys = new BreakingSequence<object>();

		xs.RightJoin(ys, x => x.GetHashCode(), SuperEnumerable.Identity,
			BreakingFunc.Of<object, object>(),
			BreakingFunc.Of<int, object, object>());
	}

	[Fact]
	public void RightJoinWithComparerIsLazy()
	{
		var xs = new BreakingSequence<int>();
		var ys = new BreakingSequence<object>();

		xs.RightJoin(ys, x => x.GetHashCode(), SuperEnumerable.Identity,
			BreakingFunc.Of<object, object>(),
			BreakingFunc.Of<int, object, object>(),
			comparer: null);
	}

	[Fact]
	public void RightJoinResults()
	{
		var foo = (1, "foo");
		var bar1 = (2, "bar");
		var bar2 = (2, "Bar");
		var bar3 = (2, "BAR");
		var baz = (3, "baz");
		var qux = (4, "qux");

		var xs = new[] { bar2, baz, bar3 };
		var ys = new[] { foo, bar1, qux };

		var missing = default((int, string));

		var result =
			xs.RightJoin(ys,
						 x => x.Item1,
						 y => y.Item1,
						 y => (Right, missing, y),
						 (x, y) => (Both, x, y));

		result.AssertSequenceEqual(
			(Right, missing, foo),
			(Both, bar2, bar1),
			(Both, bar3, bar1),
			(Right, missing, qux));
	}

	[Fact]
	public void RightJoinWithComparerResults()
	{
		var foo = ("one", "foo");
		var bar1 = ("two", "bar");
		var bar2 = ("Two", "bar");
		var bar3 = ("TWO", "bar");
		var baz = ("three", "baz");
		var qux = ("four", "qux");

		var xs = new[] { bar2, baz, bar3 };
		var ys = new[] { foo, bar1, qux };

		var missing = default((string, string));

		var result =
			xs.RightJoin(ys,
						 x => x.Item1,
						 y => y.Item1,
						 y => (Right, missing, y),
						 (x, y) => (Both, x, y),
						 StringComparer.OrdinalIgnoreCase);

		result.AssertSequenceEqual(
			(Right, missing, foo),
			(Both, bar2, bar1),
			(Both, bar3, bar1),
			(Right, missing, qux));
	}

	[Fact]
	public void RightJoinEmptyLeft()
	{
		var foo = (1, "foo");
		var bar = (2, "bar");
		var baz = (3, "baz");

		var xs = Array.Empty<(int, string)>();
		var ys = new[] { foo, bar, baz };

		var missing = default((int, string));

		var result =
			xs.RightJoin(ys,
						 x => x.Item1,
						 y => y.Item1,
						 y => (Right, missing, y),
						 (x, y) => (Both, x, y));

		result.AssertSequenceEqual(
			(Right, missing, foo),
			(Right, missing, bar),
			(Right, missing, baz));
	}

	[Fact]
	public void RightJoinEmptyRight()
	{
		var foo = (1, "foo");
		var bar = (2, "bar");
		var baz = (3, "baz");

		var xs = new[] { foo, bar, baz };
		var ys = Array.Empty<(int, string)>();

		var missing = default((int, string));

		var result =
			xs.RightJoin(ys,
						x => x.Item1,
						y => y.Item1,
						y => (Right, missing, y),
						(x, y) => (Both, x, y));

		Assert.Empty(result);
	}
}

#pragma warning restore CS0618 // Type or member is obsolete

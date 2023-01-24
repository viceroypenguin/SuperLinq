using static Test.FullOuterJoinTest.Side;

namespace Test;

#pragma warning disable CS0618 // Type or member is obsolete

public class LeftJoinTest
{
	[Fact]
	public void LeftJoinWithHomogeneousSequencesIsLazy()
	{
		var xs = new BreakingSequence<int>();
		var ys = new BreakingSequence<int>();

		xs.LeftJoin(ys, SuperEnumerable.Identity,
			BreakingFunc.Of<int, object>(),
			BreakingFunc.Of<int, int, object>());
	}

	[Fact]
	public void LeftJoinWithHomogeneousSequencesWithComparerIsLazy()
	{
		var xs = new BreakingSequence<int>();
		var ys = new BreakingSequence<int>();

		xs.LeftJoin(ys, SuperEnumerable.Identity,
			BreakingFunc.Of<int, object>(),
			BreakingFunc.Of<int, int, object>(),
			comparer: null);
	}

	[Fact]
	public void LeftJoinIsLazy()
	{
		var xs = new BreakingSequence<int>();
		var ys = new BreakingSequence<object>();

		xs.LeftJoin(ys, SuperEnumerable.Identity, y => y.GetHashCode(),
			BreakingFunc.Of<int, object>(),
			BreakingFunc.Of<int, object, object>());
	}

	[Fact]
	public void LeftJoinWithComparerIsLazy()
	{
		var xs = new BreakingSequence<int>();
		var ys = new BreakingSequence<object>();

		xs.LeftJoin(ys, SuperEnumerable.Identity, y => y.GetHashCode(),
			BreakingFunc.Of<int, object>(),
			BreakingFunc.Of<int, object, object>(),
			comparer: null);
	}

	[Fact]
	public void LeftJoinResults()
	{
		var foo = (1, "foo");
		var bar1 = (2, "bar");
		var bar2 = (2, "Bar");
		var bar3 = (2, "BAR");
		var baz = (3, "baz");
		var qux = (4, "qux");

		var xs = new[] { foo, bar1, qux };
		var ys = new[] { bar2, baz, bar3 };

		var missing = default((int, string));

		var result =
			xs.LeftJoin(ys,
						x => x.Item1,
						y => y.Item1,
						x => (Left, x, missing),
						(x, y) => (Both, x, y));

		result.AssertSequenceEqual(
			(Left, foo, missing),
			(Both, bar1, bar2),
			(Both, bar1, bar3),
			(Left, qux, missing));
	}

	[Fact]
	public void LeftJoinWithComparerResults()
	{
		var foo = ("one", "foo");
		var bar1 = ("two", "bar");
		var bar2 = ("Two", "bar");
		var bar3 = ("TWO", "bar");
		var baz = ("three", "baz");
		var qux = ("four", "qux");

		var xs = new[] { foo, bar1, qux };
		var ys = new[] { bar2, baz, bar3 };

		var missing = default((string, string));

		var result =
			xs.LeftJoin(ys,
						x => x.Item1,
						y => y.Item1,
						x => (Left, x, missing),
						(x, y) => (Both, x, y),
						StringComparer.OrdinalIgnoreCase);

		result.AssertSequenceEqual(
			(Left, foo, missing),
			(Both, bar1, bar2),
			(Both, bar1, bar3),
			(Left, qux, missing));
	}

	[Fact]
	public void LeftJoinEmptyLeft()
	{
		var foo = (1, "foo");
		var bar = (2, "bar");
		var baz = (3, "baz");

		var xs = Array.Empty<(int, string)>();
		var ys = new[] { foo, bar, baz };

		var missing = default((int, string));

		var result =
			xs.LeftJoin(ys,
						x => x.Item1,
						y => y.Item1,
						x => (Left, x, missing),
						(x, y) => (Both, x, y));

		Assert.Empty(result);
	}

	[Fact]
	public void LeftJoinEmptyRight()
	{
		var foo = (1, "foo");
		var bar = (2, "bar");
		var baz = (3, "baz");

		var xs = new[] { foo, bar, baz };
		var ys = Array.Empty<(int, string)>();

		var missing = default((int, string));

		var result =
			xs.LeftJoin(ys,
						x => x.Item1,
						y => y.Item1,
						x => (Left, x, missing),
						(x, y) => (Both, x, y));

		result.AssertSequenceEqual(
			(Left, foo, missing),
			(Left, bar, missing),
			(Left, baz, missing));
	}
}

#pragma warning restore CS0618 // Type or member is obsolete

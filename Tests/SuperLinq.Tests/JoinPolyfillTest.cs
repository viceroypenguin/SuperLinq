namespace SuperLinq.Tests;

[Obsolete("Tests join methods that are obsolete on .NET 10 and later.")]
public sealed class JoinPolyfillTest
{
	private static readonly (string Key, int Value)[] s_outer =
	[
		("a", 1),
		("B", 2),
	];

	private static readonly (string Key, int Value)[] s_inner =
	[
		("A", 3),
		("c", 4),
	];

	[Fact]
	public void LeftJoinUsesDefaultForMissingInner()
	{
		var result = SuperEnumerable.LeftJoin(
			s_outer,
			s_inner,
			outer => outer.Key,
			inner => inner.Key,
			(outer, inner) => (outer.Value, inner.Value),
			StringComparer.OrdinalIgnoreCase);

		result.AssertSequenceEqual((1, 3), (2, 0));
	}

	[Fact]
	public void RightJoinUsesDefaultForMissingOuter()
	{
		var result = SuperEnumerable.RightJoin(
			s_outer,
			s_inner,
			outer => outer.Key,
			inner => inner.Key,
			(outer, inner) => (outer.Value, inner.Value),
			StringComparer.OrdinalIgnoreCase);

		result.AssertSequenceEqual((1, 3), (0, 4));
	}

	[Fact]
	public void FullJoinReturnsMatchedAndUnmatchedElements()
	{
		var result = SuperEnumerable.FullJoin(
			s_outer,
			s_inner,
			outer => outer.Key,
			inner => inner.Key,
			StringComparer.OrdinalIgnoreCase);

		result.AssertSequenceEqual(
			(s_outer[0], s_inner[0]),
			(s_outer[1], default),
			(default, s_inner[1]));
	}

	[Fact]
	public void JoinPolyfillsAreLazy()
	{
		var outer = new BreakingSequence<int>();
		var inner = new BreakingSequence<int>();

		_ = SuperEnumerable.LeftJoin(outer, inner, x => x, x => x, ValueTuple.Create);
		_ = SuperEnumerable.RightJoin(outer, inner, x => x, x => x, ValueTuple.Create);
		_ = SuperEnumerable.FullJoin(outer, inner, x => x, x => x, ValueTuple.Create);
	}
}

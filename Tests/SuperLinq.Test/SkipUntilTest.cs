namespace Test;

public sealed class SkipUntilTest
{
	[Fact]
	public void SkipUntilPredicateNeverFalse()
	{
		using var sequence = Enumerable.Range(0, 5)
			.AsTestingSequence();
		sequence
			.SkipUntil(x => x != 100)
			.AssertSequenceEqual(1, 2, 3, 4);
	}

	[Fact]
	public void SkipUntilPredicateNeverTrue()
	{
		using var sequence = Enumerable.Range(0, 5)
			.AsTestingSequence();
		Assert.Empty(sequence.SkipUntil(x => x == 100));
	}

	[Fact]
	public void SkipUntilPredicateBecomesTrueHalfWay()
	{
		using var sequence = Enumerable.Range(0, 5)
			.AsTestingSequence();
		sequence
			.SkipUntil(x => x == 2)
			.AssertSequenceEqual(3, 4);
	}

	[Fact]
	public void SkipUntilEvaluatesSourceLazily()
	{
		_ = new BreakingSequence<string>().SkipUntil(x => x.Length == 0);
	}

	[Fact]
	public void SkipUntilEvaluatesPredicateLazily()
	{
		using var sequence = Enumerable.Range(-2, 5)
			.AsTestingSequence();
		// Predicate would explode at x == 0, but we never need to evaluate it as we've
		// started returning items after -1.
		sequence
			.SkipUntil(x => 1 / x == -1)
			.AssertSequenceEqual(0, 1, 2);
	}

	public static readonly IEnumerable<object[]> TestData =
		[
			[Array.Empty<int>(), 0, Array.Empty<int>()], // empty sequence
			[new[] { 0 }, 0, Array.Empty<int>()], // one-item sequence, predicate succeed
			[new[] { 0 }, 1, Array.Empty<int>()], // one-item sequence, predicate don't succeed
			[new[] { 1, 2, 3 }, 0, new[] { 2, 3 }], // predicate succeed on first item
			[new[] { 1, 2, 3 }, 1, new[] { 2, 3 }],
			[new[] { 1, 2, 3 }, 2, new[] { 3 }],
			[new[] { 1, 2, 3 }, 3, Array.Empty<int>()], // predicate succeed on last item
			[new[] { 1, 2, 3 }, 4, Array.Empty<int>()], // predicate never succeed
		];

	[Theory, MemberData(nameof(TestData))]
	public void TestSkipUntil(int[] source, int min, int[] expected)
	{
		using var xs = source.AsTestingSequence();
		Assert.Equal(expected, xs.SkipUntil(v => v >= min).ToArray());
	}
}

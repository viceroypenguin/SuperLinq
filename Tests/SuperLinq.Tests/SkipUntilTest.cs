namespace SuperLinq.Tests;

public sealed class SkipUntilTest
{
	[Test]
	public void SkipUntilPredicateNeverFalse()
	{
		using var sequence = Enumerable.Range(0, 5)
			.AsTestingSequence();
		sequence
			.SkipUntil(x => x != 100)
			.AssertSequenceEqual(1, 2, 3, 4);
	}

	[Test]
	public void SkipUntilPredicateNeverTrue()
	{
		using var sequence = Enumerable.Range(0, 5)
			.AsTestingSequence();
		Assert.Empty(sequence.SkipUntil(x => x == 100));
	}

	[Test]
	public void SkipUntilPredicateBecomesTrueHalfWay()
	{
		using var sequence = Enumerable.Range(0, 5)
			.AsTestingSequence();
		sequence
			.SkipUntil(x => x == 2)
			.AssertSequenceEqual(3, 4);
	}

	[Test]
	public void SkipUntilEvaluatesSourceLazily()
	{
		_ = new BreakingSequence<string>().SkipUntil(x => x.Length == 0);
	}

	[Test]
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

	public static IEnumerable<(int[] source, int min, int[] expected)> TestData() =>
		[
			([], 0, []),
			([0], 0, []),
			([0], 1, []),
			([1, 2, 3], 0, [2, 3]),
			([1, 2, 3], 1, [2, 3]),
			([1, 2, 3], 2, [3]),
			([1, 2, 3], 3, []),
			([1, 2, 3], 4, []),
		];

	[Test]
	[MethodDataSource(nameof(TestData))]
	public void TestSkipUntil(int[] source, int min, int[] expected)
	{
		using var xs = source.AsTestingSequence();
		Assert.Equal(expected, xs.SkipUntil(v => v >= min).ToArray());
	}
}

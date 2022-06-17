namespace Test;

public class SkipUntilTest
{
	[Fact]
	public void SkipUntilPredicateNeverFalse()
	{
		var sequence = Enumerable.Range(0, 5).SkipUntil(x => x != 100);
		sequence.AssertSequenceEqual(1, 2, 3, 4);
	}

	[Fact]
	public void SkipUntilPredicateNeverTrue()
	{
		var sequence = Enumerable.Range(0, 5).SkipUntil(x => x == 100);
		Assert.Empty(sequence);
	}

	[Fact]
	public void SkipUntilPredicateBecomesTrueHalfWay()
	{
		var sequence = Enumerable.Range(0, 5).SkipUntil(x => x == 2);
		sequence.AssertSequenceEqual(3, 4);
	}

	[Fact]
	public void SkipUntilEvaluatesSourceLazily()
	{
		new BreakingSequence<string>().SkipUntil(x => x.Length == 0);
	}

	[Fact]
	public void SkipUntilEvaluatesPredicateLazily()
	{
		// Predicate would explode at x == 0, but we never need to evaluate it as we've
		// started returning items after -1.
		var sequence = Enumerable.Range(-2, 5).SkipUntil(x => 1 / x == -1);
		sequence.AssertSequenceEqual(0, 1, 2);
	}

	public static readonly IEnumerable<object[]> TestData =
		new[]
		{
			new object[] { Array.Empty<int>(), 0, Array.Empty<int>() }, // empty sequence
            new object[] { new[] { 0       } , 0, Array.Empty<int>() }, // one-item sequence, predicate succeed
            new object[] { new[] { 0       } , 1, Array.Empty<int>() }, // one-item sequence, predicate don't succeed
            new object[] { new[] { 1, 2, 3 } , 0, new[] { 2, 3 }     }, // predicate succeed on first item
            new object[] { new[] { 1, 2, 3 } , 1, new[] { 2, 3 }     },
			new object[] { new[] { 1, 2, 3 } , 2, new[] { 3    }     },
			new object[] { new[] { 1, 2, 3 } , 3, Array.Empty<int>() }, // predicate succeed on last item
            new object[] { new[] { 1, 2, 3 } , 4, Array.Empty<int>() }, // predicate never succeed
		};

	[Theory, MemberData(nameof(TestData))]
	public void TestSkipUntil(int[] source, int min, int[] expected)
	{
		Assert.Equal(expected, source.AsTestingSequence().SkipUntil(v => v >= min).ToArray());
	}
}

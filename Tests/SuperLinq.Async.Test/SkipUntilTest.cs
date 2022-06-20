namespace Test.Async;

public class SkipUntilTest
{
	[Fact]
	public ValueTask SkipUntilPredicateNeverFalse()
	{
		var sequence = AsyncEnumerable.Range(0, 5).SkipUntil(x => x != 100);
		return sequence.AssertSequenceEqual(1, 2, 3, 4);
	}

	[Fact]
	public async ValueTask SkipUntilPredicateNeverTrue()
	{
		var sequence = AsyncEnumerable.Range(0, 5).SkipUntil(x => x == 100);
		Assert.Empty(await sequence.ToListAsync());
	}

	[Fact]
	public ValueTask SkipUntilPredicateBecomesTrueHalfWay()
	{
		var sequence = AsyncEnumerable.Range(0, 5).SkipUntil(x => x == 2);
		return sequence.AssertSequenceEqual(3, 4);
	}

	[Fact]
	public void SkipUntilEvaluatesSourceLazily()
	{
		new AsyncBreakingSequence<string>().SkipUntil(x => x.Length == 0);
	}

	[Fact]
	public ValueTask SkipUntilEvaluatesPredicateLazily()
	{
		// Predicate would explode at x == 0, but we never need to evaluate it as we've
		// started returning items after -1.
		var sequence = AsyncEnumerable.Range(-2, 5).SkipUntil(x => 1 / x == -1);
		return sequence.AssertSequenceEqual(0, 1, 2);
	}

	[Fact]
	public async ValueTask SkipUntilDisposesEnumerator()
	{
		await using var seq1 = TestingSequence.Of<int>();
		await seq1.SkipUntil(x => true).ToListAsync();
	}

	public static readonly IEnumerable<object[]> TestData =
		new[]
		{
				new object[] { Array.Empty<int>(), 0, Array.Empty<int>()     }, // empty sequence
                new object[] { new[] { 0       } , 0, Array.Empty<int>()     }, // one-item sequence, predicate succeed
                new object[] { new[] { 0       } , 1, Array.Empty<int>()     }, // one-item sequence, predicate don't succeed
                new object[] { new[] { 1, 2, 3 } , 0, new[] { 2, 3 } },         // predicate succeed on first item
                new object[] { new[] { 1, 2, 3 } , 1, new[] { 2, 3 } },
				new object[] { new[] { 1, 2, 3 } , 2, new[] { 3    } },
				new object[] { new[] { 1, 2, 3 } , 3, Array.Empty<int>()     }, // predicate succeed on last item
                new object[] { new[] { 1, 2, 3 } , 4, Array.Empty<int>()     }, // predicate never succeed
		};

	[Theory, MemberData(nameof(TestData))]
	public async ValueTask TestSkipUntil(int[] source, int min, int[] expected)
	{
		Assert.Equal(expected, await source.ToAsyncEnumerable().SkipUntil(v => v >= min).ToListAsync());
	}
}

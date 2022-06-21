namespace Test.Async;

/// <summary>
/// Verify the behavior of the Segment operator
/// </summary>
public class SegmentTests
{
	/// <summary>
	/// Verify that the Segment operator behaves in a lazy manner
	/// </summary>
	[Fact]
	public void TestSegmentIsLazy()
	{
		new AsyncBreakingSequence<int>().Segment(BreakingFunc.Of<int, bool>());
		new AsyncBreakingSequence<int>().Segment(BreakingFunc.Of<int, int, bool>());
		new AsyncBreakingSequence<int>().Segment(BreakingFunc.Of<int, int, int, bool>());
	}

	/// <summary>
	/// Verify that segmenting a sequence into a single sequence results in the original sequence.
	/// </summary>
	[Fact]
	public async Task TestIdentitySegment()
	{
		var sequence = Enumerable.Range(1, 5);
		var result = sequence.ToAsyncEnumerable().Segment(x => false);

		(await result.SingleAsync())
			.AssertSequenceEqual(sequence);
	}

	/// <summary>
	/// Verify that segmenting an empty sequence results in an empty sequence of segments.
	/// </summary>
	[Fact]
	public async Task TestEmptySequence()
	{
		var sequence = AsyncEnumerable.Repeat(-1, 0);
		var result = await sequence.Segment(x => true).ToListAsync();
		Assert.Empty(result);
	}

	/// <summary>
	/// Verify that the segments returned can be enumerated more than once.
	/// </summary>
	[Fact]
	public async Task TestSegmentIsIdempotent()
	{
		var sequence = Enumerable.Repeat(-1, 10);
		var result = sequence.ToAsyncEnumerable().Segment(x => true);

		foreach (var segment in await result.ToListAsync())
		{
			Assert.True(segment.Any());
			Assert.Equal(-1, segment.Single());
		}
	}

	/// <summary>
	/// Verify that the first segment is never empty. By definition, segmentation
	/// begins with the second element in the source sequence.
	/// </summary>
	[Fact]
	public async Task TestFirstSegmentNeverEmpty()
	{
		var sequence = Enumerable.Repeat(-1, 10);
		var resultA = sequence.ToAsyncEnumerable().Segment(x => true);
		var resultB = sequence.ToAsyncEnumerable().Segment((x, index) => true);
		var resultC = sequence.ToAsyncEnumerable().Segment((x, prevX, index) => true);

		Assert.NotEmpty(await resultA.FirstAsync());
		Assert.NotEmpty(await resultB.FirstAsync());
		Assert.NotEmpty(await resultC.FirstAsync());
	}

	/// <summary>
	/// Verify invariant that segmentation begins with second element of source sequence.
	/// </summary>
	[Fact]
	public async Task TestSegmentationStartsWithSecondItem()
	{
		var resultA = AsyncSeq(0).Segment(BreakingFunc.Of<int, bool>());
		var resultB = AsyncSeq(0).Segment(BreakingFunc.Of<int, int, bool>());
		var resultC = AsyncSeq(0).Segment(BreakingFunc.Of<int, int, int, bool>());

		Assert.NotEmpty(await resultA.FirstAsync());
		Assert.NotEmpty(await resultB.FirstAsync());
		Assert.NotEmpty(await resultC.FirstAsync());
	}

	/// <summary>
	/// Verify we can segment a source sequence by it's zero-based index
	/// </summary>
	[Fact]
	public async Task VerifyCanSegmentByIndex()
	{
		const int Count = 100;
		const int SegmentSize = 2;

		var sequence = Enumerable.Repeat(1, Count);
		var result = sequence.ToAsyncEnumerable()
			.Segment((x, i) => i % SegmentSize == 0);

		Assert.Equal(Count / SegmentSize, await result.CountAsync());
		foreach (var segment in await result.ToListAsync())
		{
			Assert.Equal(SegmentSize, segment.Count());
		}
	}

	/// <summary>
	/// Verify that we can segment a source sequence by the change in adjacent items
	/// </summary>
	[Fact]
	public async Task VerifyCanSegmentByPrevious()
	{
		var sequence = Enumerable.Range(1, 3)
								 .SelectMany(x => Enumerable.Repeat(x, 5));
		var result = sequence.ToAsyncEnumerable()
			.Segment((curr, prev, i) => curr != prev);

		Assert.Equal(sequence.Distinct().Count(), await result.CountAsync());
		Assert.True(await result.AllAsync(s => s.Count() == 5));
	}

	public static readonly IEnumerable<object[]> TestData =
		from e in new[]
		{
            // input sequence is empty
            new { Source = AsyncSeq<int>(),            Expected = AsyncSeq<IEnumerable<int>>()         },
            // input sequence contains only new segment start
            new { Source = AsyncSeq(0, 3, 6),          Expected = AsyncSeq(Seq(0), Seq(3), Seq(6))     },
            // input sequence do not contains new segment start
            new { Source = AsyncSeq(1, 2, 4, 5),       Expected = AsyncSeq(Seq(1, 2, 4, 5))            },
            // input sequence start with a segment start
            new { Source = AsyncSeq(0, 1, 2, 3, 4, 5), Expected = AsyncSeq(Seq(0, 1, 2), Seq(3, 4, 5)) },
            // input sequence do not start with a segment start
            new { Source = AsyncSeq(1, 2, 3, 4, 5),    Expected = AsyncSeq(Seq(1, 2), Seq(3, 4, 5))    },
		}
		select new object[] { e.Source, e.Expected, };

	[Theory]
	[MemberData(nameof(TestData))]
	public async Task TestSegment(IEnumerable<int> source, IAsyncEnumerable<IEnumerable<int>> expected)
	{
		await foreach (var (first, second) in
			source.AsTestingSequence()
				.Segment(v => v % 3 == 0)
				.Zip(expected))
		{
			Assert.Equal(second, first);
		}
	}
}

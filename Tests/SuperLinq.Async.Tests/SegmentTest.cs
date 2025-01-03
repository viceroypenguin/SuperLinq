namespace SuperLinq.Async.Tests;

/// <summary>
/// Verify the behavior of the Segment operator
/// </summary>
public sealed class SegmentTests
{
	/// <summary>
	/// Verify that the Segment operator behaves in a lazy manner
	/// </summary>
	[Test]
	public void TestSegmentIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().Segment(BreakingFunc.Of<int, bool>());
		_ = new AsyncBreakingSequence<int>().Segment(BreakingFunc.Of<int, int, bool>());
		_ = new AsyncBreakingSequence<int>().Segment(BreakingFunc.Of<int, int, int, bool>());
	}

	/// <summary>
	/// Verify that segmenting a sequence into a single sequence results in the original sequence.
	/// </summary>
	[Test]
	public async Task TestIdentitySegment()
	{
		await using var sequence = Enumerable.Range(1, 5).AsTestingSequence();

		var result = sequence.Segment(x => false);
		(await result.SingleAsync())
			.AssertSequenceEqual(Enumerable.Range(1, 5));
	}

	/// <summary>
	/// Verify that segmenting an empty sequence results in an empty sequence of segments.
	/// </summary>
	[Test]
	public async Task TestEmptySequence()
	{
		await using var sequence = AsyncEnumerable.Repeat(-1, 0).AsTestingSequence();
		var result = await sequence.Segment(x => true).ToListAsync();
		Assert.Empty(result);
	}

	/// <summary>
	/// Verify that the segments returned can be enumerated more than once.
	/// </summary>
	[Test]
	public async Task TestSegmentIsIdempotent()
	{
		await using var sequence = Enumerable.Repeat(-1, 10).AsTestingSequence();

		var result = sequence.Segment(x => true);
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
	[Test]
	public async Task TestFirstSegmentNeverEmpty()
	{
		await using (var sequence = Enumerable.Repeat(-1, 10).AsTestingSequence())
			Assert.NotEmpty(await sequence.Segment(x => true).FirstAsync());

		await using (var sequence = Enumerable.Repeat(-1, 10).AsTestingSequence())
			Assert.NotEmpty(await sequence.Segment((x, index) => true).FirstAsync());

		await using (var sequence = Enumerable.Repeat(-1, 10).AsTestingSequence())
			Assert.NotEmpty(await sequence.Segment((x, prevX, index) => true).FirstAsync());
	}

	/// <summary>
	/// Verify invariant that segmentation begins with second element of source sequence.
	/// </summary>
	[Test]
	public async Task TestSegmentationStartsWithSecondItem()
	{
		await using (var sequence = TestingSequence.Of(0))
			Assert.NotEmpty(await sequence.Segment(BreakingFunc.Of<int, bool>()).FirstAsync());

		await using (var sequence = TestingSequence.Of(0))
			Assert.NotEmpty(await sequence.Segment(BreakingFunc.Of<int, int, bool>()).FirstAsync());

		await using (var sequence = TestingSequence.Of(0))
			Assert.NotEmpty(await sequence.Segment(BreakingFunc.Of<int, int, int, bool>()).FirstAsync());
	}

	/// <summary>
	/// Verify we can segment a source sequence by it's zero-based index
	/// </summary>
	[Test]
	public async Task VerifyCanSegmentByIndex()
	{
		await using var sequence = Enumerable.Repeat(1, 100)
			.AsTestingSequence();
		var result = await sequence
			.Segment((x, i) => i % 2 == 0)
			.ToListAsync();

		Assert.Equal(100 / 2, result.Count);
		Assert.True(result.All(s => s.Count == 2));
	}

	/// <summary>
	/// Verify that we can segment a source sequence by the change in adjacent items
	/// </summary>
	[Test]
	public async Task VerifyCanSegmentByPrevious()
	{
		var sequence = Enumerable.Range(1, 3)
			.SelectMany(x => Enumerable.Repeat(x, 5));

		await using var xs = sequence.AsTestingSequence();
		var result = await xs
			.Segment((curr, prev, i) => curr != prev)
			.ToListAsync();

		Assert.Equal(sequence.Distinct().Count(), result.Count);
		Assert.True(result.All(s => s.Count == 5));
	}

	public static IEnumerable<(IEnumerable<int> source, IAsyncEnumerable<IEnumerable<int>> expected)> TestData() =>
		[
			([],                 AsyncSeq<IEnumerable<int>>()        ),
			([0, 3, 6],          AsyncSeq(Seq(0), Seq(3), Seq(6))    ),
			([1, 2, 4, 5],       AsyncSeq(Seq(1, 2, 4, 5))           ),
			([0, 1, 2, 3, 4, 5], AsyncSeq(Seq(0, 1, 2), Seq(3, 4, 5))),
			([1, 2, 3, 4, 5],    AsyncSeq(Seq(1, 2), Seq(3, 4, 5))   ),
		];

	[Test]
	[MethodDataSource(nameof(TestData))]
	public async Task TestSegment(IEnumerable<int> source, IAsyncEnumerable<IEnumerable<int>> expected)
	{
		await using var xs = source.AsTestingSequence();
		await foreach (var (first, second) in xs
			.Segment(v => v % 3 == 0)
			.Zip(expected))
		{
			Assert.Equal(second, first);
		}
	}
}

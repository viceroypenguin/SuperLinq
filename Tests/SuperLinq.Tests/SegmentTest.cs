namespace SuperLinq.Tests;

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
		_ = new BreakingSequence<int>().Segment(BreakingFunc.Of<int, bool>());
		_ = new BreakingSequence<int>().Segment(BreakingFunc.Of<int, int, bool>());
		_ = new BreakingSequence<int>().Segment(BreakingFunc.Of<int, int, int, bool>());
	}

	/// <summary>
	/// Verify that segmenting a sequence into a single sequence results in the original sequence.
	/// </summary>
	[Test]
	public void TestIdentitySegment()
	{
		using var sequence = Enumerable.Range(1, 5)
			.AsTestingSequence();
		sequence.Segment(x => false).Single()
			.AssertSequenceEqual(Enumerable.Range(1, 5));
	}

	/// <summary>
	/// Verify that segmenting an empty sequence results in an empty sequence of segments.
	/// </summary>
	[Test]
	public void TestEmptySequence()
	{
		using var sequence = Array.Empty<int>().AsTestingSequence();
		Assert.Empty(sequence.Segment(x => true));
	}

	/// <summary>
	/// Verify that the segments returned can be enumerated more than once.
	/// </summary>
	[Test]
	public void TestSegmentIsIdempotent()
	{
		using var sequence = Enumerable.Repeat(-1, 10)
			.AsTestingSequence();

		var result = sequence.Segment(x => true);
		foreach (var segment in result)
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
	public void TestFirstSegmentNeverEmpty()
	{
		using (var sequence = Enumerable.Repeat(-1, 10).AsTestingSequence())
			Assert.True(sequence.Segment(x => true).First().Any());

		using (var sequence = Enumerable.Repeat(-1, 10).AsTestingSequence())
			Assert.True(sequence.Segment((x, index) => true).First().Any());

		using (var sequence = Enumerable.Repeat(-1, 10).AsTestingSequence())
			Assert.True(sequence.Segment((x, prevX, index) => true).First().Any());
	}

	/// <summary>
	/// Verify invariant that segmentation begins with second element of source sequence.
	/// </summary>
	[Test]
	public void TestSegmentationStartsWithSecondItem()
	{
		using (var sequence = TestingSequence.Of(0))
			Assert.True(sequence.Segment(BreakingFunc.Of<int, bool>()).Any());

		using (var sequence = TestingSequence.Of(0))
			Assert.True(sequence.Segment(BreakingFunc.Of<int, int, bool>()).Any());

		using (var sequence = TestingSequence.Of(0))
			Assert.True(sequence.Segment(BreakingFunc.Of<int, int, int, bool>()).Any());
	}

	/// <summary>
	/// Verify we can segment a source sequence by it's zero-based index
	/// </summary>
	[Test]
	public void VerifyCanSegmentByIndex()
	{

		using var sequence = Enumerable.Repeat(1, 100)
			.AsTestingSequence();

		var result = sequence.Segment((x, i) => i % 2 == 0).ToList();
		Assert.Equal(100 / 2, result.Count);
		Assert.True(result.All(s => s.Count == 2));
	}

	/// <summary>
	/// Verify that we can segment a source sequence by the change in adjacent items
	/// </summary>
	[Test]
	public void VerifyCanSegmentByPrevious()
	{
		var sequence = Enumerable.Range(1, 3)
			.SelectMany(x => Enumerable.Repeat(x, 5));

		using var xs = sequence.AsTestingSequence();
		var result = xs
			.Segment((curr, prev, i) => curr != prev)
			.ToList();

		Assert.Equal(sequence.Distinct().Count(), result.Count);
		Assert.True(result.All(s => s.Count == 5));
	}

	public static IEnumerable<(IEnumerable<int> source, IEnumerable<IEnumerable<int>> expectedSegments)> TestData() =>
		[
			([],                 [[]]                  ),
			([0, 3, 6],          [[0], [3], [6]]       ),
			([1, 2, 4, 5],       [[1, 2, 4, 5]]        ),
			([0, 1, 2, 3, 4, 5], [[0, 1, 2], [3, 4, 5]]),
			([1, 2, 3, 4, 5],    [[1, 2], [3, 4, 5]]   ),
		];

	[Test]
	[MethodDataSource(nameof(TestData))]
	public void TestSegment(IEnumerable<int> source, IEnumerable<IEnumerable<int>> expectedSegments)
	{
		using var sequence = source.AsTestingSequence();

		var result = sequence.Segment(v => v % 3 == 0);
		foreach (var (actual, expected) in result.Zip(expectedSegments))
			actual.AssertSequenceEqual(expected);
	}
}

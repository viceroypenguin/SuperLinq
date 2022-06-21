namespace Test;

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
		new BreakingSequence<int>().Segment(BreakingFunc.Of<int, bool>());
		new BreakingSequence<int>().Segment(BreakingFunc.Of<int, int, bool>());
		new BreakingSequence<int>().Segment(BreakingFunc.Of<int, int, int, bool>());
	}

	/// <summary>
	/// Verify that segmenting a sequence into a single sequence results in the original sequence.
	/// </summary>
	[Fact]
	public void TestIdentitySegment()
	{
		const int count = 5;
		var sequence = Enumerable.Range(1, count);
		var result = sequence.Segment(x => false);

		Assert.Equal(sequence, result.Single());
	}

	/// <summary>
	/// Verify that segmenting an empty sequence results in an empty sequence of segments.
	/// </summary>
	[Fact]
	public void TestEmptySequence()
	{
		var sequence = Enumerable.Repeat(-1, 0);
		var result = sequence.Segment(x => true);
		Assert.Empty(result);
	}

	/// <summary>
	/// Verify that the segments returned can be enumerated more than once.
	/// </summary>
	[Fact]
	public void TestSegmentIsIdempotent()
	{
		const int value = -1;
		var sequence = Enumerable.Repeat(value, 10);
		var result = sequence.Segment(x => true);

		foreach (var segment in result)
		{
			Assert.True(segment.Any());
			Assert.Equal(value, segment.Single());
		}
	}

	/// <summary>
	/// Verify that the first segment is never empty. By definition, segmentation
	/// begins with the second element in the source sequence.
	/// </summary>
	[Fact]
	public void TestFirstSegmentNeverEmpty()
	{
		var sequence = Enumerable.Repeat(-1, 10);
		var resultA = sequence.Segment(x => true);
		var resultB = sequence.Segment((x, index) => true);
		var resultC = sequence.Segment((x, prevX, index) => true);

		Assert.True(resultA.First().Any());
		Assert.True(resultB.First().Any());
		Assert.True(resultC.First().Any());
	}

	/// <summary>
	/// Verify invariant that segmentation begins with second element of source sequence.
	/// </summary>
	[Fact]
	public void TestSegmentationStartsWithSecondItem()
	{
		var sequence = new[] { 0 };
		var resultA = sequence.Segment(BreakingFunc.Of<int, bool>());
		var resultB = sequence.Segment(BreakingFunc.Of<int, int, bool>());
		var resultC = sequence.Segment(BreakingFunc.Of<int, int, int, bool>());

		Assert.True(resultA.Any());
		Assert.True(resultB.Any());
		Assert.True(resultC.Any());
	}

	/// <summary>
	/// Verify we can segment a source sequence by it's zero-based index
	/// </summary>
	[Fact]
	public void VerifyCanSegmentByIndex()
	{
		const int count = 100;
		const int segmentSize = 2;

		var sequence = Enumerable.Repeat(1, count);
		var result = sequence.Segment((x, i) => i % segmentSize == 0);

		Assert.Equal(count / segmentSize, result.Count());
		foreach (var segment in result)
		{
			Assert.Equal(segmentSize, segment.Count());
		}
	}

	/// <summary>
	/// Verify that we can segment a source sequence by the change in adjacent items
	/// </summary>
	[Fact]
	public void VerifyCanSegmentByPrevious()
	{
		var sequence = Enumerable.Range(1, 3)
								 .SelectMany(x => Enumerable.Repeat(x, 5));
		var result = sequence.Segment((curr, prev, i) => curr != prev);

		Assert.Equal(sequence.Distinct().Count(), result.Count());
		Assert.True(result.All(s => s.Count() == 5));
	}

	static IEnumerable<T> Seq<T>(params T[] values) => values;

	public static readonly IEnumerable<object[]> TestData =
		from e in new[]
		{
            // input sequence is empty
            new { Source = Seq<int>(),            Expected = Seq<IEnumerable<int>>()         },
            // input sequence contains only new segment start
            new { Source = Seq(0, 3, 6),          Expected = Seq(Seq(0), Seq(3), Seq(6))     },
            // input sequence do not contains new segment start
            new { Source = Seq(1, 2, 4, 5),       Expected = Seq(Seq(1, 2, 4, 5))            },
            // input sequence start with a segment start
            new { Source = Seq(0, 1, 2, 3, 4, 5), Expected = Seq(Seq(0, 1, 2), Seq(3, 4, 5)) },
            // input sequence do not start with a segment start
            new { Source = Seq(1, 2, 3, 4, 5),    Expected = Seq(Seq(1, 2), Seq(3, 4, 5))    },
		}
		select new object[] { e.Source, e.Expected, };

	[Theory]
	[MemberData(nameof(TestData))]
	public void TestSegment(IEnumerable<int> source, IEnumerable<IEnumerable<int>> expected)
	{
		source.AsTestingSequence().Segment(v => v % 3 == 0)
			.Zip(expected)
			.ForEach(x => Assert.Equal(x.Second, x.First));
	}
}

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
		using var sequence = Enumerable.Range(1, 5)
			.AsTestingSequence();
		sequence.Segment(x => false).Single()
			.AssertSequenceEqual(Enumerable.Range(1, 5));
	}

	/// <summary>
	/// Verify that segmenting an empty sequence results in an empty sequence of segments.
	/// </summary>
	[Fact]
	public void TestEmptySequence()
	{
		using var sequence = Array.Empty<int>().AsTestingSequence();
		Assert.Empty(sequence.Segment(x => true));
	}

	/// <summary>
	/// Verify that the segments returned can be enumerated more than once.
	/// </summary>
	[Fact]
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
	[Fact]
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
	[Fact]
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
	[Fact]
	public void VerifyCanSegmentByIndex()
	{

		using var sequence = Enumerable.Repeat(1, 100)
			.AsTestingSequence();

		var result = sequence.Segment((x, i) => i % 2 == 0).ToList();
		Assert.Equal(100 / 2, result.Count);
		Assert.True(result.All(s => s.Count() == 2));
	}

	/// <summary>
	/// Verify that we can segment a source sequence by the change in adjacent items
	/// </summary>
	[Fact]
	public void VerifyCanSegmentByPrevious()
	{
		var sequence = Enumerable.Range(1, 3)
			.SelectMany(x => Enumerable.Repeat(x, 5));

		using var xs = sequence.AsTestingSequence();
		var result = xs
			.Segment((curr, prev, i) => curr != prev)
			.ToList();

		Assert.Equal(sequence.Distinct().Count(), result.Count);
		Assert.True(result.All(s => s.Count() == 5));
	}

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
		using var sequence = source.AsTestingSequence();
		sequence.Segment(v => v % 3 == 0)
			.Zip(expected)
			.ForEach(x => Assert.Equal(x.Second, x.First));
	}
}

namespace Test;

/// <summary>
/// Verify the behavior of the Exclude operator
/// </summary>
public class ExcludeTests
{
	/// <summary>
	/// Verify that Exclude behaves in a lazy manner
	/// </summary>
	[Fact]
	public void TestExcludeIsLazy()
	{
		_ = new BreakingSequence<int>().Exclude(0, 10);
	}

	/// <summary>
	/// Verify that a negative startIndex parameter results in an exception
	/// </summary>
	[Fact]
	public void TestExcludeNegativeStartIndexException()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().Exclude(-10, 10));
	}

	/// <summary>
	/// Verify that a negative count parameter results in an exception
	/// </summary>
	[Fact]
	public void TestExcludeNegativeCountException()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().Exclude(0, -5));
	}

	/// <summary>
	/// Verify that excluding with count equals zero returns the original source
	/// </summary>
	[Fact]
	public void TestExcludeWithCountEqualsZero()
	{
		using var sequence = Enumerable.Range(1, 10).AsTestingSequence();

		var result = sequence.Exclude(5, 0);
		result.AssertSequenceEqual(Enumerable.Range(1, 10));
	}

	/// <summary>
	/// Verify that excluding from an empty sequence results in an empty sequence
	/// </summary>
	[Fact]
	public void TestExcludeEmptySequence()
	{
		using var sequence = Enumerable.Empty<int>().AsTestingSequence(maxEnumerations: 3);

		Assert.Equal(Enumerable.Empty<int>(), sequence.Exclude(0, 0));
		// shouldn't matter how many we ask for past end
		Assert.Equal(Enumerable.Empty<int>(), sequence.Exclude(0, 10));
		// shouldn't matter where we start
		Assert.Equal(Enumerable.Empty<int>(), sequence.Exclude(5, 5));
	}

	/// <summary>
	/// Verify we can exclude the beginning portion of a sequence
	/// </summary>
	[Fact]
	public void TestExcludeSequenceHead()
	{
		using var sequence = Enumerable.Range(1, 10).AsTestingSequence();

		var result = sequence.Exclude(0, 5);
		result.AssertSequenceEqual(Enumerable.Range(6, 5));
	}

	/// <summary>
	/// Verify we can exclude the tail portion of a sequence
	/// </summary>
	[Fact]
	public void TestExcludeSequenceTail()
	{
		using var sequence = Enumerable.Range(1, 10).AsTestingSequence();

		var result = sequence.Exclude(5, 10);
		result.AssertSequenceEqual(Enumerable.Range(1, 5));
	}

	/// <summary>
	/// Verify we can exclude the middle portion of a sequence
	/// </summary>
	[Fact]
	public void TestExcludeSequenceMiddle()
	{
		using var sequence = Enumerable.Range(1, 10).AsTestingSequence();

		var result = sequence.Exclude(3, 5);
		Assert.Equal(Seq(1, 2, 3, 9, 10), result);
	}

	/// <summary>
	/// Verify that excluding the entire sequence results in an empty sequence
	/// </summary>
	[Fact]
	public void TestExcludeEntireSequence()
	{
		using var sequence = Enumerable.Range(1, 10).AsTestingSequence();

		var result = sequence.Exclude(0, 10);
		Assert.Empty(result);
	}

	/// <summary>
	/// Verify that excluding past the end on a sequence excludes the appropriate elements
	/// </summary>
	[Fact]
	public void TestExcludeCountGreaterThanSequenceLength()
	{
		using var sequence = Enumerable.Range(1, 10).AsTestingSequence();

		var result = sequence.Exclude(1, 10 * 10);
		Assert.Equal(Seq(1), result);
	}

	/// <summary>
	/// Verify that beginning exclusion past the end of a sequence has no effect
	/// </summary>
	[Fact]
	public void TestExcludeStartIndexGreaterThanSequenceLength()
	{
		using var sequence = Enumerable.Range(1, 10).AsTestingSequence();

		var result = sequence.Exclude(10 + 5, 10);
		Assert.Equal(Enumerable.Range(1, 10), result);
	}
}

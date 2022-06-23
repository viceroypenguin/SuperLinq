namespace Test.Async;

/// <summary>
/// Verify the behavior of the RunLengthEncode() operator
/// </summary>
public class RunLengthEncodeTests
{
	/// <summary>
	/// Verify that the RunLengthEncode() methods behave in a lazy manner.
	/// </summary>
	[Fact]
	public void TestRunLengthEncodeIsLazy()
	{
		new AsyncBreakingSequence<int>().RunLengthEncode();
		new AsyncBreakingSequence<int>().RunLengthEncode(EqualityComparer<int>.Default);
	}

	/// <summary>
	/// Verify that run-length encoding an empty sequence results in an empty sequence.
	/// </summary>
	[Fact]
	public async Task TestRunLengthEncodeEmptySequence()
	{
		var sequence = AsyncEnumerable.Empty<int>();
		var result = sequence.RunLengthEncode();

		Assert.Empty(await result.ToListAsync());
	}

	/// <summary>
	/// Verify that run-length encoding correctly accepts and uses custom equality comparers.
	/// </summary>
	[Fact]
	public Task TestRunLengthEncodeCustomComparer()
	{
		var sequence = AsyncSeq("a", "A", "a", "b", "b", "B", "B");

		var result = sequence
			.RunLengthEncode(StringComparer.InvariantCultureIgnoreCase)
			.Select(kvp => (kvp.value.ToLowerInvariant(), kvp.count));

		var expectedResult = new[]
		{
			("a", 3),
			("b", 4),
		};

		return result.AssertSequenceEqual(expectedResult);
	}

	/// <summary>
	/// Verify that run-length encoding a known sequence produced a correct result.
	/// </summary>
	[Fact]
	public Task TestRunLengthEncodeResults()
	{
		var sequence = AsyncSeq(1, 2, 2, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6);
		var result = sequence.RunLengthEncode();

		var expectedResult = Enumerable.Range(1, 6).Select(x => (x, x));
		return result.AssertSequenceEqual(expectedResult);
	}

	/// <summary>
	/// Verify that run-length encoding a sequence with no runs produces a correct result.
	/// </summary>
	[Fact]
	public Task TestRunLengthEncodeNoRuns()
	{
		var sequence = AsyncEnumerable.Range(1, 10);
		var result = sequence.RunLengthEncode();

		var expectedResult = Enumerable.Range(1, 10).Select(x => (x, 1));
		return result.AssertSequenceEqual(expectedResult);
	}

	/// <summary>
	/// Verify that run-length encoding a sequence consisting of a single repeated value
	/// produces a correct result.
	/// </summary>
	[Fact]
	public Task TestRunLengthEncodeOneRun()
	{
		var sequence = AsyncEnumerable.Repeat('q', 10);
		var result = sequence.RunLengthEncode();

		var expectedResult = new[] { (Value: 'q', repeatCount: 10) };
		return result.AssertSequenceEqual(expectedResult);
	}
}

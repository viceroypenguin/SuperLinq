﻿namespace Test;

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
		new BreakingSequence<int>().RunLengthEncode();
		new BreakingSequence<int>().RunLengthEncode(EqualityComparer<int>.Default);
	}

	/// <summary>
	/// Verify that run-length encoding an empty sequence results in an empty sequence.
	/// </summary>
	[Fact]
	public void TestRunLengthEncodeEmptySequence()
	{
		using var sequence = TestingSequence.Of<int>();

		var result = sequence.RunLengthEncode();
		result.AssertSequenceEqual();
	}

	/// <summary>
	/// Verify that run-length encoding correctly accepts and uses custom equality comparers.
	/// </summary>
	[Fact]
	public void TestRunLengthEncodeCustomComparer()
	{
		using var sequence = TestingSequence.Of("a", "A", "a", "b", "b", "B", "B");

		var result = sequence
			.RunLengthEncode(StringComparer.InvariantCultureIgnoreCase);

		result
			.Select(kvp => (kvp.value.ToLowerInvariant(), kvp.count))
			.AssertSequenceEqual(
				("a", 3),
				("b", 4));
	}

	/// <summary>
	/// Verify that run-length encoding a known sequence produced a correct result.
	/// </summary>
	[Fact]
	public void TestRunLengthEncodeResults()
	{
		using var sequence = TestingSequence.Of(1, 2, 2, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6);

		var result = sequence.RunLengthEncode();
		result.AssertSequenceEqual(
			Enumerable.Range(1, 6).Select(x => (x, x)));
	}

	/// <summary>
	/// Verify that run-length encoding a sequence with no runs produces a correct result.
	/// </summary>
	[Fact]
	public void TestRunLengthEncodeNoRuns()
	{
		using var sequence = Enumerable.Range(1, 10).AsTestingSequence();

		var result = sequence.RunLengthEncode();
		result.AssertSequenceEqual(
			Enumerable.Range(1, 10).Select(x => (x, 1)));
	}

	/// <summary>
	/// Verify that run-length encoding a sequence consisting of a single repeated value
	/// produces a correct result.
	/// </summary>
	[Fact]
	public void TestRunLengthEncodeOneRun()
	{
		using var sequence = Enumerable.Repeat('q', 10).AsTestingSequence();

		var result = sequence.RunLengthEncode();
		result.AssertSequenceEqual(('q', 10));
	}
}

using System.Diagnostics.CodeAnalysis;

namespace SuperLinq.Tests;

public sealed class SplitTest
{
	[Test]
	public void SplitIsLazy()
	{
		_ = new BreakingSequence<int>().Split(1);
		_ = new BreakingSequence<int>().Split(1, 2);
	}

	[Test]
	public void SplitWithComparer()
	{
		using var sequence = Enumerable.Range(1, 10).AsTestingSequence();
		var result = sequence.Split(2, EqualityComparer.Create<int>((x, y) => x % 2 == y % 2));
		result.AssertSequenceEqual(Enumerable.Range(1, 5).Select(x => new[] { (x * 2) - 1 }));
	}

	[Test]
	public void SplitWithComparerUptoMaxCount()
	{
		using var sequence = Enumerable.Range(1, 10).AsTestingSequence();
		var result = sequence.Split(2, EqualityComparer.Create<int>((x, y) => x % 2 == y % 2), 2).ToList();
		result.AssertSequenceEqual([1], [3], Enumerable.Range(5, 6));
	}

	[Test]
	[SuppressMessage("Style", "IDE0305:Simplify collection initialization")]
	public void SplitWithSeparatorAndResultTransformation()
	{
		using var sequence = "the quick brown fox".AsTestingSequence();
		var result = sequence.Split(' ', chars => new string(chars.ToArray()));
		result.AssertSequenceEqual("the", "quick", "brown", "fox");
	}

	[Test]
	[SuppressMessage("Style", "IDE0305:Simplify collection initialization")]
	public void SplitUptoMaxCount()
	{
		using var sequence = "the quick brown fox".AsTestingSequence();
		var result = sequence.Split(' ', 2, chars => new string(chars.ToArray()));
		result.AssertSequenceEqual("the", "quick", "brown fox");
	}

	[Test]
	public void SplitWithSeparatorSelector()
	{
		using var sequence = TestingSequence.Of<int?>(1, 2, null, 3, null, 4, 5, 6);
		var result = sequence.Split(n => n is null);

		using var reader = result.Read();
		reader.Read().AssertSequenceEqual(1, 2);
		reader.Read().AssertSequenceEqual(3);
		reader.Read().AssertSequenceEqual(4, 5, 6);
		reader.ReadEnd();
	}

	[Test]
	public void SplitWithSeparatorSelectorUptoMaxCount()
	{
		using var sequence = TestingSequence.Of<int?>(1, 2, null, 3, null, 4, 5, 6);
		var result = sequence.Split(n => n is null, 1);

		using var reader = result.Read();
		reader.Read().AssertSequenceEqual(1, 2);
		reader.Read().AssertSequenceEqual(3, null, 4, 5, 6);
		reader.ReadEnd();
	}
}

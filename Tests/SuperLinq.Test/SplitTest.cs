namespace Test;

public class SplitTest
{
	[Fact]
	public void SplitIsLazy()
	{
		new BreakingSequence<int>().Split(1);
		new BreakingSequence<int>().Split(1, 2);
	}

	[Fact]
	public void SplitWithSeparatorAndResultTransformation()
	{
		using var sequence = "the quick brown fox".AsTestingSequence();
		var result = sequence.Split(' ', chars => new string(chars.ToArray()));
		result.AssertSequenceEqual("the", "quick", "brown", "fox");
	}

	[Fact]
	public void SplitUptoMaxCount()
	{
		using var sequence = "the quick brown fox".AsTestingSequence();
		var result = sequence.Split(' ', 2, chars => new string(chars.ToArray()));
		result.AssertSequenceEqual("the", "quick", "brown fox");
	}

	[Fact]
	public void SplitWithSeparatorSelector()
	{
		using var sequence = TestingSequence.Of<int?>(1, 2, null, 3, null, 4, 5, 6);
		var result = sequence.Split(n => n == null);

		using var reader = result.Read();
		reader.Read().AssertSequenceEqual(1, 2);
		reader.Read().AssertSequenceEqual(3);
		reader.Read().AssertSequenceEqual(4, 5, 6);
		reader.ReadEnd();
	}

	[Fact]
	public void SplitWithSeparatorSelectorUptoMaxCount()
	{
		using var sequence = TestingSequence.Of<int?>(1, 2, null, 3, null, 4, 5, 6);
		var result = sequence.Split(n => n == null, 1);

		using var reader = result.Read();
		reader.Read().AssertSequenceEqual(1, 2);
		reader.Read().AssertSequenceEqual(3, null, 4, 5, 6);
		reader.ReadEnd();
	}
}

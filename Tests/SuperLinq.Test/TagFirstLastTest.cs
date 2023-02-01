namespace Test;

public class TagFirstLastTest
{
	[Fact]
	public void TagFirstLastIsLazy()
	{
		new BreakingSequence<object>().TagFirstLast();
		new BreakingSequence<object>().TagFirstLast(BreakingFunc.Of<object, bool, bool, object>());
	}

	[Fact]
	public void TagFirstLastEmptySequence()
	{
		using var sequence = TestingSequence.Of<int>();

		var result = sequence.TagFirstLast();
		result.AssertSequenceEqual();
	}

	[Fact]
	public void TagFirstLastWithSourceSequenceOfOne()
	{
		using var sequence = TestingSequence.Of(123);

		var result = sequence.TagFirstLast((item, isFirst, isLast) => new { Item = item, IsFirst = isFirst, IsLast = isLast });
		result.AssertSequenceEqual(new { Item = 123, IsFirst = true, IsLast = true });
	}

	[Fact]
	public void TagFirstLastWithSourceSequenceOfTwo()
	{
		using var sequence = TestingSequence.Of(123, 456);

		var result = sequence.TagFirstLast((item, isFirst, isLast) => new { Item = item, IsFirst = isFirst, IsLast = isLast });
		result.AssertSequenceEqual(
			new { Item = 123, IsFirst = true, IsLast = false },
			new { Item = 456, IsFirst = false, IsLast = true });
	}

	[Fact]
	public void TagFirstLastWithSourceSequenceOfThree()
	{
		using var sequence = TestingSequence.Of(123, 456, 789);

		var result = sequence.TagFirstLast((item, isFirst, isLast) => new { Item = item, IsFirst = isFirst, IsLast = isLast });
		result.AssertSequenceEqual(
			new { Item = 123, IsFirst = true, IsLast = false },
			new { Item = 456, IsFirst = false, IsLast = false },
			new { Item = 789, IsFirst = false, IsLast = true });
	}
}

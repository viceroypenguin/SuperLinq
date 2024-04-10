﻿namespace Test.Async;

public sealed class TagFirstLastTest
{
	[Fact]
	public void TagFirstLastIsLazy()
	{
		_ = new AsyncBreakingSequence<object>().TagFirstLast();
		_ = new AsyncBreakingSequence<object>().TagFirstLast(BreakingFunc.Of<object, bool, bool, object>());
	}

	[Fact]
	public async Task TagFirstLastEmptySequence()
	{
		await using var sequence = TestingSequence.Of<int>();

		var result = sequence.TagFirstLast();
		await result.AssertSequenceEqual();
	}

	[Fact]
	public async Task TagFirstLastWithSourceSequenceOfOne()
	{
		await using var sequence = TestingSequence.Of(123);

		var result = sequence.TagFirstLast((item, isFirst, isLast) => new { Item = item, IsFirst = isFirst, IsLast = isLast });
		await result.AssertSequenceEqual(new { Item = 123, IsFirst = true, IsLast = true });
	}

	[Fact]
	public async Task TagFirstLastWithSourceSequenceOfTwo()
	{
		await using var sequence = TestingSequence.Of(123, 456);

		var result = sequence.TagFirstLast((item, isFirst, isLast) => new { Item = item, IsFirst = isFirst, IsLast = isLast });
		await result.AssertSequenceEqual(
			new { Item = 123, IsFirst = true, IsLast = false },
			new { Item = 456, IsFirst = false, IsLast = true });
	}

	[Fact]
	public async Task TagFirstLastWithSourceSequenceOfThree()
	{
		await using var sequence = TestingSequence.Of(123, 456, 789);

		var result = sequence.TagFirstLast((item, isFirst, isLast) => new { Item = item, IsFirst = isFirst, IsLast = isLast });
		await result.AssertSequenceEqual(
			new { Item = 123, IsFirst = true, IsLast = false },
			new { Item = 456, IsFirst = false, IsLast = false },
			new { Item = 789, IsFirst = false, IsLast = true });
	}
}

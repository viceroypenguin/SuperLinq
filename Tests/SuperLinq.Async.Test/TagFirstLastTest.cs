using SuperLinq;

namespace Test.Async;

public class TagFirstLastTest
{
	[Fact]
	public void TagFirstLastIsLazy()
	{
		new AsyncBreakingSequence<object>().TagFirstLast(BreakingFunc.Of<object, bool, bool, object>());
	}

	[Fact]
	public async Task TagFirstLastEmptySequence()
	{
		await AsyncSeq<int>().TagFirstLast((item, isFirst, isLast) => new { Item = item, IsFirst = isFirst, IsLast = isLast })
			.AssertSequenceEqual();
	}

	[Fact]
	public Task TagFirstLastWithSourceSequenceOfOne()
	{
		return AsyncSeq(123)
			.TagFirstLast((item, isFirst, isLast) => new { Item = item, IsFirst = isFirst, IsLast = isLast })
			.AssertSequenceEqual(new { Item = 123, IsFirst = true, IsLast = true });
	}

	[Fact]
	public Task TagFirstLastWithSourceSequenceOfTwo()
	{
		return AsyncSeq(123, 456)
			.TagFirstLast((item, isFirst, isLast) => new { Item = item, IsFirst = isFirst, IsLast = isLast })
			.AssertSequenceEqual(new { Item = 123, IsFirst = true, IsLast = false },
								 new { Item = 456, IsFirst = false, IsLast = true });
	}

	[Fact]
	public Task TagFirstLastWithSourceSequenceOfThree()
	{
		return AsyncSeq(123, 456, 789)
			.TagFirstLast((item, isFirst, isLast) => new { Item = item, IsFirst = isFirst, IsLast = isLast })
			.AssertSequenceEqual(new { Item = 123, IsFirst = true, IsLast = false },
								 new { Item = 456, IsFirst = false, IsLast = false },
								 new { Item = 789, IsFirst = false, IsLast = true });
	}
}

using NUnit.Framework;

namespace Test;

[TestFixture]
public class TagFirstLastTest
{
	[Test]
	public void TagFirstLastIsLazy()
	{
		new BreakingSequence<object>().TagFirstLast(BreakingFunc.Of<object, bool, bool, object>());
	}

	[Test]
	public void TagFirstLastWithSourceSequenceOfOne()
	{
		var source = new[] { 123 };
		source.TagFirstLast((item, isFirst, isLast) => new { Item = item, IsFirst = isFirst, IsLast = isLast })
			  .AssertSequenceEqual(new { Item = 123, IsFirst = true, IsLast = true });
	}

	[Test]
	public void TagFirstLastWithSourceSequenceOfTwo()
	{
		var source = new[] { 123, 456 };
		source.TagFirstLast((item, isFirst, isLast) => new { Item = item, IsFirst = isFirst, IsLast = isLast })
			  .AssertSequenceEqual(new { Item = 123, IsFirst = true, IsLast = false },
								   new { Item = 456, IsFirst = false, IsLast = true });
	}

	[Test]
	public void TagFirstLastWithSourceSequenceOfThree()
	{
		var source = new[] { 123, 456, 789 };
		source.TagFirstLast((item, isFirst, isLast) => new { Item = item, IsFirst = isFirst, IsLast = isLast })
			  .AssertSequenceEqual(new { Item = 123, IsFirst = true, IsLast = false },
								   new { Item = 456, IsFirst = false, IsLast = false },
								   new { Item = 789, IsFirst = false, IsLast = true });
	}
}

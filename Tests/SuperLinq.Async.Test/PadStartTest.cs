﻿namespace Test.Async;

public class PadStartTest
{
	// PadStart(source, width)

	[Fact]
	public void PadStartWithNegativeWidth()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			AsyncSeq<int>().PadStart(-1));
	}

	[Fact]
	public void PadStartIsLazy()
	{
		new AsyncBreakingSequence<int>().PadStart(0);
	}

	public class PadStartWithDefaultPadding
	{
		[Theory]
		[InlineData(new[] { 123, 456, 789 }, 2, new[] { 123, 456, 789 })]
		[InlineData(new[] { 123, 456, 789 }, 3, new[] { 123, 456, 789 })]
		[InlineData(new[] { 123, 456, 789 }, 4, new[] { 0, 123, 456, 789 })]
		[InlineData(new[] { 123, 456, 789 }, 5, new[] { 0, 0, 123, 456, 789 })]
		public Task ValueTypeElements(ICollection<int> source, int width, IEnumerable<int> expected)
		{
			return source.ToAsyncEnumerable()
				.PadStart(width)
				.AssertSequenceEqual(expected);
		}

		[Theory]
		[InlineData(new[] { "foo", "bar", "baz" }, 2, new[] { "foo", "bar", "baz" })]
		[InlineData(new[] { "foo", "bar", "baz" }, 3, new[] { "foo", "bar", "baz" })]
		[InlineData(new[] { "foo", "bar", "baz" }, 4, new[] { null, "foo", "bar", "baz" })]
		[InlineData(new[] { "foo", "bar", "baz" }, 5, new[] { null, null, "foo", "bar", "baz" })]
		public Task ReferenceTypeElements(ICollection<string?> source, int width, IEnumerable<string?> expected)
		{
			return source.ToAsyncEnumerable()
				.PadStart(width)
				.AssertSequenceEqual(expected);
		}
	}

	// PadStart(source, width, padding)

	[Fact]
	public void PadStartWithPaddingWithNegativeWidth()
	{
		Assert.Throws<ArgumentOutOfRangeException>(
			() => AsyncSeq<int>().PadStart(-1, 1));
	}

	[Fact]
	public void PadStartWithPaddingIsLazy()
	{
		new AsyncBreakingSequence<int>().PadStart(0, -1);
	}

	public class PadStartWithPadding
	{
		[Theory]
		[InlineData(new[] { 123, 456, 789 }, 2, new[] { 123, 456, 789 })]
		[InlineData(new[] { 123, 456, 789 }, 3, new[] { 123, 456, 789 })]
		[InlineData(new[] { 123, 456, 789 }, 4, new[] { -1, 123, 456, 789 })]
		[InlineData(new[] { 123, 456, 789 }, 5, new[] { -1, -1, 123, 456, 789 })]
		public Task ValueTypeElements(ICollection<int> source, int width, IEnumerable<int> expected)
		{
			return source.ToAsyncEnumerable()
				.PadStart(width, -1)
				.AssertSequenceEqual(expected);
		}

		[Theory]
		[InlineData(new[] { "foo", "bar", "baz" }, 2, new[] { "foo", "bar", "baz" })]
		[InlineData(new[] { "foo", "bar", "baz" }, 3, new[] { "foo", "bar", "baz" })]
		[InlineData(new[] { "foo", "bar", "baz" }, 4, new[] { "", "foo", "bar", "baz" })]
		[InlineData(new[] { "foo", "bar", "baz" }, 5, new[] { "", "", "foo", "bar", "baz" })]
		public Task ReferenceTypeElements(ICollection<string> source, int width, IEnumerable<string> expected)
		{
			return source.ToAsyncEnumerable()
				.PadStart(width, string.Empty)
				.AssertSequenceEqual(expected);
		}
	}

	// PadStart(source, width, paddingSelector)

	[Fact]
	public void PadStartWithSelectorWithNegativeWidth()
	{
		Assert.Throws<ArgumentOutOfRangeException>(
			() => AsyncSeq<int>().PadStart(-1, x => x));
	}

	[Fact]
	public void PadStartWithSelectorIsLazy()
	{
		new AsyncBreakingSequence<int>().PadStart(0, BreakingFunc.Of<int, int>());
	}

	public class PadStartWithSelector
	{
		[Theory]
		[InlineData(new[] { 123, 456, 789 }, 2, new[] { 123, 456, 789 })]
		[InlineData(new[] { 123, 456, 789 }, 3, new[] { 123, 456, 789 })]
		[InlineData(new[] { 123, 456, 789 }, 4, new[] { 0, 123, 456, 789 })]
		[InlineData(new[] { 123, 456, 789 }, 5, new[] { 0, -1, 123, 456, 789 })]
		[InlineData(new[] { 123, 456, 789 }, 6, new[] { 0, -1, -4, 123, 456, 789 })]
		[InlineData(new[] { 123, 456, 789 }, 7, new[] { 0, -1, -4, -9, 123, 456, 789 })]
		public Task ValueTypeElements(ICollection<int> source, int width, IEnumerable<int> expected)
		{
			return source.ToAsyncEnumerable()
				.PadStart(width, y => y * -y)
				.AssertSequenceEqual(expected);
		}

		[Theory]
		[InlineData(new[] { "foo", "bar", "baz" }, 2, new[] { "foo", "bar", "baz" })]
		[InlineData(new[] { "foo", "bar", "baz" }, 3, new[] { "foo", "bar", "baz" })]
		[InlineData(new[] { "foo", "bar", "baz" }, 4, new[] { "+", "foo", "bar", "baz" })]
		[InlineData(new[] { "foo", "bar", "baz" }, 5, new[] { "+", "++", "foo", "bar", "baz" })]
		[InlineData(new[] { "foo", "bar", "baz" }, 6, new[] { "+", "++", "+++", "foo", "bar", "baz" })]
		[InlineData(new[] { "foo", "bar", "baz" }, 7, new[] { "+", "++", "+++", "++++", "foo", "bar", "baz" })]
		public Task ReferenceTypeElements(ICollection<string> source, int width, IEnumerable<string> expected)
		{
			return source.ToAsyncEnumerable()
				.PadStart(width, y => new string('+', y + 1))
				.AssertSequenceEqual(expected);
		}
	}
}
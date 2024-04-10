namespace Test.Async;

public sealed class PadStartTest
{
	// PadStart(source, width)

	[Fact]
	public void PadStartWithNegativeWidth()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			AsyncSeq<int>().PadStart(-1));
	}

	[Fact]
	public void PadStartIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().PadStart(0);
	}

	public sealed class PadStartWithDefaultPadding
	{
		[Theory]
		[InlineData(new[] { 123, 456, 789 }, 2, new[] { 123, 456, 789 })]
		[InlineData(new[] { 123, 456, 789 }, 3, new[] { 123, 456, 789 })]
		[InlineData(new[] { 123, 456, 789 }, 4, new[] { 0, 123, 456, 789 })]
		[InlineData(new[] { 123, 456, 789 }, 5, new[] { 0, 0, 123, 456, 789 })]
		public async Task ValueTypeElements(ICollection<int> source, int width, IEnumerable<int> expected)
		{
			await using var xs = source.AsTestingSequence();
			await xs
				.PadStart(width)
				.AssertSequenceEqual(expected);
		}

		[Theory]
		[InlineData(new[] { "foo", "bar", "baz" }, 2, new[] { "foo", "bar", "baz" })]
		[InlineData(new[] { "foo", "bar", "baz" }, 3, new[] { "foo", "bar", "baz" })]
		[InlineData(new[] { "foo", "bar", "baz" }, 4, new[] { null, "foo", "bar", "baz" })]
		[InlineData(new[] { "foo", "bar", "baz" }, 5, new[] { null, null, "foo", "bar", "baz" })]
		public async Task ReferenceTypeElements(ICollection<string?> source, int width, IEnumerable<string?> expected)
		{
			await using var xs = source.AsTestingSequence();
			await xs
				.PadStart(width)
				.AssertSequenceEqual(expected);
		}
	}

	// PadStart(source, width, padding)

	[Fact]
	public void PadStartWithPaddingWithNegativeWidth()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(
			() => new AsyncBreakingSequence<int>().PadStart(-1, 1));
	}

	[Fact]
	public void PadStartWithPaddingIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().PadStart(0, -1);
	}

	public sealed class PadStartWithPadding
	{
		[Theory]
		[InlineData(new[] { 123, 456, 789 }, 2, new[] { 123, 456, 789 })]
		[InlineData(new[] { 123, 456, 789 }, 3, new[] { 123, 456, 789 })]
		[InlineData(new[] { 123, 456, 789 }, 4, new[] { -1, 123, 456, 789 })]
		[InlineData(new[] { 123, 456, 789 }, 5, new[] { -1, -1, 123, 456, 789 })]
		public async Task ValueTypeElements(ICollection<int> source, int width, IEnumerable<int> expected)
		{
			await using var xs = source.AsTestingSequence();
			await xs
				.PadStart(width, -1)
				.AssertSequenceEqual(expected);
		}

		[Theory]
		[InlineData(new[] { "foo", "bar", "baz" }, 2, new[] { "foo", "bar", "baz" })]
		[InlineData(new[] { "foo", "bar", "baz" }, 3, new[] { "foo", "bar", "baz" })]
		[InlineData(new[] { "foo", "bar", "baz" }, 4, new[] { "", "foo", "bar", "baz" })]
		[InlineData(new[] { "foo", "bar", "baz" }, 5, new[] { "", "", "foo", "bar", "baz" })]
		public async Task ReferenceTypeElements(ICollection<string> source, int width, IEnumerable<string> expected)
		{
			await using var xs = source.AsTestingSequence();
			await xs
				.PadStart(width, string.Empty)
				.AssertSequenceEqual(expected);
		}
	}

	// PadStart(source, width, paddingSelector)

	[Fact]
	public void PadStartWithSelectorWithNegativeWidth()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(
			() => new AsyncBreakingSequence<int>().PadStart(-1, SuperEnumerable.Identity));
	}

	[Fact]
	public void PadStartWithSelectorIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().PadStart(0, BreakingFunc.Of<int, int>());
	}

	public sealed class PadStartWithSelector
	{
		[Theory]
		[InlineData(new[] { 123, 456, 789 }, 2, new[] { 123, 456, 789 })]
		[InlineData(new[] { 123, 456, 789 }, 3, new[] { 123, 456, 789 })]
		[InlineData(new[] { 123, 456, 789 }, 4, new[] { 0, 123, 456, 789 })]
		[InlineData(new[] { 123, 456, 789 }, 5, new[] { 0, -1, 123, 456, 789 })]
		[InlineData(new[] { 123, 456, 789 }, 6, new[] { 0, -1, -4, 123, 456, 789 })]
		[InlineData(new[] { 123, 456, 789 }, 7, new[] { 0, -1, -4, -9, 123, 456, 789 })]
		public async Task ValueTypeElements(ICollection<int> source, int width, IEnumerable<int> expected)
		{
			await using var xs = source.AsTestingSequence();
			await xs
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
		public async Task ReferenceTypeElements(ICollection<string> source, int width, IEnumerable<string> expected)
		{
			await using var xs = source.AsTestingSequence();
			await xs
				.PadStart(width, y => new string('+', y + 1))
				.AssertSequenceEqual(expected);
		}
	}
}

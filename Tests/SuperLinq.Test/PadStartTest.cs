namespace Test;

public class PadStartTest
{
	// PadStart(source, width)

	[Fact]
	public void PadStartWithNegativeWidth()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			Array.Empty<int>().PadStart(-1));
	}

	[Fact]
	public void PadStartIsLazy()
	{
		new BreakingSequence<int>().PadStart(0);
	}

	public class PadStartWithDefaultPadding
	{
		[Theory]
		[InlineData(new[] { 123, 456, 789 }, 2, new[] { 123, 456, 789 })]
		[InlineData(new[] { 123, 456, 789 }, 3, new[] { 123, 456, 789 })]
		[InlineData(new[] { 123, 456, 789 }, 4, new[] { 0, 123, 456, 789 })]
		[InlineData(new[] { 123, 456, 789 }, 5, new[] { 0, 0, 123, 456, 789 })]
		public void ValueTypeElements(ICollection<int> source, int width, IEnumerable<int> expected)
		{
			AssertEqual(source, x => x.PadStart(width), expected);
		}

		[Theory]
		[InlineData(new[] { "foo", "bar", "baz" }, 2, new[] { "foo", "bar", "baz" })]
		[InlineData(new[] { "foo", "bar", "baz" }, 3, new[] { "foo", "bar", "baz" })]
		[InlineData(new[] { "foo", "bar", "baz" }, 4, new[] { null, "foo", "bar", "baz" })]
		[InlineData(new[] { "foo", "bar", "baz" }, 5, new[] { null, null, "foo", "bar", "baz" })]
		public void ReferenceTypeElements(ICollection<string?> source, int width, IEnumerable<string?> expected)
		{
			AssertEqual(source, x => x.PadStart(width), expected);
		}
	}

	// PadStart(source, width, padding)

	[Fact]
	public void PadStartWithPaddingWithNegativeWidth()
	{
		Assert.Throws<ArgumentOutOfRangeException>(
			() => Array.Empty<int>().PadStart(-1, 1));
	}

	[Fact]
	public void PadStartWithPaddingIsLazy()
	{
		new BreakingSequence<int>().PadStart(0, -1);
	}

	public class PadStartWithPadding
	{
		[Theory]
		[InlineData(new[] { 123, 456, 789 }, 2, new[] { 123, 456, 789 })]
		[InlineData(new[] { 123, 456, 789 }, 3, new[] { 123, 456, 789 })]
		[InlineData(new[] { 123, 456, 789 }, 4, new[] { -1, 123, 456, 789 })]
		[InlineData(new[] { 123, 456, 789 }, 5, new[] { -1, -1, 123, 456, 789 })]
		public void ValueTypeElements(ICollection<int> source, int width, IEnumerable<int> expected)
		{
			AssertEqual(source, x => x.PadStart(width, -1), expected);
		}

		[Theory]
		[InlineData(new[] { "foo", "bar", "baz" }, 2, new[] { "foo", "bar", "baz" })]
		[InlineData(new[] { "foo", "bar", "baz" }, 3, new[] { "foo", "bar", "baz" })]
		[InlineData(new[] { "foo", "bar", "baz" }, 4, new[] { "", "foo", "bar", "baz" })]
		[InlineData(new[] { "foo", "bar", "baz" }, 5, new[] { "", "", "foo", "bar", "baz" })]
		public void ReferenceTypeElements(ICollection<string> source, int width, IEnumerable<string> expected)
		{
			AssertEqual(source, x => x.PadStart(width, string.Empty), expected);
		}
	}

	// PadStart(source, width, paddingSelector)

	[Fact]
	public void PadStartWithSelectorWithNegativeWidth()
	{
		Assert.Throws<ArgumentOutOfRangeException>(
			() => Array.Empty<int>().PadStart(-1, SuperEnumerable.Identity));
	}

	[Fact]
	public void PadStartWithSelectorIsLazy()
	{
		new BreakingSequence<int>().PadStart(0, BreakingFunc.Of<int, int>());
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
		public void ValueTypeElements(ICollection<int> source, int width, IEnumerable<int> expected)
		{
			AssertEqual(source, x => x.PadStart(width, y => y * -y), expected);
		}

		[Theory]
		[InlineData(new[] { "foo", "bar", "baz" }, 2, new[] { "foo", "bar", "baz" })]
		[InlineData(new[] { "foo", "bar", "baz" }, 3, new[] { "foo", "bar", "baz" })]
		[InlineData(new[] { "foo", "bar", "baz" }, 4, new[] { "+", "foo", "bar", "baz" })]
		[InlineData(new[] { "foo", "bar", "baz" }, 5, new[] { "+", "++", "foo", "bar", "baz" })]
		[InlineData(new[] { "foo", "bar", "baz" }, 6, new[] { "+", "++", "+++", "foo", "bar", "baz" })]
		[InlineData(new[] { "foo", "bar", "baz" }, 7, new[] { "+", "++", "+++", "++++", "foo", "bar", "baz" })]
		public void ReferenceTypeElements(ICollection<string> source, int width, IEnumerable<string> expected)
		{
			AssertEqual(source, x => x.PadStart(width, y => new string('+', y + 1)), expected);
		}
	}


	private static void AssertEqual<T>(ICollection<T> input, Func<IEnumerable<T>, IEnumerable<T>> op, IEnumerable<T> expected)
	{
		// Test that the behaviour does not change whether a collection
		// or a sequence is used as the source.

		op(input).AssertSequenceEqual(expected);

		using var xs = input.AsTestingSequence();
		op(xs).AssertSequenceEqual(expected);
	}
}

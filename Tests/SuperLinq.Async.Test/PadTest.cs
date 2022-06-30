namespace Test.Async;

public class PadTest
{
	[Fact]
	public void PadNegativeWidth()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			AsyncSeq<object>().Pad(-1));
	}

	[Fact]
	public void PadIsLazy()
	{
		new AsyncBreakingSequence<object>().Pad(0);
	}

	[Fact]
	public void PadWithFillerIsLazy()
	{
		new AsyncBreakingSequence<object>().Pad(0, new object());
	}

	public class ValueTypeElements
	{
		[Fact]
		public Task PadWideSourceSequence()
		{
			var result = AsyncSeq(123, 456, 789).Pad(2);
			return result.AssertSequenceEqual(123, 456, 789);
		}

		[Fact]
		public Task PadEqualSourceSequence()
		{
			var result = AsyncSeq(123, 456, 789).Pad(3);
			return result.AssertSequenceEqual(123, 456, 789);
		}

		[Fact]
		public Task PadNarrowSourceSequenceWithDefaultPadding()
		{
			var result = AsyncSeq(123, 456, 789).Pad(5);
			return result.AssertSequenceEqual(123, 456, 789, 0, 0);
		}

		[Fact]
		public Task PadNarrowSourceSequenceWithNonDefaultPadding()
		{
			var result = AsyncSeq(123, 456, 789).Pad(5, -1);
			return result.AssertSequenceEqual(123, 456, 789, -1, -1);
		}

		[Fact]
		public Task PadNarrowSourceSequenceWithDynamicPadding()
		{
			var result = "hello".ToCharArray().ToAsyncEnumerable().Pad(15, i => i % 2 == 0 ? '+' : '-');
			return result.AssertSequenceEqual("hello-+-+-+-+-+".ToCharArray());
		}
	}

	public class ReferenceTypeElements
	{
		[Fact]
		public Task PadWideSourceSequence()
		{
			var result = AsyncSeq("foo", "bar", "baz").Pad(2);
			return result.AssertSequenceEqual("foo", "bar", "baz");
		}

		[Fact]
		public Task PadEqualSourceSequence()
		{
			var result = AsyncSeq("foo", "bar", "baz").Pad(3);
			return result.AssertSequenceEqual("foo", "bar", "baz");
		}

		[Fact]
		public Task PadNarrowSourceSequenceWithDefaultPadding()
		{
			var result = AsyncSeq("foo", "bar", "baz").Pad(5);
			return result.AssertSequenceEqual("foo", "bar", "baz", null, null);
		}

		[Fact]
		public Task PadNarrowSourceSequenceWithNonDefaultPadding()
		{
			var result = AsyncSeq("foo", "bar", "baz").Pad(5, string.Empty);
			return result.AssertSequenceEqual("foo", "bar", "baz", string.Empty, string.Empty);
		}
	}
}

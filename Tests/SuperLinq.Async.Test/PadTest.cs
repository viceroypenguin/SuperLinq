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
		public async Task PadWideSourceSequence()
		{
			await using var sequence = TestingSequence.Of(123, 456, 789);
			var result = sequence.Pad(2);
			await result.AssertSequenceEqual(123, 456, 789);
		}

		[Fact]
		public async Task PadEqualSourceSequence()
		{
			await using var sequence = TestingSequence.Of(123, 456, 789);
			var result = sequence.Pad(3);
			await result.AssertSequenceEqual(123, 456, 789);
		}

		[Fact]
		public async Task PadNarrowSourceSequenceWithDefaultPadding()
		{
			await using var sequence = TestingSequence.Of(123, 456, 789);
			var result = sequence.Pad(5);
			await result.AssertSequenceEqual(123, 456, 789, 0, 0);
		}

		[Fact]
		public async Task PadNarrowSourceSequenceWithNonDefaultPadding()
		{
			await using var sequence = TestingSequence.Of(123, 456, 789);
			var result = sequence.Pad(5, -1);
			await result.AssertSequenceEqual(123, 456, 789, -1, -1);
		}

		[Fact]
		public async Task PadNarrowSourceSequenceWithDynamicPadding()
		{
			var result = "hello".ToCharArray().ToAsyncEnumerable().Pad(15, i => i % 2 == 0 ? '+' : '-');
			await result.AssertSequenceEqual("hello-+-+-+-+-+".ToCharArray());
		}
	}

	public class ReferenceTypeElements
	{
		[Fact]
		public async Task PadWideSourceSequence()
		{
			await using var sequence = TestingSequence.Of("foo", "bar", "baz");
			var result = sequence.Pad(2);
			await result.AssertSequenceEqual("foo", "bar", "baz");
		}

		[Fact]
		public async Task PadEqualSourceSequence()
		{
			await using var sequence = TestingSequence.Of("foo", "bar", "baz");
			var result = sequence.Pad(3);
			await result.AssertSequenceEqual("foo", "bar", "baz");
		}

		[Fact]
		public async Task PadNarrowSourceSequenceWithDefaultPadding()
		{
			await using var sequence = TestingSequence.Of("foo", "bar", "baz");
			var result = sequence.Pad(5);
			await result.AssertSequenceEqual("foo", "bar", "baz", null, null);
		}

		[Fact]
		public async Task PadNarrowSourceSequenceWithNonDefaultPadding()
		{
			await using var sequence = TestingSequence.Of("foo", "bar", "baz");
			var result = sequence.Pad(5, string.Empty);
			await result.AssertSequenceEqual("foo", "bar", "baz", string.Empty, string.Empty);
		}
	}
}

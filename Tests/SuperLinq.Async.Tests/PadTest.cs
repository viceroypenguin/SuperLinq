namespace SuperLinq.Async.Tests;

public sealed class PadTest
{
	[Test]
	public void PadNegativeWidth()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			AsyncSeq<object>().Pad(-1));
	}

	[Test]
	public void PadIsLazy()
	{
		_ = new AsyncBreakingSequence<object>().Pad(0);
	}

	[Test]
	public void PadWithFillerIsLazy()
	{
		_ = new AsyncBreakingSequence<object>().Pad(0, new object());
	}

	public sealed class ValueTypeElements
	{
		[Test]
		public async Task PadWideSourceSequence()
		{
			await using var sequence = TestingSequence.Of(123, 456, 789);
			var result = sequence.Pad(2);
			await result.AssertSequenceEqual(123, 456, 789);
		}

		[Test]
		public async Task PadEqualSourceSequence()
		{
			await using var sequence = TestingSequence.Of(123, 456, 789);
			var result = sequence.Pad(3);
			await result.AssertSequenceEqual(123, 456, 789);
		}

		[Test]
		public async Task PadNarrowSourceSequenceWithDefaultPadding()
		{
			await using var sequence = TestingSequence.Of(123, 456, 789);
			var result = sequence.Pad(5);
			await result.AssertSequenceEqual(123, 456, 789, 0, 0);
		}

		[Test]
		public async Task PadNarrowSourceSequenceWithNonDefaultPadding()
		{
			await using var sequence = TestingSequence.Of(123, 456, 789);
			var result = sequence.Pad(5, -1);
			await result.AssertSequenceEqual(123, 456, 789, -1, -1);
		}

		[Test]
		public async Task PadNarrowSourceSequenceWithDynamicPadding()
		{
			var result = "hello".ToCharArray().ToAsyncEnumerable().Pad(15, i => i % 2 == 0 ? '+' : '-');
			await result.AssertSequenceEqual("hello-+-+-+-+-+".ToCharArray());
		}
	}

	public sealed class ReferenceTypeElements
	{
		[Test]
		public async Task PadWideSourceSequence()
		{
			await using var sequence = TestingSequence.Of("foo", "bar", "baz");
			var result = sequence.Pad(2);
			await result.AssertSequenceEqual("foo", "bar", "baz");
		}

		[Test]
		public async Task PadEqualSourceSequence()
		{
			await using var sequence = TestingSequence.Of("foo", "bar", "baz");
			var result = sequence.Pad(3);
			await result.AssertSequenceEqual("foo", "bar", "baz");
		}

		[Test]
		public async Task PadNarrowSourceSequenceWithDefaultPadding()
		{
			await using var sequence = TestingSequence.Of("foo", "bar", "baz");
			var result = sequence.Pad(5);
			await result.AssertSequenceEqual("foo", "bar", "baz", null, null);
		}

		[Test]
		public async Task PadNarrowSourceSequenceWithNonDefaultPadding()
		{
			await using var sequence = TestingSequence.Of("foo", "bar", "baz");
			var result = sequence.Pad(5, "");
			await result.AssertSequenceEqual("foo", "bar", "baz", "", "");
		}
	}
}

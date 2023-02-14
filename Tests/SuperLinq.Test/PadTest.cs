namespace Test;

public class PadTest
{
	[Fact]
	public void PadNegativeWidth()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<object>().Pad(-1));
	}

	[Fact]
	public void PadIsLazy()
	{
		_ = new BreakingSequence<object>().Pad(0);
	}

	[Fact]
	public void PadWithFillerIsLazy()
	{
		_ = new BreakingSequence<object>().Pad(0, new object());
	}

	public class ValueTypeElements
	{
		[Fact]
		public void PadWideSourceSequence()
		{
			using var sequence = TestingSequence.Of(123, 456, 789);
			var result = sequence.Pad(2);
			result.AssertSequenceEqual(123, 456, 789);
		}

		[Fact]
		public void PadEqualSourceSequence()
		{
			using var sequence = TestingSequence.Of(123, 456, 789);
			var result = sequence.Pad(3);
			result.AssertSequenceEqual(123, 456, 789);
		}

		[Fact]
		public void PadNarrowSourceSequenceWithDefaultPadding()
		{
			using var sequence = TestingSequence.Of(123, 456, 789);
			var result = sequence.Pad(5);
			result.AssertSequenceEqual(123, 456, 789, 0, 0);
		}

		[Fact]
		public void PadNarrowSourceSequenceWithNonDefaultPadding()
		{
			using var sequence = TestingSequence.Of(123, 456, 789);
			var result = sequence.Pad(5, -1);
			result.AssertSequenceEqual(123, 456, 789, -1, -1);
		}

		[Fact]
		public void PadNarrowSourceSequenceWithDynamicPadding()
		{
			using var sequence = "hello".AsTestingSequence();
			var result = sequence.Pad(15, i => i % 2 == 0 ? '+' : '-');
			result.AssertSequenceEqual("hello-+-+-+-+-+".ToCharArray());
		}
	}

	public class ReferenceTypeElements
	{
		[Fact]
		public void PadWideSourceSequence()
		{
			using var sequence = TestingSequence.Of("foo", "bar", "baz");
			var result = sequence.Pad(2);
			result.AssertSequenceEqual("foo", "bar", "baz");
		}

		[Fact]
		public void PadEqualSourceSequence()
		{
			using var sequence = TestingSequence.Of("foo", "bar", "baz");
			var result = sequence.Pad(3);
			result.AssertSequenceEqual("foo", "bar", "baz");
		}

		[Fact]
		public void PadNarrowSourceSequenceWithDefaultPadding()
		{
			using var sequence = TestingSequence.Of("foo", "bar", "baz");
			var result = sequence.Pad(5);
			result.AssertSequenceEqual("foo", "bar", "baz", null, null);
		}

		[Fact]
		public void PadNarrowSourceSequenceWithNonDefaultPadding()
		{
			using var sequence = TestingSequence.Of("foo", "bar", "baz");
			var result = sequence.Pad(5, string.Empty);
			result.AssertSequenceEqual("foo", "bar", "baz", string.Empty, string.Empty);
		}
	}
}

namespace Test;

public class PadStartTest
{
	[Fact]
	public void PadStartNegativeWidth()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<object>().PadStart(-1));
	}

	[Fact]
	public void PadStartIsLazy()
	{
		_ = new BreakingSequence<object>().PadStart(0);
		_ = new BreakingSequence<object>().PadStart(0, new object());
		_ = new BreakingSequence<object>().PadStart(0, BreakingFunc.Of<int, object>());
	}

	public class ValueTypeElements
	{
		public static IEnumerable<object[]> GetIntSequences() =>
			new LinkedList<int>(Seq(123, 456, 789))
				.GetListSequencesWithSelf()
				.Select(x => new object[] { x });

		[Theory]
		[MemberData(nameof(GetIntSequences))]
		public void PadStartWideSourceSequence(IEnumerable<int> seq)
		{
			using (seq as IDisposableEnumerable<int>)
			{
				var result = seq.PadStart(2);
				result.AssertSequenceEqual(123, 456, 789);
			}
		}

		[Fact]
		public void PadStartWideListBehavior()
		{
			using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

			var result = seq.PadStart(5_000, x => x % 1_000);
			Assert.Equal(10_000, result.Count());
			Assert.Equal(1_200, result.ElementAt(1_200));
			Assert.Equal(8_800, result.ElementAt(^1_200));

			_ = Assert.Throws<ArgumentOutOfRangeException>(
				"index",
				() => result.ElementAt(10_001));
		}

		[Theory]
		[MemberData(nameof(GetIntSequences))]
		public void PadStartEqualSourceSequence(IEnumerable<int> seq)
		{
			using (seq as IDisposableEnumerable<int>)
			{
				var result = seq.PadStart(3);
				result.AssertSequenceEqual(123, 456, 789);
			}
		}

		[Theory]
		[MemberData(nameof(GetIntSequences))]
		public void PadStartNarrowSourceSequenceWithDefaultPadding(IEnumerable<int> seq)
		{
			using (seq as IDisposableEnumerable<int>)
			{
				var result = seq.PadStart(5);
				result.AssertSequenceEqual(0, 0, 123, 456, 789);
			}
		}

		[Theory]
		[MemberData(nameof(GetIntSequences))]
		public void PadStartNarrowSourceSequenceWithNonDefaultPadding(IEnumerable<int> seq)
		{
			using (seq as IDisposableEnumerable<int>)
			{
				var result = seq.PadStart(5, -1);
				result.AssertSequenceEqual(-1, -1, 123, 456, 789);
			}
		}

		[Fact]
		public void PadStartNarrowListBehavior()
		{
			using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

			var result = seq.PadStart(40_000, x => x % 1_000);
			Assert.Equal(40_000, result.Count());
			Assert.Equal(200, result.ElementAt(1_200));
			Assert.Equal(1_200, result.ElementAt(31_200));
			Assert.Equal(8_800, result.ElementAt(^1_200));

			_ = Assert.Throws<ArgumentOutOfRangeException>(
				"index",
				() => result.ElementAt(40_001));
		}

		public static IEnumerable<object[]> GetCharSequences() =>
			new LinkedList<char>("hello")
				.GetListSequencesWithSelf()
				.Select(x => new object[] { x });

		[Theory]
		[MemberData(nameof(GetCharSequences))]
		public void PadStartNarrowSourceSequenceWithDynamicPadding(IEnumerable<char> seq)
		{
			using (seq as IDisposableEnumerable<char>)
			{
				var result = seq.PadStart(15, i => i % 2 == 0 ? '+' : '-');
				result.AssertSequenceEqual("+-+-+-+-+-hello".ToCharArray());
			}
		}
	}

	public class ReferenceTypeElements
	{
		public static IEnumerable<object[]> GetStringSequences() =>
			new LinkedList<string>(Seq("foo", "bar", "baz"))
				.GetListSequencesWithSelf()
				.Select(x => new object[] { x });

		[Theory]
		[MemberData(nameof(GetStringSequences))]
		public void PadStartWideSourceSequence(IEnumerable<string> seq)
		{
			using (seq as IDisposableEnumerable<string>)
			{
				var result = seq.PadStart(2);
				result.AssertSequenceEqual("foo", "bar", "baz");
			}
		}

		[Fact]
		public void PadStartWideListBehavior()
		{
			using var seq = Seq("foo", "bar", "baz").AsBreakingList();

			var result = seq.PadStart(2, x => $"Extra{x}");
			Assert.Equal(3, result.Count());
			Assert.Equal("bar", result.ElementAt(1));
			Assert.Equal("baz", result.ElementAt(^1));

			_ = Assert.Throws<ArgumentOutOfRangeException>(
				"index",
				() => result.ElementAt(3));
		}

		[Theory]
		[MemberData(nameof(GetStringSequences))]
		public void PadStartEqualSourceSequence(IEnumerable<string> seq)
		{
			using (seq as IDisposableEnumerable<string>)
			{
				var result = seq.PadStart(3);
				result.AssertSequenceEqual("foo", "bar", "baz");
			}
		}

		[Theory]
		[MemberData(nameof(GetStringSequences))]
		public void PadStartNarrowSourceSequenceWithDefaultPadding(IEnumerable<string> seq)
		{
			using (seq as IDisposableEnumerable<string>)
			{
				var result = seq.PadStart(5);
				result.AssertSequenceEqual(default(string), null, "foo", "bar", "baz");
			}
		}

		[Theory]
		[MemberData(nameof(GetStringSequences))]
		public void PadStartNarrowSourceSequenceWithNonDefaultPadding(IEnumerable<string> seq)
		{
			using (seq as IDisposableEnumerable<string>)
			{
				var result = seq.PadStart(5, string.Empty);
				result.AssertSequenceEqual(string.Empty, string.Empty, "foo", "bar", "baz");
			}
		}

		[Theory]
		[MemberData(nameof(GetStringSequences))]
		public void PadStartNarrowSourceSequenceWithDynamicPadding(IEnumerable<string> seq)
		{
			using (seq as IDisposableEnumerable<string>)
			{
				var result = seq.PadStart(5, x => $"Extra{x}");
				result.AssertSequenceEqual("Extra0", "Extra1", "foo", "bar", "baz");
			}
		}

		[Fact]
		public void PadStartNarrowListBehavior()
		{
			using var seq = Seq("foo", "bar", "baz").AsBreakingList();

			var result = seq.PadStart(5, x => $"Extra{x}");
			Assert.Equal(5, result.Count());
			Assert.Equal("Extra1", result.ElementAt(1));
			Assert.Equal("baz", result.ElementAt(^1));

			_ = Assert.Throws<ArgumentOutOfRangeException>(
				"index",
				() => result.ElementAt(5));
		}
	}

	[Fact]
	public void PadStartUsesCollectionCountAtIterationTime()
	{
		var queue = new Queue<int>(Enumerable.Range(1, 3));
		var result = queue.PadStart(5, -1);
		result.AssertSequenceEqual(-1, -1, 1, 2, 3);
		queue.Enqueue(4);
		result.AssertSequenceEqual(-1, 1, 2, 3, 4);
	}
}

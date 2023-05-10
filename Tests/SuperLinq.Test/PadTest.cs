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
		_ = new BreakingSequence<object>().Pad(0, new object());
		_ = new BreakingSequence<object>().Pad(0, BreakingFunc.Of<int, object>());
	}

	public class ValueTypeElements
	{
		public static IEnumerable<object[]> GetIntSequences()
		{
			var seq = Seq(123, 456, 789);
			yield return new object[] { seq.AsTestingSequence(maxEnumerations: 2), false, };
			yield return new object[] { seq.AsBreakingList(), false, };
			yield return new object[] { seq.AsBreakingList(), true, };
		}

		[Theory]
		[MemberData(nameof(GetIntSequences))]
		public void PadWideSourceSequence(IDisposableEnumerable<int> seq, bool select)
		{
			using (seq)
			{
				var result = seq.Pad(2);
				if (select) result = result.Select(SuperEnumerable.Identity);
				result.AssertSequenceEqual(123, 456, 789);
			}
		}

		[Fact]
		public void PadWideListBehavior()
		{
			using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

			var result = seq.Pad(5_000, x => x % 1_000);
			Assert.Equal(10_000, result.Count());
			Assert.Equal(1_200, result.ElementAt(1_200));
			Assert.Equal(8_800, result.ElementAt(^1_200));

			_ = Assert.Throws<ArgumentOutOfRangeException>(
				"index",
				() => result.ElementAt(10_001));
		}

		[Theory]
		[MemberData(nameof(GetIntSequences))]
		public void PadEqualSourceSequence(IDisposableEnumerable<int> seq, bool select)
		{
			using (seq)
			{
				var result = seq.Pad(3);
				if (select) result = result.Select(SuperEnumerable.Identity);
				result.AssertSequenceEqual(123, 456, 789);
			}
		}

		[Theory]
		[MemberData(nameof(GetIntSequences))]
		public void PadNarrowSourceSequenceWithDefaultPadding(IDisposableEnumerable<int> seq, bool select)
		{
			using (seq)
			{
				var result = seq.Pad(5);
				if (select) result = result.Select(SuperEnumerable.Identity);
				result.AssertSequenceEqual(123, 456, 789, 0, 0);
			}
		}

		[Theory]
		[MemberData(nameof(GetIntSequences))]
		public void PadNarrowSourceSequenceWithNonDefaultPadding(IDisposableEnumerable<int> seq, bool select)
		{
			using (seq)
			{
				var result = seq.Pad(5, -1);
				if (select) result = result.Select(SuperEnumerable.Identity);
				result.AssertSequenceEqual(123, 456, 789, -1, -1);
			}
		}

		[Fact]
		public void PadNarrowListBehavior()
		{
			using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

			var result = seq.Pad(40_000, x => x % 1_000);
			Assert.Equal(40_000, result.Count());
			Assert.Equal(1_200, result.ElementAt(1_200));
			Assert.Equal(200, result.ElementAt(11_200));
			Assert.Equal(800, result.ElementAt(^1_200));

			_ = Assert.Throws<ArgumentOutOfRangeException>(
				"index",
				() => result.ElementAt(40_001));
		}

		public static IEnumerable<object[]> GetCharSequences()
		{
			var seq = "hello".AsEnumerable();
			yield return new object[] { seq.AsTestingSequence(maxEnumerations: 2), false, };
			yield return new object[] { seq.AsBreakingList(), false, };
			yield return new object[] { seq.AsBreakingList(), true, };
		}

		[Theory]
		[MemberData(nameof(GetCharSequences))]
		public void PadNarrowSourceSequenceWithDynamicPadding(IDisposableEnumerable<char> seq, bool select)
		{
			using (seq)
			{
				var result = seq.Pad(15, i => i % 2 == 0 ? '+' : '-');
				if (select) result = result.Select(SuperEnumerable.Identity);
				result.AssertSequenceEqual("hello-+-+-+-+-+".ToCharArray());
			}
		}
	}

	public class ReferenceTypeElements
	{
		public static IEnumerable<object[]> GetStringSequences()
		{
			var seq = Seq("foo", "bar", "baz");
			yield return new object[] { seq.AsTestingSequence(maxEnumerations: 2), false, };
			yield return new object[] { seq.AsBreakingList(), false, };
			yield return new object[] { seq.AsBreakingList(), true, };
		}

		[Theory]
		[MemberData(nameof(GetStringSequences))]
		public void PadWideSourceSequence(IDisposableEnumerable<string> seq, bool select)
		{
			using (seq)
			{
				var result = seq.Pad(2);
				if (select) result = result.Select(SuperEnumerable.Identity);
				result.AssertSequenceEqual("foo", "bar", "baz");
			}
		}

		[Fact]
		public void PadWideListBehavior()
		{
			using var seq = Seq("foo", "bar", "baz").AsBreakingList();

			var result = seq.Pad(2, x => $"Extra{x}");
			Assert.Equal(3, result.Count());
			Assert.Equal("bar", result.ElementAt(1));
			Assert.Equal("baz", result.ElementAt(^1));

			_ = Assert.Throws<ArgumentOutOfRangeException>(
				"index",
				() => result.ElementAt(3));
		}

		[Theory]
		[MemberData(nameof(GetStringSequences))]
		public void PadEqualSourceSequence(IDisposableEnumerable<string> seq, bool select)
		{
			using (seq)
			{
				var result = seq.Pad(3);
				if (select) result = result.Select(SuperEnumerable.Identity);
				result.AssertSequenceEqual("foo", "bar", "baz");
			}
		}

		[Theory]
		[MemberData(nameof(GetStringSequences))]
		public void PadNarrowSourceSequenceWithDefaultPadding(IDisposableEnumerable<string> seq, bool select)
		{
			using (seq)
			{
				var result = seq.Pad(5);
				if (select) result = result.Select(SuperEnumerable.Identity);
				result.AssertSequenceEqual("foo", "bar", "baz", null, null);
			}
		}

		[Theory]
		[MemberData(nameof(GetStringSequences))]
		public void PadNarrowSourceSequenceWithNonDefaultPadding(IDisposableEnumerable<string> seq, bool select)
		{
			using (seq)
			{
				var result = seq.Pad(5, string.Empty);
				if (select) result = result.Select(SuperEnumerable.Identity);
				result.AssertSequenceEqual("foo", "bar", "baz", string.Empty, string.Empty);
			}
		}

		[Theory]
		[MemberData(nameof(GetStringSequences))]
		public void PadNarrowSourceSequenceWithDynamicPadding(IDisposableEnumerable<string> seq, bool select)
		{
			using (seq)
			{
				var result = seq.Pad(5, x => $"Extra{x}");
				if (select) result = result.Select(SuperEnumerable.Identity);
				result.AssertSequenceEqual("foo", "bar", "baz", "Extra3", "Extra4");
			}
		}

		[Fact]
		public void PadNarrowListBehavior()
		{
			using var seq = Seq("foo", "bar", "baz").AsBreakingList();

			var result = seq.Pad(5, x => $"Extra{x}");
			Assert.Equal(5, result.Count());
			Assert.Equal("bar", result.ElementAt(1));
			Assert.Equal("Extra4", result.ElementAt(^1));

			_ = Assert.Throws<ArgumentOutOfRangeException>(
				"index",
				() => result.ElementAt(5));
		}
	}
}

namespace Test;

public class MaxElementsByTest
{
	[Fact]
	public void MaxElementsByIsLazy()
	{
		new BreakingSequence<int>().MaxElementsBy(BreakingFunc.Of<int, int>());
	}

	[Fact]
	public void MaxElementsByReturnsMaxima()
	{
		Assert.Equal(new[] { "hello", "world" },
						SampleData.Strings.MaxElementsBy(x => x.Length));
	}

	[Fact]
	public void MaxElementsByNullComparer()
	{
		Assert.Equal(SampleData.Strings.MaxElementsBy(x => x.Length),
						SampleData.Strings.MaxElementsBy(x => x.Length, null));
	}

	[Fact]
	public void MaxElementsByEmptySequence()
	{
		Assert.Empty(Array.Empty<string>().MaxElementsBy(x => x.Length));
	}

	[Fact]
	public void MaxElementsByWithNaturalComparer()
	{
		Assert.Equal(new[] { "az" }, SampleData.Strings.MaxElementsBy(x => x[1]));
	}

	[Fact]
	public void MaxElementsByWithComparer()
	{
		Assert.Equal(new[] { "aa" }, SampleData.Strings.MaxElementsBy(x => x[1], Comparable<char>.DescendingOrderComparer));
	}

	public class First
	{
		[Fact]
		public void ReturnsMaximum()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var maxima = strings.MaxElementsBy(s => s.Length);
			Assert.Equal("hello", maxima.First());
		}

		[Fact]
		public void WithComparerReturnsMaximum()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var maxima = strings.MaxElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
			Assert.Equal("ax", maxima.First());
		}

		[Fact]
		public void WithEmptySourceThrows()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			Assert.Throws<InvalidOperationException>(() =>
				strings.MaxElementsBy(s => s.Length).First());
		}

		[Fact]
		public void WithEmptySourceWithComparerThrows()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			Assert.Throws<InvalidOperationException>(() =>
				strings.MaxElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer).First());
		}
	}

	public class FirstOrDefault
	{
		[Fact]
		public void ReturnsMaximum()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var maxima = strings.MaxElementsBy(s => s.Length);
			Assert.Equal("hello", maxima.FirstOrDefault());
		}

		[Fact]
		public void WithComparerReturnsMaximum()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var maxima = strings.MaxElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
			Assert.Equal("ax", maxima.FirstOrDefault());
		}

		[Fact]
		public void WithEmptySourceReturnsDefault()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			var maxima = strings.MaxElementsBy(s => s.Length);
			Assert.Null(maxima.FirstOrDefault());
		}

		[Fact]
		public void WithEmptySourceWithComparerReturnsDefault()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			var maxima = strings.MaxElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
			Assert.Null(maxima.FirstOrDefault());
		}
	}

	public class Last
	{
		[Fact]
		public void ReturnsMaximum()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var maxima = strings.MaxElementsBy(s => s.Length);
			Assert.Equal("world", maxima.Last());
		}

		[Fact]
		public void WithComparerReturnsMaximumPerComparer()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var maxima = strings.MaxElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
			Assert.Equal("az", maxima.Last());
		}

		[Fact]
		public void WithEmptySourceThrows()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			Assert.Throws<InvalidOperationException>(() =>
				strings.MaxElementsBy(s => s.Length).Last());
		}

		[Fact]
		public void WithEmptySourceWithComparerThrows()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			Assert.Throws<InvalidOperationException>(() =>
				strings.MaxElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer).Last());
		}
	}

	public class LastOrDefault
	{
		[Fact]
		public void ReturnsMaximum()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var maxima = strings.MaxElementsBy(s => s.Length);
			Assert.Equal("world", maxima.LastOrDefault());
		}

		[Fact]
		public void WithComparerReturnsMaximumPerComparer()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var maxima = strings.MaxElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
			Assert.Equal("az", maxima.LastOrDefault());
		}

		[Fact]
		public void WithEmptySourceReturnsDefault()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			var maxima = strings.MaxElementsBy(s => s.Length);
			Assert.Null(maxima.LastOrDefault());
		}

		[Fact]
		public void WithEmptySourceWithComparerReturnsDefault()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			var maxima = strings.MaxElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
			Assert.Null(maxima.LastOrDefault());
		}
	}

	public class Take
	{
		[Theory]
		[InlineData(0, new string[0])]
		[InlineData(1, new[] { "hello" })]
		[InlineData(2, new[] { "hello", "world" })]
		[InlineData(3, new[] { "hello", "world" })]
		public void ReturnsMaxima(int count, string[] expected)
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			Assert.Equal(expected, strings.MaxElementsBy(s => s.Length).Take(count));
		}

		[Theory]
		[InlineData(0, 0, new string[0])]
		[InlineData(3, 1, new[] { "aa" })]
		[InlineData(1, 0, new[] { "ax" })]
		[InlineData(2, 0, new[] { "ax", "aa" })]
		[InlineData(3, 0, new[] { "ax", "aa", "ab" })]
		[InlineData(4, 0, new[] { "ax", "aa", "ab", "ay" })]
		[InlineData(5, 0, new[] { "ax", "aa", "ab", "ay", "az" })]
		[InlineData(6, 0, new[] { "ax", "aa", "ab", "ay", "az" })]
		public void WithComparerReturnsMaximaPerComparer(int count, int index, string[] expected)
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			Assert.Equal(
				expected,
				strings.MaxElementsBy(s => s[index], Comparable<char>.DescendingOrderComparer)
						  .Take(count));
		}
	}

	public class TakeLast
	{
		[Theory]
		[InlineData(0, new string[0])]
		[InlineData(1, new[] { "world" })]
		[InlineData(2, new[] { "hello", "world" })]
		[InlineData(3, new[] { "hello", "world" })]
		public void TakeLastReturnsMaxima(int count, string[] expected)
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			Assert.Equal(expected, strings.MaxElementsBy(s => s.Length).TakeLast(count));
		}

		[Theory]
		[InlineData(0, 0, new string[0])]
		[InlineData(3, 1, new[] { "aa" })]
		[InlineData(1, 0, new[] { "az" })]
		[InlineData(2, 0, new[] { "ay", "az" })]
		[InlineData(3, 0, new[] { "ab", "ay", "az" })]
		[InlineData(4, 0, new[] { "aa", "ab", "ay", "az" })]
		[InlineData(5, 0, new[] { "ax", "aa", "ab", "ay", "az" })]
		[InlineData(6, 0, new[] { "ax", "aa", "ab", "ay", "az" })]
		public void WithComparerReturnsMaximaPerComparer(int count, int index, string[] expected)
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			Assert.Equal(expected, strings.MaxElementsBy(s => s[index], Comparable<char>.DescendingOrderComparer)
						  .TakeLast(count));
		}
	}
}

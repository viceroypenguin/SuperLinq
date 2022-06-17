namespace Test;

public class MinElementsByTest
{
	[Fact]
	public void MinElementsByIsLazy()
	{
		new BreakingSequence<int>().MinElementsBy(BreakingFunc.Of<int, int>());
	}

	[Fact]
	public void MinElementsByReturnsMinima()
	{
		Assert.Equal(new[] { "ax", "aa", "ab", "ay", "az" },
						SampleData.Strings.MinElementsBy(x => x.Length));
	}

	[Fact]
	public void MinElementsByNullComparer()
	{
		Assert.Equal(SampleData.Strings.MinElementsBy(x => x.Length),
						SampleData.Strings.MinElementsBy(x => x.Length, null));
	}

	[Fact]
	public void MinElementsByEmptySequence()
	{
		Assert.Empty(Array.Empty<string>().MinElementsBy(x => x.Length));
	}

	[Fact]
	public void MinElementsByWithNaturalComparer()
	{
		Assert.Equal(new[] { "aa" }, SampleData.Strings.MinElementsBy(x => x[1]));
	}

	[Fact]
	public void MinElementsByWithComparer()
	{
		Assert.Equal(new[] { "az" }, SampleData.Strings.MinElementsBy(x => x[1], Comparable<char>.DescendingOrderComparer));
	}

	public class First
	{
		[Fact]
		public void ReturnsMinimum()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var minima = strings.MinElementsBy(s => s.Length).First();
			Assert.Equal("ax", minima);
		}

		[Fact]
		public void WithComparerReturnsMinimum()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var minima = strings.MinElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
			Assert.Equal("hello", minima.First());
		}

		[Fact]
		public void WithEmptySourceThrows()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			Assert.Throws<InvalidOperationException>(() =>
				strings.MinElementsBy(s => s.Length).First());
		}

		[Fact]
		public void WithEmptySourceWithComparerThrows()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			Assert.Throws<InvalidOperationException>(() =>
				strings.MinElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer).First());
		}
	}

	public class FirstOrDefault
	{
		[Fact]
		public void ReturnsMinimum()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var minima = strings.MinElementsBy(s => s.Length);
			Assert.Equal("ax", minima.FirstOrDefault());
		}

		[Fact]
		public void WithComparerReturnsMinimum()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var minima = strings.MinElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
			Assert.Equal("hello", minima.FirstOrDefault());
		}

		[Fact]
		public void WithEmptySourceReturnsDefault()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			var minima = strings.MinElementsBy(s => s.Length);
			Assert.Null(minima.FirstOrDefault());
		}

		[Fact]
		public void WithEmptySourceWithComparerReturnsDefault()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			var minima = strings.MinElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
			Assert.Null(minima.FirstOrDefault());
		}
	}

	public class Last
	{
		[Fact]
		public void ReturnsMinimum()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var minima = strings.MinElementsBy(s => s.Length);
			Assert.Equal("az", minima.Last());
		}

		[Fact]
		public void WithComparerReturnsMinimumPerComparer()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var minima = strings.MinElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
			Assert.Equal("world", minima.Last());
		}

		[Fact]
		public void WithEmptySourceThrows()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			Assert.Throws<InvalidOperationException>(() =>
				strings.MinElementsBy(s => s.Length).Last());
		}

		[Fact]
		public void WithEmptySourceWithComparerThrows()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			Assert.Throws<InvalidOperationException>(() =>
				strings.MinElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer).Last());
		}
	}

	public class LastOrDefault
	{
		[Fact]
		public void ReturnsMinimum()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var minima = strings.MinElementsBy(s => s.Length);
			Assert.Equal("az", minima.LastOrDefault());
		}

		[Fact]
		public void WithComparerReturnsMinimumPerComparer()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var minima = strings.MinElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
			Assert.Equal("world", minima.LastOrDefault());
		}

		[Fact]
		public void WithEmptySourceReturnsDefault()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			var minima = strings.MinElementsBy(s => s.Length);
			Assert.Null(minima.LastOrDefault());
		}

		[Fact]
		public void WithEmptySourceWithComparerReturnsDefault()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			var minima = strings.MinElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
			Assert.Null(minima.LastOrDefault());
		}
	}

	public class Take
	{
		[Theory]
		[InlineData(0, new string[0])]
		[InlineData(1, new[] { "ax" })]
		[InlineData(2, new[] { "ax", "aa" })]
		[InlineData(3, new[] { "ax", "aa", "ab" })]
		[InlineData(4, new[] { "ax", "aa", "ab", "ay" })]
		[InlineData(5, new[] { "ax", "aa", "ab", "ay", "az" })]
		[InlineData(6, new[] { "ax", "aa", "ab", "ay", "az" })]
		public void ReturnsMinima(int count, string[] expected)
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			Assert.Equal(expected, strings.MinElementsBy(s => s.Length).Take(count));
		}

		[Theory]
		[InlineData(0, new string[0])]
		[InlineData(1, new[] { "hello", })]
		[InlineData(2, new[] { "hello", "world" })]
		[InlineData(3, new[] { "hello", "world" })]
		public void WithComparerReturnsMinimaPerComparer(int count, string[] expected)
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			Assert.Equal(expected, strings.MinElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer)
						  .Take(count));
		}
	}

	public class TakeLast
	{
		[Theory]
		[InlineData(0, new string[0])]
		[InlineData(1, new[] { "az" })]
		[InlineData(2, new[] { "ay", "az" })]
		[InlineData(3, new[] { "ab", "ay", "az" })]
		[InlineData(4, new[] { "aa", "ab", "ay", "az" })]
		[InlineData(5, new[] { "ax", "aa", "ab", "ay", "az" })]
		[InlineData(6, new[] { "ax", "aa", "ab", "ay", "az" })]
		public void ReturnsMinima(int count, string[] expected)
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			Assert.Equal(expected, strings.MinElementsBy(s => s.Length).TakeLast(count));
		}

		[Theory]
		[InlineData(0, new string[0])]
		[InlineData(1, new[] { "world", })]
		[InlineData(2, new[] { "hello", "world" })]
		[InlineData(3, new[] { "hello", "world" })]
		public void WithComparerReturnsMinimaPerComparer(int count, string[] expected)
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			Assert.Equal(expected, strings.MinElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer)
						  .TakeLast(count));
		}
	}
}

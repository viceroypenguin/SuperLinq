using NUnit.Framework;

namespace Test;

[TestFixture]
public class MaxElementsByTest
{
	[Test]
	public void MaxElementsByIsLazy()
	{
		new BreakingSequence<int>().MaxElementsBy(BreakingFunc.Of<int, int>());
	}

	[Test]
	public void MaxElementsByReturnsMaxima()
	{
		Assert.AreEqual(new[] { "hello", "world" },
						SampleData.Strings.MaxElementsBy(x => x.Length));
	}

	[Test]
	public void MaxElementsByNullComparer()
	{
		Assert.AreEqual(SampleData.Strings.MaxElementsBy(x => x.Length),
						SampleData.Strings.MaxElementsBy(x => x.Length, null));
	}

	[Test]
	public void MaxElementsByEmptySequence()
	{
		Assert.That(Array.Empty<string>().MaxElementsBy(x => x.Length), Is.Empty);
	}

	[Test]
	public void MaxElementsByWithNaturalComparer()
	{
		Assert.AreEqual(new[] { "az" }, SampleData.Strings.MaxElementsBy(x => x[1]));
	}

	[Test]
	public void MaxElementsByWithComparer()
	{
		Assert.AreEqual(new[] { "aa" }, SampleData.Strings.MaxElementsBy(x => x[1], Comparable<char>.DescendingOrderComparer));
	}

	public class First
	{
		[Test]
		public void ReturnsMaximum()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var maxima = strings.MaxElementsBy(s => s.Length);
			Assert.That(maxima.First(), Is.EqualTo("hello"));
		}

		[Test]
		public void WithComparerReturnsMaximum()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var maxima = strings.MaxElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
			Assert.That(maxima.First(), Is.EqualTo("ax"));
		}

		[Test]
		public void WithEmptySourceThrows()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			Assert.Throws<InvalidOperationException>(() =>
				strings.MaxElementsBy(s => s.Length).First());
		}

		[Test]
		public void WithEmptySourceWithComparerThrows()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			Assert.Throws<InvalidOperationException>(() =>
				strings.MaxElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer).First());
		}
	}

	public class FirstOrDefault
	{
		[Test]
		public void ReturnsMaximum()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var maxima = strings.MaxElementsBy(s => s.Length);
			Assert.That(maxima.FirstOrDefault(), Is.EqualTo("hello"));
		}

		[Test]
		public void WithComparerReturnsMaximum()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var maxima = strings.MaxElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
			Assert.That(maxima.FirstOrDefault(), Is.EqualTo("ax"));
		}

		[Test]
		public void WithEmptySourceReturnsDefault()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			var maxima = strings.MaxElementsBy(s => s.Length);
			Assert.That(maxima.FirstOrDefault(), Is.Null);
		}

		[Test]
		public void WithEmptySourceWithComparerReturnsDefault()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			var maxima = strings.MaxElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
			Assert.That(maxima.FirstOrDefault(), Is.Null);
		}
	}

	public class Last
	{
		[Test]
		public void ReturnsMaximum()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var maxima = strings.MaxElementsBy(s => s.Length);
			Assert.That(maxima.Last(), Is.EqualTo("world"));
		}

		[Test]
		public void WithComparerReturnsMaximumPerComparer()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var maxima = strings.MaxElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
			Assert.That(maxima.Last(), Is.EqualTo("az"));
		}

		[Test]
		public void WithEmptySourceThrows()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			Assert.Throws<InvalidOperationException>(() =>
				strings.MaxElementsBy(s => s.Length).Last());
		}

		[Test]
		public void WithEmptySourceWithComparerThrows()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			Assert.Throws<InvalidOperationException>(() =>
				strings.MaxElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer).Last());
		}
	}

	public class LastOrDefault
	{
		[Test]
		public void ReturnsMaximum()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var maxima = strings.MaxElementsBy(s => s.Length);
			Assert.That(maxima.LastOrDefault(), Is.EqualTo("world"));
		}

		[Test]
		public void WithComparerReturnsMaximumPerComparer()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var maxima = strings.MaxElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
			Assert.That(maxima.LastOrDefault(), Is.EqualTo("az"));
		}

		[Test]
		public void WithEmptySourceReturnsDefault()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			var maxima = strings.MaxElementsBy(s => s.Length);
			Assert.That(maxima.LastOrDefault(), Is.Null);
		}

		[Test]
		public void WithEmptySourceWithComparerReturnsDefault()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			var maxima = strings.MaxElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
			Assert.That(maxima.LastOrDefault(), Is.Null);
		}
	}

	public class Take
	{
		[TestCase(0, ExpectedResult = new string[0])]
		[TestCase(1, ExpectedResult = new[] { "hello" })]
		[TestCase(2, ExpectedResult = new[] { "hello", "world" })]
		[TestCase(3, ExpectedResult = new[] { "hello", "world" })]
		public string[] ReturnsMaxima(int count)
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			return strings.MaxElementsBy(s => s.Length).Take(count).ToArray();
		}

		[TestCase(0, 0, ExpectedResult = new string[0])]
		[TestCase(3, 1, ExpectedResult = new[] { "aa" })]
		[TestCase(1, 0, ExpectedResult = new[] { "ax" })]
		[TestCase(2, 0, ExpectedResult = new[] { "ax", "aa" })]
		[TestCase(3, 0, ExpectedResult = new[] { "ax", "aa", "ab" })]
		[TestCase(4, 0, ExpectedResult = new[] { "ax", "aa", "ab", "ay" })]
		[TestCase(5, 0, ExpectedResult = new[] { "ax", "aa", "ab", "ay", "az" })]
		[TestCase(6, 0, ExpectedResult = new[] { "ax", "aa", "ab", "ay", "az" })]
		public string[] WithComparerReturnsMaximaPerComparer(int count, int index)
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			return strings.MaxElementsBy(s => s[index], Comparable<char>.DescendingOrderComparer)
						  .Take(count)
						  .ToArray();
		}
	}

	public class TakeLast
	{
		[TestCase(0, ExpectedResult = new string[0])]
		[TestCase(1, ExpectedResult = new[] { "world" })]
		[TestCase(2, ExpectedResult = new[] { "hello", "world" })]
		[TestCase(3, ExpectedResult = new[] { "hello", "world" })]
		public string[] TakeLastReturnsMaxima(int count)
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			return strings.MaxElementsBy(s => s.Length).TakeLast(count).ToArray();
		}

		[TestCase(0, 0, ExpectedResult = new string[0])]
		[TestCase(3, 1, ExpectedResult = new[] { "aa" })]
		[TestCase(1, 0, ExpectedResult = new[] { "az" })]
		[TestCase(2, 0, ExpectedResult = new[] { "ay", "az" })]
		[TestCase(3, 0, ExpectedResult = new[] { "ab", "ay", "az" })]
		[TestCase(4, 0, ExpectedResult = new[] { "aa", "ab", "ay", "az" })]
		[TestCase(5, 0, ExpectedResult = new[] { "ax", "aa", "ab", "ay", "az" })]
		[TestCase(6, 0, ExpectedResult = new[] { "ax", "aa", "ab", "ay", "az" })]
		public string[] WithComparerReturnsMaximaPerComparer(int count, int index)
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			return strings.MaxElementsBy(s => s[index], Comparable<char>.DescendingOrderComparer)
						  .TakeLast(count)
						  .ToArray();
		}
	}
}

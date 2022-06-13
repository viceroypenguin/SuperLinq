using NUnit.Framework;

namespace SuperLinq.Test;

[TestFixture]
public class MinElementsByTest
{
	[Test]
	public void MinElementsByIsLazy()
	{
		new BreakingSequence<int>().MinElementsBy(BreakingFunc.Of<int, int>());
	}

	[Test]
	public void MinElementsByReturnsMinima()
	{
		Assert.AreEqual(new[] { "ax", "aa", "ab", "ay", "az" },
						SampleData.Strings.MinElementsBy(x => x.Length));
	}

	[Test]
	public void MinElementsByNullComparer()
	{
		Assert.AreEqual(SampleData.Strings.MinElementsBy(x => x.Length),
						SampleData.Strings.MinElementsBy(x => x.Length, null));
	}

	[Test]
	public void MinElementsByEmptySequence()
	{
		Assert.That(Array.Empty<string>().MinElementsBy(x => x.Length), Is.Empty);
	}

	[Test]
	public void MinElementsByWithNaturalComparer()
	{
		Assert.AreEqual(new[] { "aa" }, SampleData.Strings.MinElementsBy(x => x[1]));
	}

	[Test]
	public void MinElementsByWithComparer()
	{
		Assert.AreEqual(new[] { "az" }, SampleData.Strings.MinElementsBy(x => x[1], Comparable<char>.DescendingOrderComparer));
	}

	public class First
	{
		[Test]
		public void ReturnsMinimum()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var minima = strings.MinElementsBy(s => s.Length).First();
			Assert.That(minima, Is.EqualTo("ax"));
		}

		[Test]
		public void WithComparerReturnsMinimum()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var minima = strings.MinElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
			Assert.That(minima.First(), Is.EqualTo("hello"));
		}

		[Test]
		public void WithEmptySourceThrows()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			Assert.Throws<InvalidOperationException>(() =>
				strings.MinElementsBy(s => s.Length).First());
		}

		[Test]
		public void WithEmptySourceWithComparerThrows()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			Assert.Throws<InvalidOperationException>(() =>
				strings.MinElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer).First());
		}
	}

	public class FirstOrDefault
	{
		[Test]
		public void ReturnsMinimum()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var minima = strings.MinElementsBy(s => s.Length);
			Assert.That(minima.FirstOrDefault(), Is.EqualTo("ax"));
		}

		[Test]
		public void WithComparerReturnsMinimum()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var minima = strings.MinElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
			Assert.That(minima.FirstOrDefault(), Is.EqualTo("hello"));
		}

		[Test]
		public void WithEmptySourceReturnsDefault()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			var minima = strings.MinElementsBy(s => s.Length);
			Assert.That(minima.FirstOrDefault(), Is.Null);
		}

		[Test]
		public void WithEmptySourceWithComparerReturnsDefault()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			var minima = strings.MinElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
			Assert.That(minima.FirstOrDefault(), Is.Null);
		}
	}

	public class Last
	{
		[Test]
		public void ReturnsMinimum()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var minima = strings.MinElementsBy(s => s.Length);
			Assert.That(minima.Last(), Is.EqualTo("az"));
		}

		[Test]
		public void WithComparerReturnsMinimumPerComparer()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var minima = strings.MinElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
			Assert.That(minima.Last(), Is.EqualTo("world"));
		}

		[Test]
		public void WithEmptySourceThrows()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			Assert.Throws<InvalidOperationException>(() =>
				strings.MinElementsBy(s => s.Length).Last());
		}

		[Test]
		public void WithEmptySourceWithComparerThrows()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			Assert.Throws<InvalidOperationException>(() =>
				strings.MinElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer).Last());
		}
	}

	public class LastOrDefault
	{
		[Test]
		public void ReturnsMinimum()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var minima = strings.MinElementsBy(s => s.Length);
			Assert.That(minima.LastOrDefault(), Is.EqualTo("az"));
		}

		[Test]
		public void WithComparerReturnsMinimumPerComparer()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var minima = strings.MinElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
			Assert.That(minima.LastOrDefault(), Is.EqualTo("world"));
		}

		[Test]
		public void WithEmptySourceReturnsDefault()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			var minima = strings.MinElementsBy(s => s.Length);
			Assert.That(minima.LastOrDefault(), Is.Null);
		}

		[Test]
		public void WithEmptySourceWithComparerReturnsDefault()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			var minima = strings.MinElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
			Assert.That(minima.LastOrDefault(), Is.Null);
		}
	}

	public class Take
	{
		[TestCase(0, ExpectedResult = new string[0])]
		[TestCase(1, ExpectedResult = new[] { "ax" })]
		[TestCase(2, ExpectedResult = new[] { "ax", "aa" })]
		[TestCase(3, ExpectedResult = new[] { "ax", "aa", "ab" })]
		[TestCase(4, ExpectedResult = new[] { "ax", "aa", "ab", "ay" })]
		[TestCase(5, ExpectedResult = new[] { "ax", "aa", "ab", "ay", "az" })]
		[TestCase(6, ExpectedResult = new[] { "ax", "aa", "ab", "ay", "az" })]
		public string[] ReturnsMinima(int count)
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			return strings.MinElementsBy(s => s.Length).Take(count).ToArray();
		}

		[TestCase(0, ExpectedResult = new string[0])]
		[TestCase(1, ExpectedResult = new[] { "hello", })]
		[TestCase(2, ExpectedResult = new[] { "hello", "world" })]
		[TestCase(3, ExpectedResult = new[] { "hello", "world" })]
		public string[] WithComparerReturnsMinimaPerComparer(int count)
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			return strings.MinElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer)
						  .Take(count)
						  .ToArray();
		}
	}

	public class TakeLast
	{
		[TestCase(0, ExpectedResult = new string[0])]
		[TestCase(1, ExpectedResult = new[] { "az" })]
		[TestCase(2, ExpectedResult = new[] { "ay", "az" })]
		[TestCase(3, ExpectedResult = new[] { "ab", "ay", "az" })]
		[TestCase(4, ExpectedResult = new[] { "aa", "ab", "ay", "az" })]
		[TestCase(5, ExpectedResult = new[] { "ax", "aa", "ab", "ay", "az" })]
		[TestCase(6, ExpectedResult = new[] { "ax", "aa", "ab", "ay", "az" })]
		public string[] ReturnsMinima(int count)
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			return strings.MinElementsBy(s => s.Length).TakeLast(count).ToArray();
		}

		[TestCase(0, ExpectedResult = new string[0])]
		[TestCase(1, ExpectedResult = new[] { "world", })]
		[TestCase(2, ExpectedResult = new[] { "hello", "world" })]
		[TestCase(3, ExpectedResult = new[] { "hello", "world" })]
		public string[] WithComparerReturnsMinimaPerComparer(int count)
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			return strings.MinElementsBy(s => s.Length, Comparable<int>.DescendingOrderComparer)
						  .TakeLast(count)
						  .ToArray();
		}
	}
}

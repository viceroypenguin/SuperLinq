using System.Collections;

namespace Test;

public class FlattenTest
{
	// Flatten(this IEnumerable source)

	[Fact]
	public void Flatten()
	{
		using var l1 = Seq<object>(4, "foo").AsTestingSequence();
		using var l2 = Seq<object>(3, l1, 5, true).AsTestingSequence();
		using var l3 = Seq(7, 8, 9, 10).AsTestingSequence();
		using var source = Seq<object>(1, 2, l2, "bar", 6, l3).AsTestingSequence();

		source
			.Flatten()
			.AssertSequenceEqual(
				1,
				2,
				3,
				4,
				"foo",
				5,
				true,
				"bar",
				6,
				7,
				8,
				9,
				10);
	}

	[Fact]
	public void FlattenCast()
	{
		using var source = new object[]
		{
			1, 2, 3, 4, 5,
			new object[]
			{
				6, 7,
				new object[]
				{
					8, 9,
					new object[]
					{
						10, 11, 12,
					},
					13, 14, 15,
				},
				16, 17,
			},
			18, 19, 20,
		}.AsTestingSequence();

		source
			.Flatten()
			.Cast<int>()
			.AssertSequenceEqual(Enumerable.Range(1, 20));
	}

	[Fact]
	public void FlattenIsLazy()
	{
		new BreakingSequence<int>().Flatten();
	}

	[Fact]
	public void FlattenPredicate()
	{
		using var source = new object[]
		{
			1,
			2,
			3,
			"bar",
			new object[]
			{
				4,
				new[]
				{
					true, false
				},
				5,
			},
			6,
			7,
		}.AsTestingSequence();

		source
			.Flatten(obj => obj is not IEnumerable<bool>)
			.AssertSequenceEqual(
				1,
				2,
				3,
				'b',
				'a',
				'r',
				4,
				new[]
				{
					true,
					false
				},
				5,
				6,
				7);
	}

	[Fact]
	public void FlattenPredicateAlwaysFalse()
	{
		var orig = new object[]
		{
			1,
			2,
			3,
			"bar",
			new[]
			{
				true,
				false,
			},
			6
		};

		using var source = orig.AsTestingSequence();
		source
			.Flatten(_ => false)
			.AssertSequenceEqual(orig);
	}

	[Fact]
	public void FlattenPredicateAlwaysTrue()
	{
		using var source = new object[]
		{
			1,
			2,
			"bar",
			3,
			new[]
			{
				4,
				5,
			},
			6
		}.AsTestingSequence();

		source
			.Flatten(_ => true)
			.AssertSequenceEqual(
				1,
				2,
				'b',
				'a',
				'r',
				3,
				4,
				5,
				6);
	}

	[Fact]
	public void FlattenPredicateIsLazy()
	{
		new BreakingSequence<int>().Flatten(BreakingFunc.Of<object, bool>());
	}

	[Fact]
	public void FlattenFullIteratedDisposesInnerSequences()
	{
		using var inner1 = TestingSequence.Of(4, 5);
		using var inner2 = TestingSequence.Of(true, false);
		using var inner3 = TestingSequence.Of<object>(6, inner2, 7);
		using var source = TestingSequence.Of<object>(inner1, inner3);

		source.Flatten()
			.AssertSequenceEqual(4, 5, 6, true, false, 7);
	}

	[Fact]
	public void FlattenInterruptedIterationDisposesInnerSequences()
	{
		using var inner1 = TestingSequence.Of(4, 5);
		using var inner2 = SeqExceptionAt(3).AsTestingSequence();
		using var inner3 = TestingSequence.Of<object>(6, inner2, 7);
		using var source = TestingSequence.Of<object>(inner1, inner3);

		Assert.Throws<TestException>(() =>
			source.Flatten().Consume());
	}

	[Fact]
	public void FlattenEvaluatesInnerSequencesLazily()
	{
		using var source = new object[]
		{
			1, 2, 3, 4, 5,
			new object[]
			{
				6, 7,
				new object[]
				{
					8, 9,
					SeqExceptionAt(2).Select(x => x + 9),
					13, 14, 15,
				},
				16, 17,
			},
			18, 19, 20,
		}.AsTestingSequence(2);

		source
			.Flatten()
			.Take(10)
			.Cast<int>()
			.AssertSequenceEqual(Enumerable.Range(1, 10));

		Assert.Throws<TestException>(() =>
			source.Flatten().ElementAt(11));
	}

	// Flatten(this IEnumerable source, Func<object, IEnumerable> selector)

	[Fact]
	public void FlattenSelectorIsLazy()
	{
		new BreakingSequence<int>().Flatten(BreakingFunc.Of<object?, IEnumerable>());
	}

	[Fact]
	public void FlattenSelector()
	{
		using var source = new[]
		{
			new Series
			{
				Name = "series1",
				Attributes = new[]
				{
					new Attribute { Values = new[] { 1, 2 } },
					new Attribute { Values = new[] { 3, 4 } },
				},
			},
			new Series
			{
				Name = "series2",
				Attributes = new[]
				{
					new Attribute { Values = new[] { 5, 6 } },
				},
			},
		}.AsTestingSequence();

		source
			.Flatten(obj =>
				obj switch
				{
					string => null,
					IEnumerable inner => inner,
					Series s => new object[] { s.Name, s.Attributes },
					Attribute a => a.Values,
					_ => null,
				})
			.AssertSequenceEqual("series1", 1, 2, 3, 4, "series2", 5, 6);
	}

	[Fact]
	public void FlattenSelectorFilteringOnlyIntegers()
	{
		using var source = new object[]
		{
			true,
			false,
			1,
			"bar",
			new object[]
			{
				2,
				new[]
				{
					3,
				},
			},
			'c',
			4,
		}.AsTestingSequence();

		source
			.Flatten(obj =>
				obj switch
				{
					int => null,
					IEnumerable inner => inner,
					_ => Enumerable.Empty<object>(),
				})
			.AssertSequenceEqual(Enumerable.Range(1, 4).Cast<object>());
	}

	[Fact]
	public void FlattenSelectorWithTree()
	{
		var source = new Tree<int>
		(
			new Tree<int>
			(
				new Tree<int>(1),
				2,
				new Tree<int>(3)
			),
			4,
			new Tree<int>
			(
				new Tree<int>(5),
				6,
				new Tree<int>(7)
			)
		);

		new[] { source }
			.Flatten(obj =>
				obj switch
				{
					int => null,
					Tree<int> tree => new object?[] { tree.Left, tree.Value, tree.Right },
					IEnumerable inner => inner,
					_ => Enumerable.Empty<object>(),
				})
			.AssertSequenceEqual(Enumerable.Range(1, 7).Cast<object>());
	}

	private class Series
	{
		public string Name { get; init; } = string.Empty;
		public Attribute[] Attributes { get; init; } = Array.Empty<Attribute>();
	}

	private class Attribute
	{
		public int[] Values { get; init; } = Array.Empty<int>();
	}

	private class Tree<T>
	{
		public readonly T Value;
		public readonly Tree<T>? Left;
		public readonly Tree<T>? Right;

		public Tree(T value) : this(null, value, null) { }
		public Tree(Tree<T>? left, T value, Tree<T>? right)
		{
			Left = left;
			Value = value;
			Right = right;
		}
	}
}

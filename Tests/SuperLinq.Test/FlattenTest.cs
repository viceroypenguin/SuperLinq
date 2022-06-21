using System.Collections;

namespace Test;

public class FlattenTest
{
	// Flatten(this IEnumerable source)

	[Fact]
	public void Flatten()
	{
		var source = new object[]
		{
				1,
				2,
				new object[]
				{
					3,
					new object[]
					{
						4,
						"foo"
					},
					5,
					true,
				},
				"bar",
				6,
				new[]
				{
					7,
					8,
					9,
					10
				},
		};

		var result = source.Flatten();

		var expectations = new object[]
		{
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
				10
		};

		Assert.Equal(expectations, result);
	}

	[Fact]
	public void FlattenCast()
	{
		var source = new object[]
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
		};

		var result = source.Flatten().Cast<int>();
		var expectations = Enumerable.Range(1, 20);

		Assert.Equal(expectations, result);
	}

	[Fact]
	public void FlattenIsLazy()
	{
		new BreakingSequence<int>().Flatten();
	}

	// Flatten(this IEnumerable source, Func<IEnumerable, bool> predicate)

	[Fact]
	public void FlattenPredicate()
	{
		var source = new object[]
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
		};

		var result = source.Flatten(obj => !(obj is IEnumerable<bool>));

		var expectations = new object[]
		{
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
				7,
		};

		Assert.Equal(expectations, result);
	}

	[Fact]
	public void FlattenPredicateAlwaysFalse()
	{
		var source = new object[]
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

		var result = source.Flatten(_ => false);

		Assert.Equal(source, result);
	}

	[Fact]
	public void FlattenPredicateAlwaysTrue()
	{
		var source = new object[]
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
		};

		var result = source.Flatten(_ => true);

		var expectations = new object[]
		{
				1,
				2,
				'b',
				'a',
				'r',
				3,
				4,
				5,
				6
		};

		Assert.Equal(expectations, result);
	}

	[Fact]
	public void FlattenPredicateIsLazy()
	{
		new BreakingSequence<int>().Flatten(BreakingFunc.Of<object, bool>());
	}

	[Fact]
	public void FlattenFullIteratedDisposesInnerSequences()
	{
		var expectations = new object[]
		{
				4,
				5,
				6,
				true,
				false,
				7,
		};

		using var inner1 = TestingSequence.Of(4, 5);
		using var inner2 = TestingSequence.Of(true, false);
		using var inner3 = TestingSequence.Of<object>(6, inner2, 7);
		using var source = TestingSequence.Of<object>(inner1, inner3);

		Assert.Equal(expectations, source.Flatten());
	}

	[Fact]
	public void FlattenInterruptedIterationDisposesInnerSequences()
	{
		using var inner1 = TestingSequence.Of(4, 5);
		using var inner2 = SuperEnumerable.From(() => true,
											   () => false,
											   () => throw new TestException())
										 .AsTestingSequence();
		using var inner3 = TestingSequence.Of<object>(6, inner2, 7);
		using var source = TestingSequence.Of<object>(inner1, inner3);

		Assert.Throws<TestException>(() =>
			source.Flatten().Consume());
	}

	[Fact]
	public void FlattenEvaluatesInnerSequencesLazily()
	{
		var source = new object[]
		{
				1, 2, 3, 4, 5,
				new object[]
				{
					6, 7,
					new object[]
					{
						8, 9,
						SuperEnumerable.From
						(
							() => 10,
							() => throw new TestException(),
							() => 12
						),
						13, 14, 15,
					},
					16, 17,
				},
				18, 19, 20,
		};

		var result = source.Flatten().Cast<int>();
		var expectations = Enumerable.Range(1, 10);

		Assert.Equal(expectations, result.Take(10));

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
		var source = new[]
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
			};

		var result = source.Flatten(obj =>
			obj switch
			{
				string => null,
				IEnumerable inner => inner,
				Series s => new object[] { s.Name, s.Attributes },
				Attribute a => a.Values,
				_ => null,
			});

		var expectations = new object[] { "series1", 1, 2, 3, 4, "series2", 5, 6 };

		Assert.Equal(expectations, result);
	}

	[Fact]
	public void FlattenSelectorFilteringOnlyIntegers()
	{
		var source = new object[]
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
		};

		var result = source.Flatten(obj =>
			obj switch
			{
				int => null,
				IEnumerable inner => inner,
				_ => Enumerable.Empty<object>(),
			});

		var expectations = new object[] { 1, 2, 3, 4 };

		Assert.Equal(expectations, result);
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

		var result = new[] { source }.Flatten(obj =>
			obj switch
			{
				int => null,
				Tree<int> tree => new object?[] { tree.Left, tree.Value, tree.Right },
				IEnumerable inner => inner,
				_ => Enumerable.Empty<object>(),
			});

		var expectations = Enumerable.Range(1, 7).Cast<object?>();

		Assert.Equal(expectations, result);
	}

	class Series
	{
		public string Name { get; init; } = string.Empty;
		public Attribute[] Attributes { get; init; } = Array.Empty<Attribute>();
	}

	class Attribute
	{
		public int[] Values { get; init; } = Array.Empty<int>();
	}

	class Tree<T>
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

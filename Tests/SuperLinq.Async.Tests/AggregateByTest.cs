// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace SuperLinq.Async.Tests;

public sealed class AggregateByTest
{
	[Test]
	public async Task AggregateBy()
	{
		static async Task DoTest<TSource, TKey, TAccumulate>(
			IEnumerable<TSource> source,
			Func<TSource, TKey> keySelector,
			Func<TKey, TAccumulate> seedSelector,
			Func<TAccumulate, TSource, TAccumulate> func,
			IEqualityComparer<TKey>? comparer,
			IEnumerable<KeyValuePair<TKey, TAccumulate>> expected) where TKey : notnull
		{
			await using var ts = source.AsTestingSequence();
			await ts.AggregateBy(keySelector, seedSelector, func, comparer)
				.AssertCollectionEqual(expected);
		}

		await DoTest(
			source: Enumerable.Empty<int>(),
			keySelector: x => x,
			seedSelector: x => 0,
			func: (x, y) => x + y,
			comparer: null,
			expected: []
		);

		await DoTest(
			source: Enumerable.Range(0, 10),
			keySelector: x => x,
			seedSelector: x => 0,
			func: (x, y) => x + y,
			comparer: null,
			expected: Enumerable.Range(0, 10).Select(x => KeyValuePair.Create(x, x))
		);

		await DoTest(
			source: Enumerable.Range(5, 10),
			keySelector: x => true,
			seedSelector: x => 0,
			func: (x, y) => x + y,
			comparer: null,
			expected: [KeyValuePair.Create(key: true, 95)]
		);

		await DoTest(
			source: Enumerable.Range(0, 20),
			keySelector: x => x % 5,
			seedSelector: x => 0,
			func: (x, y) => x + y,
			comparer: null,
			expected: Enumerable.Range(0, 5).Select(x => KeyValuePair.Create(x, 30 + (4 * x)))
		);

		await DoTest(
			source: Enumerable.Repeat(5, 20),
			keySelector: x => x,
			seedSelector: x => 0,
			func: (x, y) => x + y,
			comparer: null,
			expected: Enumerable.Repeat(5, 1).Select(x => KeyValuePair.Create(x, 100))
		);

		await DoTest(
			source: ["Bob", "bob", "tim", "Bob", "Tim"],
			keySelector: x => x,
			seedSelector: x => "",
			func: (x, y) => x + y,
			comparer: null,
			expected:
			[
				KeyValuePair.Create("Bob", "BobBob"),
				KeyValuePair.Create("bob", "bob"),
				KeyValuePair.Create("tim", "tim"),
				KeyValuePair.Create("Tim", "Tim"),
			]
		);

		await DoTest(
			source: ["Bob", "bob", "tim", "Bob", "Tim"],
			keySelector: x => x,
			seedSelector: x => "",
			func: (x, y) => x + y,
			StringComparer.OrdinalIgnoreCase,
			expected:
			[
				KeyValuePair.Create("Bob", "BobbobBob"),
				KeyValuePair.Create("tim", "timTim"),
			]
		);

		await DoTest(
			source: new (string Name, int Age)[] { ("Tom", 20), ("Dick", 30), ("Harry", 40) },
			keySelector: x => x.Age,
			seedSelector: x => $"I am {x} and my name is ",
			func: (x, y) => x + y.Name,
			comparer: null,
			expected:
			[
				KeyValuePair.Create(20, "I am 20 and my name is Tom"),
				KeyValuePair.Create(30, "I am 30 and my name is Dick"),
				KeyValuePair.Create(40, "I am 40 and my name is Harry"),
			]
		);

		await DoTest(
			source: new (string Name, int Age)[] { ("Tom", 20), ("Dick", 20), ("Harry", 40) },
			keySelector: x => x.Age,
			seedSelector: x => $"I am {x} and my name is",
			func: (x, y) => $"{x} maybe {y.Name}",
			comparer: null,
			expected:
			[
				KeyValuePair.Create(20, "I am 20 and my name is maybe Tom maybe Dick"),
				KeyValuePair.Create(40, "I am 40 and my name is maybe Harry"),
			]
		);

		await DoTest(
			source: new (string Name, int Age)[] { ("Bob", 20), ("bob", 20), ("Harry", 20) },
			keySelector: x => x.Name,
			seedSelector: x => 0,
			func: (x, y) => x + y.Age,
			comparer: null,
			expected:
			[
				KeyValuePair.Create("Bob", 20),
				KeyValuePair.Create("bob", 20),
				KeyValuePair.Create("Harry", 20),
			]
		);

		await DoTest(
			source: new (string Name, int Age)[] { ("Bob", 20), ("bob", 30), ("Harry", 40) },
			keySelector: x => x.Name,
			seedSelector: x => 0,
			func: (x, y) => x + y.Age,
			comparer: StringComparer.OrdinalIgnoreCase,
			expected:
			[
				KeyValuePair.Create("Bob", 50),
				KeyValuePair.Create("Harry", 40),
			]
		);
	}

	[Test]
	public async Task GroupBy()
	{
		static IAsyncEnumerable<KeyValuePair<TKey, List<TSource>>> GroupBy<TSource, TKey>(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
			where TKey : notnull =>
			source.AggregateBy(
				keySelector,
				seedSelector: _ => new List<TSource>(),
				(group, element) => { group.Add(element); return group; });

		await using var ts = TestingSequence.Of(1, 2, 3, 4);
		var oddsEvens = await
			GroupBy(
				ts,
				i => i % 2 == 0)
			.ToDictionaryAsync(x => x.Key, x => x.Value);

		Assert.True(oddsEvens[true].CollectionEqual([2, 4,]));
		Assert.True(oddsEvens[false].CollectionEqual([1, 3,]));
	}

	[Test]
	public async Task LongCountBy()
	{
		static IAsyncEnumerable<KeyValuePair<TKey, long>> LongCountBy<TSource, TKey>(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
			where TKey : notnull =>
			source.AggregateBy(
				keySelector,
				seed: 0L,
				(count, _) => ++count);

		await using var ts = TestingSequence.Of(1, 2, 3, 4);
		var oddsEvens = LongCountBy(
			ts,
			i => i % 2 == 0);

		await oddsEvens
			.AssertCollectionEqual(new Dictionary<bool, long>
			{
				[false] = 2,
				[true] = 2,
			});
	}

	[Test]
	public async Task Score()
	{
		await using var data = TestingSequence.Of<(string id, int score)>(
			("0", 42),
			("1", 5),
			("2", 4),
			("1", 10),
			("0", 25)
		);

		var scores = await data
			.AggregateBy(
				keySelector: entry => entry.id,
				seed: 0,
				(totalScore, curr) => totalScore + curr.score)
			.ToDictionaryAsync(x => x.Key, x => x.Value);

		Assert.Equal(67, scores["0"]);
		Assert.Equal(15, scores["1"]);
		Assert.Equal(4, scores["2"]);
	}
}

file static class KeyValuePair
{
	/// <summary>
	/// Creates a new key/value pair instance using provided values.
	/// </summary>
	/// <param name="key">The key of the new <see cref="KeyValuePair{TKey,TValue}"/> to be created.</param>
	/// <param name="value">The value of the new <see cref="KeyValuePair{TKey,TValue}"/> to be created.</param>
	/// <typeparam name="TKey">The type of the key.</typeparam>
	/// <typeparam name="TValue">The type of the value.</typeparam>
	/// <returns>A key/value pair containing the provided arguments as values.</returns>
	//Link: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.keyvaluepair.create
	public static KeyValuePair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value) =>
		new(key, value);
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Test.Async;

public sealed class AggregateByTest
{
	[Theory]
	[MemberData(nameof(AggregateBy_TestData))]
	public static async Task AggregateBy_HasExpectedOutput<TSource, TKey, TAccumulate>(
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

	public static IEnumerable<object?[]> AggregateBy_TestData()
	{
		yield return WrapArgs(
			source: Enumerable.Empty<int>(),
			keySelector: x => x,
			seedSelector: x => 0,
			func: (x, y) => x + y,
			comparer: null,
			expected: []);

		yield return WrapArgs(
			source: Enumerable.Range(0, 10),
			keySelector: x => x,
			seedSelector: x => 0,
			func: (x, y) => x + y,
			comparer: null,
			expected: Enumerable.Range(0, 10).ToDictionary(x => x, x => x));

		yield return WrapArgs(
			source: Enumerable.Range(5, 10),
			keySelector: x => true,
			seedSelector: x => 0,
			func: (x, y) => x + y,
			comparer: null,
			expected: Enumerable.Repeat(true, 1).ToDictionary(x => x, x => 95));

		yield return WrapArgs(
			source: Enumerable.Range(0, 20),
			keySelector: x => x % 5,
			seedSelector: x => 0,
			func: (x, y) => x + y,
			comparer: null,
			expected: Enumerable.Range(0, 5).ToDictionary(x => x, x => 30 + (4 * x)));

		yield return WrapArgs(
			source: Enumerable.Repeat(5, 20),
			keySelector: x => x,
			seedSelector: x => 0,
			func: (x, y) => x + y,
			comparer: null,
			expected: Enumerable.Repeat(5, 1).ToDictionary(x => x, x => 100));

		yield return WrapArgs(
			source: ["Bob", "bob", "tim", "Bob", "Tim"],
			keySelector: x => x,
			seedSelector: x => string.Empty,
			func: (x, y) => x + y,
			comparer: null,
			expected: new Dictionary<string, string>(StringComparer.Ordinal)
			{
				{ "Bob", "BobBob" },
				{ "bob", "bob" },
				{ "tim", "tim" },
				{ "Tim", "Tim" },
			});

		yield return WrapArgs(
			source: ["Bob", "bob", "tim", "Bob", "Tim"],
			keySelector: x => x,
			seedSelector: x => string.Empty,
			func: (x, y) => x + y,
			StringComparer.OrdinalIgnoreCase,
			expected: new Dictionary<string, string>(StringComparer.Ordinal)
			{
				{ "Bob", "BobbobBob" },
				{ "tim", "timTim" },
			});

		yield return WrapArgs(
			source: new (string Name, int Age)[] { ("Tom", 20), ("Dick", 30), ("Harry", 40) },
			keySelector: x => x.Age,
			seedSelector: x => $"I am {x} and my name is ",
			func: (x, y) => x + y.Name,
			comparer: null,
			expected: new Dictionary<int, string>
			{
				{ 20, "I am 20 and my name is Tom" },
				{ 30, "I am 30 and my name is Dick" },
				{ 40, "I am 40 and my name is Harry" },
			});

		yield return WrapArgs(
			source: new (string Name, int Age)[] { ("Tom", 20), ("Dick", 20), ("Harry", 40) },
			keySelector: x => x.Age,
			seedSelector: x => $"I am {x} and my name is",
			func: (x, y) => $"{x} maybe {y.Name}",
			comparer: null,
			expected: new Dictionary<int, string>
			{
				{ 20, "I am 20 and my name is maybe Tom maybe Dick" },
				{ 40, "I am 40 and my name is maybe Harry" },
			});

		yield return WrapArgs(
			source: new (string Name, int Age)[] { ("Bob", 20), ("bob", 20), ("Harry", 20) },
			keySelector: x => x.Name,
			seedSelector: x => 0,
			func: (x, y) => x + y.Age,
			comparer: null,
			expected: new string[] { "Bob", "bob", "Harry" }
				.ToDictionary(x => x, x => 20, StringComparer.Ordinal));

		yield return WrapArgs(
			source: new (string Name, int Age)[] { ("Bob", 20), ("bob", 30), ("Harry", 40) },
			keySelector: x => x.Name,
			seedSelector: x => 0,
			func: (x, y) => x + y.Age,
			comparer: StringComparer.OrdinalIgnoreCase,
			expected: new Dictionary<string, int>(StringComparer.Ordinal)
			{
				{ "Bob", 50 },
				{ "Harry", 40 },
			});

		static object?[] WrapArgs<TSource, TKey, TAccumulate>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, TAccumulate> seedSelector, Func<TAccumulate, TSource, TAccumulate> func, IEqualityComparer<TKey>? comparer, IEnumerable<KeyValuePair<TKey, TAccumulate>> expected)
			=> [source, keySelector, seedSelector, func, comparer, expected];
	}

	[Fact]
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

	[Fact]
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

	[Fact]
	public async Task Score()
	{
		await using var data = TestingSequence.Of<(string id, int score)>(
			("0", 42),
			("1", 5),
			("2", 4),
			("1", 10),
			("0", 25));

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

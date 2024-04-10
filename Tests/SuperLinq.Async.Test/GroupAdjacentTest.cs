namespace Test.Async;

public sealed class GroupAdjacentTest
{
	[Fact]
	public void GroupAdjacentIsLazy()
	{
		var bs = new AsyncBreakingSequence<object>();
		var bf = BreakingFunc.Of<object, int>();
		var bfo = BreakingFunc.Of<object, object>();
		var bfg = BreakingFunc.Of<int, IEnumerable<object>, IEnumerable<object>>();

		_ = bs.GroupAdjacent(bf);
		_ = bs.GroupAdjacent(bf, bfo);
		_ = bs.GroupAdjacent(bf, bfo, EqualityComparer<int>.Default);
		_ = bs.GroupAdjacent(bf, EqualityComparer<int>.Default);
		_ = bs.GroupAdjacent(bf, bfg);
		_ = bs.GroupAdjacent(bf, bfg, EqualityComparer<int>.Default);
	}

	[Fact]
	public async Task GroupAdjacentSourceSequence()
	{
		const string One = "one";
		const string Two = "two";
		const string Three = "three";
		const string Four = "four";
		const string Five = "five";
		const string Six = "six";
		const string Seven = "seven";
		const string Eight = "eight";
		const string Nine = "nine";
		const string Ten = "ten";

		await using var source = TestingSequence.Of(One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten);

		var groupings = source.GroupAdjacent(s => s.Length);

		await using var reader = groupings.Read();
		await AssertGrouping(reader, 3, One, Two);
		await AssertGrouping(reader, 5, Three);
		await AssertGrouping(reader, 4, Four, Five);
		await AssertGrouping(reader, 3, Six);
		await AssertGrouping(reader, 5, Seven, Eight);
		await AssertGrouping(reader, 4, Nine);
		await AssertGrouping(reader, 3, Ten);
		await reader.ReadEnd();
	}

	[Fact]
	public async Task GroupAdjacentSourceSequenceComparer()
	{
		await using var source = TestingSequence.Of("foo", "FOO", "Foo", "bar", "BAR", "Bar");

		var groupings = source.GroupAdjacent(SuperEnumerable.Identity, StringComparer.OrdinalIgnoreCase);

		await using var reader = groupings.Read();
		await AssertGrouping(reader, "foo", "foo", "FOO", "Foo");
		await AssertGrouping(reader, "bar", "bar", "BAR", "Bar");
		await reader.ReadEnd();
	}

	[Fact]
	public async Task GroupAdjacentSourceSequenceElementSelector()
	{
		await using var source = TestingSequence.Of(
			new { Month = 1, Value = 123 },
			new { Month = 1, Value = 456 },
			new { Month = 1, Value = 789 },
			new { Month = 2, Value = 987 },
			new { Month = 2, Value = 654 },
			new { Month = 2, Value = 321 },
			new { Month = 3, Value = 789 },
			new { Month = 3, Value = 456 },
			new { Month = 3, Value = 123 },
			new { Month = 1, Value = 123 },
			new { Month = 1, Value = 456 },
			new { Month = 1, Value = 781 });

		var groupings = source.GroupAdjacent(e => e.Month, e => e.Value * 2);

		await using var reader = groupings.Read();
		await AssertGrouping(reader, 1, 123 * 2, 456 * 2, 789 * 2);
		await AssertGrouping(reader, 2, 987 * 2, 654 * 2, 321 * 2);
		await AssertGrouping(reader, 3, 789 * 2, 456 * 2, 123 * 2);
		await AssertGrouping(reader, 1, 123 * 2, 456 * 2, 781 * 2);
		await reader.ReadEnd();
	}

	[Fact]
	public async Task GroupAdjacentSourceSequenceElementSelectorComparer()
	{
		await using var source = TestingSequence.Of(
			new { Month = "jan", Value = 123 },
			new { Month = "Jan", Value = 456 },
			new { Month = "JAN", Value = 789 },
			new { Month = "feb", Value = 987 },
			new { Month = "Feb", Value = 654 },
			new { Month = "FEB", Value = 321 },
			new { Month = "mar", Value = 789 },
			new { Month = "Mar", Value = 456 },
			new { Month = "MAR", Value = 123 },
			new { Month = "jan", Value = 123 },
			new { Month = "Jan", Value = 456 },
			new { Month = "JAN", Value = 781 });

		var groupings = source.GroupAdjacent(e => e.Month, e => e.Value * 2, StringComparer.OrdinalIgnoreCase);

		await using var reader = groupings.Read();
		await AssertGrouping(reader, "jan", 123 * 2, 456 * 2, 789 * 2);
		await AssertGrouping(reader, "feb", 987 * 2, 654 * 2, 321 * 2);
		await AssertGrouping(reader, "mar", 789 * 2, 456 * 2, 123 * 2);
		await AssertGrouping(reader, "jan", 123 * 2, 456 * 2, 781 * 2);
		await reader.ReadEnd();
	}

	[Fact]
	public async Task GroupAdjacentSourceSequenceResultSelector()
	{
		await using var source = TestingSequence.Of(
			new { Month = 1, Value = 123 },
			new { Month = 1, Value = 456 },
			new { Month = 1, Value = 789 },
			new { Month = 2, Value = 987 },
			new { Month = 2, Value = 654 },
			new { Month = 2, Value = 321 },
			new { Month = 3, Value = 789 },
			new { Month = 3, Value = 456 },
			new { Month = 3, Value = 123 },
			new { Month = 1, Value = 123 },
			new { Month = 1, Value = 456 },
			new { Month = 1, Value = 781 });

		var groupings = source.GroupAdjacent(e => e.Month, (key, group) => group.Sum(v => v.Value));

		await using var reader = groupings.Read();
		await AssertResult(reader, 123 + 456 + 789);
		await AssertResult(reader, 987 + 654 + 321);
		await AssertResult(reader, 789 + 456 + 123);
		await AssertResult(reader, 123 + 456 + 781);
		await reader.ReadEnd();
	}

	[Fact]
	public async Task GroupAdjacentSourceSequenceResultSelectorComparer()
	{
		await using var source = TestingSequence.Of(
			new { Month = "jan", Value = 123 },
			new { Month = "Jan", Value = 456 },
			new { Month = "JAN", Value = 789 },
			new { Month = "feb", Value = 987 },
			new { Month = "Feb", Value = 654 },
			new { Month = "FEB", Value = 321 },
			new { Month = "mar", Value = 789 },
			new { Month = "Mar", Value = 456 },
			new { Month = "MAR", Value = 123 },
			new { Month = "jan", Value = 123 },
			new { Month = "Jan", Value = 456 },
			new { Month = "JAN", Value = 781 });

		var groupings = source.GroupAdjacent(e => e.Month, (key, group) => group.Sum(v => v.Value), StringComparer.OrdinalIgnoreCase);

		await using var reader = groupings.Read();
		await AssertResult(reader, 123 + 456 + 789);
		await AssertResult(reader, 987 + 654 + 321);
		await AssertResult(reader, 789 + 456 + 123);
		await AssertResult(reader, 123 + 456 + 781);
		await reader.ReadEnd();
	}

	[Fact]
	public async Task GroupAdjacentSourceSequenceWithSomeNullKeys()
	{
		using var source = AsyncEnumerable.Range(1, 5)
			.SelectMany(x => Enumerable.Repeat((int?)x, x).Append(null).ToAsyncEnumerable())
			.AsTestingSequence();

		var groupings = source.GroupAdjacent(SuperEnumerable.Identity);

		var aNull = new int?[] { null };

		await using var reader = groupings.Read();
		await AssertGrouping(reader, 1, 1);
		await AssertGrouping(reader, null, aNull);
		await AssertGrouping(reader, 2, 2, 2);
		await AssertGrouping(reader, null, aNull);
		await AssertGrouping(reader, 3, 3, 3, 3);
		await AssertGrouping(reader, null, aNull);
		await AssertGrouping(reader, 4, 4, 4, 4, 4);
		await AssertGrouping(reader, null, aNull);
		await AssertGrouping(reader, 5, 5, 5, 5, 5, 5);
		await AssertGrouping(reader, null, aNull);
		await reader.ReadEnd();
	}

	private static async Task AssertGrouping<TKey, TElement>(
		SequenceReader<IGrouping<TKey, TElement>> reader,
		TKey key, params TElement[] elements)
	{
		var grouping = await reader.Read();
		Assert.NotNull(grouping);
		Assert.Equal(key, grouping.Key);
		grouping.AssertSequenceEqual(elements);
	}

	private static async Task AssertResult<TElement>(SequenceReader<TElement> reader, TElement element)
	{
		var result = await reader.Read();
		Assert.NotNull(result);
		Assert.Equal(element, result);
	}
}

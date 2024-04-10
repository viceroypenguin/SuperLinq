namespace Test;

public sealed class GroupAdjacentTest
{
	[Fact]
	public void GroupAdjacentIsLazy()
	{
		var bs = new BreakingSequence<object>();
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
	public void GroupAdjacentSourceSequence()
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

		using var source = TestingSequence.Of(One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten);

		var groupings = source.GroupAdjacent(s => s.Length);

		using var reader = groupings.Read();
		AssertGrouping(reader, 3, One, Two);
		AssertGrouping(reader, 5, Three);
		AssertGrouping(reader, 4, Four, Five);
		AssertGrouping(reader, 3, Six);
		AssertGrouping(reader, 5, Seven, Eight);
		AssertGrouping(reader, 4, Nine);
		AssertGrouping(reader, 3, Ten);
		reader.ReadEnd();
	}

	[Fact]
	public void GroupAdjacentSourceSequenceComparer()
	{
		using var source = TestingSequence.Of("foo", "FOO", "Foo", "bar", "BAR", "Bar");

		var groupings = source.GroupAdjacent(SuperEnumerable.Identity, StringComparer.OrdinalIgnoreCase);

		using var reader = groupings.Read();
		AssertGrouping(reader, "foo", "foo", "FOO", "Foo");
		AssertGrouping(reader, "bar", "bar", "BAR", "Bar");
		reader.ReadEnd();
	}

	[Fact]
	public void GroupAdjacentSourceSequenceElementSelector()
	{
		using var source = TestingSequence.Of(
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

		using var reader = groupings.Read();
		AssertGrouping(reader, 1, 123 * 2, 456 * 2, 789 * 2);
		AssertGrouping(reader, 2, 987 * 2, 654 * 2, 321 * 2);
		AssertGrouping(reader, 3, 789 * 2, 456 * 2, 123 * 2);
		AssertGrouping(reader, 1, 123 * 2, 456 * 2, 781 * 2);
		reader.ReadEnd();
	}

	[Fact]
	public void GroupAdjacentSourceSequenceElementSelectorComparer()
	{
		using var source = TestingSequence.Of(
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

		using var reader = groupings.Read();
		AssertGrouping(reader, "jan", 123 * 2, 456 * 2, 789 * 2);
		AssertGrouping(reader, "feb", 987 * 2, 654 * 2, 321 * 2);
		AssertGrouping(reader, "mar", 789 * 2, 456 * 2, 123 * 2);
		AssertGrouping(reader, "jan", 123 * 2, 456 * 2, 781 * 2);
		reader.ReadEnd();
	}

	[Fact]
	public void GroupAdjacentSourceSequenceResultSelector()
	{
		using var source = TestingSequence.Of(
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

		using var reader = groupings.Read();
		AssertResult(reader, 123 + 456 + 789);
		AssertResult(reader, 987 + 654 + 321);
		AssertResult(reader, 789 + 456 + 123);
		AssertResult(reader, 123 + 456 + 781);
		reader.ReadEnd();
	}

	[Fact]
	public void GroupAdjacentSourceSequenceResultSelectorComparer()
	{
		using var source = TestingSequence.Of(
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

		using var reader = groupings.Read();
		AssertResult(reader, 123 + 456 + 789);
		AssertResult(reader, 987 + 654 + 321);
		AssertResult(reader, 789 + 456 + 123);
		AssertResult(reader, 123 + 456 + 781);
		reader.ReadEnd();
	}

	[Fact]
	public void GroupAdjacentSourceSequenceWithSomeNullKeys()
	{
		using var source = Enumerable.Range(1, 5)
			.SelectMany(x => Enumerable.Repeat((int?)x, x).Append(null))
			.AsTestingSequence();

		var groupings = source.GroupAdjacent(SuperEnumerable.Identity);

		var aNull = new int?[] { null };

		using var reader = groupings.Read();
		AssertGrouping(reader, 1, 1);
		AssertGrouping(reader, null, aNull);
		AssertGrouping(reader, 2, 2, 2);
		AssertGrouping(reader, null, aNull);
		AssertGrouping(reader, 3, 3, 3, 3);
		AssertGrouping(reader, null, aNull);
		AssertGrouping(reader, 4, 4, 4, 4, 4);
		AssertGrouping(reader, null, aNull);
		AssertGrouping(reader, 5, 5, 5, 5, 5, 5);
		AssertGrouping(reader, null, aNull);
		reader.ReadEnd();
	}

	private static void AssertGrouping<TKey, TElement>(SequenceReader<IGrouping<TKey, TElement>> reader,
		TKey key, params TElement[] elements)
	{
		var grouping = reader.Read();
		Assert.NotNull(grouping);
		Assert.Equal(key, grouping.Key);
		grouping.AssertSequenceEqual(elements);
	}

	private static void AssertResult<TElement>(SequenceReader<TElement> reader, TElement element)
	{
		var result = reader.Read();
		Assert.NotNull(result);
		Assert.Equal(element, result);
	}
}

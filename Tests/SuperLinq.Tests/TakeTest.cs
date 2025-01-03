// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if !NO_INDEX

namespace SuperLinq.Tests;

public sealed class TakeTest
{
	[Test]
	public void SameResultsRepeatCallsIntQuery()
	{
		var q = from x in new[] { 9999, 0, 888, -1, 66, -777, 1, 2, -12345 }
				where x > int.MinValue
				select x;

		Assert.Equal(q.Take(9), q.Take(9));

		Assert.Equal(q.Take(0..9), q.Take(0..9));
		Assert.Equal(q.Take(^9..9), q.Take(^9..9));
		Assert.Equal(q.Take(0..^0), q.Take(0..^0));
		Assert.Equal(q.Take(^9..^0), q.Take(^9..^0));
	}

	[Test]
	public void SameResultsRepeatCallsIntQueryIList()
	{
		var q = (from x in new[] { 9999, 0, 888, -1, 66, -777, 1, 2, -12345 }
				 where x > int.MinValue
				 select x).ToList();

		Assert.Equal(q.Take(9), q.Take(9));

		Assert.Equal(q.Take(0..9), q.Take(0..9));
		Assert.Equal(q.Take(^9..9), q.Take(^9..9));
		Assert.Equal(q.Take(0..^0), q.Take(0..^0));
		Assert.Equal(q.Take(^9..^0), q.Take(^9..^0));
	}

	[Test]
	public void SameResultsRepeatCallsStringQuery()
	{
		var q = from x in new[] { "!@#$%^", "C", "AAA", "", "Calling Twice", "SoS", "" }
				where !string.IsNullOrEmpty(x)
				select x;

		Assert.Equal(q.Take(7), q.Take(7));

		Assert.Equal(q.Take(0..7), q.Take(0..7));
		Assert.Equal(q.Take(^7..7), q.Take(^7..7));
		Assert.Equal(q.Take(0..^0), q.Take(0..^0));
		Assert.Equal(q.Take(^7..^0), q.Take(^7..^0));
	}

	[Test]
	public void SameResultsRepeatCallsStringQueryIList()
	{
		var q = (from x in new[] { "!@#$%^", "C", "AAA", "", "Calling Twice", "SoS", "" }
				 where !string.IsNullOrEmpty(x)
				 select x).ToList();

		Assert.Equal(q.Take(7), q.Take(7));

		Assert.Equal(q.Take(0..7), q.Take(0..7));
		Assert.Equal(q.Take(^7..7), q.Take(^7..7));
		Assert.Equal(q.Take(0..^0), q.Take(0..^0));
		Assert.Equal(q.Take(^7..^0), q.Take(^7..^0));
	}

	[Test]
	public void SourceEmptyCountPositive()
	{
		var source = Array.Empty<int>();
		Assert.Empty(source.Take(5));

		Assert.Empty(source.Take(0..5));
		Assert.Empty(source.Take(^5..5));
		Assert.Empty(source.Take(0..^0));
		Assert.Empty(source.Take(^5..^0));
	}

	[Test]
	public void SourceEmptyCountPositiveNotIList()
	{
		var source = Enumerable.Range(0, 0);
		Assert.Empty(source.Take(5));

		Assert.Empty(source.Take(0..5));
		Assert.Empty(source.Take(^5..5));
		Assert.Empty(source.Take(0..^0));
		Assert.Empty(source.Take(^5..^0));
	}

	[Test]
	public void SourceNonEmptyCountNegative()
	{
		var source = new[] { 2, 5, 9, 1 };
		Assert.Empty(source.Take(-5));

		Assert.Empty(source.Take(^9..0));
	}

	[Test]
	public void SourceNonEmptyCountNegativeNotIList()
	{
		var source = Seq(2, 5, 9, 1);
		Assert.Empty(source.Take(-5));

		Assert.Empty(source.Take(^9..0));
	}

	[Test]
	public void SourceNonEmptyCountZero()
	{
		var source = new[] { 2, 5, 9, 1 };
		Assert.Empty(source.Take(0));

		Assert.Empty(source.Take(0..0));
		Assert.Empty(source.Take(^4..0));
		Assert.Empty(source.Take(0..^4));
		Assert.Empty(source.Take(^4..^4));
	}

	[Test]
	public void SourceNonEmptyCountZeroNotIList()
	{
		var source = Seq(2, 5, 9, 1);
		Assert.Empty(source.Take(0));

		Assert.Empty(source.Take(0..0));
		Assert.Empty(source.Take(^4..0));
		Assert.Empty(source.Take(0..^4));
		Assert.Empty(source.Take(^4..^4));
	}

	[Test]
	public void SourceNonEmptyCountOne()
	{
		var source = new[] { 2, 5, 9, 1 };
		var expected = new[] { 2 };

		Assert.Equal(expected, source.Take(1));

		Assert.Equal(expected, source.Take(0..1));
		Assert.Equal(expected, source.Take(^4..1));
		Assert.Equal(expected, source.Take(0..^3));
		Assert.Equal(expected, source.Take(^4..^3));
	}

	[Test]
	public void SourceNonEmptyCountOneNotIList()
	{
		var source = Seq(2, 5, 9, 1);
		var expected = new[] { 2 };

		Assert.Equal(expected, source.Take(1));

		Assert.Equal(expected, source.Take(0..1));
		Assert.Equal(expected, source.Take(^4..1));
		Assert.Equal(expected, source.Take(0..^3));
		Assert.Equal(expected, source.Take(^4..^3));
	}

	[Test]
	public void SourceNonEmptyTakeAllExactly()
	{
		var source = new[] { 2, 5, 9, 1 };

		Assert.Equal(source, source.Take(source.Length));

		Assert.Equal(source, source.Take(0..source.Length));
		Assert.Equal(source, source.Take(^source.Length..source.Length));
		Assert.Equal(source, source.Take(0..^0));
		Assert.Equal(source, source.Take(^source.Length..^0));
	}

	[Test]
	public void SourceNonEmptyTakeAllExactlyNotIList()
	{
		var source = Seq(2, 5, 9, 1);

		Assert.Equal(source, source.Take(source.Count()));

		Assert.Equal(source, source.Take(0..source.Count()));
		Assert.Equal(source, source.Take(^source.Count()..source.Count()));
		Assert.Equal(source, source.Take(0..^0));
		Assert.Equal(source, source.Take(^source.Count()..^0));
	}

	[Test]
	public void SourceNonEmptyTakeAllButOne()
	{
		var source = new[] { 2, 5, 9, 1 };
		var expected = new[] { 2, 5, 9 };

		Assert.Equal(expected, source.Take(3));

		Assert.Equal(expected, source.Take(0..3));
		Assert.Equal(expected, source.Take(^4..3));
		Assert.Equal(expected, source.Take(0..^1));
		Assert.Equal(expected, source.Take(^4..^1));
	}

	[Test]
	public void RunOnce()
	{
		var source = new[] { 2, 5, 9, 1 };
		var expected = new[] { 2, 5, 9 };

		Assert.Equal(expected, source.Take(3));

		Assert.Equal(expected, source.Take(0..3));
		Assert.Equal(expected, source.Take(^4..3));
		Assert.Equal(expected, source.Take(0..^1));
		Assert.Equal(expected, source.Take(^4..^1));
	}

	[Test]
	public void SourceNonEmptyTakeAllButOneNotIList()
	{
		var source = Seq(2, 5, 9, 1);
		var expected = new[] { 2, 5, 9 };

		Assert.Equal(expected, source.Take(3));

		Assert.Equal(expected, source.Take(0..3));
		Assert.Equal(expected, source.Take(^4..3));
		Assert.Equal(expected, source.Take(0..^1));
		Assert.Equal(expected, source.Take(^4..^1));
	}

	[Test]
	public void SourceNonEmptyTakeExcessive()
	{
		var source = new int?[] { 2, 5, null, 9, 1 };

		Assert.Equal(source, source.Take(source.Length + 1));

		Assert.Equal(source, source.Take(0..(source.Length + 1)));
		Assert.Equal(source, source.Take(^(source.Length + 1)..(source.Length + 1)));
	}

	[Test]
	public void SourceNonEmptyTakeExcessiveNotIList()
	{
		var source = Seq<int?>(2, 5, null, 9, 1);

		Assert.Equal(source, source.Take(source.Count() + 1));

		Assert.Equal(source, source.Take(0..(source.Count() + 1)));
		Assert.Equal(source, source.Take(^(source.Count() + 1)..(source.Count() + 1)));
	}

	[Test]
	public void ThrowsOnNullSource()
	{
		int[] source = null!;
		_ = Assert.Throws<ArgumentNullException>("source", () => source.Take(5));

		_ = Assert.Throws<ArgumentNullException>("source", () => source.Take(0..5));
		_ = Assert.Throws<ArgumentNullException>("source", () => source.Take(^5..5));
		_ = Assert.Throws<ArgumentNullException>("source", () => source.Take(0..^0));
		_ = Assert.Throws<ArgumentNullException>("source", () => source.Take(^5..^0));
	}

	[Test]
	public void ForcedToEnumeratorDoesNotEnumerate()
	{
		var iterator1 = Enumerable.Range(0, 3).Take(2);
		// Don't insist on this behaviour, but check it's correct if it happens
		var en1 = iterator1 as IEnumerator<int>;
		Assert.False(en1 is not null && en1.MoveNext());

		var iterator2 = Enumerable.Range(0, 3).Take(0..2);
		var en2 = iterator2 as IEnumerator<int>;
		Assert.False(en2 is not null && en2.MoveNext());

		var iterator3 = Enumerable.Range(0, 3).Take(^3..2);
		var en3 = iterator3 as IEnumerator<int>;
		Assert.False(en3 is not null && en3.MoveNext());

		var iterator4 = Enumerable.Range(0, 3).Take(0..^1);
		var en4 = iterator4 as IEnumerator<int>;
		Assert.False(en4 is not null && en4.MoveNext());

		var iterator5 = Enumerable.Range(0, 3).Take(^3..^1);
		var en5 = iterator5 as IEnumerator<int>;
		Assert.False(en5 is not null && en5.MoveNext());
	}

#pragma warning disable xUnit2013 // Do not use equality check to check for collection size.
	[Test]
	public void Count()
	{
		Assert.Equal(2, Enumerable.Range(0, 3).Take(2).Count());
		Assert.Equal(2, new[] { 1, 2, 3 }.Take(2).Count());
		Assert.Equal(0, Enumerable.Range(0, 3).Take(0).Count());

		Assert.Equal(2, Enumerable.Range(0, 3).Take(0..2).Count());
		Assert.Equal(2, new[] { 1, 2, 3 }.Take(0..2).Count());
		Assert.Equal(0, Enumerable.Range(0, 3).Take(0..0).Count());

		Assert.Equal(2, Enumerable.Range(0, 3).Take(^3..2).Count());
		Assert.Equal(2, new[] { 1, 2, 3 }.Take(^3..2).Count());
		Assert.Equal(0, Enumerable.Range(0, 3).Take(^3..0).Count());

		Assert.Equal(2, Enumerable.Range(0, 3).Take(0..^1).Count());
		Assert.Equal(2, new[] { 1, 2, 3 }.Take(0..^1).Count());
		Assert.Equal(0, Enumerable.Range(0, 3).Take(0..^3).Count());

		Assert.Equal(2, Enumerable.Range(0, 3).Take(^3..^1).Count());
		Assert.Equal(2, new[] { 1, 2, 3 }.Take(^3..^1).Count());
		Assert.Equal(0, Enumerable.Range(0, 3).Take(^3..^3).Count());
	}
#pragma warning restore xUnit2013 // Do not use equality check to check for collection size.

	[Test]
	public void ForcedToEnumeratorDoesntEnumerateIList()
	{
		var iterator1 = Enumerable.Range(0, 3).ToList().Take(2);
		// Don't insist on this behaviour, but check it's correct if it happens
		var en1 = iterator1 as IEnumerator<int>;
		Assert.False(en1 is not null && en1.MoveNext());

		var iterator2 = Enumerable.Range(0, 3).ToList().Take(0..2);
		var en2 = iterator2 as IEnumerator<int>;
		Assert.False(en2 is not null && en2.MoveNext());

		var iterator3 = Enumerable.Range(0, 3).ToList().Take(^3..2);
		var en3 = iterator3 as IEnumerator<int>;
		Assert.False(en3 is not null && en3.MoveNext());

		var iterator4 = Enumerable.Range(0, 3).ToList().Take(0..^1);
		var en4 = iterator4 as IEnumerator<int>;
		Assert.False(en4 is not null && en4.MoveNext());

		var iterator5 = Enumerable.Range(0, 3).ToList().Take(^3..^1);
		var en5 = iterator5 as IEnumerator<int>;
		Assert.False(en5 is not null && en5.MoveNext());
	}

	[Test]
	public void FollowWithTake()
	{
		var source = new[] { 5, 6, 7, 8 };
		var expected = new[] { 5, 6 };
		Assert.Equal(expected, source.Take(5).Take(3).Take(2).Take(40));

		Assert.Equal(expected, source.Take(0..5).Take(0..3).Take(0..2).Take(0..40));
		Assert.Equal(expected, source.Take(^4..5).Take(^4..3).Take(^3..2).Take(^2..40));
		Assert.Equal(expected, source.Take(0..^0).Take(0..^1).Take(0..^1).Take(0..^0));
		Assert.Equal(expected, source.Take(^4..^0).Take(^4..^1).Take(^3..^1).Take(^2..^0));
	}

	[Test]
	public void FollowWithTakeNotIList()
	{
		var source = Enumerable.Range(5, 4);
		var expected = new[] { 5, 6 };
		Assert.Equal(expected, source.Take(5).Take(3).Take(2));

		Assert.Equal(expected, source.Take(0..5).Take(0..3).Take(0..2));
		Assert.Equal(expected, source.Take(^4..5).Take(^4..3).Take(^3..2));
		Assert.Equal(expected, source.Take(0..^0).Take(0..^1).Take(0..^1));
		Assert.Equal(expected, source.Take(^4..^0).Take(^4..^1).Take(^3..^1));
	}

	[Test]
	public void FollowWithSkip()
	{
		var source = new[] { 1, 2, 3, 4, 5, 6 };
		var expected = new[] { 3, 4, 5 };
		Assert.Equal(expected, source.Take(5).Skip(2).Skip(-4));

		Assert.Equal(expected, source.Take(0..5).Skip(2).Skip(-4));
		Assert.Equal(expected, source.Take(^6..5).Skip(2).Skip(-4));
		Assert.Equal(expected, source.Take(0..^1).Skip(2).Skip(-4));
		Assert.Equal(expected, source.Take(^6..^1).Skip(2).Skip(-4));
	}

	[Test]
	public void FollowWithSkipNotIList()
	{
		var source = Enumerable.Range(1, 6);
		var expected = new[] { 3, 4, 5 };
		Assert.Equal(expected, source.Take(5).Skip(2).Skip(-4));

		Assert.Equal(expected, source.Take(0..5).Skip(2).Skip(-4));
		Assert.Equal(expected, source.Take(^6..5).Skip(2).Skip(-4));
		Assert.Equal(expected, source.Take(0..^1).Skip(2).Skip(-4));
		Assert.Equal(expected, source.Take(^6..^1).Skip(2).Skip(-4));
	}

	[Test]
	public void ElementAt()
	{
		var source = new[] { 1, 2, 3, 4, 5, 6 };
		var taken0 = source.Take(3);
		Assert.Equal(1, taken0.ElementAt(0));
		Assert.Equal(3, taken0.ElementAt(2));
		_ = Assert.Throws<ArgumentOutOfRangeException>("index", () => taken0.ElementAt(-1));
		_ = Assert.Throws<ArgumentOutOfRangeException>("index", () => taken0.ElementAt(3));

		var taken1 = source.Take(0..3);
		Assert.Equal(1, taken1.ElementAt(0));
		Assert.Equal(3, taken1.ElementAt(2));
		_ = Assert.Throws<ArgumentOutOfRangeException>("index", () => taken1.ElementAt(-1));
		_ = Assert.Throws<ArgumentOutOfRangeException>("index", () => taken1.ElementAt(3));

		var taken2 = source.Take(^6..3);
		Assert.Equal(1, taken2.ElementAt(0));
		Assert.Equal(3, taken2.ElementAt(2));
		_ = Assert.Throws<ArgumentOutOfRangeException>("index", () => taken2.ElementAt(-1));
		_ = Assert.Throws<ArgumentOutOfRangeException>("index", () => taken2.ElementAt(3));

		var taken3 = source.Take(0..^3);
		Assert.Equal(1, taken3.ElementAt(0));
		Assert.Equal(3, taken3.ElementAt(2));
		_ = Assert.Throws<ArgumentOutOfRangeException>("index", () => taken3.ElementAt(-1));
		_ = Assert.Throws<ArgumentOutOfRangeException>("index", () => taken3.ElementAt(3));

		var taken4 = source.Take(^6..^3);
		Assert.Equal(1, taken4.ElementAt(0));
		Assert.Equal(3, taken4.ElementAt(2));
		_ = Assert.Throws<ArgumentOutOfRangeException>("index", () => taken4.ElementAt(-1));
		_ = Assert.Throws<ArgumentOutOfRangeException>("index", () => taken4.ElementAt(3));
	}

	[Test]
	public void ElementAtNotIList()
	{
		var source = Seq(1, 2, 3, 4, 5, 6);
		var taken0 = source.Take(3);
		Assert.Equal(1, taken0.ElementAt(0));
		Assert.Equal(3, taken0.ElementAt(2));
		_ = Assert.Throws<ArgumentOutOfRangeException>("index", () => taken0.ElementAt(-1));
		_ = Assert.Throws<ArgumentOutOfRangeException>("index", () => taken0.ElementAt(3));

		var taken1 = source.Take(0..3);
		Assert.Equal(1, taken1.ElementAt(0));
		Assert.Equal(3, taken1.ElementAt(2));
		_ = Assert.Throws<ArgumentOutOfRangeException>("index", () => taken1.ElementAt(-1));
		_ = Assert.Throws<ArgumentOutOfRangeException>("index", () => taken1.ElementAt(3));

		var taken2 = source.Take(^6..3);
		Assert.Equal(1, taken2.ElementAt(0));
		Assert.Equal(3, taken2.ElementAt(2));
		_ = Assert.Throws<ArgumentOutOfRangeException>("index", () => taken2.ElementAt(-1));
		_ = Assert.Throws<ArgumentOutOfRangeException>("index", () => taken2.ElementAt(3));

		var taken3 = source.Take(0..^3);
		Assert.Equal(1, taken3.ElementAt(0));
		Assert.Equal(3, taken3.ElementAt(2));
		_ = Assert.Throws<ArgumentOutOfRangeException>("index", () => taken3.ElementAt(-1));
		_ = Assert.Throws<ArgumentOutOfRangeException>("index", () => taken3.ElementAt(3));

		var taken4 = source.Take(^6..^3);
		Assert.Equal(1, taken4.ElementAt(0));
		Assert.Equal(3, taken4.ElementAt(2));
		_ = Assert.Throws<ArgumentOutOfRangeException>("index", () => taken4.ElementAt(-1));
		_ = Assert.Throws<ArgumentOutOfRangeException>("index", () => taken4.ElementAt(3));
	}

	[Test]
	public void ElementAtOrDefault()
	{
		var source = new[] { 1, 2, 3, 4, 5, 6 };
		var taken0 = source.Take(3);
		Assert.Equal(1, taken0.ElementAtOrDefault(0));
		Assert.Equal(3, taken0.ElementAtOrDefault(2));
		Assert.Equal(0, taken0.ElementAtOrDefault(-1));
		Assert.Equal(0, taken0.ElementAtOrDefault(3));

		var taken1 = source.Take(0..3);
		Assert.Equal(1, taken1.ElementAtOrDefault(0));
		Assert.Equal(3, taken1.ElementAtOrDefault(2));
		Assert.Equal(0, taken1.ElementAtOrDefault(-1));
		Assert.Equal(0, taken1.ElementAtOrDefault(3));

		var taken2 = source.Take(^6..3);
		Assert.Equal(1, taken2.ElementAtOrDefault(0));
		Assert.Equal(3, taken2.ElementAtOrDefault(2));
		Assert.Equal(0, taken2.ElementAtOrDefault(-1));
		Assert.Equal(0, taken2.ElementAtOrDefault(3));

		var taken3 = source.Take(0..^3);
		Assert.Equal(1, taken3.ElementAtOrDefault(0));
		Assert.Equal(3, taken3.ElementAtOrDefault(2));
		Assert.Equal(0, taken3.ElementAtOrDefault(-1));
		Assert.Equal(0, taken3.ElementAtOrDefault(3));

		var taken4 = source.Take(^6..^3);
		Assert.Equal(1, taken4.ElementAtOrDefault(0));
		Assert.Equal(3, taken4.ElementAtOrDefault(2));
		Assert.Equal(0, taken4.ElementAtOrDefault(-1));
		Assert.Equal(0, taken4.ElementAtOrDefault(3));
	}

	[Test]
	public void ElementAtOrDefaultNotIList()
	{
		var source = Seq(1, 2, 3, 4, 5, 6);
		var taken0 = source.Take(3);
		Assert.Equal(1, taken0.ElementAtOrDefault(0));
		Assert.Equal(3, taken0.ElementAtOrDefault(2));
		Assert.Equal(0, taken0.ElementAtOrDefault(-1));
		Assert.Equal(0, taken0.ElementAtOrDefault(3));

		var taken1 = source.Take(0..3);
		Assert.Equal(1, taken1.ElementAtOrDefault(0));
		Assert.Equal(3, taken1.ElementAtOrDefault(2));
		Assert.Equal(0, taken1.ElementAtOrDefault(-1));
		Assert.Equal(0, taken1.ElementAtOrDefault(3));

		var taken2 = source.Take(^6..3);
		Assert.Equal(1, taken2.ElementAtOrDefault(0));
		Assert.Equal(3, taken2.ElementAtOrDefault(2));
		Assert.Equal(0, taken2.ElementAtOrDefault(-1));
		Assert.Equal(0, taken2.ElementAtOrDefault(3));

		var taken3 = source.Take(0..^3);
		Assert.Equal(1, taken3.ElementAtOrDefault(0));
		Assert.Equal(3, taken3.ElementAtOrDefault(2));
		Assert.Equal(0, taken3.ElementAtOrDefault(-1));
		Assert.Equal(0, taken3.ElementAtOrDefault(3));

		var taken4 = source.Take(^6..^3);
		Assert.Equal(1, taken4.ElementAtOrDefault(0));
		Assert.Equal(3, taken4.ElementAtOrDefault(2));
		Assert.Equal(0, taken4.ElementAtOrDefault(-1));
		Assert.Equal(0, taken4.ElementAtOrDefault(3));
	}

	[Test]
	public void First()
	{
		var source = new[] { 1, 2, 3, 4, 5 };
		Assert.Equal(1, source.Take(1).First());
		Assert.Equal(1, source.Take(4).First());
		Assert.Equal(1, source.Take(40).First());
		_ = Assert.Throws<InvalidOperationException>(() => source.Take(0).First());
		_ = Assert.Throws<InvalidOperationException>(() => source.Skip(5).Take(10).First());

		Assert.Equal(1, source.Take(0..1).First());
		Assert.Equal(1, source.Take(0..4).First());
		Assert.Equal(1, source.Take(0..40).First());
		_ = Assert.Throws<InvalidOperationException>(() => source.Take(0..0).First());
		_ = Assert.Throws<InvalidOperationException>(() => source.Skip(5).Take(0..10).First());

		Assert.Equal(1, source.Take(^5..1).First());
		Assert.Equal(1, source.Take(^5..4).First());
		Assert.Equal(1, source.Take(^5..40).First());
		_ = Assert.Throws<InvalidOperationException>(() => source.Take(^5..0).First());
		_ = Assert.Throws<InvalidOperationException>(() => source.Skip(5).Take(^5..10).First());

		Assert.Equal(1, source.Take(0..^4).First());
		Assert.Equal(1, source.Take(0..^1).First());
		Assert.Equal(1, source.Take(0..^0).First());
		_ = Assert.Throws<InvalidOperationException>(() => source.Take(0..^5).First());
		_ = Assert.Throws<InvalidOperationException>(() => source.Skip(5).Take(0..^5).First());

		Assert.Equal(1, source.Take(^5..^4).First());
		Assert.Equal(1, source.Take(^5..^1).First());
		Assert.Equal(1, source.Take(^5..^0).First());
		_ = Assert.Throws<InvalidOperationException>(() => source.Take(^5..^5).First());
		_ = Assert.Throws<InvalidOperationException>(() => source.Skip(5).Take(^10..^0).First());
	}

	[Test]
	public void FirstNotIList()
	{
		var source = Seq(1, 2, 3, 4, 5);
		Assert.Equal(1, source.Take(1).First());
		Assert.Equal(1, source.Take(4).First());
		Assert.Equal(1, source.Take(40).First());
		_ = Assert.Throws<InvalidOperationException>(() => source.Take(0).First());
		_ = Assert.Throws<InvalidOperationException>(() => source.Skip(5).Take(10).First());

		Assert.Equal(1, source.Take(0..1).First());
		Assert.Equal(1, source.Take(0..4).First());
		Assert.Equal(1, source.Take(0..40).First());
		_ = Assert.Throws<InvalidOperationException>(() => source.Take(0..0).First());
		_ = Assert.Throws<InvalidOperationException>(() => source.Skip(5).Take(0..10).First());

		Assert.Equal(1, source.Take(^5..1).First());
		Assert.Equal(1, source.Take(^5..4).First());
		Assert.Equal(1, source.Take(^5..40).First());
		_ = Assert.Throws<InvalidOperationException>(() => source.Take(^5..0).First());
		_ = Assert.Throws<InvalidOperationException>(() => source.Skip(5).Take(^5..10).First());

		Assert.Equal(1, source.Take(0..^4).First());
		Assert.Equal(1, source.Take(0..^1).First());
		Assert.Equal(1, source.Take(0..^0).First());
		_ = Assert.Throws<InvalidOperationException>(() => source.Take(0..^5).First());
		_ = Assert.Throws<InvalidOperationException>(() => source.Skip(5).Take(0..^5).First());

		Assert.Equal(1, source.Take(^5..^4).First());
		Assert.Equal(1, source.Take(^5..^1).First());
		Assert.Equal(1, source.Take(^5..^0).First());
		_ = Assert.Throws<InvalidOperationException>(() => source.Take(^5..^5).First());
		_ = Assert.Throws<InvalidOperationException>(() => source.Skip(5).Take(^10..^0).First());
	}

	[Test]
	public void FirstOrDefault()
	{
		var source = new[] { 1, 2, 3, 4, 5 };
		Assert.Equal(1, source.Take(1).FirstOrDefault());
		Assert.Equal(1, source.Take(4).FirstOrDefault());
		Assert.Equal(1, source.Take(40).FirstOrDefault());
		Assert.Equal(0, source.Take(0).FirstOrDefault());
		Assert.Equal(0, source.Skip(5).Take(10).FirstOrDefault());

		Assert.Equal(1, source.Take(0..1).FirstOrDefault());
		Assert.Equal(1, source.Take(0..4).FirstOrDefault());
		Assert.Equal(1, source.Take(0..40).FirstOrDefault());
		Assert.Equal(0, source.Take(0..0).FirstOrDefault());
		Assert.Equal(0, source.Skip(5).Take(0..10).FirstOrDefault());

		Assert.Equal(1, source.Take(^5..1).FirstOrDefault());
		Assert.Equal(1, source.Take(^5..4).FirstOrDefault());
		Assert.Equal(1, source.Take(^5..40).FirstOrDefault());
		Assert.Equal(0, source.Take(^5..0).FirstOrDefault());
		Assert.Equal(0, source.Skip(5).Take(^10..10).FirstOrDefault());

		Assert.Equal(1, source.Take(0..^4).FirstOrDefault());
		Assert.Equal(1, source.Take(0..^1).FirstOrDefault());
		Assert.Equal(1, source.Take(0..^0).FirstOrDefault());
		Assert.Equal(0, source.Take(0..^5).FirstOrDefault());
		Assert.Equal(0, source.Skip(5).Take(0..^10).FirstOrDefault());

		Assert.Equal(1, source.Take(^5..^4).FirstOrDefault());
		Assert.Equal(1, source.Take(^5..^1).FirstOrDefault());
		Assert.Equal(1, source.Take(^5..^0).FirstOrDefault());
		Assert.Equal(0, source.Take(^5..^5).FirstOrDefault());
		Assert.Equal(0, source.Skip(5).Take(^10..^0).FirstOrDefault());
	}

	[Test]
	public void FirstOrDefaultNotIList()
	{
		var source = Seq(1, 2, 3, 4, 5);
		Assert.Equal(1, source.Take(1).FirstOrDefault());
		Assert.Equal(1, source.Take(4).FirstOrDefault());
		Assert.Equal(1, source.Take(40).FirstOrDefault());
		Assert.Equal(0, source.Take(0).FirstOrDefault());
		Assert.Equal(0, source.Skip(5).Take(10).FirstOrDefault());

		Assert.Equal(1, source.Take(0..1).FirstOrDefault());
		Assert.Equal(1, source.Take(0..4).FirstOrDefault());
		Assert.Equal(1, source.Take(0..40).FirstOrDefault());
		Assert.Equal(0, source.Take(0..0).FirstOrDefault());
		Assert.Equal(0, source.Skip(5).Take(0..10).FirstOrDefault());

		Assert.Equal(1, source.Take(^5..1).FirstOrDefault());
		Assert.Equal(1, source.Take(^5..4).FirstOrDefault());
		Assert.Equal(1, source.Take(^5..40).FirstOrDefault());
		Assert.Equal(0, source.Take(^5..0).FirstOrDefault());
		Assert.Equal(0, source.Skip(5).Take(^10..10).FirstOrDefault());

		Assert.Equal(1, source.Take(0..^4).FirstOrDefault());
		Assert.Equal(1, source.Take(0..^1).FirstOrDefault());
		Assert.Equal(1, source.Take(0..^0).FirstOrDefault());
		Assert.Equal(0, source.Take(0..^5).FirstOrDefault());
		Assert.Equal(0, source.Skip(5).Take(0..^10).FirstOrDefault());

		Assert.Equal(1, source.Take(^5..^4).FirstOrDefault());
		Assert.Equal(1, source.Take(^5..^1).FirstOrDefault());
		Assert.Equal(1, source.Take(^5..^0).FirstOrDefault());
		Assert.Equal(0, source.Take(^5..^5).FirstOrDefault());
		Assert.Equal(0, source.Skip(5).Take(^10..^0).FirstOrDefault());
	}

	[Test]
	public void Last()
	{
		var source = new[] { 1, 2, 3, 4, 5 };
		Assert.Equal(1, source.Take(1).Last());
		Assert.Equal(5, source.Take(5).Last());
		Assert.Equal(5, source.Take(40).Last());
		_ = Assert.Throws<InvalidOperationException>(() => source.Take(0).Last());
		_ = Assert.Throws<InvalidOperationException>(() => Array.Empty<int>().Take(40).Last());

		Assert.Equal(1, source.Take(0..1).Last());
		Assert.Equal(5, source.Take(0..5).Last());
		Assert.Equal(5, source.Take(0..40).Last());
		_ = Assert.Throws<InvalidOperationException>(() => source.Take(0..0).Last());
		_ = Assert.Throws<InvalidOperationException>(() => Array.Empty<int>().Take(0..40).Last());

		Assert.Equal(1, source.Take(^5..1).Last());
		Assert.Equal(5, source.Take(^5..5).Last());
		Assert.Equal(5, source.Take(^5..40).Last());
		_ = Assert.Throws<InvalidOperationException>(() => source.Take(^5..0).Last());
		_ = Assert.Throws<InvalidOperationException>(() => Array.Empty<int>().Take(^5..40).Last());

		Assert.Equal(1, source.Take(0..^4).Last());
		Assert.Equal(5, source.Take(0..^0).Last());
		Assert.Equal(5, source.Take(3..^0).Last());
		_ = Assert.Throws<InvalidOperationException>(() => source.Take(0..^5).Last());
		_ = Assert.Throws<InvalidOperationException>(() => Array.Empty<int>().Take(0..^0).Last());

		Assert.Equal(1, source.Take(^5..^4).Last());
		Assert.Equal(5, source.Take(^5..^0).Last());
		Assert.Equal(5, source.Take(^5..^0).Last());
		_ = Assert.Throws<InvalidOperationException>(() => source.Take(^5..^5).Last());
		_ = Assert.Throws<InvalidOperationException>(() => Array.Empty<int>().Take(^40..^0).Last());
	}

	[Test]
	public void LastNotIList()
	{
		var source = Seq(1, 2, 3, 4, 5);
		Assert.Equal(1, source.Take(1).Last());
		Assert.Equal(5, source.Take(5).Last());
		Assert.Equal(5, source.Take(40).Last());
		_ = Assert.Throws<InvalidOperationException>(() => source.Take(0).Last());
		_ = Assert.Throws<InvalidOperationException>(() => Enumerable.Empty<int>().Take(40).Last());

		Assert.Equal(1, source.Take(0..1).Last());
		Assert.Equal(5, source.Take(0..5).Last());
		Assert.Equal(5, source.Take(0..40).Last());
		_ = Assert.Throws<InvalidOperationException>(() => source.Take(0..0).Last());
		_ = Assert.Throws<InvalidOperationException>(() => Enumerable.Empty<int>().Take(0..40).Last());

		Assert.Equal(1, source.Take(^5..1).Last());
		Assert.Equal(5, source.Take(^5..5).Last());
		Assert.Equal(5, source.Take(^5..40).Last());
		_ = Assert.Throws<InvalidOperationException>(() => source.Take(^5..0).Last());
		_ = Assert.Throws<InvalidOperationException>(() => Enumerable.Empty<int>().Take(^5..40).Last());

		Assert.Equal(1, source.Take(0..^4).Last());
		Assert.Equal(5, source.Take(0..^0).Last());
		Assert.Equal(5, source.Take(3..^0).Last());
		_ = Assert.Throws<InvalidOperationException>(() => source.Take(0..^5).Last());
		_ = Assert.Throws<InvalidOperationException>(() => Enumerable.Empty<int>().Take(0..^0).Last());

		Assert.Equal(1, source.Take(^5..^4).Last());
		Assert.Equal(5, source.Take(^5..^0).Last());
		Assert.Equal(5, source.Take(^5..^0).Last());
		_ = Assert.Throws<InvalidOperationException>(() => source.Take(^5..^5).Last());
		_ = Assert.Throws<InvalidOperationException>(() => Enumerable.Empty<int>().Take(^40..^0).Last());
	}

	[Test]
	public void LastOrDefault()
	{
		var source = new[] { 1, 2, 3, 4, 5 };
		Assert.Equal(1, source.Take(1).LastOrDefault());
		Assert.Equal(5, source.Take(5).LastOrDefault());
		Assert.Equal(5, source.Take(40).LastOrDefault());
		Assert.Equal(0, source.Take(0).LastOrDefault());
		Assert.Equal(0, Array.Empty<int>().Take(40).LastOrDefault());

		Assert.Equal(1, source.Take(0..1).LastOrDefault());
		Assert.Equal(5, source.Take(0..5).LastOrDefault());
		Assert.Equal(5, source.Take(0..40).LastOrDefault());
		Assert.Equal(0, source.Take(0..0).LastOrDefault());
		Assert.Equal(0, Array.Empty<int>().Take(0..40).LastOrDefault());

		Assert.Equal(1, source.Take(^5..1).LastOrDefault());
		Assert.Equal(5, source.Take(^5..5).LastOrDefault());
		Assert.Equal(5, source.Take(^5..40).LastOrDefault());
		Assert.Equal(0, source.Take(^5..0).LastOrDefault());
		Assert.Equal(0, Array.Empty<int>().Take(^5..40).LastOrDefault());

		Assert.Equal(1, source.Take(0..^4).LastOrDefault());
		Assert.Equal(5, source.Take(0..^0).LastOrDefault());
		Assert.Equal(5, source.Take(3..^0).LastOrDefault());
		Assert.Equal(0, source.Take(0..^5).LastOrDefault());
		Assert.Equal(0, Array.Empty<int>().Take(0..^0).LastOrDefault());

		Assert.Equal(1, source.Take(^5..^4).LastOrDefault());
		Assert.Equal(5, source.Take(^5..^0).LastOrDefault());
		Assert.Equal(5, source.Take(^40..^0).LastOrDefault());
		Assert.Equal(0, source.Take(^5..^5).LastOrDefault());
		Assert.Equal(0, Array.Empty<int>().Take(^40..^0).LastOrDefault());
	}

	[Test]
	public void LastOrDefaultNotIList()
	{
		var source = Seq(1, 2, 3, 4, 5);
		Assert.Equal(1, source.Take(1).LastOrDefault());
		Assert.Equal(5, source.Take(5).LastOrDefault());
		Assert.Equal(5, source.Take(40).LastOrDefault());
		Assert.Equal(0, source.Take(0).LastOrDefault());
		Assert.Equal(0, Enumerable.Empty<int>().Take(40).LastOrDefault());

		Assert.Equal(1, source.Take(0..1).LastOrDefault());
		Assert.Equal(5, source.Take(0..5).LastOrDefault());
		Assert.Equal(5, source.Take(0..40).LastOrDefault());
		Assert.Equal(0, source.Take(0..0).LastOrDefault());
		Assert.Equal(0, Enumerable.Empty<int>().Take(0..40).LastOrDefault());

		Assert.Equal(1, source.Take(^5..1).LastOrDefault());
		Assert.Equal(5, source.Take(^5..5).LastOrDefault());
		Assert.Equal(5, source.Take(^5..40).LastOrDefault());
		Assert.Equal(0, source.Take(^5..0).LastOrDefault());
		Assert.Equal(0, Enumerable.Empty<int>().Take(^5..40).LastOrDefault());

		Assert.Equal(1, source.Take(0..^4).LastOrDefault());
		Assert.Equal(5, source.Take(0..^0).LastOrDefault());
		Assert.Equal(5, source.Take(3..^0).LastOrDefault());
		Assert.Equal(0, source.Take(0..^5).LastOrDefault());
		Assert.Equal(0, Enumerable.Empty<int>().Take(0..^0).LastOrDefault());

		Assert.Equal(1, source.Take(^5..^4).LastOrDefault());
		Assert.Equal(5, source.Take(^5..^0).LastOrDefault());
		Assert.Equal(5, source.Take(^40..^0).LastOrDefault());
		Assert.Equal(0, source.Take(^5..^5).LastOrDefault());
		Assert.Equal(0, Enumerable.Empty<int>().Take(^40..^0).LastOrDefault());
	}

	[Test]
	public void TakeCanOnlyBeOneList()
	{
		var source = new[] { 2, 4, 6, 8, 10 };
		Assert.Equal([2], source.Take(1));
		Assert.Equal([4], source.Skip(1).Take(1));
		Assert.Equal([6], source.Take(3).Skip(2));
		Assert.Equal([2], source.Take(3).Take(1));

		Assert.Equal([2], source.Take(0..1));
		Assert.Equal([4], source.Skip(1).Take(0..1));
		Assert.Equal([6], source.Take(0..3).Skip(2));
		Assert.Equal([2], source.Take(0..3).Take(0..1));

		Assert.Equal([2], source.Take(^5..1));
		Assert.Equal([4], source.Skip(1).Take(^4..1));
		Assert.Equal([6], source.Take(^5..3).Skip(2));
		Assert.Equal([2], source.Take(^5..3).Take(^4..1));

		Assert.Equal([2], source.Take(0..^4));
		Assert.Equal([4], source.Skip(1).Take(0..^3));
		Assert.Equal([6], source.Take(0..^2).Skip(2));
		Assert.Equal([2], source.Take(0..^2).Take(0..^2));

		Assert.Equal([2], source.Take(^5..^4));
		Assert.Equal([4], source.Skip(1).Take(^4..^3));
		Assert.Equal([6], source.Take(^5..^2).Skip(2));
		Assert.Equal([2], source.Take(^5..^2).Take(^4..^2));
	}

	[Test]
	public void TakeCanOnlyBeOneNotList()
	{
		var source = Seq(2, 4, 6, 8, 10);
		Assert.Equal([2], source.Take(1));
		Assert.Equal([4], source.Skip(1).Take(1));
		Assert.Equal([6], source.Take(3).Skip(2));
		Assert.Equal([2], source.Take(3).Take(1));

		Assert.Equal([2], source.Take(0..1));
		Assert.Equal([4], source.Skip(1).Take(0..1));
		Assert.Equal([6], source.Take(0..3).Skip(2));
		Assert.Equal([2], source.Take(0..3).Take(0..1));

		Assert.Equal([2], source.Take(^5..1));
		Assert.Equal([4], source.Skip(1).Take(^4..1));
		Assert.Equal([6], source.Take(^5..3).Skip(2));
		Assert.Equal([2], source.Take(^5..3).Take(^4..1));

		Assert.Equal([2], source.Take(0..^4));
		Assert.Equal([4], source.Skip(1).Take(0..^3));
		Assert.Equal([6], source.Take(0..^2).Skip(2));
		Assert.Equal([2], source.Take(0..^2).Take(0..^2));

		Assert.Equal([2], source.Take(^5..^4));
		Assert.Equal([4], source.Skip(1).Take(^4..^3));
		Assert.Equal([6], source.Take(^5..^2).Skip(2));
		Assert.Equal([2], source.Take(^5..^2).Take(^4..^2));
	}

	[Test]
	public void RepeatEnumerating()
	{
		var source = new[] { 1, 2, 3, 4, 5 };
		var taken1 = source.Take(3);
		Assert.Equal(taken1, taken1);

		var taken2 = source.Take(0..3);
		Assert.Equal(taken2, taken2);

		var taken3 = source.Take(^5..3);
		Assert.Equal(taken3, taken3);

		var taken4 = source.Take(0..^2);
		Assert.Equal(taken4, taken4);

		var taken5 = source.Take(^5..^2);
		Assert.Equal(taken5, taken5);
	}

	[Test]
	public void RepeatEnumeratingNotList()
	{
		var source = Seq(1, 2, 3, 4, 5);
		var taken1 = source.Take(3);
		Assert.Equal(taken1, taken1);

		var taken2 = source.Take(0..3);
		Assert.Equal(taken2, taken2);

		var taken3 = source.Take(^5..3);
		Assert.Equal(taken3, taken3);

		var taken4 = source.Take(0..^2);
		Assert.Equal(taken4, taken4);

		var taken5 = source.Take(^5..^2);
		Assert.Equal(taken5, taken5);
	}

	[Test]
	public void LazyOverflowRegression()
	{
		var range = Enumerable.Range(1, 100);
		var skipped = range.Skip(42); // Min index is 42.
		var taken1 = skipped.Take(int.MaxValue); // May try to calculate max index as 42 + int.MaxValue, leading to integer overflow.
		Assert.Equal(Enumerable.Range(43, 100 - 42), taken1);
		Assert.Equal(100 - 42, taken1.Count());
		Assert.Equal(Enumerable.Range(43, 100 - 42), taken1.ToArray());
		Assert.Equal(Enumerable.Range(43, 100 - 42), taken1.ToList());

		var taken2 = Enumerable.Range(1, 100).Take(42..int.MaxValue);
		Assert.Equal(Enumerable.Range(43, 100 - 42), taken2);
		Assert.Equal(100 - 42, taken2.Count());
		Assert.Equal(Enumerable.Range(43, 100 - 42), taken2.ToArray());
		Assert.Equal(Enumerable.Range(43, 100 - 42), taken2.ToList());

		var taken3 = Enumerable.Range(1, 100).Take(^(100 - 42)..int.MaxValue);
		Assert.Equal(Enumerable.Range(43, 100 - 42), taken3);
		Assert.Equal(100 - 42, taken3.Count());
		Assert.Equal(Enumerable.Range(43, 100 - 42), taken3.ToArray());
		Assert.Equal(Enumerable.Range(43, 100 - 42), taken3.ToList());

		var taken4 = Enumerable.Range(1, 100).Take(42..^0);
		Assert.Equal(Enumerable.Range(43, 100 - 42), taken4);
		Assert.Equal(100 - 42, taken4.Count());
		Assert.Equal(Enumerable.Range(43, 100 - 42), taken4.ToArray());
		Assert.Equal(Enumerable.Range(43, 100 - 42), taken4.ToList());

		var taken5 = Enumerable.Range(1, 100).Take(^(100 - 42)..^0);
		Assert.Equal(Enumerable.Range(43, 100 - 42), taken5);
		Assert.Equal(100 - 42, taken5.Count());
		Assert.Equal(Enumerable.Range(43, 100 - 42), taken5.ToArray());
		Assert.Equal(Enumerable.Range(43, 100 - 42), taken5.ToList());
	}

	[Test]
	[Arguments(0, 0, 0)]
	[Arguments(1, 1, 1)]
	[Arguments(0, int.MaxValue, 100)]
	[Arguments(int.MaxValue, 0, 0)]
	[Arguments(0xffff, 1, 0)]
	[Arguments(1, 0xffff, 99)]
	[Arguments(int.MaxValue, int.MaxValue, 0)]
	[Arguments(1, int.MaxValue, 99)] // Regression test: The max index is precisely int.MaxValue.
	[Arguments(0, 100, 100)]
	[Arguments(10, 100, 90)]
	public void CountOfLazySkipTakeChain(int skip, int take, int expected)
	{
		const int TotalCount = 100;

		var partition1 = Enumerable.Range(1, TotalCount).Skip(skip).Take(take);
		Assert.Equal(expected, partition1.Count());
		Assert.Equal(expected, partition1.Select(SuperEnumerable.Identity).Count());
		Assert.Equal(expected, partition1.Select(SuperEnumerable.Identity).ToArray().Length);

		int end;
		try
		{
			end = checked(skip + take);
		}
		catch (OverflowException)
		{
			end = int.MaxValue;
		}

		var partition2 = Enumerable.Range(1, TotalCount).Take(skip..end);
		Assert.Equal(expected, partition2.Count());
		Assert.Equal(expected, partition2.Select(SuperEnumerable.Identity).Count());
		Assert.Equal(expected, partition2.Select(SuperEnumerable.Identity).ToArray().Length);

		var partition3 = Enumerable.Range(1, TotalCount).Take(^Math.Max(TotalCount - skip, 0)..end);
		Assert.Equal(expected, partition3.Count());
		Assert.Equal(expected, partition3.Select(SuperEnumerable.Identity).Count());
		Assert.Equal(expected, partition3.Select(SuperEnumerable.Identity).ToArray().Length);

		var partition4 = Enumerable.Range(1, TotalCount).Take(skip..^Math.Max(TotalCount - end, 0));
		Assert.Equal(expected, partition4.Count());
		Assert.Equal(expected, partition4.Select(SuperEnumerable.Identity).Count());
		Assert.Equal(expected, partition4.Select(SuperEnumerable.Identity).ToArray().Length);

		var partition5 = Enumerable.Range(1, TotalCount).Take(^Math.Max(TotalCount - skip, 0)..^Math.Max(TotalCount - end, 0));
		Assert.Equal(expected, partition5.Count());
		Assert.Equal(expected, partition5.Select(SuperEnumerable.Identity).Count());
		Assert.Equal(expected, partition5.Select(SuperEnumerable.Identity).ToArray().Length);
	}

	[Test]
	[Arguments(new[] { 1, 2, 3, 4 }, 1, 3, 2, 4)]
	[Arguments(new[] { 1 }, 0, 1, 1, 1)]
	[Arguments(new[] { 1, 2, 3, 5, 8, 13 }, 1, int.MaxValue, 2, 13)] // Regression test: The max index is precisely int.MaxValue.
	[Arguments(new[] { 1, 2, 3, 5, 8, 13 }, 0, 2, 1, 2)]
	[Arguments(new[] { 1, 2, 3, 5, 8, 13 }, 500, 2, 0, 0)]
	[Arguments(new int[] { }, 10, 8, 0, 0)]
	public void FirstAndLastOfLazySkipTakeChain(int[] source, int skip, int take, int first, int last)
	{
		var partition1 = ForceNotCollection(source).Skip(skip).Take(take);

		Assert.Equal(first, partition1.FirstOrDefault());
		Assert.Equal(first, partition1.ElementAtOrDefault(0));
		Assert.Equal(last, partition1.LastOrDefault());
		Assert.Equal(last, partition1.ElementAtOrDefault(partition1.Count() - 1));

		int end;
		try
		{
			end = checked(skip + take);
		}
		catch (OverflowException)
		{
			end = int.MaxValue;
		}

		var partition2 = ForceNotCollection(source).Take(skip..end);

		Assert.Equal(first, partition2.FirstOrDefault());
		Assert.Equal(first, partition2.ElementAtOrDefault(0));
		Assert.Equal(last, partition2.LastOrDefault());
		Assert.Equal(last, partition2.ElementAtOrDefault(partition2.Count() - 1));

		var partition3 = ForceNotCollection(source).Take(^Math.Max(source.Length - skip, 0)..end);

		Assert.Equal(first, partition3.FirstOrDefault());
		Assert.Equal(first, partition3.ElementAtOrDefault(0));
		Assert.Equal(last, partition3.LastOrDefault());
		Assert.Equal(last, partition3.ElementAtOrDefault(partition3.Count() - 1));

		var partition4 = ForceNotCollection(source).Take(skip..^Math.Max(source.Length - end, 0));

		Assert.Equal(first, partition4.FirstOrDefault());
		Assert.Equal(first, partition4.ElementAtOrDefault(0));
		Assert.Equal(last, partition4.LastOrDefault());
		Assert.Equal(last, partition4.ElementAtOrDefault(partition4.Count() - 1));

		var partition5 = ForceNotCollection(source).Take(^Math.Max(source.Length - skip, 0)..^Math.Max(source.Length - end, 0));

		Assert.Equal(first, partition5.FirstOrDefault());
		Assert.Equal(first, partition5.ElementAtOrDefault(0));
		Assert.Equal(last, partition5.LastOrDefault());
		Assert.Equal(last, partition5.ElementAtOrDefault(partition5.Count() - 1));
	}

	[Test]
	[Arguments(new[] { 1, 2, 3, 4, 5 }, 1, 3, new[] { -1, 0, 1, 2 }, new[] { 0, 2, 3, 4 })]
	[Arguments(new[] { 0xfefe, 7000, 123 }, 0, 3, new[] { -1, 0, 1, 2 }, new[] { 0, 0xfefe, 7000, 123 })]
	[Arguments(new[] { 0xfefe }, 100, 100, new[] { -1, 0, 1, 2 }, new[] { 0, 0, 0, 0 })]
	[Arguments(new[] { 0xfefe, 123, 456, 7890, 5555, 55 }, 1, 10, new[] { -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }, new[] { 0, 123, 456, 7890, 5555, 55, 0, 0, 0, 0, 0, 0, 0 })]
	public void ElementAtOfLazySkipTakeChain(int[] source, int skip, int take, int[] indices, int[] expectedValues)
	{
		var partition1 = ForceNotCollection(source).Skip(skip).Take(take);

		Assert.Equal(indices.Length, expectedValues.Length);
		for (var i = 0; i < indices.Length; i++)
			Assert.Equal(expectedValues[i], partition1.ElementAtOrDefault(indices[i]));

		int end;
		try
		{
			end = checked(skip + take);
		}
		catch (OverflowException)
		{
			end = int.MaxValue;
		}

		var partition2 = ForceNotCollection(source).Take(skip..end);
		for (var i = 0; i < indices.Length; i++)
			Assert.Equal(expectedValues[i], partition2.ElementAtOrDefault(indices[i]));

		var partition3 = ForceNotCollection(source).Take(^Math.Max(source.Length - skip, 0)..end);
		for (var i = 0; i < indices.Length; i++)
			Assert.Equal(expectedValues[i], partition3.ElementAtOrDefault(indices[i]));

		var partition4 = ForceNotCollection(source).Take(skip..^Math.Max(source.Length - end, 0));
		for (var i = 0; i < indices.Length; i++)
			Assert.Equal(expectedValues[i], partition4.ElementAtOrDefault(indices[i]));

		var partition5 = ForceNotCollection(source).Take(^Math.Max(source.Length - skip, 0)..^Math.Max(source.Length - end, 0));
		for (var i = 0; i < indices.Length; i++)
			Assert.Equal(expectedValues[i], partition5.ElementAtOrDefault(indices[i]));
	}

	[Test]
	public void OutOfBoundNoException()
	{
		static int[] Source() => [1, 2, 3, 4, 5];

		Assert.Equal(Source(), Source().Take(0..6));
		Assert.Equal(Source(), Source().Take(0..int.MaxValue));

		Assert.Equal([1, 2, 3, 4], Source().Take(^10..4));
		Assert.Equal([1, 2, 3, 4], Source().Take(^int.MaxValue..4));
		Assert.Equal(Source(), Source().Take(^10..6));
		Assert.Equal(Source(), Source().Take(^int.MaxValue..6));
		Assert.Equal(Source(), Source().Take(^10..int.MaxValue));
		Assert.Equal(Source(), Source().Take(^int.MaxValue..int.MaxValue));

		Assert.Empty(Source().Take(0..^6));
		Assert.Empty(Source().Take(0..^int.MaxValue));
		Assert.Empty(Source().Take(4..^6));
		Assert.Empty(Source().Take(4..^int.MaxValue));
		Assert.Empty(Source().Take(6..^6));
		Assert.Empty(Source().Take(6..^int.MaxValue));
		Assert.Empty(Source().Take(int.MaxValue..^6));
		Assert.Empty(Source().Take(int.MaxValue..^int.MaxValue));

		Assert.Equal([1, 2, 3, 4], Source().Take(^10..^1));
		Assert.Equal([1, 2, 3, 4], Source().Take(^int.MaxValue..^1));
		Assert.Empty(Source().Take(^0..^6));
		Assert.Empty(Source().Take(^1..^6));
		Assert.Empty(Source().Take(^6..^6));
		Assert.Empty(Source().Take(^10..^6));
		Assert.Empty(Source().Take(^int.MaxValue..^6));
		Assert.Empty(Source().Take(^0..^int.MaxValue));
		Assert.Empty(Source().Take(^1..^int.MaxValue));
		Assert.Empty(Source().Take(^6..^int.MaxValue));
		Assert.Empty(Source().Take(^int.MaxValue..^int.MaxValue));
	}

	[Test]
	public void OutOfBoundNoExceptionNotList()
	{
		var source = new[] { 1, 2, 3, 4, 5 };

		Assert.Equal(source, ForceNotCollection(source).Take(0..6));
		Assert.Equal(source, ForceNotCollection(source).Take(0..int.MaxValue));

		Assert.Equal([1, 2, 3, 4], ForceNotCollection(source).Take(^10..4));
		Assert.Equal([1, 2, 3, 4], ForceNotCollection(source).Take(^int.MaxValue..4));
		Assert.Equal(source, ForceNotCollection(source).Take(^10..6));
		Assert.Equal(source, ForceNotCollection(source).Take(^int.MaxValue..6));
		Assert.Equal(source, ForceNotCollection(source).Take(^10..int.MaxValue));
		Assert.Equal(source, ForceNotCollection(source).Take(^int.MaxValue..int.MaxValue));

		Assert.Empty(ForceNotCollection(source).Take(0..^6));
		Assert.Empty(ForceNotCollection(source).Take(0..^int.MaxValue));
		Assert.Empty(ForceNotCollection(source).Take(4..^6));
		Assert.Empty(ForceNotCollection(source).Take(4..^int.MaxValue));
		Assert.Empty(ForceNotCollection(source).Take(6..^6));
		Assert.Empty(ForceNotCollection(source).Take(6..^int.MaxValue));
		Assert.Empty(ForceNotCollection(source).Take(int.MaxValue..^6));
		Assert.Empty(ForceNotCollection(source).Take(int.MaxValue..^int.MaxValue));

		Assert.Equal([1, 2, 3, 4], ForceNotCollection(source).Take(^10..^1));
		Assert.Equal([1, 2, 3, 4], ForceNotCollection(source).Take(^int.MaxValue..^1));
		Assert.Empty(ForceNotCollection(source).Take(^0..^6));
		Assert.Empty(ForceNotCollection(source).Take(^1..^6));
		Assert.Empty(ForceNotCollection(source).Take(^6..^6));
		Assert.Empty(ForceNotCollection(source).Take(^10..^6));
		Assert.Empty(ForceNotCollection(source).Take(^int.MaxValue..^6));
		Assert.Empty(ForceNotCollection(source).Take(^0..^int.MaxValue));
		Assert.Empty(ForceNotCollection(source).Take(^1..^int.MaxValue));
		Assert.Empty(ForceNotCollection(source).Take(^6..^int.MaxValue));
		Assert.Empty(ForceNotCollection(source).Take(^int.MaxValue..^int.MaxValue));
	}

	[Test]
	public void MutableSource()
	{
		var source1 = new List<int>() { 0, 1, 2, 3, 4 };
		var query1 = source1.Take(3);
		source1.RemoveAt(0);
		source1.InsertRange(2, [-1, -2]);
		Assert.Equal([1, 2, -1], query1);

		var source2 = new List<int>() { 0, 1, 2, 3, 4 };
		var query2 = source2.Take(0..3);
		source2.RemoveAt(0);
		source2.InsertRange(2, [-1, -2]);
		Assert.Equal([1, 2, -1], query2);

		var source3 = new List<int>() { 0, 1, 2, 3, 4 };
		var query3 = source3.Take(^6..3);
		source3.RemoveAt(0);
		source3.InsertRange(2, [-1, -2]);
		Assert.Equal([1, 2, -1], query3);

		var source4 = new List<int>() { 0, 1, 2, 3, 4 };
		var query4 = source4.Take(^6..^3);
		source4.RemoveAt(0);
		source4.InsertRange(2, [-1, -2]);
		Assert.Equal([1, 2, -1], query4);
	}

	[Test]
	public void MutableSourceNotList()
	{
		var source1 = new List<int>() { 0, 1, 2, 3, 4 };
		var query1 = ForceNotCollection(source1).Select(SuperEnumerable.Identity).Take(3);
		source1.RemoveAt(0);
		source1.InsertRange(2, [-1, -2]);
		Assert.Equal([1, 2, -1], query1);

		var source2 = new List<int>() { 0, 1, 2, 3, 4 };
		var query2 = ForceNotCollection(source2).Select(SuperEnumerable.Identity).Take(0..3);
		source2.RemoveAt(0);
		source2.InsertRange(2, [-1, -2]);
		Assert.Equal([1, 2, -1], query2);

		var source3 = new List<int>() { 0, 1, 2, 3, 4 };
		var query3 = ForceNotCollection(source3).Select(SuperEnumerable.Identity).Take(^6..3);
		source3.RemoveAt(0);
		source3.InsertRange(2, [-1, -2]);
		Assert.Equal([1, 2, -1], query3);

		var source4 = new List<int>() { 0, 1, 2, 3, 4 };
		var query4 = ForceNotCollection(source4).Select(SuperEnumerable.Identity).Take(^6..^3);
		source4.RemoveAt(0);
		source4.InsertRange(2, [-1, -2]);
		Assert.Equal([1, 2, -1], query4);
	}

	[Test]
	public void NonEmptySourceConsistencyWithCountable()
	{
		static int[] Source() => [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];

		// Multiple elements in the middle.
		Assert.Equal(Source()[^9..5], Source().Take(^9..5));
		Assert.Equal(Source()[2..7], Source().Take(2..7));
		Assert.Equal(Source()[2..^4], Source().Take(2..^4));
		Assert.Equal(Source()[^7..^4], Source().Take(^7..^4));

		// Range with default index.
		Assert.Equal(Source()[^9..], Source().Take(^9..));
		Assert.Equal(Source()[2..], Source().Take(2..));
		Assert.Equal(Source()[..^4], Source().Take(..^4));
		Assert.Equal(Source()[..6], Source().Take(..6));

		// All.
		Assert.Equal(Source()[..], Source().Take(..));

		// Single element in the middle.
		Assert.Equal(Source()[^9..2], Source().Take(^9..2));
		Assert.Equal(Source()[2..3], Source().Take(2..3));
		Assert.Equal(Source()[2..^7], Source().Take(2..^7));
		Assert.Equal(Source()[^5..^4], Source().Take(^5..^4));

		// Single element at start.
		Assert.Equal(Source()[^10..1], Source().Take(^10..1));
		Assert.Equal(Source()[0..1], Source().Take(0..1));
		Assert.Equal(Source()[0..^9], Source().Take(0..^9));
		Assert.Equal(Source()[^10..^9], Source().Take(^10..^9));

		// Single element at end.
		Assert.Equal(Source()[^1..10], Source().Take(^1..10));
		Assert.Equal(Source()[9..10], Source().Take(9..10));
		Assert.Equal(Source()[9..^0], Source().Take(9..^0));
		Assert.Equal(Source()[^1..^0], Source().Take(^1..^0));

		// No element.
		Assert.Equal(Source()[3..3], Source().Take(3..3));
		Assert.Equal(Source()[6..^4], Source().Take(6..^4));
		Assert.Equal(Source()[3..^7], Source().Take(3..^7));
		Assert.Equal(Source()[^3..7], Source().Take(^3..7));
		Assert.Equal(Source()[^6..^6], Source().Take(^6..^6));
	}

	[Test]
	public void NonEmptySourceConsistencyWithCountableNotList()
	{
		int[] source = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];

		// Multiple elements in the middle.
		Assert.Equal(source[^9..5], ForceNotCollection(source).Take(^9..5));
		Assert.Equal(source[2..7], ForceNotCollection(source).Take(2..7));
		Assert.Equal(source[2..^4], ForceNotCollection(source).Take(2..^4));
		Assert.Equal(source[^7..^4], ForceNotCollection(source).Take(^7..^4));

		// Range with default index.
		Assert.Equal(source[^9..], ForceNotCollection(source).Take(^9..));
		Assert.Equal(source[2..], ForceNotCollection(source).Take(2..));
		Assert.Equal(source[..^4], ForceNotCollection(source).Take(..^4));
		Assert.Equal(source[..6], ForceNotCollection(source).Take(..6));

		// All.
		Assert.Equal(source[..], ForceNotCollection(source).Take(..));

		// Single element in the middle.
		Assert.Equal(source[^9..2], ForceNotCollection(source).Take(^9..2));
		Assert.Equal(source[2..3], ForceNotCollection(source).Take(2..3));
		Assert.Equal(source[2..^7], ForceNotCollection(source).Take(2..^7));
		Assert.Equal(source[^5..^4], ForceNotCollection(source).Take(^5..^4));

		// Single element at start.
		Assert.Equal(source[^10..1], ForceNotCollection(source).Take(^10..1));
		Assert.Equal(source[0..1], ForceNotCollection(source).Take(0..1));
		Assert.Equal(source[0..^9], ForceNotCollection(source).Take(0..^9));
		Assert.Equal(source[^10..^9], ForceNotCollection(source).Take(^10..^9));

		// Single element at end.
		Assert.Equal(source[^1..10], ForceNotCollection(source).Take(^1..10));
		Assert.Equal(source[9..10], ForceNotCollection(source).Take(9..10));
		Assert.Equal(source[9..^0], ForceNotCollection(source).Take(9..^0));
		Assert.Equal(source[^1..^0], ForceNotCollection(source).Take(^1..^0));

		// No element.
		Assert.Equal(source[3..3], ForceNotCollection(source).Take(3..3));
		Assert.Equal(source[6..^4], ForceNotCollection(source).Take(6..^4));
		Assert.Equal(source[3..^7], ForceNotCollection(source).Take(3..^7));
		Assert.Equal(source[^3..7], ForceNotCollection(source).Take(^3..7));
		Assert.Equal(source[^6..^6], ForceNotCollection(source).Take(^6..^6));
	}

	[Test]
	public void NonEmptySourceDoNotThrowException()
	{
		static int[] Source() => [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];

		Assert.Empty(Source().Take(3..2));
		Assert.Empty(Source().Take(6..^5));
		Assert.Empty(Source().Take(3..^8));
		Assert.Empty(Source().Take(^6..^7));
	}

	[Test]
	public void NonEmptySourceDoNotThrowExceptionNotList()
	{
		int[] source = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];

		Assert.Empty(ForceNotCollection(source).Take(3..2));
		Assert.Empty(ForceNotCollection(source).Take(6..^5));
		Assert.Empty(ForceNotCollection(source).Take(3..^8));
		Assert.Empty(ForceNotCollection(source).Take(^6..^7));
	}

	[Test]
	public void EmptySourceDoNotThrowException()
	{
		static int[] Source() => [];

		// Multiple elements in the middle.
		Assert.Empty(Source().Take(^9..5));
		Assert.Empty(Source().Take(2..7));
		Assert.Empty(Source().Take(2..^4));
		Assert.Empty(Source().Take(^7..^4));

		// Range with default index.
		Assert.Empty(Source().Take(^9..));
		Assert.Empty(Source().Take(2..));
		Assert.Empty(Source().Take(..^4));
		Assert.Empty(Source().Take(..6));

		// All.
		Assert.Equal(Source()[..], Source().Take(..));

		// Single element in the middle.
		Assert.Empty(Source().Take(^9..2));
		Assert.Empty(Source().Take(2..3));
		Assert.Empty(Source().Take(2..^7));
		Assert.Empty(Source().Take(^5..^4));

		// Single element at start.
		Assert.Empty(Source().Take(^10..1));
		Assert.Empty(Source().Take(0..1));
		Assert.Empty(Source().Take(0..^9));
		Assert.Empty(Source().Take(^10..^9));

		// Single element at end.
		Assert.Empty(Source().Take(^1..^10));
		Assert.Empty(Source().Take(9..10));
		Assert.Empty(Source().Take(9..^9));
		Assert.Empty(Source().Take(^1..^9));

		// No element.
		Assert.Empty(Source().Take(3..3));
		Assert.Empty(Source().Take(6..^4));
		Assert.Empty(Source().Take(3..^7));
		Assert.Empty(Source().Take(^3..7));
		Assert.Empty(Source().Take(^6..^6));

		// Invalid range.
		Assert.Empty(Source().Take(3..2));
		Assert.Empty(Source().Take(6..^5));
		Assert.Empty(Source().Take(3..^8));
		Assert.Empty(Source().Take(^6..^7));
	}

	[Test]
	public void EmptySourceDoNotThrowExceptionNotList()
	{
		var source = Array.Empty<int>();

		// Multiple elements in the middle.
		Assert.Empty(ForceNotCollection(source).Take(^9..5));
		Assert.Empty(ForceNotCollection(source).Take(2..7));
		Assert.Empty(ForceNotCollection(source).Take(2..^4));
		Assert.Empty(ForceNotCollection(source).Take(^7..^4));

		// Range with default index.
		Assert.Empty(ForceNotCollection(source).Take(^9..));
		Assert.Empty(ForceNotCollection(source).Take(2..));
		Assert.Empty(ForceNotCollection(source).Take(..^4));
		Assert.Empty(ForceNotCollection(source).Take(..6));

		// All.
		Assert.Equal(source[..], ForceNotCollection(source).Take(..));

		// Single element in the middle.
		Assert.Empty(ForceNotCollection(source).Take(^9..2));
		Assert.Empty(ForceNotCollection(source).Take(2..3));
		Assert.Empty(ForceNotCollection(source).Take(2..^7));
		Assert.Empty(ForceNotCollection(source).Take(^5..^4));

		// Single element at start.
		Assert.Empty(ForceNotCollection(source).Take(^10..1));
		Assert.Empty(ForceNotCollection(source).Take(0..1));
		Assert.Empty(ForceNotCollection(source).Take(0..^9));
		Assert.Empty(ForceNotCollection(source).Take(^10..^9));

		// Single element at end.
		Assert.Empty(ForceNotCollection(source).Take(^1..^10));
		Assert.Empty(ForceNotCollection(source).Take(9..10));
		Assert.Empty(ForceNotCollection(source).Take(9..^9));
		Assert.Empty(ForceNotCollection(source).Take(^1..^9));

		// No element.
		Assert.Empty(ForceNotCollection(source).Take(3..3));
		Assert.Empty(ForceNotCollection(source).Take(6..^4));
		Assert.Empty(ForceNotCollection(source).Take(3..^7));
		Assert.Empty(ForceNotCollection(source).Take(^3..7));
		Assert.Empty(ForceNotCollection(source).Take(^6..^6));

		// Invalid range.
		Assert.Empty(ForceNotCollection(source).Take(3..2));
		Assert.Empty(ForceNotCollection(source).Take(6..^5));
		Assert.Empty(ForceNotCollection(source).Take(3..^8));
		Assert.Empty(ForceNotCollection(source).Take(^6..^7));
	}
	private static IEnumerable<T> ForceNotCollection<T>(IEnumerable<T> source)
	{
		foreach (var i in source)
			yield return i;
	}
}

#endif

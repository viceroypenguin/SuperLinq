// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Test.Async;

public class TakeTest
{
	[Fact]
	public async Task SameResultsRepeatCallsIntQuery()
	{
		var q = (from x in new[] { 9999, 0, 888, -1, 66, -777, 1, 2, -12345 }
				 where x > int.MinValue
				 select x).ToAsyncEnumerable();

		Assert.Equal(await q.Take(9).ToListAsync(), await q.Take(9).ToListAsync());

		Assert.Equal(await q.Take(0..9).ToListAsync(), await q.Take(0..9).ToListAsync());
		Assert.Equal(await q.Take(^9..9).ToListAsync(), await q.Take(^9..9).ToListAsync());
		Assert.Equal(await q.Take(0..^0).ToListAsync(), await q.Take(0..^0).ToListAsync());
		Assert.Equal(await q.Take(^9..^0).ToListAsync(), await q.Take(^9..^0).ToListAsync());
	}

	[Fact]
	public async Task SameResultsRepeatCallsStringQuery()
	{
		var q = (from x in new[] { "!@#$%^", "C", "AAA", "", "Calling Twice", "SoS", string.Empty }
				 where !string.IsNullOrEmpty(x)
				 select x).ToAsyncEnumerable();

		Assert.Equal(await q.Take(7).ToListAsync(), await q.Take(7).ToListAsync());

		Assert.Equal(await q.Take(0..7).ToListAsync(), await q.Take(0..7).ToListAsync());
		Assert.Equal(await q.Take(^7..7).ToListAsync(), await q.Take(^7..7).ToListAsync());
		Assert.Equal(await q.Take(0..^0).ToListAsync(), await q.Take(0..^0).ToListAsync());
		Assert.Equal(await q.Take(^7..^0).ToListAsync(), await q.Take(^7..^0).ToListAsync());
	}

	[Fact]
	public async Task SourceEmptyCountPositive()
	{
		var source = AsyncEnumerable.Empty<int>();
		Assert.Empty(await source.Take(5).ToListAsync());
		Assert.Empty(await source.Take(0..5).ToListAsync());
		Assert.Empty(await source.Take(^5..5).ToListAsync());
		Assert.Empty(await source.Take(0..^0).ToListAsync());
		Assert.Empty(await source.Take(^5..^0).ToListAsync());
	}

	[Fact]
	public async Task SourceNonEmptyCountNegative()
	{
		var source = AsyncSeq(2, 5, 9, 1);
		Assert.Empty(await source.Take(-5).ToListAsync());
		Assert.Empty(await source.Take(^9..0).ToListAsync());
	}

	[Fact]
	public async Task SourceNonEmptyCountZero()
	{
		var source = AsyncSeq(2, 5, 9, 1);
		Assert.Empty(await source.Take(0).ToListAsync());
		Assert.Empty(await source.Take(0..0).ToListAsync());
		Assert.Empty(await source.Take(^4..0).ToListAsync());
		Assert.Empty(await source.Take(0..^4).ToListAsync());
		Assert.Empty(await source.Take(^4..^4).ToListAsync());
	}

	[Fact]
	public async Task SourceNonEmptyCountOne()
	{
		var source = AsyncSeq(2, 5, 9, 1);
		int[] expected = { 2 };

		Assert.Equal(expected, await source.Take(1).ToListAsync());
		Assert.Equal(expected, await source.Take(0..1).ToListAsync());
		Assert.Equal(expected, await source.Take(^4..1).ToListAsync());
		Assert.Equal(expected, await source.Take(0..^3).ToListAsync());
		Assert.Equal(expected, await source.Take(^4..^3).ToListAsync());
	}

	[Fact]
	public async Task SourceNonEmptyTakeAllExactly()
	{
		var source = new[] { 2, 5, 9, 1 };

		Assert.Equal(source, await source.ToAsyncEnumerable().Take(source.Length).ToListAsync());
		Assert.Equal(source, await source.ToAsyncEnumerable().Take(0..source.Length).ToListAsync());
		Assert.Equal(source, await source.ToAsyncEnumerable().Take(^source.Length..source.Length).ToListAsync());
		Assert.Equal(source, await source.ToAsyncEnumerable().Take(0..^0).ToListAsync());
		Assert.Equal(source, await source.ToAsyncEnumerable().Take(^source.Length..^0).ToListAsync());
	}

	[Fact]
	public async Task SourceNonEmptyTakeAllButOne()
	{
		var source = AsyncSeq(2, 5, 9, 1);
		int[] expected = { 2, 5, 9 };

		Assert.Equal(expected, await source.Take(3).ToListAsync());
		Assert.Equal(expected, await source.Take(0..3).ToListAsync());
		Assert.Equal(expected, await source.Take(^4..3).ToListAsync());
		Assert.Equal(expected, await source.Take(0..^1).ToListAsync());
		Assert.Equal(expected, await source.Take(^4..^1).ToListAsync());
	}

	[Fact]
	public async Task RunOnce()
	{
		var source = new[] { 2, 5, 9, 1 };
		int[] expected = { 2, 5, 9 };

		Assert.Equal(expected, await source.AsTestingSequence().Take(3).ToListAsync());
		Assert.Equal(expected, await source.AsTestingSequence().Take(0..3).ToListAsync());
		Assert.Equal(expected, await source.AsTestingSequence().Take(^4..3).ToListAsync());
		Assert.Equal(expected, await source.AsTestingSequence().Take(0..^1).ToListAsync());
		Assert.Equal(expected, await source.AsTestingSequence().Take(^4..^1).ToListAsync());
	}

	[Fact]
	public async Task SourceNonEmptyTakeExcessive()
	{
		var source = new int?[] { 2, 5, null, 9, 1 };

		Assert.Equal(source, await source.ToAsyncEnumerable().Take(source.Length + 1).ToListAsync());
		Assert.Equal(source, await source.ToAsyncEnumerable().Take(0..(source.Length + 1)).ToListAsync());
		Assert.Equal(source, await source.ToAsyncEnumerable().Take(^(source.Length + 1)..(source.Length + 1)).ToListAsync());
	}

	[Fact]
	public void ThrowsOnNullSource()
	{
		IAsyncEnumerable<int> source = null!;
		Assert.Throws<ArgumentNullException>("source", () => source.Take(5));
		Assert.Throws<ArgumentNullException>("source", () => source.Take(0..5));
		Assert.Throws<ArgumentNullException>("source", () => source.Take(^5..5));
		Assert.Throws<ArgumentNullException>("source", () => source.Take(0..^0));
		Assert.Throws<ArgumentNullException>("source", () => source.Take(^5..^0));
	}

	[Fact]
	public async Task FollowWithTake()
	{
		var source = AsyncSeq(5, 6, 7, 8);
		var expected = new[] { 5, 6 };

		Assert.Equal(expected, await source.Take(5).Take(3).Take(2).Take(40).ToListAsync());
		Assert.Equal(expected, await source.Take(0..5).Take(0..3).Take(0..2).Take(0..40).ToListAsync());
		Assert.Equal(expected, await source.Take(^4..5).Take(^4..3).Take(^3..2).Take(^2..40).ToListAsync());
		Assert.Equal(expected, await source.Take(0..^0).Take(0..^1).Take(0..^1).Take(0..^0).ToListAsync());
		Assert.Equal(expected, await source.Take(^4..^0).Take(^4..^1).Take(^3..^1).Take(^2..^0).ToListAsync());
	}

	[Fact]
	public async Task FollowWithSkip()
	{
		var source = AsyncSeq(1, 2, 3, 4, 5, 6);
		var expected = new[] { 3, 4, 5 };

		Assert.Equal(expected, await source.Take(5).Skip(2).Skip(-4).ToListAsync());
		Assert.Equal(expected, await source.Take(0..5).Skip(2).Skip(-4).ToListAsync());
		Assert.Equal(expected, await source.Take(^6..5).Skip(2).Skip(-4).ToListAsync());
		Assert.Equal(expected, await source.Take(0..^1).Skip(2).Skip(-4).ToListAsync());
		Assert.Equal(expected, await source.Take(^6..^1).Skip(2).Skip(-4).ToListAsync());
	}

	[Fact]
	public async Task LazyOverflowRegression()
	{
		var range = AsyncEnumerable.Range(1, 100);
		var skipped = range.Skip(42); // Min index is 42.

		var taken1 = skipped.Take(int.MaxValue); // May try to calculate max index as 42 + int.MaxValue, leading to integer overflow.
		Assert.Equal(100 - 42, await taken1.CountAsync());
		Assert.Equal(Enumerable.Range(43, 100 - 42), await taken1.ToListAsync());

		var taken2 = AsyncEnumerable.Range(1, 100).Take(42..int.MaxValue);
		Assert.Equal(100 - 42, await taken2.CountAsync());
		Assert.Equal(Enumerable.Range(43, 100 - 42), await taken2.ToListAsync());

		var taken3 = AsyncEnumerable.Range(1, 100).Take(^(100 - 42)..int.MaxValue);
		Assert.Equal(100 - 42, await taken3.CountAsync());
		Assert.Equal(Enumerable.Range(43, 100 - 42), await taken3.ToListAsync());

		var taken4 = AsyncEnumerable.Range(1, 100).Take(42..^0);
		Assert.Equal(100 - 42, await taken4.CountAsync());
		Assert.Equal(Enumerable.Range(43, 100 - 42), await taken4.ToListAsync());

		var taken5 = AsyncEnumerable.Range(1, 100).Take(^(100 - 42)..^0);
		Assert.Equal(100 - 42, await taken5.CountAsync());
		Assert.Equal(Enumerable.Range(43, 100 - 42), await taken5.ToListAsync());
	}

	[Fact]
	public async Task OutOfBoundNoException()
	{
		Func<IAsyncEnumerable<int>> source = () => AsyncSeq( 1, 2, 3, 4, 5 );

		Assert.Equal(new int[] { 1, 2, 3, 4, 5 }, await source().Take(0..6).ToListAsync());
		Assert.Equal(new int[] { 1, 2, 3, 4, 5 }, await source().Take(0..int.MaxValue).ToListAsync());

		Assert.Equal(new int[] { 1, 2, 3, 4 }, await source().Take(^10..4).ToListAsync());
		Assert.Equal(new int[] { 1, 2, 3, 4 }, await source().Take(^int.MaxValue..4).ToListAsync());
		Assert.Equal(new int[] { 1, 2, 3, 4, 5 }, await source().Take(^10..6).ToListAsync());
		Assert.Equal(new int[] { 1, 2, 3, 4, 5 }, await source().Take(^int.MaxValue..6).ToListAsync());
		Assert.Equal(new int[] { 1, 2, 3, 4, 5 }, await source().Take(^10..int.MaxValue).ToListAsync());
		Assert.Equal(new int[] { 1, 2, 3, 4, 5 }, await source().Take(^int.MaxValue..int.MaxValue).ToListAsync());

		Assert.Empty(await source().Take(0..^6).ToListAsync());
		Assert.Empty(await source().Take(0..^int.MaxValue).ToListAsync());
		Assert.Empty(await source().Take(4..^6).ToListAsync());
		Assert.Empty(await source().Take(4..^int.MaxValue).ToListAsync());
		Assert.Empty(await source().Take(6..^6).ToListAsync());
		Assert.Empty(await source().Take(6..^int.MaxValue).ToListAsync());
		Assert.Empty(await source().Take(int.MaxValue..^6).ToListAsync());
		Assert.Empty(await source().Take(int.MaxValue..^int.MaxValue).ToListAsync());

		Assert.Equal(new int[] { 1, 2, 3, 4 }, await source().Take(^10..^1).ToListAsync());
		Assert.Equal(new int[] { 1, 2, 3, 4 }, await source().Take(^int.MaxValue..^1).ToListAsync());
		Assert.Empty(await source().Take(^0..^6).ToListAsync());
		Assert.Empty(await source().Take(^1..^6).ToListAsync());
		Assert.Empty(await source().Take(^6..^6).ToListAsync());
		Assert.Empty(await source().Take(^10..^6).ToListAsync());
		Assert.Empty(await source().Take(^int.MaxValue..^6).ToListAsync());
		Assert.Empty(await source().Take(^0..^int.MaxValue).ToListAsync());
		Assert.Empty(await source().Take(^1..^int.MaxValue).ToListAsync());
		Assert.Empty(await source().Take(^6..^int.MaxValue).ToListAsync());
		Assert.Empty(await source().Take(^int.MaxValue..^int.MaxValue).ToListAsync());
	}

	[Fact]
	public async Task NonEmptySourceDoNotThrowException()
	{
		Func<IAsyncEnumerable<int>> source = () => AsyncSeq( 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 );

		Assert.Empty(await source().Take(3..2).ToListAsync());
		Assert.Empty(await source().Take(6..^5).ToListAsync());
		Assert.Empty(await source().Take(3..^8).ToListAsync());
		Assert.Empty(await source().Take(^6..^7).ToListAsync());
	}

	[Fact]
	public async Task EmptySourceDoNotThrowException()
	{
		Func<IAsyncEnumerable<int>> source = () => AsyncSeq<int>();

		// Multiple elements in the middle.
		Assert.Empty(await source().Take(^9..5).ToListAsync());
		Assert.Empty(await source().Take(2..7).ToListAsync());
		Assert.Empty(await source().Take(2..^4).ToListAsync());
		Assert.Empty(await source().Take(^7..^4).ToListAsync());

		// Range with default index.
		Assert.Empty(await source().Take(^9..).ToListAsync());
		Assert.Empty(await source().Take(2..).ToListAsync());
		Assert.Empty(await source().Take(..^4).ToListAsync());
		Assert.Empty(await source().Take(..6).ToListAsync());

		// All.
		Assert.Empty(await source().Take(..).ToListAsync());

		// Single element in the middle.
		Assert.Empty(await source().Take(^9..2).ToListAsync());
		Assert.Empty(await source().Take(2..3).ToListAsync());
		Assert.Empty(await source().Take(2..^7).ToListAsync());
		Assert.Empty(await source().Take(^5..^4).ToListAsync());

		// Single element at start.
		Assert.Empty(await source().Take(^10..1).ToListAsync());
		Assert.Empty(await source().Take(0..1).ToListAsync());
		Assert.Empty(await source().Take(0..^9).ToListAsync());
		Assert.Empty(await source().Take(^10..^9).ToListAsync());

		// Single element at end.
		Assert.Empty(await source().Take(^1..^10).ToListAsync());
		Assert.Empty(await source().Take(9..10).ToListAsync());
		Assert.Empty(await source().Take(9..^9).ToListAsync());
		Assert.Empty(await source().Take(^1..^9).ToListAsync());

		// No element.
		Assert.Empty(await source().Take(3..3).ToListAsync());
		Assert.Empty(await source().Take(6..^4).ToListAsync());
		Assert.Empty(await source().Take(3..^7).ToListAsync());
		Assert.Empty(await source().Take(^3..7).ToListAsync());
		Assert.Empty(await source().Take(^6..^6).ToListAsync());

		// Invalid range.
		Assert.Empty(await source().Take(3..2).ToListAsync());
		Assert.Empty(await source().Take(6..^5).ToListAsync());
		Assert.Empty(await source().Take(3..^8).ToListAsync());
		Assert.Empty(await source().Take(^6..^7).ToListAsync());
	}
}

﻿namespace SuperLinq.Async.Tests;

public sealed class StartsWithTest
{
	[Test]
	[Arguments(new[] { 1, 2, 3 }, new[] { 1, 2 }, true)]
	[Arguments(new[] { 1, 2, 3 }, new[] { 1, 2, 3 }, true)]
	[Arguments(new[] { 1, 2, 3 }, new[] { 1, 2, 3, 4 }, false)]
	public async Task StartsWithWithIntegers(IEnumerable<int> first, IEnumerable<int> second, bool expected)
	{
		await using var f = first.AsTestingSequence();

		Assert.Equal(expected, await f.StartsWith(second));
	}

	[Test]
	[Arguments(new[] { '1', '2', '3' }, new[] { '1', '2' }, true)]
	[Arguments(new[] { '1', '2', '3' }, new[] { '1', '2', '3' }, true)]
	[Arguments(new[] { '1', '2', '3' }, new[] { '1', '2', '3', '4' }, false)]
	public async Task StartsWithWithChars(IEnumerable<char> first, IEnumerable<char> second, bool expected)
	{
		await using var f = first.AsTestingSequence();
		await using var s = second.AsTestingSequence();

		Assert.Equal(expected, await f.StartsWith(s));
	}

	[Test]
	[Arguments("123", "12", true)]
	[Arguments("123", "123", true)]
	[Arguments("123", "1234", false)]
	public async Task StartsWithWithStrings(string first, string second, bool expected)
	{
		await using var f = first.AsTestingSequence();
		await using var s = second.AsTestingSequence();

		Assert.Equal(expected, await f.StartsWith(s));
	}

	[Test]
	public async Task StartsWithReturnsTrueIfBothEmpty()
	{
		await using var f = Array.Empty<int>().AsTestingSequence();
		await using var s = Array.Empty<int>().AsTestingSequence();

		Assert.True(await f.StartsWith(s));
	}

	[Test]
	public async Task StartsWithReturnsFalseIfOnlyFirstIsEmpty()
	{
		await using var f = Array.Empty<int>().AsTestingSequence();
		await using var s = Seq(1, 2, 3).AsTestingSequence();

		Assert.False(await f.StartsWith(s));
	}

	[Test]
	[Arguments("", "", true)]
	[Arguments("1", "", true)]
	public async Task StartsWithReturnsTrueIfSecondIsEmpty(string first, string second, bool expected)
	{
		await using var f = first.AsTestingSequence();
		await using var s = second.AsTestingSequence();

		Assert.Equal(expected, await f.StartsWith(s));
	}

	[Test]
	public async Task StartsWithUsesSpecifiedEqualityComparerOrDefault()
	{
		var first = AsyncSeq(1, 2, 3);
		var second = AsyncSeq(4, 5, 6);

		Assert.False(await first.StartsWith(second, comparer: null));
		Assert.False(await first.StartsWith(second, EqualityComparer.Create<int>(delegate { return false; })));
		Assert.True(await first.StartsWith(second, EqualityComparer.Create<int>(delegate { return true; })));
	}
}

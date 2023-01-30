﻿using System.Collections.ObjectModel;

namespace Test.Async;

public class CopyToTest
{
	[Fact]
	public async Task NullArgumentTest()
	{
		await Assert.ThrowsAsync<ArgumentNullException>(
			"source",
			async () => await default(IAsyncEnumerable<int>)!.CopyTo(Array.Empty<int>()));
		await Assert.ThrowsAsync<ArgumentNullException>(
			"source",
			async () => await default(IAsyncEnumerable<int>)!.CopyTo(Array.Empty<int>(), 1));
		await Assert.ThrowsAsync<ArgumentNullException>(
			"list",
			async () => await AsyncSeq<int>().CopyTo(default(int[])!));
		await Assert.ThrowsAsync<ArgumentNullException>(
			"list",
			async () => await AsyncSeq<int>().CopyTo(default(int[])!, 1));
	}

	[Fact]
	public Task ThrowsOnNegativeIndex()
	{
		return Assert.ThrowsAsync<ArgumentOutOfRangeException>(
			"index",
			async () => await AsyncSeq<int>().CopyTo(Array.Empty<int>(), -1));
	}

	[Fact]
	public Task ThrowsOnTooMuchDataForArray()
	{
		return Assert.ThrowsAsync<IndexOutOfRangeException>(
			async () => await AsyncSeq(1).CopyTo(Array.Empty<int>()));
	}

	[Fact]
	public async Task CopiesDataToArray()
	{
		var array = new int[4];

		var cnt = await AsyncSeq(1).CopyTo(array);
		array.AssertSequenceEqual(1, 0, 0, 0);
		Assert.Equal(1, cnt);

		cnt = await AsyncSeq(2).CopyTo(array, 1);
		array.AssertSequenceEqual(1, 2, 0, 0);
		Assert.Equal(1, cnt);
	}

	[Fact]
	public async Task CopiesDataToList()
	{
		var list = new List<int>();

		var cnt = await AsyncSeq(1).CopyTo(list);
		list.AssertSequenceEqual(1);
		Assert.Equal(1, cnt);

		cnt = await AsyncSeq(2).CopyTo(list, 1);
		list.AssertSequenceEqual(1, 2);
		Assert.Equal(1, cnt);

		cnt = await AsyncSeq(3, 4).CopyTo(list, 1);
		list.AssertSequenceEqual(1, 3, 4);
		Assert.Equal(2, cnt);
	}

	[Fact]
	public async Task CopiesDataToIList()
	{
		var list = new Collection<int>();

		var cnt = await AsyncSeq(1).CopyTo(list);
		list.AssertSequenceEqual(1);
		Assert.Equal(1, cnt);

		cnt = await AsyncSeq(2).CopyTo(list, 1);
		list.AssertSequenceEqual(1, 2);
		Assert.Equal(1, cnt);

		cnt = await AsyncSeq(3, 4).CopyTo(list, 1);
		list.AssertSequenceEqual(1, 3, 4);
		Assert.Equal(2, cnt);
	}
}

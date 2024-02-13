using System.Collections.ObjectModel;

namespace Test.Async;

public class CopyToTest
{
	[Fact]
	public async Task NullArgumentTest()
	{
		_ = await Assert.ThrowsAsync<ArgumentNullException>(
			"source",
			async () => await default(IAsyncEnumerable<int>)!.CopyTo([]));
		_ = await Assert.ThrowsAsync<ArgumentNullException>(
			"source",
			async () => await default(IAsyncEnumerable<int>)!.CopyTo([], 1));
		_ = await Assert.ThrowsAsync<ArgumentNullException>(
			"list",
			async () => await AsyncSeq<int>().CopyTo(default(int[])!));
		_ = await Assert.ThrowsAsync<ArgumentNullException>(
			"list",
			async () => await AsyncSeq<int>().CopyTo(default(int[])!, 1));
	}

	[Fact]
	public Task ThrowsOnNegativeIndex()
	{
		return Assert.ThrowsAsync<ArgumentOutOfRangeException>(
			"index",
			async () => await AsyncSeq<int>().CopyTo([], -1));
	}

	[Fact]
	public Task ThrowsOnTooMuchDataForArray()
	{
		return Assert.ThrowsAsync<IndexOutOfRangeException>(
			async () => await AsyncSeq(1).CopyTo([]));
	}

	[Fact]
	public async Task CopiesDataToArray()
	{
		var array = new int[4];

		await using (var xs = TestingSequence.Of(1))
		{
			var cnt = await xs.CopyTo(array);
			array.AssertSequenceEqual(1, 0, 0, 0);
			Assert.Equal(1, cnt);
		}

		await using (var xs = TestingSequence.Of(2))
		{
			var cnt = await xs.CopyTo(array, 1);
			array.AssertSequenceEqual(1, 2, 0, 0);
			Assert.Equal(1, cnt);
		}
	}

	[Fact]
	public async Task CopiesDataToList()
	{
		var list = new List<int>();

		await using (var xs = TestingSequence.Of(1))
		{
			var cnt = await xs.CopyTo(list);
			list.AssertSequenceEqual(1);
			Assert.Equal(1, cnt);
		}

		await using (var xs = TestingSequence.Of(2))
		{
			var cnt = await xs.CopyTo(list, 1);
			list.AssertSequenceEqual(1, 2);
			Assert.Equal(1, cnt);
		}

		await using (var xs = TestingSequence.Of(3, 4))
		{
			var cnt = await xs.CopyTo(list, 1);
			list.AssertSequenceEqual(1, 3, 4);
			Assert.Equal(2, cnt);
		}
	}

	[Fact]
	public async Task CopiesDataToIList()
	{
		var list = new Collection<int>();

		await using (var xs = TestingSequence.Of(1))
		{
			var cnt = await xs.CopyTo(list);
			list.AssertSequenceEqual(1);
			Assert.Equal(1, cnt);
		}

		await using (var xs = TestingSequence.Of(2))
		{
			var cnt = await xs.CopyTo(list, 1);
			list.AssertSequenceEqual(1, 2);
			Assert.Equal(1, cnt);
		}

		await using (var xs = TestingSequence.Of(3, 4))
		{
			var cnt = await xs.CopyTo(list, 1);
			list.AssertSequenceEqual(1, 3, 4);
			Assert.Equal(2, cnt);
		}
	}
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if !NO_INDEX

namespace SuperLinq.Async.Tests;

public sealed class ElementAtTest
{
	[Fact]
	public async Task FromStartIndexInt()
	{
		var q = AsyncSeq(9999, 0, 888, -1, 66, -777, 1, 2, -12345);

		Assert.Equal(-1, await q.ElementAtAsync(new Index(3)));
		Assert.Equal(-1, await q.ElementAtOrDefaultAsync(new Index(3)));
	}

	[Fact]
	public async Task FromStartIndexOutOfRangeInt()
	{
		var q = AsyncSeq(9999, 0, 888, -1, 66, -777, 1, 2, -12345);

		_ = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await q.ElementAtAsync(new Index(10)));
		Assert.Equal(default, await q.ElementAtOrDefaultAsync(new Index(10)));
	}

	[Fact]
	public async Task FromEndIndexInt()
	{
		var q = AsyncSeq(9999, 0, 888, -1, 66, -777, 1, 2, -12345);

		Assert.Equal(888, await q.ElementAtAsync(^7));
		Assert.Equal(888, await q.ElementAtOrDefaultAsync(^7));
	}

	[Fact]
	public async Task FromEndIndexOutOfRangeInt()
	{
		var q = AsyncSeq(9999, 0, 888, -1, 66, -777, 1, 2, -12345);

		_ = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await q.ElementAtAsync(^10));
		Assert.Equal(default, await q.ElementAtOrDefaultAsync(^10));
	}

	[Fact]
	public async Task FromStartIndexString()
	{
		var q = AsyncSeq("!@#$%^", "C", "AAA", "", "Calling Twice", "SoS", "");

		Assert.Equal("", await q.ElementAtAsync(new Index(3)));
		Assert.Equal("", await q.ElementAtOrDefaultAsync(new Index(3)));
	}

	[Fact]
	public async Task FromStartIndexOutOfRangeString()
	{
		var q = AsyncSeq("!@#$%^", "C", "AAA", "", "Calling Twice", "SoS", "");

		_ = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await q.ElementAtAsync(new Index(10)));
		Assert.Equal(default, await q.ElementAtOrDefaultAsync(new Index(10)));
	}

	[Fact]
	public async Task FromEndIndexString()
	{
		var q = AsyncSeq("!@#$%^", "C", "AAA", "", "Calling Twice", "SoS", "");

		Assert.Equal("", await q.ElementAtAsync(^4));
		Assert.Equal("", await q.ElementAtOrDefaultAsync(^4));
	}

	[Fact]
	public async Task FromEndIndexOutOfRangeString()
	{
		var q = AsyncSeq("!@#$%^", "C", "AAA", "", "Calling Twice", "SoS", "");

		_ = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await q.ElementAtAsync(^10));
		Assert.Equal(default, await q.ElementAtOrDefaultAsync(^10));
	}
}

#endif

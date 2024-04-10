// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.

namespace Test;

public sealed class DeferTest
{
	[Fact]
	public void DeferIsLazy()
	{
		_ = SuperEnumerable.Defer(BreakingFunc.Of<IEnumerable<int>>());
	}

	[Fact]
	public void DeferBehavior()
	{
		var starts = 0;
		var length = 5;

		var seq = SuperEnumerable.Defer(() =>
		{
			starts++;
			return Enumerable.Range(1, length);
		});

		Assert.Equal(0, starts);

		seq.AssertSequenceEqual(Enumerable.Range(1, length));
		Assert.Equal(1, starts);

		length = 10;
		seq.AssertSequenceEqual(Enumerable.Range(1, length));
		Assert.Equal(2, starts);
	}
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Test;

public class TakeTest
{
	// simplified tests - already tested by fx, only need to prove that we don't step on fx toes

	[Fact]
	public void SameResultsRepeatCallsIntQuery()
	{
		var q = from x in new[] { 9999, 0, 888, -1, 66, -777, 1, 2, -12345 }
				where x > int.MinValue
				select x;

		Assert.Equal(q.Take(0..9), q.Take(0..9));
		Assert.Equal(q.Take(^9..9), q.Take(^9..9));
		Assert.Equal(q.Take(0..^0), q.Take(0..^0));
		Assert.Equal(q.Take(^9..^0), q.Take(^9..^0));
	}

	[Fact]
	public void SameResultsRepeatCallsStringQuery()
	{
		var q = from x in new[] { "!@#$%^", "C", "AAA", "", "Calling Twice", "SoS", string.Empty }
				where !string.IsNullOrEmpty(x)
				select x;

		Assert.Equal(q.Take(0..7), q.Take(0..7));
		Assert.Equal(q.Take(^7..7), q.Take(^7..7));
		Assert.Equal(q.Take(0..^0), q.Take(0..^0));
		Assert.Equal(q.Take(^7..^0), q.Take(^7..^0));
	}

}